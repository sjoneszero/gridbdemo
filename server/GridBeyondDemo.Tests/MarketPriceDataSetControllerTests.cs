using GridBeyond.Database;
using GridBeyond.Database.Models;
using GridBeyondDemo.Api.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Xunit;

namespace GridBeyondDemo.Tests
{
    /// <summary>
    /// Unit tests for MarketPriceDataSetController
    /// </summary>
    public class MarketPriceDataSetControllerTests
    {
        public MarketPriceDataSetControllerTests() { }

        /// <summary>
        /// Test to ensure all datasets can be returned
        /// </summary>
        [Fact]
        public void Get_ShouldReturnMarketDataSets()
        {

            var testData = new List<MarketPriceDataSet>
            {
                new MarketPriceDataSet
                {
                    Id = 1,
                    Description = "test",
                    CreationDate = DateTime.Now,
                    MarketPrices = null
                },
                new MarketPriceDataSet
                {
                    Id = 2,
                    CreationDate = DateTime.Now,
                    MarketPrices = null
                }
            };
            using (var databaseContext = GetInMemoryDatabaseContext())
            {
                databaseContext.Set<MarketPriceDataSet>().AddRange(testData);
                databaseContext.SaveChanges();

                var testController = new MarketPriceDataSetController(databaseContext);
                var response = testController.GetAnalysis() as OkObjectResult;

                Assert.Equal(200, response.StatusCode);
                var returnedObjects = response.Value as List<MarketPriceDataSet>;
                Assert.Equal(2, returnedObjects.Count);
            }
        }

        /// <summary>
        /// Test to ensure a single dataset can be requested containing the market price data and calculated values
        /// </summary>
        [Fact]
        public void GetAnalysis_ShouldReturnMarketDataSetWithCalculatedValues()
        {
            var testData = new List<MarketPriceDataSet>
            {
               new MarketPriceDataSet
                  {
                      Id = 1,
                      Description = "test",
                      CreationDate = DateTime.Today,
                      MarketPrices = new List<MarketPrice>()
                      {
                        new MarketPrice {
                        Id = 1,
                        TimeStamp = Convert.ToDateTime("2021-08-27 20:30:00"),
                        Price = 100.00M
                        },
                        new MarketPrice {
                        Id = 2,
                        TimeStamp = Convert.ToDateTime("2021-08-27 21:00:00"),
                        Price = 200.00M
                        },
                        new MarketPrice {
                        Id = 3,
                        TimeStamp = Convert.ToDateTime("2021-08-27 21:30:00"),
                        Price = 300.00M
                        }
                      }
                  },
              new MarketPriceDataSet
              {
                  Id = 2,
                  CreationDate = DateTime.Now,
                  MarketPrices = null
              }
            };

            using (var databaseContext = GetInMemoryDatabaseContext())
            {
                databaseContext.Set<MarketPriceDataSet>().AddRange(testData);
                databaseContext.SaveChanges();

                var testController = new MarketPriceDataSetController(databaseContext);
                var response = testController.GetAnalysis(1) as OkObjectResult;

                Assert.Equal(200, response.StatusCode);
                var returnedObject = response.Value as MarketPriceDataSet;
                Assert.Equal(3, returnedObject.MarketPrices.Count);
                Assert.Equal(300, returnedObject.Max.Price);
                Assert.Equal(100, returnedObject.Min.Price);
                Assert.Equal(200, returnedObject.Average.Price);
                Assert.Equal(Convert.ToDateTime("2021-08-27 21:00:00"), returnedObject.MostExpensiveHourWindow.TimeStamp);
            }
        }

        /// <summary>
        /// Test to ensure the correct error code is returned if the dataset requested does not exist
        /// </summary>
        [Fact]
        public void GetAnalysis_ShouldReturnErrorIfRequestedDatasetDoesNotExist()
        {

            using (var databaseContext = GetInMemoryDatabaseContext())
            {
                var testController = new MarketPriceDataSetController(databaseContext);
                var response = testController.GetAnalysis(1);
                Assert.IsType<BadRequestResult>(response);
            }
        }

        /// <summary>
        /// Test to ensure CSV file can be uploaded and saved as a new dataset the database
        /// </summary>
        [Fact]
        public void Post_ShouldUploadCsvDataToDatabase()
        {
            string csvData =
                "Date,Market Price EX1" + Environment.NewLine +
                "10/01/2017,50.29000092" + Environment.NewLine +
                "10/01/2017,50.29000092" + Environment.NewLine +
                "10/01/2017 00:30,50" + Environment.NewLine +
                "10/01/2017 01:00,50"; 

            // convert string to stream
            var stream = new MemoryStream(Encoding.ASCII.GetBytes(csvData));
            var csvFile = new FormFile(stream, 0, stream.Length, null, "test.csv"); 

            using (var databaseContext = GetInMemoryDatabaseContext())
            {
                var testController = new MarketPriceDataSetController(databaseContext);
                var postResponse = testController.Post(csvFile, "testDescription") as CreatedResult;
                Assert.IsType<CreatedResult>(postResponse);
                var newId = postResponse.Value as int?;

                var getResponse = testController.GetAnalysis((int)newId) as OkObjectResult;
                Assert.Equal(200, getResponse.StatusCode);
                var returnedObject = getResponse.Value as MarketPriceDataSet;
                Assert.Equal(4, returnedObject.MarketPrices.Count);
                Assert.Equal("testDescription", returnedObject.Description); 
            }
        }

        /// <summary>
        /// Test to ensure the correct error code is returned if the CSV file is malformed
        /// </summary>
        [Fact]
        public void Post_ShouldReturnErrorIfCsvDataIsMalformed()
        {
            string csvData =
                "Date,Market Price EX1" + Environment.NewLine +
                "10/01/2017,50.29000092" + Environment.NewLine +
                "10/01/2017,50.29000092" + Environment.NewLine +
                "not_a_datetime_string,50" + Environment.NewLine +
                "10/01/2017 01:00,50"; 

            // convert string to stream
            var stream = new MemoryStream(Encoding.ASCII.GetBytes(csvData));
            var csvFile = new FormFile(stream, 0, stream.Length, null, "test.csv");

            var testController = new MarketPriceDataSetController(null);
            var postResponse = testController.Post(csvFile, "testDescription");
            Assert.IsType<BadRequestResult>(postResponse);
        }
        /// <summary>
        /// Test to ensure the correct error code is returned if the file upload is not in CSV format
        /// </summary>
        [Fact]
        public void Post_ShouldReturnErrorIfFileisNotCsv()
        {
            string csvData =
                "Date,Market Price EX1" + Environment.NewLine +
                "10/01/2017,50.29000092" + Environment.NewLine +
                "10/01/2017,50.29000092" + Environment.NewLine +
                "not_a_datetime_string,50" + Environment.NewLine +
                "10/01/2017 01:00,50";

            // convert string to stream
            var stream = new MemoryStream(Encoding.ASCII.GetBytes(csvData));
            var csvFile = new FormFile(stream, 0, stream.Length, null, "test.doc");

            var testController = new MarketPriceDataSetController(null);
            var postResponse = testController.Post(csvFile, "testDescription");
            Assert.IsType<BadRequestResult>(postResponse);
        }

        private static DatabaseContext GetInMemoryDatabaseContext()
        {
            var options = new DbContextOptionsBuilder<DatabaseContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            return new DatabaseContext(options);
        }
    }
}
