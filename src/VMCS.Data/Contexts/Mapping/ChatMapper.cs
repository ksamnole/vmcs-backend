using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VMCS.Core.Domains.Channels;
using VMCS.Core.Domains.Chats;
using VMCS.Core.Domains.Meetings;

namespace VMCS.Data.Contexts.Mapping;

public static class ChatMapper
{
    public static EntityTypeBuilder<Chat> Setup(this EntityTypeBuilder<Chat> typeBuilder)
    {
        typeBuilder.HasOne(x => x.Channel)
            .WithOne(x => x.Chat)
            .HasForeignKey<Chat>(x => x.ChannelId)
            .OnDelete(DeleteBehavior.Cascade);
        
        typeBuilder.HasOne(x => x.Meeting)
            .WithOne(x => x.Chat)
            .HasForeignKey<Chat>(x => x.MeetingId)
            .OnDelete(DeleteBehavior.Cascade);
        
        return typeBuilder;
    }
}