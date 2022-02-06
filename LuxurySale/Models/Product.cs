﻿using System;
using System.ComponentModel.DataAnnotations;

namespace LuxurySale.Models
{
    public class Product
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int CategoryId { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        // Navigation properties
        public virtual Category Category { get; set; }

    }
}
