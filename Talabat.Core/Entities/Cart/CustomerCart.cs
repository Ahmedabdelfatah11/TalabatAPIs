﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Talabat.Core.Entities.Cart
{
    public class CustomerCart
    {
        public string Id { get; set; }
        public List<CartItem> Items  { get; set; }
        public CustomerCart(string id)
        {
            Id = id;
            Items= new List<CartItem>();
        }
    }
}
