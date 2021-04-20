using DormitoryManagement.DAL;
using DormitoryManagement.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DormitoryManagement.Controllers {
    //[HandleError]
    public class StudentController : Controller {

        // GET: Student
        // Done
        public ActionResult Index() {
            if (Session["Account"] == null) { return Redirect("/Login"); }
            Account account = Session["Account"] as Account;
            if (account.IsManager) { return Redirect("/Manager"); }

            ViewBag.Success = TempData["Success"];
            ViewBag.Error = TempData["Error"];

            return View();
        }
        public ActionResult ViewProfile() {
            if (Session["Account"] == null) { return Redirect("/Login"); }
            Account account = Session["Account"] as Account;
            if (account.IsManager) { return Redirect("/Manager"); }

            StudentDAO studentDao = new StudentDAO();
            Student student = studentDao.GetStudentByStudentCode(account.Username);

            RoomDAO roomDao = new RoomDAO();
            RoomUsage roomUsage = roomDao.GetRoomUsageByRoomId(student.Bed.RoomId);

            // Student basic info
            ViewBag.StudentCode = student.StudentCode;
            ViewBag.StudentName = student.Name;
            ViewBag.StudentEmail = student.Email;
            ViewBag.StudentGender = student.Gender == true ? "Male" : "Female";

            // Student's room info
            ViewBag.Bed = student.Bed.BedNumber;
            ViewBag.Room = student.Bed.Room.GetRoomName();
            ViewBag.UsageElectric = roomUsage.ElectricUsage;
            ViewBag.UsageWater = roomUsage.WaterUsage;

            return View();

        }
        // GET: Student/CheckBed
        public ActionResult CheckBed(int? domId, int? floorNumber) {
            if (Session["Account"] == null) { return Redirect("/Login"); }
            Account account = Session["Account"] as Account;
            if (account.IsManager) { return Redirect("/Manager"); }

            StudentDAO studentDao = new StudentDAO();
            Student student = studentDao.GetStudentByStudentCode(account.Username);

            ViewBag.HasInDom = student.BedId.HasValue;
            if (ViewBag.HasInDom) {
                ViewBag.BedNumber = student.Bed.BedNumber;
                ViewBag.RoomName = student.Bed.Room.GetRoomName();
                ViewBag.DomName = student.Bed.Room.Floor.Dom.Name;
            }

            ViewBag.StudentGender = studentDao.GetStudentByStudentCode(account.Username).Gender;
            RoomDAO roomDao = new RoomDAO();
            DomDAO domDao = new DomDAO();
            domId = domId.HasValue ? domId : 1;
            floorNumber = floorNumber is null ? 1 : floorNumber;
            ViewBag.Doms = domDao.GetAllDom();
            return View(roomDao.GetRoomsByDomAndFloor((int)domId, (int)floorNumber));
        }
        // GET: Student/BookRoom
        // Done
        public ActionResult BookRoom(int bedId, string roomName, int bedNumber) {
            if (Session["Account"] == null) { return Redirect("/Login"); }
            Account account = Session["Account"] as Account;
            if (account.IsManager) { return Redirect("/Manager"); }
            
            ViewBag.Success = TempData["Success"];
            ViewBag.Error = TempData["Error"];

            ViewBag.DomName = roomName.First();
            ViewBag.RoomName = roomName;
            ViewBag.BedNumber = bedNumber;
            ViewBag.BedId = bedId;
            PriceDAO priceDao = new PriceDAO();
            Price priceDetail = priceDao.GetPriceDetailByType("Room");
            ViewBag.Price = priceDetail.StandardPrice;
            return View();
        }
        // GET: Student/Payment
        // Done
        [HttpPost]
        public ActionResult Payment(string paymentType, int? bedId, int? numberOfUse, int? invoiceId, string note) {
            if (Session["Account"] == null) { return Redirect("/Login"); }
            Account account = Session["Account"] as Account;
            if (account.IsManager) { return Redirect("/Manager"); }

            StudentDAO studentDao = new StudentDAO();
            Student student = studentDao.GetStudentByStudentCode(account.Username);
            ViewBag.StudentCode = student.StudentCode;

            PriceDAO priceDao = new PriceDAO();
            Price priceDetail = priceDao.GetPriceDetailByType(paymentType);
            ViewBag.PriceTypeName = priceDetail.PriceType.Name;
            ViewBag.TypeId = priceDetail.Id;
            ViewBag.BedId = bedId;
            numberOfUse = numberOfUse.HasValue ? numberOfUse : 1;
            ViewBag.Price = priceDetail.StandardPrice * numberOfUse;
            ViewBag.InvoiceId = invoiceId;
            ViewBag.Note = note;
            ViewBag.NumberOfUse = numberOfUse;

            return View();
        }
        // Post: Student/PayInvoice
        // Done
        [HttpPost]
        public ActionResult PayInvoice(int typeId, int? bedId, int? numberOfUse, int? invoiceId, string note) {
            if (Session["Account"] == null) { return Redirect("/Login"); }
            Account account = Session["Account"] as Account;
            if (account.IsManager) { return Redirect("/Manager"); }

            StudentDAO studentDao = new StudentDAO();
            Student student = studentDao.GetStudentByStudentCode(account.Username);

            BedDAO bedDao = new BedDAO();
            // Cast int? to int
            Bed bed = bedId.HasValue ? bedDao.GetBedById((int)bedId) : null;
            InvoiceDAO invoiceDao = new InvoiceDAO();

            PriceDAO priceDao = new PriceDAO();
            Price priceDetail = priceDao.GetPriceDetailById(typeId);

            bool result;
            // Không có invoiceId thì sẽ là trả tiền phòng. Tạo InvoiceID mới sau khi thanh toán thành công
            if (!invoiceId.HasValue) {
                numberOfUse = numberOfUse.HasValue ? numberOfUse : 1;
                result = invoiceDao.PayRoomInvoice(student.Id, typeId, bed, (double)(priceDetail.StandardPrice * numberOfUse), note);
                if (result) {
                    TempData["Success"] = "Book room sucessfully. Now you are in bed number " + bed.BedNumber + " of room " + bed.Room.GetRoomName();
                    return RedirectToAction("Index");
                } else {
                    TempData["Error"] = "Book Room has failed. Try again!";
                    return RedirectToAction("CheckBed");
                }
            }
            // Pay others payments

            // bed is null when student does not stay in dormitory
            if(bed is null) {
                result = invoiceDao.PayInvoice((int)invoiceId, null);
            } else {
                result = invoiceDao.PayInvoice((int)invoiceId, bed.Room.Id);
            }

            if (result) {
                TempData["Success"] = "Pay invoice sucessfully!";
            } else {
                TempData["Error"] = "Pay invoice failed. Try again!";
            }
                return RedirectToAction("ViewPaymentRequests");
        }

        public ActionResult ViewPaymentRequests() {
            if (Session["Account"] == null) { return Redirect("/Login"); }
            Account account = Session["Account"] as Account;
            if (account.IsManager) { return Redirect("/Manager"); }

            ViewBag.Success = TempData["Success"];
            ViewBag.Error = TempData["Error"];

            InvoiceDAO invoiceDao = new InvoiceDAO();
            StudentDAO studentDao = new StudentDAO();
            Student student = studentDao.GetStudentByStudentCode(account.Username);

            ViewBag.BedId = student.BedId;
            return View(invoiceDao.GetPaymentRequestsByStudentId(student.Id));
        }
        /// <summary>
        /// Update code ViewRequest date 31/03/2021 
        /// </summary>
        /// <param name="page"></param>
        /// <param name="studentID"></param>
        /// <returns></returns>
        public ActionResult ViewRequests(int? page) {
            if (Session["Account"] == null) { return Redirect("/Login"); }
            Account account = Session["Account"] as Account;
            if (account.IsManager) { return Redirect("/Manager"); }

            int pageIndex;
            int maxPage;
            int pageSize = 5;
            pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;

            RequestDAO requestDao = new RequestDAO();
            List<Request> list = requestDao.GetRequestsByStudentCode(account.Username);

            if (list.Count % pageSize != 0) {
                maxPage = list.Count / pageSize + 1;
            } else {
                maxPage = list.Count / pageSize;
            }
            IPagedList<Request> requests = list.ToPagedList(pageIndex, pageSize);
            ViewBag.StudentRequest = requests;
            ViewBag.maxPage = maxPage;
            ViewBag.pageIndex = pageIndex;
            return View();
        }

        /// <summary>
        /// Update code ViewInvoice date 31/03/2021
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        public ActionResult ViewInvoices(int? page) {
            if (Session["Account"] == null) { return Redirect("/Login"); }
            Account account = Session["Account"] as Account;
            if (account.IsManager) { return Redirect("/Manager"); }

            int pageIndex;
            int maxPage;
            int pageSize = 5;
            pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;

            InvoiceDAO invoiceDao = new InvoiceDAO();
            List<Invoice> list = invoiceDao.GetPaidInvoicesByStudentCode(account.Username);
            if (list.Count % pageSize != 0) {
                maxPage = list.Count / pageSize + 1;
            } else {
                maxPage = list.Count / pageSize;
            }
            IPagedList<Invoice> invoices = list.ToPagedList(pageIndex, pageSize);
            ViewBag.StudentInvoice = invoices;
            ViewBag.maxPage = maxPage;
            ViewBag.pageIndex = pageIndex;
            return View();
        }
        public ActionResult ViewInvoiceDetail(int? id) {
            if (Session["Account"] == null) { return Redirect("/Login"); }
            Account account = Session["Account"] as Account;
            if (account.IsManager) { return Redirect("/Manager"); }

            int invoiceId = id.HasValue ? Convert.ToInt32(id) : 4;

            InvoiceDAO invoiceDao = new InvoiceDAO();
            Invoice invoice = invoiceDao.GetInvoiceById(invoiceId);

            return View(invoice);
        }
        [HttpGet]
        public ActionResult CreateRequest() {
            if (Session["Account"] == null) { return Redirect("/Login"); }
            Account account = Session["Account"] as Account;
            if (account.IsManager) { return Redirect("/Manager"); }

            RequestDAO requestDao = new RequestDAO();
            List<RequestType> requestTypes = requestDao.GetAllRequestType();
            ViewBag.RequestTypes = requestTypes;
            return View();
        }

        [HttpPost]
        public ActionResult CreateRequest(string RequestTypeName, string title, string content) {
            if (Session["Account"] == null) { return Redirect("/Login"); }
            Account account = Session["Account"] as Account;
            if (account.IsManager) { return Redirect("/Manager"); }

            RequestDAO requestDAO = new RequestDAO();
            RequestType requestType = requestDAO.GetRequestTypeByName(RequestTypeName);

            StudentDAO studentDao = new StudentDAO();
            Student student = studentDao.GetStudentByStudentCode(account.Username);
            bool r = requestDAO.CreateRequest(student.Id, requestType.Id, title, content);
            if (r) {
                return Redirect("ViewRequests");
            } else {
                return Redirect("CreateRequest");
            }
        }
    }
}
