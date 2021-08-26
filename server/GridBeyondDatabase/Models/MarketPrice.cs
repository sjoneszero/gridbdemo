using GridBeyondDatabase.Interfaces;
using System;

namespace GridBeyondDatabase.Models
{
    /// <summary>
    /// Database model to represent a the market price at a given point in time
    /// </summary>
    public class MarketPrice : IDbModel
    {
        /// <summary>
        /// The Id of the market price
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// The point in time when the price was recorded
        /// </summary>
        public DateTime TimeStamp { get; set; }
        /// <summary>
        /// The price recorded
        /// </summary>
        public decimal Price { get; set; }
        /// <summary>
        /// The parent dataset id
        /// </summary>
        public int MarketPriceDataSetId { get; set; }
        /// <summary>
        /// The parent dataset object
        /// </summary>
        public virtual MarketPriceDataSet MarketPriceDataSet { get; set; }

    }
}