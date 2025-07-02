using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Data;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ApplicationDbContext context;

        public int Id { get; private set; }

        public CategoryController(ApplicationDbContext context)
        {
            this.context = context;
        }
        [AllowAnonymous]
        public IActionResult Index()
        {
            var result = context.Categories.ToList();
            return View(result);
        }
     //   [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }
      //  [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult Create(Category category)
        {
            if (!ModelState.IsValid)
            {
                context.Categories.Add(category);
                context.SaveChanges();

                TempData["success"] = "Created successfully";
                return RedirectToAction("Index");
            }

            return View(category);
        }
     //   [Authorize(Roles = "Admin")]
        public IActionResult Edit(int id)
        {

            var category = context.Categories.Find(Id = id);
            return View(category);
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult Edit(int id, [Bind("CategoryId", "Name")] Category category)
        {

            if (!ModelState.IsValid)
            {
                context.Update(category);
                context.SaveChanges();

                TempData["alert"] = "Created successfully";

                return RedirectToAction("Index");


            }
            return View(category);

        }
      //  [Authorize(Roles = "Admin")]
        public IActionResult Delete(int id)
        {
            var category = context.Categories.Find(id);
            return View(category);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult Deleteok(int Id)
        {
            // Mapping
            var category = context.Categories.Find(Id);

            context.Categories.Remove(category);
            context.SaveChanges();

            TempData["alert"] = "Deleted successfuly";

            return RedirectToAction("Index");

        }
    }
}
