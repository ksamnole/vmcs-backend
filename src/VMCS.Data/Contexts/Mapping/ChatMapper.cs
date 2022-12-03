using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VMCS.Core.Domains.Chats;

namespace VMCS.Data.Contexts.Mapping;

public static class ChatMapper
{
    public static EntityTypeBuilder<Chat> Setup(this EntityTypeBuilder<Chat> typeBuilder)
    {
        return typeBuilder;
    }
}