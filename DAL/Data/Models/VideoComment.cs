using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Data.Models
{
    public class VideoComment : BaseEntity
    {
        [MaxLength(3000)]
        public string? Text { get; set; }
        public DateTime Time { get; set; }

        // User relationship (correct)
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }

        // Video relationship (fixed)
        public string VideoId { get; set; }
        public Video Video { get; set; }  // Changed to uppercase to match convention
    }
}
