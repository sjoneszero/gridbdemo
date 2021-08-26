
using GridBeyondDatabase.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace GridBeyondDatabase.Models
{
    /// <summary>
    /// Database model to represent a single set of Market Price data
    /// </summary>
    public class MarketPriceDataSet : IDbModel
    {
        /// <summary>
        /// Id of the dataset
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// A string indentifier for the dataset to distinguish the dataset from others
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// The date the dataset was created
        /// </summary>
        public DateTime CreationDate { get; set; }
        /// <summary>
        /// The collection of child market prices
        /// </summary>
        public virtual ICollection<MarketPrice> MarketPrices { get; set; }
        /// <summary>
        /// The max market price for the current dataset
        /// </summary>
        [NotMapped]
        public MarketPrice Min { get; set; }
        /// <summary>
        /// The min market price for the current dataset
        /// </summary>
        [NotMapped]
        public MarketPrice Max { get; set; }
        /// <summary>
        /// The average market price for the current dataset
        /// </summary>
        [NotMapped]
        public MarketPrice Average { get; set; }
        /// <summary>
        /// The most expensive hour window for the current dataset
        /// </summary
        [NotMapped]
        public MarketPrice MostExpensiveHourWindow { get; set; }

    }
}
