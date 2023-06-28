using Microsoft.EntityFrameworkCore;
using Struktura_drzewiasta.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Struktura_drzewiasta.Context
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<TreeNode> TreeNodes { get; set; }
        public ApplicationDbContext(DbContextOptions options) : base(options) { }
        //skonfigurowanie opcji połączenia z bazą danych SQL Server dla naszego kontekstu
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // Ustawiamy nazwę bazy danych
            optionsBuilder.UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=TreeNodes;Trusted_Connection=True;",
                x => x.MigrationsHistoryTable("__EFMigrationsHistory", "Identity"));
        }
    }
}
