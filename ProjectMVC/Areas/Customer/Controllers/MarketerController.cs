using Microsoft.AspNetCore.Mvc;
using Utilities;
using DataAccessLayer.Implementation;
using Entities.Models;
using Entities.Reposatories;
using Entities.ViewModels;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
namespace ProjectMVC.Areas.Customer.Controllers
{
    [Area("Customer")]
    [Authorize(Roles = SD.MarketerRole)]
    public class MarketerController : Controller
    {

        private readonly IUnitOfWork _unitOfWork;


        public MarketerController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        private string GetUserId()
        {
            return User?.FindFirstValue(ClaimTypes.NameIdentifier);
        }


        public IActionResult Index()
        {

            var userId = GetUserId();
            var marketer = _unitOfWork.ApplicationUser.GetByID(x=> x.Id ==userId);

            if (marketer == null)
            {
                Console.WriteLine(userId);
                return NotFound("Marketer profile not found.");
            }
            ViewBag.Name = marketer.UserName;
            //ViewBag.TotalProfit = marketer.TotalProfit;
            //ViewBag.ProofPaymentImage = marketer.ProofPaymentImage;

            return View();
        }


        [HttpPost]
        [Authorize]
        public IActionResult SendMoney(string walletNumber)
        {
            if (string.IsNullOrWhiteSpace(walletNumber))
            {
                TempData["ResponseMessage"] = "رقم المحفظة غير صالح.";
                return RedirectToAction("Index");
            }

            var userId = GetUserId(); 

            var marketer = _unitOfWork.Marketer.GetByID(x=>x.UserId==userId);

            if (marketer != null)
            {
                marketer.WalletNumber = walletNumber;
                marketer.IsPaymentRequested = true;
                _unitOfWork.Marketer.update(marketer);
                _unitOfWork.complete();

                TempData["ResponseMessage"] = "تم إرسال الطلب بنجاح.";
            }
            else
            {
                TempData["ResponseMessage"] = "حدث خطأ. يرجى المحاولة مرة أخرى.";
            }

            return RedirectToAction("Index");
        }


    }
}
