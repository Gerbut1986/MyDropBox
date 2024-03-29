﻿namespace WcfService
{
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class DropBoxCloudEntities : DbContext
    {
        public DropBoxCloudEntities() : base("name=DropBoxCloudEntities") { }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<FileSystemElements> FileSystemElements { get; set; }
        public virtual DbSet<Users> Users { get; set; }
    }
}
