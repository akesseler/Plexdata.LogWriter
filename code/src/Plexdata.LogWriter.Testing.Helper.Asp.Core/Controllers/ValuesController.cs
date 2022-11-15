/*
 * MIT License
 * 
 * Copyright (c) 2022 plexdata.de
 * 
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in all
 * copies or substantial portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
 * SOFTWARE.
 */

using Microsoft.AspNetCore.Mvc;
using Plexdata.LogWriter.Abstraction;
using Plexdata.LogWriter.Extensions;
using System;
using System.Collections.Generic;

namespace Plexdata.LogWriter.Testing.Helper.Asp.Core.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ValuesController : ControllerBase
    {
        private readonly IConsoleLogger logger = null;

        public ValuesController(IConsoleLogger logger)
            : base()
        {
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        // GET api/values
        [HttpGet]
        public ActionResult<IEnumerable<String>> Get()
        {
            this.logger.Trace("GET api/values called");
            return new String[] { "value1", "value2" };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<String> Get(Int32 id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] String value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(Int32 id, [FromBody] String value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(Int32 id)
        {
        }
    }
}
