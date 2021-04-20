using DormitoryManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DormitoryManagement.DAL {
    public class DomDAO {
        ModelDBContext db = new ModelDBContext();
        public List<Dom> GetAllDom() {
            return db.Doms.ToList();
        }
        public Dom GetDomById(int id) {
            return db.Doms.Where(d => d.Id == id).FirstOrDefault() ;
        }
    }
}