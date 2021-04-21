using BPMWebConsole.Models.Entities.Configuration;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BPMWebConsole.Models.Entities
{
    public class ApplicationContext : IdentityDbContext<User>
    {
        #region =====[Public] Property=====

        public DbSet<Employee> Employees { get; set; }

        #endregion

        #region =====[Public] Constructor & Destructor=====

        /// <summary>
        /// 建構子
        /// </summary>
        /// <param name="options"></param>
        public ApplicationContext(DbContextOptions options) : base(options)
        {

        }

        #endregion

        #region =====[Protected|Inherit] Method=====

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ApplyConfiguration(new EmployeeConfiguration());
            builder.ApplyConfiguration(new RoleConfiguration());
        }

        #endregion
    }
}
