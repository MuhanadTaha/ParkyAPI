using ParkyAPI.Models;
using ParkyAPI.Models.DTO;
using System.Collections;
using System.Collections.Generic;

namespace ParkyAPI.Repository.IRepository
{
    public interface INationalParkRepository
    {
        IEnumerable<NationalPark> GetNationalParks();

        NationalPark GetNationalPark(int Id);
        NationalPark GetNationalPark(string Name);

        bool CheckNationalParkExists(int Id); 
        bool CheckNationalParkExists(string Name);
        bool CreateNationalPark(NationalPark nationalPark);
        bool UpdateNationalPark(NationalPark nationalPark);
        bool DeleteNationalPark(NationalPark nationalPark);
        bool Save();
        
    }
}
