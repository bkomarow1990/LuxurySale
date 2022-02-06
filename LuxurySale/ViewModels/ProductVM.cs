using LuxurySale.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LuxurySale.ViewModels
{
    public class ProductVM
    {
        public IEnumerable<SelectListItem> Categories { get; set; }
        public Product Product { get; set; }
    }
}
