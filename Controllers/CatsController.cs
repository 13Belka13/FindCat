using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FindKočka.Data;
using FindKočka.Models;
using Microsoft.AspNetCore.Authorization;
using FindKočka.Services;
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

        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            return View(await _context.Cats.ToListAsync());
        }

        public async Task<IActionResult> MyCats()
        {
            var userId = _userService.GetUserId(this.User);
            return View(await _context.Cats.Where(x => x.OwnerId == userId).ToListAsync());
        }

        [AllowAnonymous]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cat = await _context.Cats.FirstOrDefaultAsync(m => m.Id == id);
            if (cat == null)
            {
                return NotFound();
            }

            return View(cat);
        }

        // GET: Cats/Create
        public IActionResult Create()
        {
            return View();
        }

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
                    model.ImageName = fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                    var filePath = Path.Combine(wwwRootPath + "/image", fileName);


                    using (var stream = System.IO.File.Create(filePath))
                    {
                        await model.Image.CopyToAsync(stream);
                    }
                }

                var user = _context.Owners.FirstOrDefault(u => u.Id == _userService.GetUserId(this.User));

                Cat cat = new Cat
                {
                    Name = model.Name,
                    Age = model.Age,
                    OwnerId = user.Id,
                    Description = model.Description,
                    ImageName = model.ImageName,
                    OwnerName = user.FirstName + " " + user.SecondName,
                    OwnerEmail = user.Email,
                    OwnerNumber = user.Number
                };

                _context.Cats.Add(cat);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

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
                    var filePath = Path.Combine(wwwRootPath + "/image", fileName);


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
