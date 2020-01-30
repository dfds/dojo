using System;
using System.Collections.Generic;

namespace DFDS.OrderService.Models 
{
    public class OrderList
    {
        public List<Order> Items { get; set; }

        public OrderList()
        {
            Items = new List<Order>();
        }
    }

    public class Order
    {
        public string Number { get; set; }
        
        public string Submittet { get; set; }
        public List<OrderLine> Lines { get; set; }

        public Order()
        {
            Lines = new List<OrderLine>();
        }
    }

    public class OrderLine
    {
        public string Descriptions { get; set; }

        public string ImageUrl { get; set; }

        public int Quantity { get; set; }

        public int Price { get; set; }
    }    
}