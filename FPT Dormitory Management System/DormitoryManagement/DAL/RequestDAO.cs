using DormitoryManagement.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace DormitoryManagement.DAL {
    public class RequestDAO {
        ModelDBContext db = new ModelDBContext();

        public List<Request> GetRequestsByStudentCode(string studentCode) {
            return db.Requests.Where(r => r.Student.StudentCode == studentCode).OrderBy(r => r.IsDone).ToList(); 
        }

        public List<RequestType> GetAllRequestType() { 
            return db.RequestTypes.ToList();
        }
        public List<Request> GetAllRequest() { 
            return db.Requests.OrderBy(c => c.IsDone).ToList();
        }
        public RequestType GetRequestTypeByName(string typeName) {
            return db.RequestTypes.Where(r => r.Name == typeName).FirstOrDefault();
        }

        public bool CreateRequest(int studentId, int typeId, string title, string purpose) {
            string sql = "insert into Request(studentid,typeid,title,purpose,isDone,dateCreated) " +
                    "values(@studentId,@typeId,@title,@content, 0, GETDATE() ) ";
            SqlParameter[] param = new SqlParameter[]{
                    new SqlParameter("@studentId",SqlDbType.Int),
                    new SqlParameter("@typeId",SqlDbType.Int),
                    new SqlParameter("@title",SqlDbType.NVarChar),
                    new SqlParameter("@content",SqlDbType.NVarChar)
                };
            param[0].Value = studentId;
            param[1].Value = typeId;
            param[2].Value = title;
            param[3].Value = purpose;
            int r = db.Database.ExecuteSqlCommand(sql, param);
            return r != 0;
        }
        public Request GetRequestById(int id) {
            Request request = db.Requests.Where(r => r.Id == id).FirstOrDefault();
            return request;
        }
        public void ReplyRequest(int id, int typeId, string content) {
            string sql = "update Request set reply = @content,typeId = @typeId, IsDone = 'True' where Id = @id";
            SqlParameter[] param = new SqlParameter[]{
                    new SqlParameter("@content",SqlDbType.NVarChar),
                    new SqlParameter("@typeId",SqlDbType.Int),
                    new SqlParameter("@id",SqlDbType.Int)
                };
            param[0].Value = content;
            param[1].Value = typeId;
            param[2].Value = id;
            db.Database.ExecuteSqlCommand(sql, param);
        }
        public int GetTotalPendingRequest() {
            return db.Requests.Where(r => r.IsDone == false).ToList().Count;
        }
    }
}