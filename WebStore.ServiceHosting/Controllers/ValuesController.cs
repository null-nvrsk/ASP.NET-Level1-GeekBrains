using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;

namespace WebStore.ServiceHosting.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private static readonly List<string> __Values = Enumerable
            .Range(1, 10)
            .Select(i => $"Value-{i:00}")
            .ToList();

        [HttpGet] // http://localhost:5001/api/values
        public IEnumerable<string> Get() => __Values;
        //public ActionResult<IEnumerable<string>> Get() => __Values;

        [HttpGet("{id}")] // http://localhost:5001/api/values/5
        public ActionResult<string> Get(int id)
        {
            if (id < 0) return BadRequest();
            if (id >= __Values.Count) return NotFound();

            return __Values[id];
        }

        [HttpPost]                // post -> http://localhost:5001/api/values
        [HttpPost("add")]  // post -> http://localhost:5001/api/values/add

        public ActionResult Post([FromBody] string value)
        {
            __Values.Add(value);

            // 200
            // return Ok();

            // 201
            // http://localhost:5001/api/values/10
            return CreatedAtAction(nameof(Get), new {id = __Values.Count - 1}); 
        }

        [HttpPut("{id}")]       // put -> http://localhost:5001/api/values/6
        [HttpPut("edit/{id}")]  // put -> http://localhost:5001/api/values/edit/6
        public ActionResult Put(int id, [FromBody] string newValue)
        {
            if (id < 0) return BadRequest();
            if (id >= __Values.Count) return NotFound();

            __Values[id] = newValue;
            return Ok();
        }

        [HttpDelete("{id}")]       // put -> http://localhost:5001/api/values/6
        public ActionResult Delete(int id)
        {
            if (id < 0) return BadRequest();
            if (id >= __Values.Count) return NotFound();

            __Values.RemoveAt(id);
            return Ok();
        }
    }
}
