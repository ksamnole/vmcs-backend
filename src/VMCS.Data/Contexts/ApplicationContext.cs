﻿using Microsoft.EntityFrameworkCore;
using VMCS.Data.Users;

namespace VMCS.Data.Contexts;

public class ApplicationContext : DbContext
{
    // TODO: Create DbContext
    public DbSet<UserDbModel> Users { get; set; }
    
    public ApplicationContext(DbContextOptions options) : base(options)
    {
    }
}