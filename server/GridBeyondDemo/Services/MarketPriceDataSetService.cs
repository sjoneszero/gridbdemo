using GridBeyond.Database;
using GridBeyond.Database.Models;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace GridBeyondDemo.Api.Services
{
    public sealed class MarketPriceDataSetService : BaseDbService<MarketPriceDataSet>
    {

        public MarketPriceDataSetService(DatabaseContext databaseContext) : base(databaseContext)
        {
        }
        /// <summary>
        /// Override methos for this particular entity
        /// Gets the MarketPriceDataSet that matches a predicate and orders the MarketPrice list by Timestamp
        /// </summary>
        /// <param name="predicate">the predicate to match</param>
        /// <param name="includedPaths">string of included paths (i.e. child entities) to return</param>
        /// <param name="readOnly">Whether or not to track the retrieved entity</param>
        /// <returns>MarketPriceDataSet object</returns>
        public override MarketPriceDataSet GetFirst(Expression<Func<MarketPriceDataSet, bool>> predicate = null, string includedPaths = null, bool readOnly = false)
        {
            var dataset =  base.GetFirst(predicate, includedPaths, readOnly);
           dataset.MarketPrices = dataset.MarketPrices.OrderBy(p => p.TimeStamp).ToList(); 
           return dataset; 
        }
    }
}
