using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ParkyAPI.Models;
using ParkyAPI.Models.DTO;
using ParkyAPI.Repository.IRepository;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ParkyAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //حطيتها هان عشان تتشيّر على كل الآكشن 
    [ProducesResponseType(StatusCodes.Status400BadRequest)] //  أول ما أفوت على الإيند بوينت قبل ما أعمل أي اشي بظهر هذا الخيار عندي واللي هو الباد ريكويست ممكن يرجع أو الساكسيس 

    public class NationalParksController : ControllerBase
    {
        private readonly INationalParkRepository npRepository;
        private readonly IMapper mapper;

        public NationalParksController(INationalParkRepository npRepository, IMapper mapper)
        {
            this.npRepository = npRepository;
            this.mapper = mapper;
        }

        /// <summary>
        /// get list of national parks
        /// </summary>
        /// <returns></returns>


        [HttpGet()] // حطيت اسمه داخل الجيت عشان يفهم البراوزر ايش الباث اللي بده يروح عليه
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<NationalParkDTO>))]
        [ProducesDefaultResponseType]
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
        /// <summary>
        /// get indivisual national park
        /// </summary>
        /// <param name="nationalParkId">National Park Id</param>
        /// <returns></returns>

        [HttpGet("{nationalParkId:int}",Name = "GetNationalPark")] // اعطيتها الإسم لأنها بتلزم بالميثود اليوست لما ترجع كريت ات روات
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(NationalParkDTO))]
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
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
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

            var obj = mapper.Map<NationalPark>(nationalParkDTO); // بحول هان من ناشيونال بارك دي تي اوه الى ناشيونال بارك باستخدام الماب
            if (!npRepository.CreateNationalPark(obj))
            {
                ModelState.AddModelError(string.Empty, $"Something Went Wrong Error when Adding Record {obj.Name}");
                return StatusCode(500, ModelState);
            }

            return CreatedAtRoute("GetNationalPark", new { nationalParkId = obj.Id },obj); // هذي معناها روح على الميثود اللي هيك اسمها ومررلي الآي دي هذا عشان ترجعلي قيمة
        }


        [HttpPatch ("nationalParkId:int")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult UpdateNationalPark(int nationalParkId, [FromBody] NationalParkDTO nationalParkDTO)
        {
            if (nationalParkDTO == null || nationalParkId != nationalParkDTO.Id)
            {
                return BadRequest(ModelState);
            }

            var objN = npRepository.GetNationalPark(nationalParkDTO.Name); // بفحص إذا الإسم موجود بقاعدة البيانات أو لا
            if (objN != null)
            {
                if (objN.Id != nationalParkDTO.Id) // إذا تكرر الإسم لما جيت أعدل رح تظهر مسج إنه موجود من قبل
                {
                    ModelState.AddModelError(string.Empty, "National Park Exists");
                    return StatusCode(404, ModelState);
                }
            }


            var obj = mapper.Map<NationalPark>(nationalParkDTO); // بحول هان من ناشيونال بارك دي تي اوه الى ناشيونال بارك باستخدام الماب

            var objFromDB = npRepository.GetNationalPark(obj.Id); // عشان أعمل مابينج بين اللي جبته من الداتا بيس وبين القيم اللي أجتني من البادي
            objFromDB.Name = obj.Name;
            objFromDB.State = obj.State;
            objFromDB.Established = obj.Established;
            objFromDB.Created = obj.Created;


            if (!npRepository.UpdateNationalPark(objFromDB))
            {
                ModelState.AddModelError(string.Empty, $"Something Went Wrong Error when Updating Record {obj.Name}");
                return StatusCode(500, ModelState);
            }


            return NoContent(); //ما بدي أرجع أي كونتينت لما أعمل أب ديت

        }



        [HttpDelete("nationalParkId:int")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public IActionResult DeleteNationalPark(int nationalParkId)
        {
            if(!npRepository.CheckNationalParkExists(nationalParkId))
            {
                return NotFound();
            }


            var obj = npRepository.GetNationalPark(nationalParkId);


            if (!npRepository.DeleteNationalPark(obj))
            {
                ModelState.AddModelError(string.Empty, $"Something Went Wrong Error when Deleting Record {obj.Name}");
                return StatusCode(500, ModelState);
            }


            return NoContent(); //ما بدي أرجع أي كونتينت لما أعمل أب ديت

        }






    }
}
