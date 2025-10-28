namespace POS.Backend.DTO.Request.POS.Model
{
    /// <summary>
    /// ExperienceModel
    /// </summary>
    public class ExperienceModel
    {
        /// <summary>
        /// Company
        /// </summary>
        public string Company { get; set; }

        /// <summary>
        /// Role
        /// </summary>
        public string Role { get; set; }

        /// <summary>
        /// StartDate
        /// </summary>
        public string StartDate { get; set; }  // Or use DateTime

        /// <summary>
        /// EndDate
        /// </summary>
        public string EndDate { get; set; }

        /// <summary>
        /// Responsibilities
        /// </summary>
        public string Responsibilities { get; set; }


    }
}
