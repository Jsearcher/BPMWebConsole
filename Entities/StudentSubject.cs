using System;
using System.Collections.Generic;
using System.Text;

namespace Lib.Entities
{
    public class StudentSubject
    {
        public Guid StudentId { get; set; }
        public Student Student { get; set; }

        /// <summary>
        /// Foreign Key
        /// </summary>
        public Guid SubjectId { get; set; }
        /// <summary>
        /// Navigational Property for <see cref="Subject"/>
        /// </summary>
        public Subject Subject { get; set; }
    }
}
