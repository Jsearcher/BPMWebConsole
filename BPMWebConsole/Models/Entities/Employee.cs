using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BPMWebConsole.Models.Entities
{
    /// <summary>
    /// 測試用資料
    /// </summary>
    public class Employee
    {
        public Guid Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public int Age { get; set; }

        [Required]
        public string Position { get; set; }
    }
}
