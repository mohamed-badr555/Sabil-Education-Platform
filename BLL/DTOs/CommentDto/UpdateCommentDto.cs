using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.DTOs.CommentDto
{
    public class UpdateCommentDto
    {
        public string CommentId { get; set; }
        public int videoOrderIndex { get; set; }
        public int unitOrderIndex { get; set; }
        public string comment { get; set; }
    }
}
