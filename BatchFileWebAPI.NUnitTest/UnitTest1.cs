using BatchFileWebAPI.Controllers;
using BatchFileWebAPI.IService;
using BatchFileWebAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Moq;
using NUnit.Framework;
using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.SqlServer;
using BatchFileWebAPI.Service;
using BatchFileWebAPI.Data;

namespace BatchFileWebAPI.NUnitTest
{
    [TestFixture]
    public class BatchFileWebAPITests
    {
        protected readonly Mock<BatchFileDBContext> batchFileDBContext;
        //protected readonly Mock<IConfiguration> configuration;
        //protected readonly Mock<IBatchService> iBatchService;
        //protected Mock<ILoggerService> logger;
        //protected readonly BatchController batchController;
        protected readonly BatchService batchService;
        Guid guid = new Guid();

        public BatchFileWebAPITests()
        {
            //configuration = new Mock<IConfiguration>();
            //iBatchService = new Mock<IBatchService>();
            //logger = new Mock<ILoggerService>();
            //batchController = new BatchController(iBatchService.Object, logger.Object, configuration.Object);
            batchService = new BatchService(batchFileDBContext.Object);
        }
        [SetUp]
        public void Setup()
        {
              

        }

        [Test]
        public async Task GetBatchFileByGUID()
        {
            guid = new Guid("f010c170-852f-4926-20f6-08d91c38173c");
            var batchObj = new BatchFile
            {
                BatchId = guid
            };
           // iBatchService.Setup(x => x.GetBatchById(guid)).ReturnsAsync(batchObj);
            //var batchResult = await batchController.GetBatch(guid);
            var batchResult = await batchService.GetBatchById(guid);
            Assert.AreEqual(guid, batchResult.BatchId);
        }
    }
}