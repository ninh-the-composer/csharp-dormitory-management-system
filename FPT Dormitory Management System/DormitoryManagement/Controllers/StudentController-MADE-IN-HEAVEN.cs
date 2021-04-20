using DormitoryManagement.DAL;
using DormitoryManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DormitoryManagement.Controllers {
    [HandleError]
    public class StudentController : Controller {

        // GET: Student
        // Done
        public ActionResult Index() {
            if (Session["Account"] == null) { return Redirect("Login"); }
            Account account = Session["Account"] as Account;
            if (account.IsManager) { return Redirect("Manager"); }

            return View();
        }
        // GET: Student/CheckBed
        public ActionResult CheckBed(int? domId, int? floorNumber) {
            //if (Session["Account"] == null) { return Redirect("Login"); }
            Account account = Session["Account"] as Account;
            //if (account.IsManager) { return Redirect("Manager"); }
            StudentDAO studentDao = new StudentDAO();
            ViewBag.StudentGender = studentDao.GetStudentByStudentCode(account.Username).Gender;
            RoomDAO roomDao = new RoomDAO();
            DomDAO domDao = new DomDAO();
            domId = domId is null ? 1 : domId;
            floorNumber = floorNumber is null ? 1 : floorNumber;
            ViewBag.Doms = domDao.GetAllDom();
            return View(roomDao.GetBedsByDomAndFloor((int)domId, (int)floorNumber));
        }
        // GET: Student/BookRoom
        public ActionResult BookRoom(int bedId, string roomName, int bedNumber) {
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
        [HttpPost]
        public ActionResult Payment(string paymentType, double amount, int? bedId, int? numberOfUse, string note) {
            StudentDAO studentDao = new StudentDAO();
            Account account = Session["Account"] as Account;
            Student student = studentDao.GetStudentByStudentCode(account.Username);
            ViewBag.StudentCode = student.StudentCode;

            PriceDAO priceDao = new PriceDAO();
            Price priceDetail = priceDao.GetPriceDetailByType(paymentType);
            ViewBag.PriceTypeName = priceDetail.PriceType.Name;
            ViewBag.TypeId = priceDetail.Id;
            ViewBag.Price = amount;
            if (bedId is null) {
                ViewBag.NumberOfUse = numberOfUse;
            } else {
                ViewBag.Price = priceDetail.StandardPrice;
            }
            ViewBag.Note = note;
            ViewBag.BedId = bedId;
            return View();
        }
        // Post: Student/PayInvoice
        [HttpPost]
        public ActionResult PayInvoice(int typeId, int roomId, double amount, int? numberOfUse, string note) {
            StudentDAO studentDao = new StudentDAO();
            Account account = Session["Account"] as Account;
            Student student = studentDao.GetStudentByStudentCode(account.Username);
            RoomDAO roomDao = new RoomDAO();
            khoi ngu
            Room room = roomDao.GetRoomById(roomId);
            InvoiceDAO invoiceDao = new InvoiceDAO();
            if(typeId == 3) {
                bool r = invoiceDao.MakeRoomInvoice(student.Id, typeId, room.Id, amount, note);
                student = studentDao.GetStudentByStudentCode(account.Username);
                if (r) {
                    ViewBag.Success = "Book room Sucessfully. Now you are in room " + room.GetRoomName();
                    return RedirectToAction("Index");
                } else {
                    ViewBag.Error = "Book Room has failed. Try again!";
                    return RedirectToAction("CheckBed");
                }
            }
            throw new NotSupportedException();
        }

    }
}
