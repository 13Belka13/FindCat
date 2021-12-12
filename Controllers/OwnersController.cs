using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FindKočka.Data;
using FindKočka.Models;
using Microsoft.AspNetCore.Authorization;
using FindKočka.Services;

namespace FindKočka.Controllers
{
    [Authorize]
    public class OwnersController : Controller
    {
        private readonly FindKočkaContext _context;
        private readonly IUserService _userService;

        public OwnersController(FindKočkaContext context, IUserService userService)
        {
            _context = context;
            _userService = userService;
        }

        public async Task<IActionResult> Index()
        {
            if (_userService.GetUserId(this.User) == 1)
            {
                return View(await _context.Owners.ToListAsync());
            }
            else
            {
                return NotFound();
            }
        }


        public async Task<IActionResult> Details(int? id)
        {
            if (_userService.GetUserId(this.User) == 1)
            {
                if (id == null)
                {
                    return NotFound();
                }

                var owner = await _context.Owners
                    .FirstOrDefaultAsync(m => m.Id == id);
                if (owner == null)
                {
                    return NotFound();
                }

                return View(owner);
            }
            else
            {
                return NotFound();
            }
        }


        public IActionResult Create()
        {
            if (_userService.GetUserId(this.User) == 1)
            {
                return View();
            }
            else
            {
                return NotFound();
            }
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Email,Password")] Owner owner)
        {
            if (_userService.GetUserId(this.User) == 1)
            {
                if (ModelState.IsValid)
                {
                    _context.Add(owner);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                return View(owner);
            }
            else
            {
                return NotFound();
            }
        }


        public async Task<IActionResult> Edit(int? id)
        {
            if (_userService.GetUserId(this.User) == 1)
            {
                if (id == null)
                {
                    return NotFound();
                }

                var owner = await _context.Owners.FindAsync(id);
                if (owner == null)
                {
                    return NotFound();
                }
                return View(owner);
            }
            else
            {
                return NotFound();
            }    
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Email,Password")] Owner owner)
        {
            if (_userService.GetUserId(this.User) == 1)
            {
                if (id != owner.Id)
                {
                    return NotFound();
                }

                if (ModelState.IsValid)
                {
                    try
                    {
                        _context.Update(owner);
                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!OwnerExists(owner.Id))
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
                return View(owner);
            }
            else
            {
                return NotFound();
            }
        }


        public async Task<IActionResult> Delete(int? id)
        {
            if (_userService.GetUserId(this.User) == 1)
            {
                if (id == null)
                {
                    return NotFound();
                }

                var owner = await _context.Owners
                    .FirstOrDefaultAsync(m => m.Id == id);
                if (owner == null)
                {
                    return NotFound();
                }

                return View(owner);
            }
            else
            {
                return NotFound();
            }    
        }


        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var owner = await _context.Owners.FindAsync(id);
            _context.Owners.Remove(owner);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OwnerExists(int id)
        {
            return _context.Owners.Any(e => e.Id == id);
        }
    }
}
