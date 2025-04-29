using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Data.Models
{
    public class QuestionChoice :BaseEntity
    {
       
        [MaxLength(100)]
        public string Text { get; set; }

        #region QuestionChoices - Question (M-1)
        //M-1
        public int? QuestionID { get; set; } //Foreign key 

        public Question Question { get; set; }
        #endregion

        #region QuestionChoices - Question (1-1)
        //1-1
        //public Question Question2 { get; set; } 
        #endregion
    }
}
