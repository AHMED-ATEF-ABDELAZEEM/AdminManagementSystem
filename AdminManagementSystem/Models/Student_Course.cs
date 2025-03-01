﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AdminManagementSystem.Models
{
    public class Student_Course
    {

        [ForeignKey(nameof(Student_ref))]
        public string Student_Id { get; set; }

        [ForeignKey(nameof(Course_ref))]
        public string Course_Id { get; set; }

        [Required (ErrorMessage = "The Mark Is Required")]
        [Range(0,100,ErrorMessage = "The Mark Must Be Between 0 and 100")]
        public int Mark {  get; set; }
        public virtual Course? Course_ref { get; set; }
        public virtual Student? Student_ref { get; set; }
    }
}
