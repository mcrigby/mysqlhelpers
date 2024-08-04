namespace Microsoft.EntityFrameworkCore.Design;

public sealed class MySqlDesignTimeDbContextCredentials
{
    /// <summary>
    /// MySQL Server Address, inc. Port if non-standard
    /// </summary>
    public string Server { get; set; }
    /// <summary>
    /// Host name or IP Address of the client
    /// </summary>
    public string Host { get; set; }

    /// <summary>
    /// Database Name
    /// </summary>
    public string Database { get; set; }

    public bool NoRootAccess { get; set; }

    /// <summary>
    /// User name of the Root User. Must be accessible from Host.
    /// </summary>
    public string RootUser { get; set; }
    /// <summary>
    /// Password of the Root User.
    /// </summary>
    public string RootPassword { get; set; }

    /// <summary>
    /// User name of the User to login to the Client Database.
    /// </summary>
    public string ClientUser { get; set; }
    /// <summary>
    /// Password of the User to login to the Client Database.
    /// </summary>
    public string ClientPassword { get; set; }
}
