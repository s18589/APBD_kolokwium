using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using APBD_kolokwium.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace APBD_kolokwium.Controllers
{
    [Route("api/prescriptions")]
    [ApiController]
    public class PrescriptionController : ControllerBase
    {
        private IDbService _service;

        public PrescriptionController(IDbService service)
        {
            _service = service;
        }

        [HttpGet("{id}")]
        public IActionResult GetPrescription(int id)
        {
            var response = _service.GetPrescription(id);

            if(response != null)
                return Ok(response);

            return NotFound("nie ma takiej recepty");
        }
    }
}