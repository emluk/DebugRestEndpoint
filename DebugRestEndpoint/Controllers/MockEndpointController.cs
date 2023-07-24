using System.IO;
using System.Text;
using DebugRestEndpoint.Config;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace DebugRestEndpoint.Controllers
{
    [ApiController]
    [Route("/")]
    public class MockEndpointController : ControllerBase
    {
        private readonly ILogger<MockEndpointController> _logger;
        private readonly string _name;

        public MockEndpointController(ILogger<MockEndpointController> logger, IOptionsMonitor<MockEndpointConfig> mockEndpointConfig)
        {
            _logger = logger;
            var mockEndpointConfig1 = mockEndpointConfig.CurrentValue;
            _name = mockEndpointConfig1.Name;
        }

        [HttpGet]
        public string Status()
        {
            return new string($"Hi. My name is {_name}. I am up and running.");
        }

        [HttpPut("{*path}")]
        public ActionResult LogPut(string path)
        {
            _logger.LogInformation($"Name: {_name}\nMethod: PUT\nRelative Path: {path}\nBody:\n{GetMessageFromBody()}");
            return Ok();
        }

        [HttpPost("{*path}")]
        public ActionResult LogPost(string path)
        {
            _logger.LogInformation($"Name: {_name}\nMethod: POST\nRelative Path: {path}\nBody:\n{GetMessageFromBody()}");
            return Ok();
        }

        private string GetMessageFromBody()
        {
            var reader = new StreamReader(Request.Body, Encoding.UTF8);
            var message = reader.ReadToEndAsync().Result;
            // replace carriage returns to avoid issues with v2 messages
            message = message.Replace("\r\n", "\n").Replace("\r","\n");
            return message;
        }
    }
}