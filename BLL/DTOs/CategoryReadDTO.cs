using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.DTOs
{
    public class CategoryReadDTO
    {
        public string Id { get; set; }  // Keep original GUID ID
        //public int SequentialId { get; set; }  // Add a sequential ID
        public string name { get; set; }
    }
}
