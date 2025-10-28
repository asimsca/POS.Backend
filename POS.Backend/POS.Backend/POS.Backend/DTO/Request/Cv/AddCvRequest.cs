using System.ComponentModel.DataAnnotations;
using POS.Backend.DTO.Request.POS.Model;

namespace POS.Backend.DTO.Request.POS
{
    public class AddPOSRequest
    {
        [Required(ErrorMessage = "Title is required")]

        public string Title { get; set; }
        public string Designation { get; set; }
        public string Summary { get; set; }
        public string ProfilePictureUrl { get; set; }

        public List<ContactModel> Contacts { get; set; }
        public List<EducationModel> Education { get; set; }
        public List<ExperienceModel> Experience { get; set; }

        public string Skills { get; set; }  // Comma-separated for now
    }
}
