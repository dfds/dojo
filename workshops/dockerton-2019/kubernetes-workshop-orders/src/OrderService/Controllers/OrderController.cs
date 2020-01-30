using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DFDS.OrderService.Models;

namespace DFDS.OrderService.Controllers
{
    public class OrderController : ControllerBase
    {
        [HttpGet]
        [Route("api/orders")]
        public OrderList GetAll()
        {
            throw new NotImplementedException();
        }
    }
}