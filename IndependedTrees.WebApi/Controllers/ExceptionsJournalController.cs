using Microsoft.AspNetCore.Mvc;

namespace IndependedTrees.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExceptionsJournalController : ControllerBase
    {
        // GET: api/<ExceptionsJournalController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<ExceptionsJournalController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<ExceptionsJournalController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<ExceptionsJournalController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<ExceptionsJournalController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
