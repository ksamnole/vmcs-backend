using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VMCS.Core.Domains.Users;

namespace VMCS.Data.Contexts.Mapping;

public static class UserMapper
{
    public static EntityTypeBuilder<User> Setup(this EntityTypeBuilder<User> typeBuilder)
    {
        typeBuilder.Property(x => x.Login).IsRequired();
        typeBuilder.Property(x => x.Username).IsRequired();
        typeBuilder.Property(x => x.Email).IsRequired();

        return typeBuilder;
    }
}