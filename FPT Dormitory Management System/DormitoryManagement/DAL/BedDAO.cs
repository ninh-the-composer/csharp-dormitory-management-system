using DormitoryManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DormitoryManagement.DAL {
    public class BedDAO {
        ModelDBContext db = new ModelDBContext();
        public Bed GetBedById(int id) {
            return db.Beds.Where(b => b.Id == id).FirstOrDefault();
        }

    }
}