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
            return new OrderList
            {
                Items = new List<Order>
                {
                    new Order
                    {
                        Number = "998879",
                        Submittet = "YES",
                        Lines = new List<OrderLine>
                        {
                            new OrderLine
                            {
                                Descriptions = "An apple",
                                Price = 15,
                                Quantity = 2,
                                ImageUrl = "https://ichef.bbci.co.uk/images/ic/960x540/p01bqdly.jpg"
                            },
                            new OrderLine
                            {
                                Descriptions = "A pear",
                                Price = 10,
                                Quantity = 3,
                                ImageUrl = "https://images-na.ssl-images-amazon.com/images/I/C1YRnQIzuTS._SL1000_.png"
                            }
                        }
                    },
                    new Order
                    {
                        Number = "1234",
                        Submittet = "YES",
                        Lines = new List<OrderLine>
                        {
                            new OrderLine
                            {
                                Descriptions = "An orange",
                                Price = 5,
                                Quantity = 8,
                                ImageUrl = "https://media.vanityfair.com/photos/5be09e5ad397092d264cc0a1/master/pass/rami-malek-queen.jpg"
                            },
                            new OrderLine
                            {
                                Descriptions = "A Mandarine",
                                Price = 88,
                                Quantity = 13,
                                ImageUrl = "https://upload.wikimedia.org/wikipedia/commons/thumb/a/a0/AdamLambert-Queen_7-1-14_SJ.jpg/1200px-AdamLambert-Queen_7-1-14_SJ.jpg"
                            }
                        }
                    }
                }
            };
        }
    }
}