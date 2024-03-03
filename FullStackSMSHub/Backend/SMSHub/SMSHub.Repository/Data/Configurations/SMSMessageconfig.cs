using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;
using SMSHub.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace SMSHub.Repository.Data.Configurations
{
    public class SMSMessageconfig : IEntityTypeConfiguration<SmsMessage>
    {
        public void Configure(EntityTypeBuilder<SmsMessage> builder)
        {
            builder
           .HasOne(sm => sm.SenderId)
           .WithMany(s => s.SmsMessages)
           .HasForeignKey(sm => sm.SenderIdId);
        }
    }
}
