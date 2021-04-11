using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Thec.Core;

namespace Thec.Service.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class TripMetricsController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IDrivingServiceDataProvider _drivingServiceDataProvider;
        private readonly ITripMetricsManager _tripMetricsManager;

        public TripMetricsController (IConfiguration configuration, IDrivingServiceDataProvider drivingServiceDataProvider, ITripMetricsManager tripMetricsManager)
        {
            _configuration = configuration;
            _drivingServiceDataProvider = drivingServiceDataProvider;
            _tripMetricsManager = tripMetricsManager;
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet]
        public ActionResult<ITripMetrics> Get (DrivingService drivingService, Borough startBorough, Borough stopBorough, string tripPickupTime)
        {
            if (TimeSpan.TryParse(tripPickupTime, out TimeSpan ts) == false)
                return BadRequest("tripPickupTime is not a valid time.");

            if (ts.Days != 0)
                return BadRequest("tripPickupTime must be a time only and you may not specify any days.");

            var metrics = _tripMetricsManager.GetMetrics(drivingService, startBorough, stopBorough, ts);

            if (metrics == null)
                return NoContent();

            return Ok(metrics);
        }
    }
}
