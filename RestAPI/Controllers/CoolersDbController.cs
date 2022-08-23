using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

//using ClassLibrary;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using RestAPI.Manager;
using RestAPI.Model;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace RestAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CoolerDbController : ControllerBase
    {
        private CoolerDbManager _dbmanager;
        public CoolerDbController(CoolerDbManager dbmanager)
        {
            _dbmanager = dbmanager;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public ActionResult<IEnumerable<Cooler>> GetAll()
        {
            List<Cooler> list = _dbmanager.GetAll().ToList();
            if (list.Count <= 0)
            {
                return NoContent();
            }
            return Ok(list);
        }

        // GET <WindController>/5
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public ActionResult<Cooler> GetById(int id)
        {
            Cooler cooler = _dbmanager.GetById(id);
            return Created("URL", cooler);
        }

        // POST <WindsController>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public ActionResult<Cooler> Post([FromBody] Cooler newData)
        {
            Cooler cooler = _dbmanager.Add(newData);
            return Created("URL", cooler);
        }

        // PUT <WindController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<WindController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
