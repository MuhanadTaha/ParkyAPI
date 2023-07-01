using ParkyAPI.Models;
using System.Collections;
using System.Collections.Generic;

namespace ParkyAPI.Repository.IRepository
{
    public interface INationalParkRepository
    {
        IEnumerable<NationalParkDTO> GetNationalParks();

        NationalParkDTO GetNationalPark(int Id);

        bool CheckNationalParkExists(int Id); 
        bool CheckNationalParkExists(string Name);
        bool CreateNationalPark(NationalParkDTO nationalPark);
        bool UpdateNationalPark(NationalParkDTO nationalPark);
        bool DeleteNationalPark(NationalParkDTO nationalPark);
        bool Save();



    }
}
