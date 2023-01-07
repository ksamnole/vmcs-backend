using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VMCS.Core.Domains.Messages;

namespace VMCS.Data.Contexts.Mapping;

public static class MessageMapper
{
    public static EntityTypeBuilder<Message> Setup(this EntityTypeBuilder<Message> typeBuilder)
    {
        typeBuilder.Property(x => x.Text).IsRequired();

        return typeBuilder;
    }
}