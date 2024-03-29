﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using VMCS.Data.Contexts;

#nullable disable

namespace VMCS.Data.Migrations
{
    [DbContext(typeof(ApplicationContext))]
    [Migration("20230122154323_Add Access Token table")]
    partial class AddAccessTokentable
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("ChannelUser", b =>
                {
                    b.Property<string>("ChannelsId")
                        .HasColumnType("text");

                    b.Property<string>("UsersId")
                        .HasColumnType("text");

                    b.HasKey("ChannelsId", "UsersId");

                    b.HasIndex("UsersId");

                    b.ToTable("ChannelUser");
                });

            modelBuilder.Entity("MeetingUser", b =>
                {
                    b.Property<string>("MeetingsId")
                        .HasColumnType("text");

                    b.Property<string>("UsersId")
                        .HasColumnType("text");

                    b.HasKey("MeetingsId", "UsersId");

                    b.HasIndex("UsersId");

                    b.ToTable("MeetingUser");
                });

            modelBuilder.Entity("VMCS.Core.Domains.ChannelInvitations.ChannelInvitation", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<string>("ChannelId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("RecipientId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("SenderId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("ChannelId");

                    b.HasIndex("RecipientId");

                    b.HasIndex("SenderId");

                    b.ToTable("ChannelInvitations", (string)null);
                });

            modelBuilder.Entity("VMCS.Core.Domains.Channels.Channel", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<string>("ChatId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("CreatorId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("CreatorId");

                    b.ToTable("Channels", (string)null);
                });

            modelBuilder.Entity("VMCS.Core.Domains.Chats.Chat", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<string>("ChannelId")
                        .HasColumnType("text");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("MeetingId")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("ChannelId")
                        .IsUnique();

                    b.HasIndex("MeetingId")
                        .IsUnique();

                    b.ToTable("Chats", (string)null);
                });

            modelBuilder.Entity("VMCS.Core.Domains.Directories.Directory", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<string>("DirectoryInJson")
                        .HasColumnType("text");

                    b.Property<byte[]>("DirectoryZip")
                        .HasColumnType("bytea");

                    b.Property<string>("MeetingId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("MeetingId")
                        .IsUnique();

                    b.ToTable("Directories", (string)null);
                });

            modelBuilder.Entity("VMCS.Core.Domains.GitHub.AccessToken", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<string>("Token")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AccessTokens", (string)null);
                });

            modelBuilder.Entity("VMCS.Core.Domains.Meetings.Meeting", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<string>("ChannelId")
                        .HasColumnType("text");

                    b.Property<string>("ChatId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime?>("ClosedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("CreatorId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("DirectoryId")
                        .HasColumnType("text");

                    b.Property<bool>("IsInChannel")
                        .HasColumnType("boolean");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("ChannelId");

                    b.HasIndex("CreatorId");

                    b.ToTable("Meetings", (string)null);
                });

            modelBuilder.Entity("VMCS.Core.Domains.Messages.Message", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<string>("ChatId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime>("ModifiedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Text")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("UserId")
                        .HasColumnType("text");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("ChatId");

                    b.HasIndex("UserId");

                    b.ToTable("Messages", (string)null);
                });

            modelBuilder.Entity("VMCS.Core.Domains.Users.User", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Login")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Users", (string)null);
                });

            modelBuilder.Entity("ChannelUser", b =>
                {
                    b.HasOne("VMCS.Core.Domains.Channels.Channel", null)
                        .WithMany()
                        .HasForeignKey("ChannelsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("VMCS.Core.Domains.Users.User", null)
                        .WithMany()
                        .HasForeignKey("UsersId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("MeetingUser", b =>
                {
                    b.HasOne("VMCS.Core.Domains.Meetings.Meeting", null)
                        .WithMany()
                        .HasForeignKey("MeetingsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("VMCS.Core.Domains.Users.User", null)
                        .WithMany()
                        .HasForeignKey("UsersId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("VMCS.Core.Domains.ChannelInvitations.ChannelInvitation", b =>
                {
                    b.HasOne("VMCS.Core.Domains.Channels.Channel", "Channel")
                        .WithMany()
                        .HasForeignKey("ChannelId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("VMCS.Core.Domains.Users.User", "Recipient")
                        .WithMany()
                        .HasForeignKey("RecipientId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("VMCS.Core.Domains.Users.User", "Sender")
                        .WithMany()
                        .HasForeignKey("SenderId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Channel");

                    b.Navigation("Recipient");

                    b.Navigation("Sender");
                });

            modelBuilder.Entity("VMCS.Core.Domains.Channels.Channel", b =>
                {
                    b.HasOne("VMCS.Core.Domains.Users.User", "Creator")
                        .WithMany()
                        .HasForeignKey("CreatorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Creator");
                });

            modelBuilder.Entity("VMCS.Core.Domains.Chats.Chat", b =>
                {
                    b.HasOne("VMCS.Core.Domains.Channels.Channel", "Channel")
                        .WithOne("Chat")
                        .HasForeignKey("VMCS.Core.Domains.Chats.Chat", "ChannelId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("VMCS.Core.Domains.Meetings.Meeting", "Meeting")
                        .WithOne("Chat")
                        .HasForeignKey("VMCS.Core.Domains.Chats.Chat", "MeetingId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.Navigation("Channel");

                    b.Navigation("Meeting");
                });

            modelBuilder.Entity("VMCS.Core.Domains.Directories.Directory", b =>
                {
                    b.HasOne("VMCS.Core.Domains.Meetings.Meeting", "Meeting")
                        .WithOne("Directory")
                        .HasForeignKey("VMCS.Core.Domains.Directories.Directory", "MeetingId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Meeting");
                });

            modelBuilder.Entity("VMCS.Core.Domains.GitHub.AccessToken", b =>
                {
                    b.HasOne("VMCS.Core.Domains.Users.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("VMCS.Core.Domains.Meetings.Meeting", b =>
                {
                    b.HasOne("VMCS.Core.Domains.Channels.Channel", "Channel")
                        .WithMany("Meetings")
                        .HasForeignKey("ChannelId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("VMCS.Core.Domains.Users.User", "Creator")
                        .WithMany()
                        .HasForeignKey("CreatorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Channel");

                    b.Navigation("Creator");
                });

            modelBuilder.Entity("VMCS.Core.Domains.Messages.Message", b =>
                {
                    b.HasOne("VMCS.Core.Domains.Chats.Chat", "Chat")
                        .WithMany("Messages")
                        .HasForeignKey("ChatId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("VMCS.Core.Domains.Users.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId");

                    b.Navigation("Chat");

                    b.Navigation("User");
                });

            modelBuilder.Entity("VMCS.Core.Domains.Channels.Channel", b =>
                {
                    b.Navigation("Chat")
                        .IsRequired();

                    b.Navigation("Meetings");
                });

            modelBuilder.Entity("VMCS.Core.Domains.Chats.Chat", b =>
                {
                    b.Navigation("Messages");
                });

            modelBuilder.Entity("VMCS.Core.Domains.Meetings.Meeting", b =>
                {
                    b.Navigation("Chat")
                        .IsRequired();

                    b.Navigation("Directory");
                });
#pragma warning restore 612, 618
        }
    }
}
