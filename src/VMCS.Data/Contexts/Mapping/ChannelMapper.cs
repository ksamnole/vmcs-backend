using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VMCS.Core.Domains.Channels;

namespace VMCS.Data.Contexts.Mapping;

public static class ChannelMapper
{
    public static EntityTypeBuilder<Channel> Setup(this EntityTypeBuilder<Channel> typeBuilder)
    {
        typeBuilder.HasOne(x => x.Creator);
        typeBuilder.HasMany(x => x.Users).WithMany(x => x.Channels);

        typeBuilder.Property(x => x.Name).IsRequired();   
        
        return typeBuilder;
    }
}