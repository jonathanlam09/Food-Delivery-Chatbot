using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Backend.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MainController : ControllerBase
    {
        private readonly Main.Imain repository;

        //Dependency Injection
        public MainController(Main.Imain repository)
        {
            this.repository = repository;
        }

        //Initialize HttpPost Route
        [HttpPost("CheckIntent")]
        //Create HttpPost function to call repository function. Inject argument if required.
        public IActionResult CheckIntent(TextMessage text)
        {
            var text_message = repository.CheckIntent(text.text);
            if (text_message is null)
            {
                return NotFound("wassup");
            }
            return Ok(text_message);
        }

        [HttpGet("GetPrice")]
        public IActionResult GetPriceList()
        {
            var text_message = repository.GetPriceList();
            if (text_message is null)
            {
                return NotFound("wassup");
            }
            return Ok(text_message);
        }
    }
}
