using DataServer.Domain;
using Microsoft.EntityFrameworkCore;
using System;

namespace DataServer.Infrastructure
{
    public sealed class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Entry> Entries { get; set; }
        public DbSet<Agent> Agents { get; set; }
        public DbSet<PermittedEntry> PermittedEntries { get; set; }
    }
}
