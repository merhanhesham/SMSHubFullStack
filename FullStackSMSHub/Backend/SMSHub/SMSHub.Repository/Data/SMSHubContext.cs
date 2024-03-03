using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using SMSHub.Core.Entities;
using SMSHub.Repository.Data;//.Configurations;
using SMSHub.Core.Entities.Identity;

namespace SMSHub.Repository.Data
{
    public class SMSHubContext : DbContext
    {
        public SMSHubContext(DbContextOptions<SMSHubContext> options):base(options)
        {
            
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //func that apply all existing configs
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            base.OnModelCreating(modelBuilder);
        }


        //dbsets 
        public DbSet<SmsMessage> SmsMessages { get; set; }
        public DbSet<SenderId> SenderIds { get; set; }
        public DbSet<Recipient> Recipients { get; set; }


        public DbSet<Template> Templates { get; set; }
        public DbSet<Users> Users { get; set; }


    }
}
