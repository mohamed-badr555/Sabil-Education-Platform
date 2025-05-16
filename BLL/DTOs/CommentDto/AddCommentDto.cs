using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.DTOs.CommentDto
{
    public class AddCommentDto
    {
        public string parentCommentId { get; set; }
        public int videoOrderIndex { get; set; }
        public int unitOrderIndex { get; set; }
        public string comment { get; set; }
    }
}
