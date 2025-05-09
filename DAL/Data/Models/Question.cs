using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Data.Models
{

    public class Question :BaseEntity
    {
       
        //[MaxLength(30)]
        //public string Type { get; set; }    We will assume that we have only MCQ 
        [MaxLength(50)]
        public string Text { get; set; }
        public float Mark { get; set; }

        #region Question - Exam (M-1)
        public string ExamID { get; set; }//Forign key
        public Exam Exam { get; set; }
        #endregion

        #region Question - QuestionChoices (1-M)
        //1-M
        public ICollection<QuestionChoice> QuestionChoices { get; set; }
        #endregion

        #region Question - QuestionChoices (1-1)
        //1-1

        public string? CorrectChoiceID { get; set; }


        public QuestionChoice CorrectChoice { get; set; }
        #endregion

        #region Question - AccountAnswer (1-M)
        public ICollection<AccountAnswer> AccountAnswers { get; set; }
        #endregion
    }
}