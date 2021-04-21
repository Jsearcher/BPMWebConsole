using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Lib.Entities
{
    public class Evaluation
    {
        [Column("EvaluationId")]
        public Guid Id { get; set; }
        [Required]
        public int Grade { get; set; }
        public string AdditionalExplanation { get; set; }

        /// <summary>
        /// Foreign Key
        /// </summary>
        public Guid StudentId { get; set; }
        /// <summary>
        /// Navigational Property for <see cref="Student"/>
        /// </summary>
        public Student Student { get; set; }
    }
}
