// BedDAO
using DormitoryManagement.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace DormitoryManagement.DAL {
    public class RoomDAO {
        ModelDBContext db = new ModelDBContext();
        public List<Room> GetRoomsByDomAndFloor(int domId, int floorNumber) {
            return db.Rooms.Where(d => d.Floor.DomId == domId && d.Floor.Number == floorNumber).ToList();
        }
        public List<Floor> GetRoomsInFloorsByDomId(int domId) {
            return db.Floors.Where(f => f.DomId == domId).ToList();
        }
        public RoomUsage GetRoomUsageByRoomId(int roomId) {
            return db.RoomUsages.Where(r => r.RoomId == roomId).OrderByDescending(r => r.Date).FirstOrDefault();
        }
        public Room GetRoomById(int id) {
            return db.Rooms.Where(r => r.Id == id).FirstOrDefault();
        }
        public int GetWaterUseageMonth() {
            string sql = "select sum(WaterUsage) from RoomUsage where MONTH([date]) = MONTH(getDate())";
            return db.Database.SqlQuery<int>(sql).FirstOrDefault();
        }
        public int GetElectricUseageMonth() {
            string sql = "select sum(ElectricUsage) from RoomUsage where MONTH([date]) = MONTH(getDate())";
            return db.Database.SqlQuery<int>(sql).FirstOrDefault();
        }
    }
}