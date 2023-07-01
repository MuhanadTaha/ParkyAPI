using ParkyAPI.Data;
using ParkyAPI.Models;
using ParkyAPI.Repository.IRepository;
using System.Collections.Generic;
using System.Linq;

namespace ParkyAPI.Repository
{
    public class NationalParkRepository : INationalParkRepository
    {
        private readonly ApplicationDbContext db;

        public NationalParkRepository(ApplicationDbContext db)
        {
            this.db = db;
        }
        public bool CheckNationalParkExists(int Id)
        {
            return db.NationalParks.Any(m=>m.Id == Id); // أني بترجع ترو في حال وجدت الآي دي
        }

        public bool CheckNationalParkExists(string Name)
        {
            return db.NationalParks.Any(m => m.Name.ToLower().Equals(Name.ToLower()));
        }

        public bool CreateNationalPark(NationalParkDTO nationalPark)
        {
            db.NationalParks.Add(nationalPark);
            return Save();
        }

        public bool DeleteNationalPark(NationalParkDTO nationalPark)
        {
            db.NationalParks.Remove(nationalPark);
            return Save();
        }

        public NationalParkDTO GetNationalPark(int Id)
        {
            return db.NationalParks.Find(Id);
        }

        public IEnumerable<NationalParkDTO> GetNationalParks()
        {
            return db.NationalParks.ToList();

        }

        public bool Save()
        {
            return db.SaveChanges() >= 0 ? true : false;
        }

        public bool UpdateNationalPark(NationalParkDTO nationalPark)
        {
            db.NationalParks.Update(nationalPark);
            return Save();
        }
    }
}
