using Microsoft.AspNetCore.Mvc;
using System;
using DFDS.CrmService.Models;

namespace DFDS.CrmService.Controllers
{
    public class CustomerController : ControllerBase
    {
        [HttpGet]
        [Route("api/customers/{id}")]
        public ActionResult<Customer> GetCustomerById(int id)
        {
            throw new NotImplementedException();
        }
    }
}