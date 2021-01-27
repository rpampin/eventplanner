using System.ComponentModel.DataAnnotations;

namespace EventPlanner.Models
{
    public class ProgramStepSong : ProgramStep
    {
        [Required]
        public string SongName { get; set; }
        [Required]
        public string SongBy { get; set; }
    }
}