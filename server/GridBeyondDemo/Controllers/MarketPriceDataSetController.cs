using GridBeyondDatabase;
using GridBeyondDatabase.Models;
using GridBeyondDemo.Enums;
using GridBeyondDemo.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace GridBeyondDemo.Controllers
{
    /// <summary>
    /// Returns market price data and allows creating and deletion of such data
    /// </summary>
    public class MarketPriceDataSetController : ControllerBase
    {
        private readonly MarketPriceDataSetService DbService = new MarketPriceDataSetService();
        private const string CLIENT_BASE_URL = "http://localhost:4200/datasets/"; 


        public MarketPriceDataSetController() 
        {
        }
        /// <summary>
        /// Returns top level information of all existing market price datasets
        /// </summary>
        /// <returns>
        /// 200 OK including an array of market datasets
        /// 500 Internal Server Error
        /// </returns>
        [HttpGet]
        [Route("api/marketprices")]
        public IActionResult Get()
        {
            return Ok(DbService.Get(null, null, true));
        }
        /// <summary>
        /// Returns analysis data for a specifc market price dataset
        /// </summary>
        /// <param name="id">The dataset Id</param>
        /// <returns>
        /// 200 OK with an object representing analysis data
        /// 500 Internal Server Error
        /// </returns>
        [HttpGet]
        [Route("api/marketprices/{id}/analysis")]
        public IActionResult GetAnalysis([FromRoute] int id)
        {
            var childEntitiesToInclude = "MarketPrices";
            var dataset = DbService.GetFirst(m => m.Id == id, childEntitiesToInclude, true);

            dataset.Max = CalculateValuesForAnalysis(dataset.MarketPrices, DatasetCalculation.Max);
            dataset.Min = CalculateValuesForAnalysis(dataset.MarketPrices, DatasetCalculation.Min);
            dataset.Average = CalculateValuesForAnalysis(dataset.MarketPrices, DatasetCalculation.Average);
            dataset.MostExpensiveHourWindow = CalculateValuesForAnalysis(dataset.MarketPrices, DatasetCalculation.MostExpensiveHourWindow);
            return Ok(dataset);
        }
        /// <summary>
        /// Receives market price data in CSV format and inserts a new dataset into the database
        /// </summary>
        /// <param name="file">Form data representing the CSV file</param>
        /// <param name="description">Form data representing the description assigned to the dataset</param>
        /// <returns>
        /// 201 Created including Analysis URL 
        /// 400 Bad Request if the data is malformed
        /// 500 Internal Server Error
        /// </returns>
        [HttpPost]
        [Route("api/marketprices")]
        public IActionResult Post([FromForm] IFormFile file, [FromForm] string description)
        {
            if (!Path.GetExtension(file.FileName).Contains("csv", StringComparison.OrdinalIgnoreCase))
            {
                return BadRequest();
            }
            var newEntries = new List<MarketPrice>();
            using (var reader = new StreamReader(file.OpenReadStream()))
            {
                reader.ReadLine(); // Read CSV headers first 
                string[] lineValue;
                DateTime dateValue;
                decimal priceValue;
                while (reader.Peek() >= 0)
                {
                    lineValue = reader.ReadLine().Split(',');
                    var allowedformats = new[] { "dd/MM/yyyy", "dd/MM/yyyy HH:mm" };

                    if (DateTime.TryParseExact(lineValue[0], allowedformats, CultureInfo.InvariantCulture, DateTimeStyles.None, out dateValue) &&
                        decimal.TryParse(lineValue[1], out priceValue))
                    {
                        newEntries.Add(
                            new MarketPrice
                            {
                                TimeStamp = dateValue,
                                Price = priceValue
                            });
                    }
                    else return BadRequest();
                }
            }
            var dataSet = new MarketPriceDataSet
            {
                Description = !string.IsNullOrEmpty(description) ? description : Guid.NewGuid().ToString(),
                CreationDate = DateTime.Now,
                MarketPrices = newEntries
            };
            DbService.Insert(dataSet);
            DbService.Commit();
            return Created(new Uri(CLIENT_BASE_URL + dataSet.Id + "/analysis"), dataSet);
        }

        /// <summary>
        /// Deletes a dataset
        /// </summary>
        /// <param name="id">The dataset Id</param>
        /// <returns>
        /// 200 OK
        /// 500 Internal Server Error
        /// </returns>
        [HttpDelete]
        [Route("api/marketprices/{id}")]
        public IActionResult Delete([FromRoute] int id)
        {
            var dataset = DbService.Get(m => m.Id == id, null, true).FirstOrDefault();
            DbService.Delete(dataset);
            DbService.Commit(); 
            return Ok();
        }


        /// <summary>
        /// Calculates analysis values
        /// </summary>
        /// <param name="datasetValues">The market price data dataset</param>
        /// <param name="calculation">The specific calcuoation to perform</param>
        /// <returns></returns>
        private MarketPrice CalculateValuesForAnalysis(IEnumerable<MarketPrice> datasetValues, DatasetCalculation calculation)
        {
            switch (calculation)
            {
                case DatasetCalculation.Min:
                    return datasetValues.OrderBy(v => v.Price).FirstOrDefault();

                case DatasetCalculation.Max:
                    return datasetValues.OrderByDescending(v => v.Price).FirstOrDefault();

                case DatasetCalculation.Average:
                    return new MarketPrice
                    {
                        Price = Math.Round(datasetValues.Select(v => v.Price).Average(), 2)
                    };
                case DatasetCalculation.MostExpensiveHourWindow:
                    foreach (var marketPrice in datasetValues.OrderBy(v => v.TimeStamp))
                    {

                    }
                    return datasetValues.OrderByDescending(v => v.Price).FirstOrDefault();
                default:
                    return null;
            }

        }

    }

}

