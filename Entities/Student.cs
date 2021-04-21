using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Lib.Entities
{
    [Table("Student")]
    public class Student
    {
        [Key]
        [Column("StudentId")]
        public Guid Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        public int? Age { get; set; }

        public bool? IsRegularStudent { get; set; }

        public bool Deleted { get; set; }

        /// <summary>
        /// Navigational Property for <see cref="StudentDetails"/>
        /// </summary>
        public StudentDetails StudentDetails { get; set; }

        public ICollection<Evaluation> Evaluations { get; set; }

        public ICollection<StudentSubject> StudentSubjects { get; set; }

        [NotMapped]
        public int LocalCalculation { get; set; }
    }
}
