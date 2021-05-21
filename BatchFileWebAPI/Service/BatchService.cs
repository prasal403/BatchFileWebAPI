using BatchFileWebAPI.Data;
using BatchFileWebAPI.IService;
using BatchFileWebAPI.Models;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Text;

namespace BatchFileWebAPI.Service
{
    public class BatchService : IBatchService
    {
        private readonly BatchFileDBContext batchDBContext;
        public BatchService(BatchFileDBContext _db)
        {
            batchDBContext = _db;
        }
        public BatchFile AddBatch(BatchFile batch)
        {
            batch.BatchPublishedDate = DateTime.Now;
            batch.Status = "Complete";
            batchDBContext.Batches.Add(batch);
            batchDBContext.SaveChanges();
            return batch;
        }
        public async Task<BatchFile> GetBatchById(Guid batchid)
        {
            var bobj=await batchDBContext.Batches.Include(b => b.AccessControl).Include(c => c.Attributes).Include(d=>d.BatchFileMetaData).FirstOrDefaultAsync(p=>p.BatchId==batchid);
            return bobj;
        }
        public async Task<bool> AddFiles(string AccountName, string AccountKey, Guid ContainerName, string filename, int filesize, string mimeType)
        {
            string blockHash;
            byte[] hashCode;
            string UserConnectionString = string.Format("DefaultEndpointsProtocol=https;AccountName={0};AccountKey={1}", AccountName, AccountKey);
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(UserConnectionString);
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
            CloudBlobContainer container = blobClient.GetContainerReference(ContainerName.ToString());
            var containerData = await container.CreateIfNotExistsAsync();
            if (containerData)
            {
                await container.SetPermissionsAsync(new BlobContainerPermissions { PublicAccess = BlobContainerPublicAccessType.Blob });
            }
            var checkIfExists = container.GetBlockBlobReference(ContainerName + "_" + filename).ExistsAsync();
            if (checkIfExists.Result == true)
            {
                return false;
            }
            string contentType;
            var provider = new FileExtensionContentTypeProvider();
            if (!provider.TryGetContentType(filename, out contentType))
            {
                contentType = "application/octet-stream";
            }
            if (mimeType != null)
            {
                if (mimeType != contentType)
                {
                    return false;
                }
                else
                {
                    contentType = mimeType;
                }
            }
            CloudBlockBlob blockBlob = container.GetBlockBlobReference(ContainerName + "_" + filename);
            string rootpath = AppContext.BaseDirectory;
            if (rootpath.Contains("bin"))
            {
                rootpath = rootpath.Substring(0, rootpath.IndexOf("bin"));
            }
            string path = rootpath + @"\BatchFiles\";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            string filePath = path + ContainerName + "_" + filename;
            using (FileStream fs = System.IO.File.Create(filePath))
            { 
                AddText(fs, "foo");
                AddText(fs, "bar\tbaz");
                hashCode = MD5.Create().ComputeHash(fs);
            }
            using (FileStream fileStream =
                   new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                int blockSize = filesize * 1024;
                long fileSize = fileStream.Length;
                int blockCount = (int)((float)fileSize / (float)blockSize) + 1;
                List<string> blockIDs = new List<string>();
                int blockNumber = 0;

                int bytesRead = 0;
                long bytesLeft = fileSize;
                while (bytesLeft > 0)
                {
                    blockNumber++;
                    int bytesToRead;
                    if (bytesLeft >= blockSize)
                    {
                        bytesToRead = blockSize;
                    }
                    else
                    {
                        bytesToRead = (int)bytesLeft;
                    }
                    string blockId =
                      Convert.ToBase64String(ASCIIEncoding.ASCII.GetBytes(string.Format("BlockId{0}",
                        blockNumber.ToString("0000000"))));
                    blockIDs.Add(blockId);
                    byte[] bytes = new byte[bytesToRead];
                    fileStream.Read(bytes, 0, bytesToRead);
                    blockHash = GetMD5HashFromStream(bytes);
                    await blockBlob.PutBlockAsync(blockId, new MemoryStream(bytes), blockHash);
                    bytesRead += bytesToRead;
                    bytesLeft -= bytesToRead;
                }

                await blockBlob.PutBlockListAsync(blockIDs);
                BatchFilesMetaData batchFilesMetaData = new BatchFilesMetaData();
                batchFilesMetaData.BatchId = ContainerName;
                batchFilesMetaData.FileName = ContainerName + "_" + filename;
                batchFilesMetaData.MIMEType = contentType;
                batchFilesMetaData.FileSize = filesize;
                batchFilesMetaData.Hash = BitConverter.ToString(hashCode);
                batchDBContext.BatchFilesMetaData.Add(batchFilesMetaData);
                batchDBContext.SaveChanges();
                return true;
            }
            
        }
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
        public static string GetMD5HashFromStream(byte[] stream)
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
