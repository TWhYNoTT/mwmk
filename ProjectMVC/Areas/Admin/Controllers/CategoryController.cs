using Entities.Models;
using Microsoft.AspNetCore.Mvc;
using Entities.Reposatories;
using Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;

namespace ProjectMVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.AdminRole)]

    public class CategoryController : Controller
    {
        private IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public CategoryController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            var CategoryList = _unitOfWork.Category.GetAll();

            return View(CategoryList);
        }

        [HttpGet]
        public IActionResult Create()
        {


            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Category category, IFormFile file)
        {
            category.Image="hd";
            if (ModelState.IsValid)
            {

                string RootPath = _webHostEnvironment.WebRootPath;
                if (file != null)
                {
                    string fileName = Guid.NewGuid().ToString();
                    var upload = Path.Combine(RootPath, @"");
                    var ext = Path.GetExtension(file.FileName);
                    using (var filestream = new FileStream(Path.Combine(upload, fileName + ext), FileMode.Create))
                    {
                        file.CopyTo(filestream);
                    }
                    // Assuming fileName and ext are variables representing the image name and extension
                    category.Image=(@"" + fileName + ext);

                }

                _unitOfWork.Category.add(category);
                _unitOfWork.complete();
                TempData["Type"] = "success";
                TempData["message"] = "created successfully";
                return RedirectToAction("Index");

            }


            return View(category);
        }
        [HttpGet]
        public IActionResult Edit(int id)
        {
            if (id == 0)
            {
                return NotFound();
            }
            else
            {
                Category CategoryFromDataBase = _unitOfWork.Category.GetByID(x => x.id == id);
                // categorys.Update(id, CategoryFromDataBase);
                // categorys.Save();
                return View(CategoryFromDataBase);
            }

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Category category)
        {

            int IDFromDataBase = category.id;


            _unitOfWork.Category.update(category);
            _unitOfWork.complete();
            TempData["Type"] = "info";
            TempData["message"] = "Updated successfully";
            return RedirectToAction("Index");

        }
        [HttpGet]
        public IActionResult Delete(int id)
        {
            if (id == null | id == 0)
            {
                NotFound();
            }
            Category CategoryFromDataBase = _unitOfWork.Category.GetByID(x => x.id == id);
            return View(CategoryFromDataBase);


        }
        [HttpPost]
        public IActionResult DeleteCategory(int id)
        {
            var categoryDB = _unitOfWork.Category.GetByID(x => x.id == id);
            if (categoryDB == null)
            {
                NotFound();
            }
            _unitOfWork.Category.remove(categoryDB);
            _unitOfWork.complete();
            TempData["Type"] = "error";
            TempData["message"] = "Deleted successfully";
            return RedirectToAction("index");
        }

    }
}
