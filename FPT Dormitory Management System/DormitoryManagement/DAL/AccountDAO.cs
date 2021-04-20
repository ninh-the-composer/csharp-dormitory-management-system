using DormitoryManagement.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace DormitoryManagement.DAL{
    public class AccountDAO {

        ModelDBContext db = new ModelDBContext();
        public Account GetAccount(string username, string password) {
            return db.Accounts.Where(c => c.Username == username && c.Password == password).FirstOrDefault();
        }
    }
}