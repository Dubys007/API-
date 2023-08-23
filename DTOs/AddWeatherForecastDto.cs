using System.ComponentModel.DataAnnotations;

namespace Sq016FirstApi.DTOs
{
    public class AddWeatherForecastDto
    {
        [Required]
        public int TemperatureC { get; set; }

        [Required]
        [StringLength(30, MinimumLength = 3, ErrorMessage = "Entry must be between 3 and 30 characters")]
        public string? Summary { get; set; }

        // validate this age to prevent users < 18 from 
        public int Age { get; set; }

        // only emails with .decagon
        public string Email { get; set; }
    }
}
