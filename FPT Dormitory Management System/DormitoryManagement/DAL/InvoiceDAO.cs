using DormitoryManagement.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace DormitoryManagement.DAL {
    public class InvoiceDAO {
        ModelDBContext db = new ModelDBContext();
        public List<Invoice> GetPaidInvoicesByStudentCode(string studentCode) {
            return db.Invoices.Where(r => r.Student.StudentCode == studentCode && r.IsPaid == true).ToList();
        }
        public Invoice GetInvoiceById(int id) {
            return db.Invoices.Where(r => r.Id == id).FirstOrDefault(); 
            //return db.Invoices.Where(r => r.Student.StudentCode == account.Username && r.Id == id).FirstOrDefault(); 
        }

        public List<Invoice> GetAllInvoiceByStudentId(int studentId) {
            return db.Invoices.Where(r => r.StudentId == studentId).OrderBy(r => r.IsPaid).ToList();
        }
        public bool PayInvoice(int invoiceId, int? roomId) {
            string sql = "update Invoice set IsPaid = 1, RoomId = @roomId, DateCreated = GETDATE() where id = @invoiceId";
            SqlParameter[] param = new SqlParameter[] {
                new SqlParameter("@roomId", SqlDbType.Int),
                new SqlParameter("@invoiceId", SqlDbType.Int)
            };
            param[0].Value = roomId;
            param[1].Value = invoiceId;
            int r = db.Database.ExecuteSqlCommand(sql, param);
            return r != 0;
        }
        public bool PayRoomInvoice(int studentId, int typeId, Bed bed, double amount, string note) {
            if (!bed.IsAvailable) {
                return false;
            }
            string sql = "insert into Invoice(StudentId,TypeId,RoomId,Amount,Note,IsPaid,DateCreated) " +
                "values(@studentId,@typeId,@roomId,@amount,@note,1,GETDATE()) ";
            SqlParameter[] param = new SqlParameter[] {
                new SqlParameter("@studentId", SqlDbType.Int),
                new SqlParameter("@typeId", SqlDbType.Int),
                new SqlParameter("@roomId", SqlDbType.Int),
                new SqlParameter("@amount", SqlDbType.Money),
                new SqlParameter("@note", SqlDbType.NVarChar),

            };
            param[0].Value = studentId;
            param[1].Value = typeId;
            param[2].Value = bed.Room.Id;
            param[3].Value = amount;
            param[4].Value = note;
            int r = db.Database.ExecuteSqlCommand(sql, param);
            int r1 = 0;
            int r2 = 0;
            int r3 = 0;
            if (r > 0) {
                string sqlUpdateStudent = "update Student set BedId = @bedId where Id = @studentId";
                SqlParameter[] paramUpdateStudent = new SqlParameter[] {
                    new SqlParameter("@bedId", SqlDbType.Int),
                    new SqlParameter("@studentId", SqlDbType.Int)
                };
                paramUpdateStudent[0].Value = bed.Id;
                paramUpdateStudent[1].Value = studentId;

                string sqlUpdateBed = "update Bed set IsAvailable = 0 where Id = @bedId";
                SqlParameter[] paramUpdateBed = new SqlParameter[] {
                    new SqlParameter("@bedId", SqlDbType.Int)
                };
                paramUpdateBed[0].Value = bed.Id;

                string sqlUpdateRoom = "update Room set RoomGender = @studentGender where Id = @roomId";
                SqlParameter[] paramUpdateRoom = new SqlParameter[] {
                    new SqlParameter("@studentGender", SqlDbType.Bit),
                    new SqlParameter("@roomId", SqlDbType.Int)
                };
                Student student = (new StudentDAO()).GetStudentById(studentId);
                paramUpdateRoom[0].Value = student.Gender;
                paramUpdateRoom[1].Value = bed.Room.Id;

                r1 = db.Database.ExecuteSqlCommand(sqlUpdateStudent, paramUpdateStudent);
                r2 = db.Database.ExecuteSqlCommand(sqlUpdateBed, paramUpdateBed);
                r3 = db.Database.ExecuteSqlCommand(sqlUpdateRoom, paramUpdateRoom);
            }
            return r != 0 && r1 != 0 && r2 != 0 && r3 != 0;
        }
        public List<Invoice> GetPaymentRequestsByStudentId(int id) {
            return db.Invoices.Where(i => i.IsPaid == false && i.StudentId == id).ToList();
        }

        public List<InvoiceType> GetAllInvoiceType() {
            return db.InvoiceTypes.ToList();
        }
        public InvoiceType GetInvoiceTypeById(int invoiceId) {
            return db.InvoiceTypes.Where(i => i.Id == invoiceId).FirstOrDefault();
        }
        public bool CreatePaymentRequest(int studentId, int typeId, double amount,int?numberOfUse, string note, DateTime deadline) {
            string sql = "insert into Invoice(StudentId,TypeId,Amount,NumberOfUse,Note,IsPaid, Deadline) " +
                "values(@studentId, @typeId, @amount, @numberOfUse,@note,0,@deadline)";
            SqlParameter[] param = new SqlParameter[] {
                    new SqlParameter("@studentId", SqlDbType.Int),
                    new SqlParameter("@typeId", SqlDbType.Int),
                    new SqlParameter("@amount", SqlDbType.Money),
                    new SqlParameter("@numberOfUse", SqlDbType.Int),
                    new SqlParameter("@note", SqlDbType.NVarChar),
                    new SqlParameter("@deadline", SqlDbType.DateTime)
                };
            param[0].Value = studentId;
            param[1].Value = typeId;
            param[2].Value = amount;
            param[3].Value = numberOfUse;
            param[4].Value = note;
            param[5].Value = deadline;
            return db.Database.ExecuteSqlCommand(sql, param) != 0;
        }
        
    }
}