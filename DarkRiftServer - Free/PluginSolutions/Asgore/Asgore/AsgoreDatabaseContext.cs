using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
//nekwjhiokewh
namespace Asgore
{
    public class AsgoreDatabaseContext : DbContext
    {
        private readonly string dbFile;

        /// <summary>
        /// The standard database file path.
        /// </summary>
        public const string StandardDbFile = "database.sql";
        
        public AsgoreDatabaseContext(string dbFile)
        {
            this.dbFile = dbFile;
        }

        /// <summary>
        /// Gets or sets the data set for the users in the DB.
        /// </summary>
        public DbSet<User> Users { get; set; }
        
        /// <summary>
        /// <para>
        /// Override this method to configure the database (and other options) to be used for this context.
        /// This method is called for each instance of the context that is created.
        /// </para>
        /// <para>
        /// In situations where an instance of <see cref="T:Microsoft.Data.Entity.Infrastructure.DbContextOptions" /> may or may not have been passed
        /// to the constructor, you can use <see cref="P:Microsoft.Data.Entity.DbContextOptionsBuilder.IsConfigured" /> to determine if
        /// the options have already been set, and skip some or all of the logic in
        /// <see cref="M:Microsoft.Data.Entity.DbContext.OnConfiguring(Microsoft.Data.Entity.DbContextOptionsBuilder)" />.
        /// </para>
        /// </summary>
        /// <param name="optionsBuilder">A builder used to create or modify options for this context. Databases (and other extensions)
        /// typically define extension methods on this object that allow you to configure the context.</param>
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var connectionStringBuilder = new SqliteConnectionStringBuilder { DataSource = dbFile };
            var connection = new SqliteConnection(connectionStringBuilder.ToString());
            optionsBuilder.UseSqlite(connection);
        }
    }
}
