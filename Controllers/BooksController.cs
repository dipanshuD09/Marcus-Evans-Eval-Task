using LibraryManagement.data;
using LibraryManagement.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

public class BooksController : Controller
{
    private readonly LibDbContext context;

    public BooksController(LibDbContext _context)
    {
        context = _context;
    }


    public async Task<IActionResult> Index()
    {
        if (HttpContext.Session.GetString("Username") == null)
        {
            return RedirectToAction("Login", "User");
        }
        
        return View(await context.books.ToListAsync());
    }


    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var book = await context.books
            .FirstOrDefaultAsync(m => m.Id == id);
        if (book == null)
        {
            return NotFound();
        }

        return View(book);
    }
    public IActionResult Create()
    {
        return View();
    }


    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Id,Title,Author,ISBN,PublishedDate,Genre")] Book book)
    {
        if (ModelState.IsValid)
        {
            context.Add(book);
            await context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(book);
    }

    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var book = await context.books.FindAsync(id);
        if (book == null)
        {
            return NotFound();
        }
        return View(book);
    }


    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Author,ISBN,PublishedDate,Genre")] Book book)
    {
        if (id != book.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                context.Update(book);
                await context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BookExists(book.Id))
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
        return View(book);
    }


    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var book = await context.books
            .FirstOrDefaultAsync(m => m.Id == id);
        if (book == null)
        {
            return NotFound();
        }

        return View(book);
    }


    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var book = await context.books.FindAsync(id);
        context.books.Remove(book);
        await context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool BookExists(int id)
    {
        return context.books.Any(e => e.Id == id);
    }
}
