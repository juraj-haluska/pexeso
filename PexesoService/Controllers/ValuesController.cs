using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using PexesoService.Data;

namespace PexesoService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly PexesoContext _pexesoContext;


        public ValuesController(PexesoContext pexesoContext)
        {
            _pexesoContext = pexesoContext;
        }

        // GET api/values
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {       
            return new string[] {"value1"};
        }
    }
}
