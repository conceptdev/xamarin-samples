using System;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using System.Diagnostics;

namespace GenerateSharedAccessSignatures
{
	public static class Table
	{
		public static void Run () {
			Console.WriteLine ("Hello DoTable!");

			DoTable ();
		}

		// TODO: get this info from the azure portal
		static string StorageConnectionString = "DefaultEndpointsProtocol=https;AccountName=FROM_PORTAL;AccountKey=FROM_PORTAL";

		static void DoTable ()
		{
			var part = "xander";
			var row = DateTime.Now.Ticks.ToString ();

			// Retrieve storage account information from connection string
			// How to create a storage connection string - http://msdn.microsoft.com/en-us/library/azure/ee758697.aspx
			CloudStorageAccount storageAccount = CreateStorageAccountFromConnectionString(StorageConnectionString);
			CloudTableClient client = storageAccount.CreateCloudTableClient();
			CloudTable table = client.GetTableReference("demotable1");

			try
			{
				table.CreateIfNotExists();

				DynamicTableEntity ent = new DynamicTableEntity() { PartitionKey = part, RowKey = row };
				ent.Properties.Add("EncryptedProp1", new EntityProperty(string.Empty));
				ent.Properties.Add("EncryptedProp2", new EntityProperty("bar"));
				ent.Properties.Add("NotEncryptedProp", new EntityProperty(1234));

				// Insert Entity
				Debug.WriteLine("Inserting the entity.");
				table.Execute(TableOperation.Insert(ent));

				// Retrieve Entity
				Debug.WriteLine("Retrieving the entity.");
				TableOperation operation = TableOperation.Retrieve(ent.PartitionKey, ent.RowKey);
				TableResult result = table.Execute(operation);
				Debug.WriteLine("tag: " + result.Etag);

				var sas = RequestSasToken (table, part);
				Debug.WriteLine("Sas : " + sas);
				Console.WriteLine("Sas : " + sas);

			}
			catch (Exception ex)
			{
				Debug.WriteLine ("error occurred " + ex);
			}
		}

		private static CloudStorageAccount CreateStorageAccountFromConnectionString(string storageConnectionString)
		{
			CloudStorageAccount storageAccount;
			try
			{
				storageAccount = CloudStorageAccount.Parse(storageConnectionString);
			}
			catch (FormatException)
			{
				Debug.WriteLine("Invalid storage account information provided. Please confirm the AccountName and AccountKey are valid in the app.config file - then restart the sample.");
				throw;
			}
			catch (ArgumentException)
			{
				Debug.WriteLine("Invalid storage account information provided. Please confirm the AccountName and AccountKey are valid in the app.config file - then restart the sample.");
				throw;
			}

			return storageAccount;
		}

		static string RequestSasToken(CloudTable table, string customerId)
		{
			// Omitting any authentication code since this is beyond the scope of
			// this sample code

			// creating a shared access policy that expires in 1 day.
			// No start time is specified, which means that the token is valid immediately.
			// The policy specifies full permissions.
			SharedAccessTablePolicy policy = new SharedAccessTablePolicy()
			{
				SharedAccessStartTime = DateTime.UtcNow.AddMinutes(-15),
				SharedAccessExpiryTime = DateTime.UtcNow.AddDays(1.0),
				Permissions = SharedAccessTablePermissions.Add
					| SharedAccessTablePermissions.Query
					| SharedAccessTablePermissions.Update
					| SharedAccessTablePermissions.Delete
			};

			// Generate the SAS token. No access policy identifier is used which
			// makes it a non-revocable token
			// limiting the table SAS access to only the request customer's id
			string sasToken = table.GetSharedAccessSignature(
				policy   /* access policy */,
				null     /* access policy identifier */,
				customerId /* start partition key */,
				null     /* start row key */,
				customerId /* end partition key */,
				null     /* end row key */);

			return sasToken;
		}
	}
}

