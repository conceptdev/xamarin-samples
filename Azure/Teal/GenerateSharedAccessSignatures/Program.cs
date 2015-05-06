using System;

using System.IO;    
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System.Text;

namespace GenerateSharedAccessSignatures
{

	// http://azure.microsoft.com/en-us/documentation/articles/storage-dotnet-shared-access-signature-part-2/

	class MainClass
	{
	// TODO: get this info from the azure portal
	static string StorageConnectionString = "DefaultEndpointsProtocol=https;AccountName=FROM_PORTAL;AccountKey=FROM_PORTAL";

		public static void Main (string[] args)
		{
			Console.WriteLine ("Hello World!");

			if (false) {
				Console.WriteLine ("Generating Table Storage Sas");
				Console.WriteLine ("---------------------------");
				Console.WriteLine ("  (set `false` to for other types)");

				Table.Run ();

				//Require user input before closing the console window.
				Console.ReadLine();
				return; // xxxxxxxxxx END xxxxxxxxxxxx
			} else if (true) {
				Console.WriteLine ("Generating Queue Storage Sas");
				Console.WriteLine ("---------------------------");
				Console.WriteLine ("  (set `false` to for other types)");

				Queue.Run ();
				//Require user input before closing the console window.
				Console.ReadLine();
				return; // xxxxxxxxxx END xxxxxxxxxxxx
			}

			Console.WriteLine ("Generating Blog Storage Sas");
			Console.WriteLine ("---------------------------");
			Console.WriteLine ("  (set `false` to for other types)");



			//Parse the connection string and return a reference to the storage account.
			CloudStorageAccount storageAccount = CloudStorageAccount.Parse(StorageConnectionString);

			//Create the blob client object.
			CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();

			//Get a reference to a container to use for the sample code, and create it if it does not exist.
			CloudBlobContainer container = blobClient.GetContainerReference("teal-test");
			container.CreateIfNotExists();

			//Insert calls to the methods created below here...
			//Generate a SAS URI for the container, without a stored access policy.
			Console.WriteLine("Container SAS URI: " + GetContainerSasUri(container));
			Console.WriteLine();

			//Generate a SAS URI for a blob within the container, without a stored access policy.
			Console.WriteLine("Blob SAS URI: " + GetBlobSasUri(container));
			Console.WriteLine();

			//Create an access policy on the container, which may be optionally used to provide constraints for 
			//shared access signatures on the container and the blob.
			string sharedAccessPolicyName = "tutorialpolicy";
			CreateSharedAccessPolicy(blobClient, container, sharedAccessPolicyName);

			//Generate a SAS URI for the container, using a stored access policy to set constraints on the SAS.
			Console.WriteLine("Container SAS URI using stored access policy: " + GetContainerSasUriWithPolicy(container, sharedAccessPolicyName));
			Console.WriteLine();

			//Generate a SAS URI for a blob within the container, using a stored access policy to set constraints on the SAS.
			Console.WriteLine("Blob SAS URI using stored access policy: " + GetBlobSasUriWithPolicy(container, sharedAccessPolicyName));
			Console.WriteLine();

			//Require user input before closing the console window.
			Console.ReadLine();
		}

		static string GetContainerSasUri(CloudBlobContainer container)
		{
			//Set the expiry time and permissions for the container.
			//In this case no start time is specified, so the shared access signature becomes valid immediately.
			SharedAccessBlobPolicy sasConstraints = new SharedAccessBlobPolicy();
			sasConstraints.SharedAccessExpiryTime = DateTime.UtcNow.AddHours(4);
			sasConstraints.Permissions = SharedAccessBlobPermissions.Write | SharedAccessBlobPermissions.List | SharedAccessBlobPermissions.Read | SharedAccessBlobPermissions.Delete;

			//Generate the shared access signature on the container, setting the constraints directly on the signature.
			string sasContainerToken = container.GetSharedAccessSignature(sasConstraints);

			//Return the URI string for the container, including the SAS token.
			return container.Uri + sasContainerToken;
		}


		static string GetBlobSasUri(CloudBlobContainer container)
		{
			//Get a reference to a blob within the container.
			CloudBlockBlob blob = container.GetBlockBlobReference("xamarinblob.txt");

			//Upload text to the blob. If the blob does not yet exist, it will be created. 
			//If the blob does exist, its existing content will be overwritten.
			string blobContent = "This blob will be accessible to clients via a Shared Access Signature.";
			MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(blobContent));
			ms.Position = 0;
			using (ms)
			{
				blob.UploadFromStream(ms);
			}

			//Set the expiry time and permissions for the blob.
			//In this case the start time is specified as a few minutes in the past, to mitigate clock skew.
			//The shared access signature will be valid immediately.
			SharedAccessBlobPolicy sasConstraints = new SharedAccessBlobPolicy();
			sasConstraints.SharedAccessStartTime = DateTime.UtcNow.AddMinutes(-5);
			sasConstraints.SharedAccessExpiryTime = DateTime.UtcNow.AddHours(4);
			sasConstraints.Permissions = SharedAccessBlobPermissions.Read | SharedAccessBlobPermissions.Write;

			//Generate the shared access signature on the blob, setting the constraints directly on the signature.
			string sasBlobToken = blob.GetSharedAccessSignature(sasConstraints);

			//Return the URI string for the container, including the SAS token.
			return blob.Uri + sasBlobToken;
		}

		static void CreateSharedAccessPolicy(CloudBlobClient blobClient, CloudBlobContainer container, string policyName)
		{
			//Create a new stored access policy and define its constraints.
			SharedAccessBlobPolicy sharedPolicy = new SharedAccessBlobPolicy()
			{
				SharedAccessExpiryTime = DateTime.UtcNow.AddHours(10),
				Permissions = SharedAccessBlobPermissions.Read | SharedAccessBlobPermissions.Write | SharedAccessBlobPermissions.List
			};

			//Get the container's existing permissions.
			BlobContainerPermissions permissions = new BlobContainerPermissions();

			//Add the new policy to the container's permissions.
			permissions.SharedAccessPolicies.Clear();
			permissions.SharedAccessPolicies.Add(policyName, sharedPolicy);
			container.SetPermissions(permissions);
		}


		static string GetContainerSasUriWithPolicy(CloudBlobContainer container, string policyName)
		{
			//Generate the shared access signature on the container. In this case, all of the constraints for the 
			//shared access signature are specified on the stored access policy.
			string sasContainerToken = container.GetSharedAccessSignature(null, policyName);

			//Return the URI string for the container, including the SAS token.
			return container.Uri + sasContainerToken;
		}

		static string GetBlobSasUriWithPolicy(CloudBlobContainer container, string policyName)
		{
			//Get a reference to a blob within the container.
			CloudBlockBlob blob = container.GetBlockBlobReference("xamarinblobpolicy.txt");

			//Upload text to the blob. If the blob does not yet exist, it will be created. 
			//If the blob does exist, its existing content will be overwritten.
			string blobContent = "This blob will be accessible to clients via a shared access signature. " + 
				"A stored access policy defines the constraints for the signature.";
			MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(blobContent));
			ms.Position = 0;
			using (ms)
			{
				blob.UploadFromStream(ms);
			}

			//Generate the shared access signature on the blob.
			string sasBlobToken = blob.GetSharedAccessSignature(null, policyName);

			//Return the URI string for the container, including the SAS token.
			return blob.Uri + sasBlobToken;
		}
	}
}
