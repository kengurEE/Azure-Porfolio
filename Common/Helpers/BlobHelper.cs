using Microsoft.Azure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.IO;
using System.Text;
using System.Web;

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
            CloudBlobContainer container = blobStorage.GetContainerReference("images");
            CloudBlockBlob blob = container.GetBlockBlobReference(blobName);
            string text = "";
            using (var stream = new MemoryStream())
            {
                blob.DownloadToStream(stream);
                text = Encoding.Default.GetString(stream.ToArray());
            }
            return text;
        }
        public string Upload(HttpPostedFileBase file)
        {
            CloudBlobContainer container = blobStorage.GetContainerReference("images");
            CloudBlockBlob blob = container.GetBlockBlobReference(Guid.NewGuid().ToString() + file.FileName.ToLower());
            blob.Properties.ContentType = file.ContentType;
            blob.UploadFromStream(file.InputStream);

            return blob.Uri.ToString();
        }
    }
}
