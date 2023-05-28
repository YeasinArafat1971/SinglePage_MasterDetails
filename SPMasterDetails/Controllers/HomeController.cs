using Microsoft.AspNetCore.Mvc;
using SPMasterDetails.Models;
using SPMasterDetails.ViewModel;
using System.Diagnostics;

namespace SPMasterDetails.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        [HttpGet]
        public IActionResult Single(int? id)
        {
            var db = new SaleDBContext();
            VmSale oSale = new VmSale();
            var oSM = db.SaleMasters.Where(x => x.SaleId == id).FirstOrDefault();
            if (oSM != null)
            {
                oSale = new VmSale();
                oSale.SaleId = oSM.SaleId;
                oSale.CustomerName = oSM.CustomerName;
                oSale.CustomerAddress = oSM.CustomerAddress;
                oSale.CreateDate = oSM.CreateDate;
                oSale.Gender = oSM.Gender;
                var ListSaleDetail = new List<VmSale.VmSaleDetail>();
                var ListSD = db.SaleDetails.Where(x => x.SaleId == id).ToList();
                foreach (var oSD in ListSD)
                {
                    var oSaleDetail = new VmSale.VmSaleDetail();
                    oSaleDetail.SaleId = oSD.SaleId;
                    oSaleDetail.SaleDetailId = oSD.SaleDetailId;
                    oSaleDetail.ProductName = oSD.ProductName;
                    oSaleDetail.Price = oSD.Price;
                    ListSaleDetail.Add(oSaleDetail);
                }
                oSale.SaleDetails = ListSaleDetail;
            }
            oSale = oSale == null ? new VmSale() : oSale;
            ViewData["List"] = db.SaleMasters.ToList();
            return View(oSale);
        }
        [HttpPost]
        public IActionResult Single(VmSale model)
        {
            var db = new SaleDBContext();
            var oSaleMaster = db.SaleMasters.Find(model.SaleId);
            if (oSaleMaster == null)
            {
                oSaleMaster = new SaleMaster();
                oSaleMaster.SaleId = model.SaleId;
                oSaleMaster.CreateDate=model.CreateDate;
                oSaleMaster.CustomerName = model.CustomerName;
                oSaleMaster.CustomerAddress = model.CustomerAddress;
                oSaleMaster.Gender = model.Gender;
                db.SaleMasters.Add(oSaleMaster);
            }
            else
            {
                oSaleMaster.SaleId = model.SaleId;
                oSaleMaster.CustomerName = model.CustomerName;
                oSaleMaster.CustomerAddress = model.CustomerAddress;
                oSaleMaster.Gender = model.Gender;
                oSaleMaster.CreateDate= model.CreateDate;
                var ListSaleDetailRem = db.SaleDetails.Where(x => x.SaleId == oSaleMaster.SaleId).ToList();
                db.SaleDetails.AddRange(ListSaleDetailRem);

            }
            db.SaveChanges();
            var ListSaleDetail = new List<SaleDetail>();
            for (var i = 0; i < model.ProductName.Length; i++)
            {
                if (!string.IsNullOrEmpty(model.ProductName[i]))
                {
                    var oSaleDetail = new SaleDetail();
                    oSaleDetail.SaleId = oSaleMaster.SaleId;
                    oSaleDetail.ProductName = model.ProductName[i];
                    oSaleDetail.Price = model.Price[i];
                    db.SaleDetails.Add(oSaleDetail);
                }

            }
            db.SaleDetails.RemoveRange(ListSaleDetail);
            db.SaveChanges();
            return RedirectToAction("Single");
        }
        [HttpGet]
        public IActionResult SingleDelete(int id)
        {
            var db = new SaleDBContext();
            var oSaleMaster = (from o in db.SaleMasters where o.SaleId == id select o).FirstOrDefault();
            if (oSaleMaster != null)
            {
                db.SaleMasters.Remove(oSaleMaster);
                db.SaveChanges();
            }
            return RedirectToAction("Single");
        }

    }
}