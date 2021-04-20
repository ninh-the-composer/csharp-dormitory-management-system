using DormitoryManagement.DAL;
using DormitoryManagement.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DormitoryManagement.Controllers {
    public class ManagerController : Controller {
        // GET: Manager
        public ActionResult Index() {
            if (Session["Account"] == null) { return Redirect("/Login"); }
            Account account = Session["Account"] as Account;
            if (!account.IsManager) { return Redirect("/Student"); }

            ViewBag.Success = TempData["Success"];
            ViewBag.Error = TempData["Error"];

            StudentDAO studentDAO = new StudentDAO();
            RoomDAO roomDAO = new RoomDAO();
            RequestDAO requestDAO = new RequestDAO();
            int totalStudentInDorm = studentDAO.GetTotalStudentInDorm();
            int waterUsageMonth = roomDAO.GetWaterUseageMonth();
            int electricUsageMonth = roomDAO.GetElectricUseageMonth();
            int requestPending = requestDAO.GetTotalPendingRequest();
            ViewBag.totalStudent = totalStudentInDorm;
            ViewBag.waterUsage = waterUsageMonth;
            ViewBag.electricUsage = electricUsageMonth;
            ViewBag.requestPending = requestPending;
            return View();

        }
        // GET: ManageStudent
        public ActionResult ManageRoom(int? domId) {
            if (Session["Account"] == null) { return Redirect("/Login"); }
            Account account = Session["Account"] as Account;
            if (!account.IsManager) { return Redirect("/Student"); }

            domId = domId.HasValue ? domId : 1;

            RoomDAO roomDao = new RoomDAO();
            DomDAO domDao = new DomDAO();
            ViewBag.Doms = domDao.GetAllDom();
            return View(roomDao.GetRoomsInFloorsByDomId((int)domId));
        }
        public ActionResult RoomDetail(int id) {
            if (Session["Account"] == null) { return Redirect("/Login"); }
            Account account = Session["Account"] as Account;
            if (!account.IsManager) { return Redirect("/Student"); }

            RoomDAO roomDao = new RoomDAO();
            RoomUsage roomUsage = roomDao.GetRoomUsageByRoomId(id);
            Room room = roomDao.GetRoomById(id);
            int numberOfFreeSlot = 0;
            List<Student> listOfStudentsInRoom = new List<Student>();
            foreach (Bed bed in room.Beds) {
                if (bed.IsAvailable) numberOfFreeSlot++;
                foreach (Student st in bed.Students) {
                    listOfStudentsInRoom.Add(st);
                }
            }

            ViewBag.WaterUsage = roomUsage.WaterUsage;
            ViewBag.ElectricUsage = roomUsage.ElectricUsage;
            ViewBag.UpdateAt = roomUsage.Date;
            ViewBag.NumberOfFreeSlot = numberOfFreeSlot;
            ViewBag.ListStudentsInRoom = listOfStudentsInRoom;
            return View(roomDao.GetRoomById(id));

        }
        // GET: CreatePaymentRequest
        public ActionResult CreatePaymentRequest(string studentCode, int? invoiceType, int? usage, string note) {
            if (Session["Account"] == null) { return Redirect("/Login"); }
            Account account = Session["Account"] as Account;
            if (!account.IsManager) { return Redirect("/Student"); }

            InvoiceDAO invoiceDao = new InvoiceDAO();
            StudentDAO studentDao = new StudentDAO();
            RoomDAO roomDao = new RoomDAO();

            ViewBag.HasStudentValid = studentDao.IsStudentExist(studentCode);
            if (invoiceType.HasValue) {
                ViewBag.PaymentTypeName = invoiceDao.GetInvoiceTypeById((int)invoiceType).Name;

                Student student = studentDao.GetStudentByStudentCode(studentCode);
                if (student.BedId.HasValue) {
                    RoomUsage roomUsage = roomDao.GetRoomUsageByRoomId(student.Bed.RoomId);
                    ViewBag.WaterUsage = roomUsage.WaterUsage;
                    ViewBag.ElectricUsage = roomUsage.ElectricUsage;
                    PriceDAO priceDAO = new PriceDAO();
                    //chua xong

                }
            }

            ViewBag.AllInvoiceTypes = invoiceDao.GetAllInvoiceType();
            return View();
        }
        //POST: CreatePaymentRequest
        [HttpPost]
        public ActionResult CreatePaymentRequest(string studentCode, int typeId, int? usage, DateTime deadline, string note) {
            if (Session["Account"] == null) { return Redirect("/Login"); }
            Account account = Session["Account"] as Account;
            if (!account.IsManager) { return Redirect("/Student"); }

            StudentDAO studentDao = new StudentDAO();
            Student student = studentDao.GetStudentByStudentCode(studentCode);
            usage = usage.HasValue ? usage : 1;

            // hard code demo result
            double amount = 90000;
            int numberOfUse = 30;

            InvoiceDAO invoiceDao = new InvoiceDAO();
            bool r = invoiceDao.CreatePaymentRequest(student.Id, typeId, amount, numberOfUse, note, deadline);
            if (r) {
                TempData["Success"] = "Create Success";
                return Redirect("CreatePaymentRequest");
            } else {
                TempData["Success"] = "Create Fail";
                return Redirect("CreatePaymentRequest");

            }
        }

        /// <summary>
        /// Quan li sinh vien 02/04/2021 
        /// author KhoiVT
        /// </summary>
        /// <param name="page"></param>
        /// <param name="studentCode"></param>
        /// <returns></returns>
        public ActionResult ManageStudent(int? page, string studentCode) {
            if (Session["Account"] == null) { return Redirect("/Login"); }
            Account account = Session["Account"] as Account;
            if (!account.IsManager) { return Redirect("/Student"); }

            int pageIndex;
            int maxPage;
            int pageSize = 10;
            studentCode = studentCode == null ? "" : studentCode;
            pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;
            //List<Request> list = db.Requests.Where(r=>r.Student.StudentCode==account.Username).ToList();
            StudentDAO studentDAO = new StudentDAO();
            List<Student> list = studentDAO.SearchStudentByStudentCode(studentCode);
            ViewBag.listCount = list.Count;

            if (list.Count > 0) {
                if (list.Count % pageSize != 0) {
                    maxPage = (list.Count / pageSize) + 1;
                } else {
                    maxPage = list.Count / pageSize;
                }
                IPagedList<Student> students = list.ToPagedList(pageIndex, pageSize);
                ViewBag.StudentList = students;
                ViewBag.maxPage = maxPage;
                ViewBag.pageIndex = pageIndex;
            } else {
                ViewBag.NoResult = "No Result";
            }
            ViewBag.searchText = studentCode;

            return View();

        }
        /// <summary>
        /// Xem thong tin sinh vien 02/04/2021 
        /// author KhoiVT
        /// </summary>
        /// <param name="page"></param>
        /// <param name="studentCode"></param>
        /// <returns></returns>
        public ActionResult ViewStudentDetail(int id, int? page) {

            StudentDAO studentDAO = new StudentDAO();
            if (Session["Account"] == null) { return Redirect("/Login"); }
            Account account = Session["Account"] as Account;
            if (!account.IsManager) { return Redirect("/Student"); }

            int pageIndex;
            int maxPage;
            int pageSize = 10;
            pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;
            InvoiceDAO invoiceDao = new InvoiceDAO();
            List<Invoice> list = invoiceDao.GetAllInvoiceByStudentId(id);
            if (list.Count % pageSize != 0) {
                maxPage = list.Count / pageSize + 1;
            } else {
                maxPage = list.Count / pageSize;
            }
            IPagedList<Invoice> invoices = list.ToPagedList(pageIndex, pageSize);
            ViewBag.StudentInvoices = invoices;
            ViewBag.maxPage = maxPage;
            ViewBag.pageIndex = pageIndex;
            ViewBag.listCount = list.Count;
            return View(studentDAO.GetStudentById(id));
        }
        /// <summary>
        /// Xem chi tiet hoa don 02/04/2021 
        /// author KhoiVT
        /// </summary>
        /// <param name="page"></param>
        /// <param name="studentCode"></param>
        /// <returns></returns>
        public ActionResult ViewInvoiceDetail(int? stId, int invoiceId) {
            if (Session["Account"] == null) { return Redirect("/Login"); }
            Account account = Session["Account"] as Account;
            if (!account.IsManager) { return Redirect("/Student"); }

            InvoiceDAO invoiceDAO = new InvoiceDAO();
            Invoice invoice = invoiceDAO.GetInvoiceById(invoiceId);
            return View(invoice);
        }
        /// <summary>
        /// Xem tat ca danh sach yeu cau cua sinh vien 02/04/2021 
        /// author KhoiVT
        /// </summary>
        /// <param name="page"></param>
        /// <param name="studentCode"></param>
        /// <returns></returns>
        public ActionResult ViewStudentRequest(int? page) {
            if (Session["Account"] == null) { return Redirect("/Login"); }
            Account account = Session["Account"] as Account;
            if (!account.IsManager) { return Redirect("/Student"); }

            int pageIndex;
            int maxPage;
            int pageSize = 10;

            pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;

            RequestDAO requestDAO = new RequestDAO();
            List<Request> list = requestDAO.GetAllRequest();
            ViewBag.listCount = list.Count;

            if (list.Count > 0) {
                if (list.Count % pageSize != 0) {
                    maxPage = (list.Count / pageSize) + 1;
                } else {
                    maxPage = list.Count / pageSize;
                }
                IPagedList<Request> requests = list.ToPagedList(pageIndex, pageSize);
                ViewBag.RequestList = requests;
                ViewBag.maxPage = maxPage;
                ViewBag.pageIndex = pageIndex;
            } else {
                ViewBag.NoResult = "No Result";
            }

            return View();
        }

        public ActionResult ReplyRequest(int? id) {
            if (Session["Account"] == null) { return Redirect("/Login"); }
            Account account = Session["Account"] as Account;
            if (!account.IsManager) { return Redirect("/Student"); }


            RequestDAO requestDAO = new RequestDAO();
            List<RequestType> requestTypes = requestDAO.GetAllRequestType();
            Request request = requestDAO.GetRequestById(Convert.ToInt32(id));
            ViewBag.RequestTypes = requestTypes;

            return View(request);
        }
        [HttpPost, ActionName("ReplyRequest")]
        public ActionResult ReplyRequestConfirm(int? id, int typeId, string content) {
            if (Session["Account"] == null) { return Redirect("/Login"); }
            Account account = Session["Account"] as Account;
            if (!account.IsManager) { return Redirect("/Student"); }

            RequestDAO requestDAO = new RequestDAO();
            requestDAO.ReplyRequest(Convert.ToInt32(id), typeId, content);
            //TempData["notify"] = "Khoi dep trai";
            //Session["notify"] = "Khoi Dep trai da tao 1 yeu cau moi";
            return Redirect("~/Manager/ViewStudentRequest");
        }
    }
}