using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace DAL.Data.Models
{

    // Username, Email, and PhoneNumber are already built into IdentityUser — no need to redefine them.
    public class ApplicationUser : IdentityUser
    {
        [MaxLength(25)]
        public string Fname { get; set; }

        [MaxLength(25)]
        public string Lname { get; set; }

        public bool Gender { get; set; }

        [MaxLength(60)]
        public int CountryId { get; set; }

        public DateOnly Birthdate { get; set; }

        [MaxLength(50)]
        public int EduLevel { get; set; }

        [MaxLength(200)]
        public string? Address { get; set; }

        [MaxLength(500)]
        public string? ImageUrl { get; set; }

        // 👇 Computed property for Age
        public int Age => DateTime.Today.Year - Birthdate.Year;

        // 👇 Many-to-Many: Courses
        public ICollection<CourseAccount> CourseAccounts { get; set; }

        // 👇 One-to-Many: Answers
        public ICollection<AccountAnswer> AccountAnswers { get; set; }

        public ICollection<VideoComment> Comments { get; set; }
    }
    //public enum Gender
    //{
    //    Male,
    //    Female
    //}


}
