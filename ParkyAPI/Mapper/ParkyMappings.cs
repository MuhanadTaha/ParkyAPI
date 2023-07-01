using AutoMapper;
using ParkyAPI.Models;
using ParkyAPI.Models.DTO;


namespace ParkyAPI.Mapper
{
    public class ParkyMappings:Profile // موروث من كلاس جاي من الأوتو مابار اللي نزلتها من الناجيت باكيج
    {
        public ParkyMappings()
        {
            CreateMap<NationalPark, NationalParkDTO>().ReverseMap(); // الريفيرس باستخدمها عشان أعمل ماب من الناشيونال بارك للناشيونال بارك ديتي اوه والعكس
            
        }
    }
}
