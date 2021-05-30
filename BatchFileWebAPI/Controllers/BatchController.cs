using BatchFileWebAPI.IService;
using BatchFileWebAPI.Models;
using BatchFileWebAPI.Service;
using BatchFileWebAPI.Utility;
using BatchFileWebAPI.Validation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace BatchFileWebAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class BatchController : ControllerBase
    {
        public int XContentSize { get; set; }
        private readonly IConfiguration configuration;
        private readonly IBatchService batchService;
        private readonly ILoggerService logger;
        ModelValidation modelValidation = new ModelValidation();
        CommonUtility commonUtility = new CommonUtility();
        public BatchController(IBatchService batch, ILoggerService loggerService, IConfiguration configurationService)
        {
            batchService = batch;
            logger = loggerService;
            configuration = configurationService;
        }
        [HttpPost]
        public IActionResult CreateBatch([FromBody] BatchFile batch)
        {
            try
            {
                if (batch.BusinessUnit == null || batch.BusinessUnit == "")
                {
                    logger.LogInfo("business unit is empty");
                    return BadRequest(modelValidation.ValidateModel("400", "business unit is empty", "business unit should not be empty"));
                }
                if (batch.BusinessUnit != null || batch.BusinessUnit != "")
                {
                    if (configuration["businessUnit"].ToString() != batch.BusinessUnit.ToString())
                    {
                        logger.LogInfo("business unit is not correct");
                        return BadRequest(modelValidation.ValidateModel("400", "business unit incorrect", "incorrect business unit is provided"));
                    }
                }
                foreach (var attribute in batch.Attributes)
                {
                    if (attribute.Key == null || attribute.Key == "")
                    {
                        logger.LogInfo("key is empty");
                        return BadRequest(modelValidation.ValidateModel("400", "key is empty", "key should not be empty"));
                    }
                    if (attribute.Value == null || attribute.Value == "")
                    {
                        logger.LogInfo("value is empty");
                        return BadRequest(modelValidation.ValidateModel("400", "value is empty", "value should not be empty"));
                    }
                }
                
                BatchFile newBatchObj = batchService.AddBatch(batch);
                if (newBatchObj != null)
                {
                    var containerData = commonUtility.CreateContainer(configuration["storageAccountName"].ToString(), configuration["storageKey"].ToString(), newBatchObj.BatchId.ToString());
                }
                var settings = new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore,
                    MissingMemberHandling = MissingMemberHandling.Ignore,
                    DefaultValueHandling=DefaultValueHandling.Ignore
                };
                BatchFile batchFileGuid = new BatchFile
                {
                    BatchId = newBatchObj.BatchId
                };
                string responseBatchFile = JsonConvert.SerializeObject(batchFileGuid,Formatting.Indented,settings);
                
                return Ok(responseBatchFile);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message.ToString());
                return BadRequest(ex.Message.ToString());
            }

        }
        [HttpGet("{batchid}")]
        public async Task<ActionResult<BatchFile>> GetBatch(Guid batchid)
        {
            try
            {
                var batchFile = await batchService.GetBatchById(batchid);
                if (batchFile == null)
                {
                    return BadRequest(modelValidation.ValidateModel("400", "Batch id incorrect", "Incorrect batch id is provided"));
                }
                else
                {
                    return batchFile;
                }
            }
            catch(Exception ex)
            {
                logger.LogError(ex.Message.ToString());
                return BadRequest(ex.Message.ToString());
            }
        }
        
        [HttpPost("{batchId}/{filename}")]
        public async Task<IActionResult> AddFile(Guid batchId,string filename)
        {
            try
            {
                var batchFile = await batchService.GetBatchById(batchId);
                if (batchFile == null)
                {
                    return BadRequest(modelValidation.ValidateModel("400", "Batch id incorrect", "Incorrect batch id is provided"));
                }
                else
                {
                    var mimeType = HttpContext.Request.Headers["X-MIME-Type"];
                    var contentSize = HttpContext.Request.Headers["X-Content-Size"];
                    var batchfile = await batchService.AddFiles(configuration["storageAccountName"].ToString(), configuration["storageKey"].ToString(), batchId, filename, Convert.ToInt32(contentSize), mimeType);
                    if (batchfile == true)
                    {
                        return Ok($"Created");
                    }
                    else
                    {
                        return BadRequest("either file name is already exists or incorrect mime type is provided");
                    }
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message.ToString());
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }
    }
    
}
