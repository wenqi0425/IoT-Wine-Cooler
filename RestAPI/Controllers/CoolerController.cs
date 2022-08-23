using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RestAPI.Manager;
using ClassLibrary;
using Microsoft.AspNetCore.Cors;
using System.Drawing;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace RestAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]

    public class CoolerController : ControllerBase
    {
        private CoolerManager _manager = new CoolerManager();

        // GET: <CoolerController>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public ActionResult<IEnumerable<Cooler>> GetAll()
        {
            List<Cooler> list = (List<Cooler>)_manager.GetAllCoolers();
            if (list.Count <= 0)
            {
                return NoContent();
            }
            return Ok(list);
        }

        // GET <CoolerController>/5
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public ActionResult<Cooler> GetById(int id)
        {
            Cooler cooler = _manager.GetCoolerById(id);
            if (cooler == null)
            {
                return NoContent();
            }
            return Ok(cooler);
        }

        // GET <CoolerController>/filter?capacity=
        [HttpGet("filter")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public ActionResult<IEnumerable<Cooler>> GetByCapacityFilter([FromQuery] int capacity)
{
            List<Cooler> list = _manager.FilterByCapacity(capacity);
            if (list.Count <= 0)
            {
                return NoContent();
            }
            return Ok(list);
        }

        [HttpGet("location")] // setting route, must be different with other method
        [ProducesResponseType(StatusCodes.Status200OK)]  // Can be remove, just for documentation
        [ProducesResponseType(StatusCodes.Status204NoContent)]

        public ActionResult<IEnumerable<Cooler>> GetByLocationFilter([FromQuery] string location)
        {
            List<Cooler> result = _manager.GetByLocation(location);
            if (result.Count <= 0)
            {
                return NoContent();
            }
            else
            {
                return Ok(result);
            }
        }

        // POST <CoolerController>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public ActionResult<Cooler> Post([FromBody] Cooler newCooler)
        {
            Cooler cooler = _manager.AddCooler(newCooler);
            return Created("URL", cooler);
        }

        /*
        // DELETE <CoolerController>/5
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<Cooler> Delete(int id)
        {
            Cooler cooler = _manager.DeleteCooler(id);
            if (cooler == null)
            {
                return NotFound(id + " not found");
            }
            return Ok(cooler);
        }
        */
        
        // DELETE <CoolerController>/delete?id=
        [HttpDelete("delete")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public ActionResult<Cooler> DeleteByQuery([FromQuery] int id)
        {
            Cooler deleted = _manager.DeleteCooler(id);
            if (deleted != null)
            {
                return NoContent();
            }
            return Ok(deleted);
        }


        // PUT <CoolerController>/5
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public ActionResult<Cooler> Put(int id, [FromBody] Cooler cooler)
        {
            Cooler updated = _manager.UpdateCooler(id, cooler);

            if (updated == null)
            {
                return NotFound(id + " not found");
            }
            return Ok(updated);
        }


        // Put <CoolerController>/5
        [HttpPut("addWine")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public ActionResult<IEnumerable<Cooler>> AddWine([FromQuery] int id)
        {
            List<Cooler> list = _manager.AddWineToCooler(id);
            if (list == null)
            {
                return NoContent();
            }
            return Ok(list);
        }
    }
}
