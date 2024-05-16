using Microsoft.Azure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.IO;
using System.Text;

namespace Common.Helpers
{

    public class BlobHelper
    {
        CloudStorageAccount storageAccount = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("DataConnectionString"));
        CloudBlobClient blobStorage;
        public BlobHelper()
        {
            blobStorage = storageAccount.CreateCloudBlobClient();
        }
        public string Download(string containerName, string blobName)
        {
            CloudBlobContainer container = blobStorage.GetContainerReference(containerName);
            CloudBlockBlob blob = container.GetBlockBlobReference(blobName);
            string text = "";
            using (var stream = new MemoryStream())
            {
                blob.DownloadToStream(stream);
                text = Encoding.Default.GetString(stream.ToArray());
            }
            return text;
        }
        public string Upload(string image, string containerName, string blobName)
        {
            CloudBlobContainer container = blobStorage.GetContainerReference(containerName);
            CloudBlockBlob blob = container.GetBlockBlobReference(blobName);
            using (var stream = new MemoryStream(Encoding.Default.GetBytes(image)))
            {
                blob.UploadFromStream(stream);
                return blob.Uri.ToString();
            }
        }
    }
}
