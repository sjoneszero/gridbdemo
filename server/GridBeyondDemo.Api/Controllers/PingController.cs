using Microsoft.AspNetCore.Mvc;

namespace GridBeyondDemo.Api.Controllers
{
    /// <summary>
    /// Enabled a ping endpoint to test connectivity
    /// </summary>
    public class PingController : ControllerBase
    {
        public PingController()
        {
        }
        /// <summary>
        /// Returns a success message to indicate the API is up and running
        /// </summary>
        /// <returns>
        /// 200 OK including an array of market datasets
        /// 500 Internal Server Error
        /// </returns>
        [HttpGet]
        [Route("api/ping")]
        public IActionResult Get()
        {
            return Ok("Api up and running");
        }
    }
}
