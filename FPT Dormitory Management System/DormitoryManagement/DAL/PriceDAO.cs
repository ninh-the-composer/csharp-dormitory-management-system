using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DormitoryManagement.Models;

namespace DormitoryManagement.DAL {
    public class PriceDAO {

        ModelDBContext db = new ModelDBContext();
        public Price GetPriceDetailByType(string type) {
            int priceType = GetPriceTypeByName(type);
            return db.Prices.Where(p => p.TypeId == priceType).FirstOrDefault();
        }
        public Price GetPriceDetailById(int id) {
            return db.Prices.Where(p => p.TypeId == id).FirstOrDefault();
        }
        public int GetPriceTypeByName(string type) {
            List<PriceType> priceTypes = db.PriceTypes.ToList();
            foreach (PriceType pt in priceTypes) {
                if (type.Equals(pt.Name)) return pt.Id;
            }
            return 0;
        }
    }
}