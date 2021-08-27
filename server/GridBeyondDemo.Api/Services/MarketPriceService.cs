using GridBeyond.Database;
using GridBeyond.Database.Models;

namespace GridBeyondDemo.Api.Services
{
    public sealed class MarketPriceService : BaseDbService<MarketPrice>
    {

        public MarketPriceService(DatabaseContext databaseContext) : base(databaseContext)
        {
        }
    }
}
