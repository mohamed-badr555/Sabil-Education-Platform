using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Data.Models;

namespace BLL.DTOs
{
    public class UnitDTO
    {
        public string Id { get; set; }
        public string Title { get; set; }

        public string Description { get; set; }
        public int Order { get; set; }

        public ICollection<VideoDetailsDTO> videos { get; set; }

    

    }
}
