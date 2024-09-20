using LibraryManagement.data;
using LibraryManagement.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

public class UserController : Controller
{
    private readonly LibDbContext context;

    public UserController(LibDbContext _context)
    {
        context = _context;
    }
    public IActionResult Index()
    {
        return View(context.users.ToList());
    }

    public IActionResult Signup()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Signup(User user)
    {
        if (ModelState.IsValid)
        {
            context.users.Add(user);
            context.SaveChanges();
            return RedirectToAction(nameof(Login));
        }
        return View(user);
    }


    public IActionResult Login()
    {
        return View();
    }


    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Login(string username, string password)
    {
        var user = context.users.FirstOrDefault(u => u.Username == username && u.Password == password);
        if (user != null)
        {
            HttpContext.Session.SetString("Username", user.Username);
            HttpContext.Session.SetString("UserRole", user.Role.ToString());

            if (HttpContext.Session.GetString("UserRole") == "NormalUser")
            {
                return RedirectToAction("Index", "Books");
            }

            return Redirect("Index");
        }


        ViewBag.ErrorMessage = "Invalid username or password.";
        return View();
    }

    public IActionResult Logout()
    {
        HttpContext.Session.Clear();
        return RedirectToAction(nameof(Login));
    }

    public IActionResult Delete(int id)
    {
        var user = context.users.FirstOrDefault(u => u.Id == id);
        if (user == null)
        {
            return NotFound();
        }

        return View(user);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public IActionResult DeleteConfirmed(int id)
    {
        var user = context.users.FirstOrDefault(u => u.Id == id);
        if (user != null)
        {
            context.users.Remove(user);
        }

        return RedirectToAction(nameof(Index));
    }
}
