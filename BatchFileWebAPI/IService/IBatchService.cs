using BatchFileWebAPI.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BatchFileWebAPI.IService
{
    public interface IBatchService
    {
        BatchFile AddBatch(BatchFile batches);
        Task<BatchFile> GetBatchById(Guid batchid);
        Task<bool> AddFiles(string AccountName, string AccountKey, Guid ContainerName, string filename, int filesize, string mimeType);
        //Task<bool> CreateContainer(string AccountName, string AccountKey, string ContainerName);
    }
}
