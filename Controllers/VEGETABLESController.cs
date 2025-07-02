using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
   // [Authorize(Roles = "Admin")]
    public class VEGETABLESController : Controller
    {
        private readonly ApplicationDbContext _context;

        // Constructor
        public VEGETABLESController(ApplicationDbContext context)
        {
            _context = context;
        }
        [AllowAnonymous]
        // عرض قائمة المنتجات
        public async Task<IActionResult> Index()
        {
            var vegetables = await _context.vegetables
    .Include(p => p.Category)
    .ToListAsync();

            return View(vegetables);
        }

        // عر ض صفحة إنشاء منتج جديد
        public IActionResult Create()
        { 
            ViewBag.CategoryId = new SelectList(_context.Categories, "CategoryId", "Name");
            return View();
        }

        // معالجة إضافة منتج جديد
        [HttpPost]
        public IActionResult Create(VEGETABLES vegetables)
        {
            if (!ModelState.IsValid)
            {
                _context.vegetables.Add(vegetables); // إضافة المنتج
                _context.SaveChanges(); // حفظ التغييرات في قاعدة البيانات

                TempData["success"] = "Product created successfully."; // رسالة نجاح
                return RedirectToAction("Index"); // إعادة التوجيه إلى صفحة القائمة
            }
            // إذا كانت البيانات غير صالحة
            ViewBag.CategoryId = new SelectList(_context.Categories, "CategoryId", "Name", vegetables.CategoryId);
            return View(vegetables);
        }
        // [Authorize(Roles = "Admin")]
        // عرض صفحة تعديل منتج
        public IActionResult Edit(int id)
        {
            var vegetables = _context.vegetables.Include(p => p.Category).FirstOrDefault(p => p.Id == id);
            if (vegetables == null)
            {
                return NotFound(); // إذا لم يتم العثور على المنتج
            }

            // تحميل قائمة الفئات للـ dropdown في الـ View
            ViewBag.Categories = new SelectList(_context.Categories, "CategoryId", "Name", vegetables.CategoryId);
            return View(vegetables); // تمرير المنتج إلى الـ View
        }

        // معالجة حفظ التعديلات على المنتج
        [HttpPost]
        public IActionResult Edit(int id, [Bind("Id,Name,Price,img,CategoryId")] VEGETABLES vegetables)
        {
            if (id != vegetables.Id)
            {
                return NotFound(); // التحقق من تطابق الـ ID
            }

            if (!ModelState.IsValid)
            {
                try
                {
                    _context.Update(vegetables); // تحديث المنتج في قاعدة البيانات
                    _context.SaveChanges(); // حفظ التغييرات
                    TempData["success"] = "vegetables updated successfully."; // رسالة نجاح
                    return RedirectToAction(nameof(Index)); // إعادة التوجيه إلى صفحة القائمة
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.vegetables.Any(p => p.Id == vegetables.Id))
                    {
                        return NotFound(); // التعامل مع الخطأ إذا لم يكن هناك منتج بهذا الـ ID
                    }
                    else
                    {
                        throw; // إعادة رمي الاستثناء في حالة وجود مشكلة أخرى
                    }
                }
            }

            // إذا كان النموذج غير صالح، إعادة عرضه مع الأخطاء
            ViewBag.Categories = new SelectList(_context.Categories, "CategoryId", "Name", vegetables.CategoryId);
            return View(vegetables);
        }
        // [Authorize(Roles = "Admin")]
        // عرض صفحة حذف منتج
        public IActionResult Delete(int id)
        {
            var vegetables = _context.vegetables.Find(id);
            if (vegetables == null)
            {
                return NotFound(); // إذا لم يتم العثور على المنتج
            }
            return View(vegetables); // تمرير المنتج إلى الـ View
        }

        // معالجة حذف المنتج
        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            var vegetables = _context.vegetables.Find(id);
            if (vegetables == null)
            {
                return NotFound(); // إذا لم يتم العثور على المنتج
            }

            _context.vegetables.Remove(vegetables); // حذف المنتج من قاعدة البيانات
            _context.SaveChanges(); // حفظ التغييرات

            TempData["success"] = "vegetables deleted successfully."; // رسالة نجاح
            return RedirectToAction(nameof(Index)); // إعادة التوجيه إلى صفحة القائمة
        }


        // عرض قائمة المنتجات


        // إضافة منتج إلى السلة
    }


}
