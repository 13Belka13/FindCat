using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FindKočka.Data;
using FindKočka.Models;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using FindKočka.Services;
using Microsoft.AspNetCore.Http;
using System.IO;
using Microsoft.AspNetCore.Hosting;

namespace FindKočka.Controllers
{
    [Authorize]
    public class CatsController : Controller
    {
        private readonly FindKočkaContext _context;
        private readonly IUserService _userService;
        private readonly IWebHostEnvironment _hostEnvironment;

        public CatsController(FindKočkaContext context, IUserService userService, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            _userService = userService;
            this._hostEnvironment = hostEnvironment;
        }

        // GET: Cats
        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            ViewBag.userId = _userService.GetUserId(this.User);

            return View(await _context.Cats.ToListAsync());
        }

        public async Task<IActionResult> MyCats()
        {
            var userId = _userService.GetUserId(this.User);
            return View(await _context.Cats.Where(x => x.OwnerId == userId).ToListAsync());
        }

        // GET: Cats/Details/5
        [AllowAnonymous]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cat = await _context.Cats
                .FirstOrDefaultAsync(m => m.Id == id);
            if (cat == null)
            {
                return NotFound();
            }

            ViewBag.userId = _userService.GetUserId(this.User);

            return View(cat);
        }

        // GET: Cats/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Cats/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Cat model)
        {
            if (ModelState.IsValid)
            {

                if (model.Image.Length > 0)
                {

                    string wwwRootPath = _hostEnvironment.WebRootPath;
                    string fileName = Path.GetFileNameWithoutExtension(model.Image.FileName);
                    string extension = Path.GetExtension(model.Image.FileName);
                    model.ImageName=fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                    var filePath = Path.Combine(wwwRootPath + "/Image", fileName);


                    using (var stream = System.IO.File.Create(filePath))
                    {
                        await model.Image.CopyToAsync(stream);
                    }
                }

                Cat cat = new Cat
                {
                    Name = model.Name,
                    Age = model.Age,
                    OwnerId = _context.Owners.FirstOrDefault(u => u.Email == this.User.Identity.Name).Id,
                    Description = model.Description,
                    ImageName = model.ImageName,
                    Image = model.Image
                };

                _context.Cats.Add(cat);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        // GET: Cats/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cat = await _context.Cats.FindAsync(id);
            if (cat == null)
            {
                return NotFound();
            }

            if (cat.OwnerId != _userService.GetUserId(this.User))
            {
                return NotFound();
            }

            return View(cat);
        }

        // POST: Cats/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Cat cat)
        {
            if (id != cat.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                cat.OwnerId = _context.Owners.FirstOrDefault(u => u.Email == this.User.Identity.Name).Id;

                if (cat.Image != null)
                {
                    if (cat.ImageName != null)
                    {
                        var imagePath = Path.Combine(_hostEnvironment.WebRootPath, "image", cat.ImageName);
                        if (System.IO.File.Exists(imagePath))
                            System.IO.File.Delete(imagePath);
                    }

                    string wwwRootPath = _hostEnvironment.WebRootPath;
                    string fileName = Path.GetFileNameWithoutExtension(cat.Image.FileName);
                    string extension = Path.GetExtension(cat.Image.FileName);
                    cat.ImageName = fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                    var filePath = Path.Combine(wwwRootPath + "/Image", fileName);


                    using (var stream = System.IO.File.Create(filePath))
                    {
                        await cat.Image.CopyToAsync(stream);
                    }
                }

                try
                {
                    _context.Update(cat);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CatExists(cat.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(cat);
        }

        // GET: Cats/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cat = await _context.Cats
                .FirstOrDefaultAsync(m => m.Id == id);
            if (cat == null)
            {
                return NotFound();
            }

            if (cat.OwnerId != _userService.GetUserId(this.User))
            {
                return NotFound();
            }

            return View(cat);
        }

        // POST: Cats/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {   
            var cat = await _context.Cats.FindAsync(id);

            if(cat.ImageName != null)
            {
                var imagePath = Path.Combine(_hostEnvironment.WebRootPath, "image", cat.ImageName);
                if (System.IO.File.Exists(imagePath))
                    System.IO.File.Delete(imagePath);
            }

            _context.Cats.Remove(cat);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CatExists(int id)
        {
            return _context.Cats.Any(e => e.Id == id);
        }
    }
}
