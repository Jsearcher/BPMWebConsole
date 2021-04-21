using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Lib.Entities
{
    public class StudentDetails
    {
        [Column("StudentDetailsId")]
        public Guid Id { get; set; }
        public string Address { get; set; }
        public string AdditionalInformation { get; set; }

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
