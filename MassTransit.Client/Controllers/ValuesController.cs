﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Common.Messages;
using System.Net;

namespace MassTransit.Client.Controllers
{
    [Route("api/[controller]")]
    public class ValuesController : Controller
    {
        private readonly IRequestClient<requestMessage, ReplyMessage> _request;

        public ValuesController(IRequestClient<requestMessage, ReplyMessage> request)
        {
            _request = request;
        }

        // GET api/values
        [HttpGet]
        public async Task<IActionResult> Get()
        {

            try
            {
                var demo = new requestMessage();

                ReplyMessage result = await _request.Request(demo);

                return Accepted(result);
            }
            catch (RequestTimeoutException exception)
            {
                return new JsonResult(@"Request Timeout" +Environment.NewLine + exception.Message) { StatusCode = (int)HttpStatusCode.RequestTimeout};
            }
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
