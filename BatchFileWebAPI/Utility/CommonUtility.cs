using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace BatchFileWebAPI.Utility
{
    public class CommonUtility
    {
        public void AddText(FileStream fs, string value)
        {
            byte[] info = new System.Text.UTF8Encoding(true).GetBytes(value);
            fs.Write(info, 0, info.Length);
        }

        public async Task<bool> CreateContainer(string AccountName, string AccountKey, string ContainerName)
        {
            string UserConnectionString = string.Format("DefaultEndpointsProtocol=https;AccountName={0};AccountKey={1}", AccountName, AccountKey);
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(UserConnectionString);
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
            CloudBlobContainer container = blobClient.GetContainerReference(ContainerName.ToLower());
            var containerData = await container.CreateIfNotExistsAsync();
            if (containerData)
            {
                await container.SetPermissionsAsync(new BlobContainerPermissions { PublicAccess = BlobContainerPublicAccessType.Blob });
            }
            return containerData;
        }
        public string GetMD5HashFromStream(byte[] stream)
        {
            // Validate MD5 Value
            var md5Check = System.Security.Cryptography.MD5.Create();
            md5Check.TransformBlock(stream, 0, stream.Length, null, 0);
            md5Check.TransformFinalBlock(new byte[0], 0, 0);

            // Get Hash Value
            byte[] hashBytes = md5Check.Hash;
            return Convert.ToBase64String(hashBytes);
        }
    }
}
