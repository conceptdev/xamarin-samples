using System;

using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Queue;
using System.Diagnostics;

namespace GenerateSharedAccessSignatures
{
	// http://azure.microsoft.com/en-us/documentation/articles/storage-dotnet-how-to-use-queues/#insert-a-message-into-a-queue 

	static class Queue
	{
		public static void Run () {
			Console.WriteLine ("Hello DoTable!");

			DoQueue ();
		}

		// TODO: get this info from the azure portal
		static string StorageConnectionString = "DefaultEndpointsProtocol=https;AccountName=FROM_PORTAL;AccountKey=FROM_PORTAL";

		static void DoQueue ()
		{			// Retrieve storage account from connection string
			CloudStorageAccount storageAccount = CloudStorageAccount.Parse(StorageConnectionString);

			// Create the queue client
			CloudQueueClient queueClient = storageAccount.CreateCloudQueueClient();

			// Retrieve a reference to a queue
			CloudQueue queue = queueClient.GetQueueReference("xqueue");

			// Create the queue if it doesn't already exist
			queue.CreateIfNotExists();

			// Create a message and add it to the queue.
			CloudQueueMessage message = new CloudQueueMessage("Hello, World " + DateTime.Now.Second);
			queue.AddMessage(message);

			message = new CloudQueueMessage("Ground Control " + DateTime.Now.Second);
			queue.AddMessage(message);

			message = new CloudQueueMessage("3-2-1 contact " + DateTime.Now.Second);
			queue.AddMessage(message);

			message = new CloudQueueMessage("Blast off! " + DateTime.Now.Second);
			queue.AddMessage(message);

			// ----------

			// Retrieve storage account from connection string

			// Create the queue client

			// Retrieve a reference to a queue

			// Get the next message
			CloudQueueMessage retrievedMessage = queue.GetMessage();

			//Process the message in less than 30 seconds, and then delete the message
			queue.DeleteMessage(retrievedMessage);


			// for client app

			var sas = RequestSasToken (queue);
			Debug.WriteLine("Sas : " + sas);
			Console.WriteLine("Sas : " + sas);
		}

		static string RequestSasToken(CloudQueue queue)
		{
			// Omitting any authentication code since this is beyond the scope of
			// this sample code

			// creating a shared access policy that expires in 1 day.
			// No start time is specified, which means that the token is valid immediately.
			// The policy specifies full permissions.
			SharedAccessQueuePolicy policy = new SharedAccessQueuePolicy()
			{
				SharedAccessStartTime = DateTime.UtcNow.AddMinutes(-15),
				SharedAccessExpiryTime = DateTime.UtcNow.AddDays(1.0),
				Permissions = SharedAccessQueuePermissions.Add |
					SharedAccessQueuePermissions.Read |
					SharedAccessQueuePermissions.Update |
					SharedAccessQueuePermissions.ProcessMessages
			};

			// Generate the SAS token. No access policy identifier is used which
			// makes it a non-revocable token
			// limiting the table SAS access to only the request customer's id
			string sasToken = queue.GetSharedAccessSignature(policy);

			return sasToken;
		}
	}
}

