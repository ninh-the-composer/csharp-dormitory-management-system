using DormitoryManagement.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace DormitoryManagement.DAL {
    public class StudentDAO {
        ModelDBContext db = new ModelDBContext();
        public Student GetStudentById(int id) {
            return db.Students.Where(s => s.Id == id).FirstOrDefault();
        }
        public Student GetStudentByStudentCode(string studentCode) {
            return db.Students.Where(s => s.StudentCode == studentCode).FirstOrDefault();
        }
        public bool IsStudentExist(string studentCode) {
            return db.Students.Where(s => s.StudentCode == studentCode).ToList().Count != 0;
        }
        public List<Student> SearchStudentByStudentCode(string studentCode) {
            List<Student> list = db.Students.Where(r => r.StudentCode.Contains(studentCode)).ToList();
            return list;
        }
        public int GetTotalStudentInDorm() {
            return db.Students.Where(s => s.BedId != null).ToList().Count;
        }

    }
}