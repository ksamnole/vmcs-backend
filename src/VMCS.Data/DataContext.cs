using Microsoft.EntityFrameworkCore;
using VMCS.Data.Users;

namespace VMCS.Data;

public class DataContext : DbContext
{
    // TODO: Create DbContext
    public DbSet<UserDbModel> Users { get; set; }
}