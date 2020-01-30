using Microsoft.AspNetCore.Mvc;
using System;
using DFDS.CrmService.Models;
using System.Collections.Generic;

namespace DFDS.CrmService.Controllers
{
    public class CustomerController : ControllerBase
    {
        [HttpGet]
        [Route("api/customers/{id}")]
        public ActionResult<Customer> GetCustomerById(int id)
        {
            return new Customer { Id = id.ToString(), Address = new Address
            { City = "Copenhagen", StreetName = "Sundkrogsgade", ZipCode = "2101" }, Email = "tpet@dfds.com", Name = "Toke", ImageUrl = "https://farm2.staticflickr.com/1115/1357526469_da0e391657_m.jpg" };
        }

        [HttpGet]
        [Route("api/customers")]
        public ActionResult<List<Customer>> GetAll()
        {
            return new List<Customer> { new Customer
            {
                Id = "1",
                Address = new Address
                { City = "Copenhagen", StreetName = "Sundkrogsgade", ZipCode = "2100" },
                Email = "tpet@dfds.com",
                Name = "Toke",
                ImageUrl = "https://farm2.staticflickr.com/1115/1357526469_da0e391657_m.jpg" },
                new Customer
                {
                Id = "2",
                Address = new Address
                { City = "Copenhagen", StreetName = "Sundkrogsgade", ZipCode = "2100" },
                Email = "tpet@dfds.com",
                Name = "Christian",
                ImageUrl = "https://c8.alamy.com/comp/BAWHY8/cool-guy-doing-the-ok-sign-BAWHY8.jpg"
                }
            };
        }
    }
}