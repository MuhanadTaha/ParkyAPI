using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ParkyAPI.Models;
using ParkyAPI.Models.DTO;
using ParkyAPI.Repository.IRepository;
using System.Collections.Generic;

namespace ParkyAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NationalParksController : ControllerBase
    {
        private readonly INationalParkRepository npRepository;
        private readonly IMapper mapper;

        public NationalParksController(INationalParkRepository npRepository, IMapper mapper)
        {
            this.npRepository = npRepository;
            this.mapper = mapper;
        }


        [HttpGet()] // حطيت اسمه داخل الجيت عشان يفهم البراوزر ايش الباث اللي بده يروح عليه
        public IActionResult GetNationalParks()
        {
            var nationalParkList = npRepository.GetNationalParks();
            var nationalParkDTOList = new List<NationalParkDTO>();

            foreach (var obj in nationalParkList) // هان الماب فعلليا بصير
            {
                // أول ملاحظة إنه باستخدم السيرفيس مابر اللي حقنتها بالكونستاركتار عشان أعمل مابينج بين كلاسين السورس والديتستينيشن
                // ثاني ملاحظة بين الأقواس الحادة دايمًا بعبّر عن الديستينيشن 
                // ثالث ملاحظة إنه الباراميتار اللي بتمرر بكون عبارة عن السورس
                nationalParkDTOList.Add(mapper.Map<NationalParkDTO>(obj));
            }
            return Ok(nationalParkDTOList);
        }


        [HttpGet("{nationalParkId:int}",Name = "GetNationalPark")] // اعطيتها الإسم لأنها بتلزم بالميثود اليوست لما ترجع كريت ات روات
        public IActionResult GetNationalPark(int nationalParkId)
        {
            var obj = npRepository.GetNationalPark(nationalParkId);

            if (obj == null)
            {
                return NotFound();
            }

            var objDTO = mapper.Map<NationalParkDTO>(obj);

            return Ok(objDTO);
        }

        [HttpPost]
        public IActionResult CreateNationalPark([FromBody] NationalParkDTO nationalParkDTO) // دي تي أوه لأنه هو اللي تمرر بالجيت
        {
            if(nationalParkDTO == null)
            {
                return BadRequest(ModelState);
            }

            if(npRepository.CheckNationalParkExists(nationalParkDTO.Name)) // بدي أتحقق من الإسم إذا أنا ممرره من قبل ولا لا
            {
                ModelState.AddModelError(string.Empty, "National Park Exists");
                return StatusCode(404,ModelState);
            }

            var obj = mapper.Map<NationalPark>(nationalParkDTO);
            if (!npRepository.CreateNationalPark(obj))
            {
                ModelState.AddModelError(string.Empty, $"Something Went Wrong Error when Adding Record {obj.Name}");
                return StatusCode(500, ModelState);
            }

            return CreatedAtRoute("GetNationalPark", new { nationalParkId = obj.Id },obj); // هذي معناها روح على الميثود اللي هيك اسمها ومررلي الآي دي هذا عشان ترجعلي قيمة
        }


    }
}
