﻿using System.ComponentModel.DataAnnotations.Schema;

namespace AdminManagementSystem.Models
{
    public class Department_Course
    {
        [ForeignKey(nameof(Department_ref))]
        public int Department_Id { get; set; }

        [ForeignKey(nameof(Course_ref))]
        public int Course_Id { get; set; }

        public virtual Department? Department_ref { get; set; }
        public virtual Course? Course_ref { get; set; }
    }
}
