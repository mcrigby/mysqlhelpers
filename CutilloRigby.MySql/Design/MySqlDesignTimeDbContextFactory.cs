using System;

namespace Microsoft.EntityFrameworkCore.Design;

public abstract class MySqlDesignTimeDbContextFactory<TDbContext> : IDesignTimeDbContextFactory<TDbContext>
    where TDbContext : MySqlDbContext
{
    public abstract MySqlDesignTimeDbContextCredentials GetDbContextCredentials();

    public TDbContext CreateDbContext(string[] args)
    {
        var credentials = GetDbContextCredentials();
        var connectionString = $"Server={credentials.Server};Database={credentials.Database};Uid={credentials.ClientUser};Password={credentials.ClientPassword};";

        if (credentials.NoRootAccess)
            return GetDbContext(connectionString);

        #region Create Database and Login
        var createDb = $"CREATE DATABASE IF NOT EXISTS {credentials.Database};";
        var createUser = $"CREATE USER IF NOT EXISTS '{credentials.ClientUser}'@'{credentials.Host}' IDENTIFIED BY '{credentials.ClientPassword}';";
        var grantPermissions = $"GRANT ALL ON {credentials.Database}.* TO '{credentials.ClientUser}'@'{credentials.Host}';";
        var createMigrationsTable = $"CREATE TABLE IF NOT EXISTS {credentials.Database}.__EFMigrationsHistory (MigrationId NVARCHAR(150) NOT NULL, ProductVersion NVARCHAR(32) NOT NULL, PRIMARY KEY(MigrationId));";

        var dbContext = GetDbContext($"Server={credentials.Server};Database=mysql;Uid={credentials.RootUser};Password={credentials.RootPassword};");

#pragma warning disable EF1000 // Possible SQL injection vulnerability.
        dbContext.Database.ExecuteSqlRaw(createDb);
        dbContext.Database.ExecuteSqlRaw(createUser);
        dbContext.Database.ExecuteSqlRaw(grantPermissions);
        dbContext.Database.ExecuteSqlRaw(createMigrationsTable);
#pragma warning restore EF1000 // Possible SQL injection vulnerability.
        #endregion

        return GetDbContext(connectionString);
    }

    protected virtual TDbContext GetDbContext(string connectionString, Action<DbContextOptionsBuilder> build = null)
    {
        var serverVersion = ServerVersion.AutoDetect(connectionString);
        var optionsBuilder = new DbContextOptionsBuilder<TDbContext>();
        build?.Invoke(optionsBuilder);

        optionsBuilder.UseMySql(connectionString, serverVersion);

        return (TDbContext)Activator.CreateInstance(typeof(TDbContext), optionsBuilder.Options);
    }
}
