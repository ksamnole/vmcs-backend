using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VMCS.Core.Domains.Meetings;

namespace VMCS.Data.Contexts.Mapping;

public static class MeetingMapper
{
    public static EntityTypeBuilder<Meeting> Setup(this EntityTypeBuilder<Meeting> typeBuilder)
    {
        typeBuilder.HasOne(x => x.Creator);
        typeBuilder.HasMany(x => x.Users).WithMany(x => x.Meetings);
        
        typeBuilder.Property(x => x.Name).IsRequired();
        typeBuilder.Property(x => x.IsInChannel).IsRequired();

        return typeBuilder;
    }
}