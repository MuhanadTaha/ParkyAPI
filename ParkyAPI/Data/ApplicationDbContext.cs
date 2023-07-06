using Microsoft.EntityFrameworkCore; // عشان أعمل انهيرت من دي بي كونتيكست
using ParkyAPI.Models;
using ParkyAPI.Models.DTO;

namespace ParkyAPI.Data
{
    public class ApplicationDbContext:DbContext // رح يكون المسؤوول عن تحويل الموديل لإنتيتي ومن إينتيتي لتيبل بالداتا بيس
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options):base(options) //كونستراكتار
            // الأوبشين هو عبارة عن أوبجكت لل دي بي كونتيكتس أوبجيكت بتكون فيه الخيارات الموجودة بالستارات أب
        {
            
        }

        public DbSet<NationalPark> NationalParks { get; set; }
    }
}
