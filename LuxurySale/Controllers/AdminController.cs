using LuxurySale.Data;
using LuxurySale.Models;
using LuxurySale.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LuxurySale.Utilities;
using Microsoft.AspNetCore.Http;
using System.IO;

namespace LuxurySale.Controllers
{
    public class AdminController : Controller
    {
        private MainDBContext _context;
        private IWebHostEnvironment _host;
        public AdminController(MainDBContext context, IWebHostEnvironment host)
        {
            _context = context;
            _host = host;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Categories()
        {
            return View(_context.Categories.ToList());
        }
        public async Task<IActionResult> Products()
        {
            return View(await _context.Products.ToListAsync());
        }
        public async Task<IActionResult> CategoryUpset(int? id)
        {
            Category category = null;
            if (id != null)
            {
                category = await _context.Categories.FindAsync(id);
            }
            return View(category);
        }
        [HttpPost]
        public async Task<IActionResult> CategoryUpset(Category model)
        {
            if (model.Id == 0)
            {
                _context.Categories.Add(model);
                // _context.SaveChanges();
            }
            else
            {
                _context.Categories.Update(model);
            }

            await _context.SaveChangesAsync();


            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> ProductUpset(int? id)
        {
            ProductVM productVM = new ProductVM()
            {
                Categories = _context.Categories.Select(i => new SelectListItem() {
                    Text = i.Name,
                    Value = i.Id.ToString()
                })
            };
            if (id != null)
            {
                productVM.Product = await _context.Products.FindAsync(id);
            }
            return View(productVM);
        }
        [HttpPost]
        public async Task<IActionResult> ProductUpset(ProductVM viewModel)
        {
            var images = HttpContext.Request.Form.Files;
            if (images != null && images.Count() != 0)
            {
                if (viewModel.Product.Image == null && System.IO.File.Exists(_host.WebRootPath + Constants.productImagesPath + viewModel.Product.Image))
                {
                    System.IO.File.Delete(_host.WebRootPath + Constants.productImagesPath + viewModel.Product.Image);
                }
                string filePath = SaveImage(images[0]);
                viewModel.Product.Image = filePath;
            }
            if (viewModel.Product.Id == 0)
            {
                await _context.Products.AddAsync(viewModel.Product);
            }
            else
            {
                _context.Products.Update(viewModel.Product);
            }
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Products));
        }
        public async Task<IActionResult> ProductDelete(int? id) {
            if (id == null || id <= 0)
            {
                return NotFound();
            }
            var removeItem = await _context.Products.FindAsync(id);
            _context.Products.Remove(removeItem);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Products));
        }
        private string SaveImage(IFormFile img)
        {
            string root = _host.WebRootPath;
            string folder = root + Constants.productImagesPath;
            string name = Guid.NewGuid().ToString();
            string extension = Path.GetExtension(img.FileName);

            string fullPath = Path.Combine(folder, name + extension);

            using (FileStream fs = new FileStream(fullPath, FileMode.Create))
            {
                img.CopyTo(fs);
            }

            return name + extension;
        }
    }
}
