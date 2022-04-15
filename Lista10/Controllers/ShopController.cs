using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Lista10.Data;
using Lista10.Models;
using System.IO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Authorization;

namespace Lista10.Controllers
{
    public class ShopController : Controller

    {
        private readonly ShopDbContext _context;
        private readonly IHostingEnvironment _hostingEnvironment;
        private string CookiePrefix = "My";

        public ShopController(ShopDbContext context, IHostingEnvironment hosting)
        {
            _context = context;
            _hostingEnvironment = hosting;
        }

        public IActionResult Increase(int id)
        {
            int amount = Request.Cookies[CookiePrefix + id.ToString()] == null ? 1 : int.Parse(Request.Cookies[CookiePrefix + id.ToString()]) + 1;
            SetCookie(id.ToString(), amount);
            return RedirectToAction("Basket");
        }

        public IActionResult Decrease(int id)
        {
            var cookie = Request.Cookies[CookiePrefix + id.ToString()];
            int amount = cookie == null ? 0 : int.Parse(cookie) - 1;
            if (amount == 0)
            {
                DeleteCookie(CookiePrefix + id.ToString());
            }
            else
            {
                SetCookie(id.ToString(), amount);
            }
            return RedirectToAction("Basket");
        }

        public IActionResult Delete(int id)
        {
            DeleteCookie(CookiePrefix + id.ToString());
            return RedirectToAction("Basket");
        }

        public async Task<IActionResult> Basket()
        {
            List<Article> shopDbContext = await _context.Articles.Include(a => a.Category).ToListAsync();

            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "CategoryName");

            foreach ( var cookie in Request.Cookies)
            {
                ViewData[cookie.Key.Substring(CookiePrefix.Length, cookie.Key.Length - 2)] = cookie.Value;
            }
            ViewData["price"] = "0";
            ViewData["flag"] = false;
            return View(shopDbContext);
        }
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> Order()
        {
            List<Article> articles = await _context.Articles.Include(a => a.Category).ToListAsync();

            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "CategoryName");

            foreach (var cookie in Request.Cookies)
            {
                ViewData[cookie.Key] = cookie.Value;
            }
            ViewData["price"] = "0";
            ViewData["flag"] = false;
            Order order = new Order
            {
                Articles = articles
            };
            return View(order);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> PlaceOrder([Bind("Name,Surname,Email,Address,City,PostalCode,PaymentEnum")] Order order)
        {
            foreach (var cookie in Request.Cookies)
            {
                if (cookie.Key.StartsWith(CookiePrefix))
                {
                    DeleteCookie(cookie.Key);
                }
            }
            return View(order);
        }

        // GET: Articles
        public async Task<IActionResult> Index()
        {
            List<Article> shopDbContext = await _context.Articles.Include(a => a.Category).ToListAsync();


            ViewData["n"] = 8;
            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "CategoryName");
            return View(shopDbContext);
        }
        [HttpPost]
        public async Task<IActionResult> Index(Category category = null, int? id=null)
        {
            List<Article> shopDbContext = null;
            if (category != null && category.CategoryId != 0)
            {
                int FilteredId = category.CategoryId;
                shopDbContext = await _context.Articles.Where(a => a.CategoryId == FilteredId).Include(a => a.Category).ToListAsync();
            }
            else
            {
                shopDbContext = await _context.Articles.Include(a => a.Category).ToListAsync();
            }
            if (id != null)
            {
                int amount = Request.Cookies[CookiePrefix + id.ToString()] == null ? 1 : int.Parse(Request.Cookies[CookiePrefix + id.ToString()])+1;
                SetCookie(id.ToString(), amount);
            }


            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "CategoryName");
            return View(shopDbContext);
        }

        public void SetCookie(string key, int value)
        {
            CookieOptions option = new CookieOptions();
            option.Expires = DateTime.Now.AddDays(7);
            string myKey = CookiePrefix + key;
            Response.Cookies.Append(myKey, value.ToString(), option);
        }

        public void DeleteCookie(string key)
        {
            CookieOptions option = new CookieOptions();
            option.Expires = DateTime.Now.AddDays(-1);
            Response.Cookies.Append(key, "", option);
        }

        private bool ArticleExists(int id)
        {
            return _context.Articles.Any(e => e.ArticleId == id);
        }
    }
}
