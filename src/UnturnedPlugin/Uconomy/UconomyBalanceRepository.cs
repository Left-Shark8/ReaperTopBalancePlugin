using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using MySql.Data.MySqlClient;

namespace UnturnedPlugin.Uconomy;

public sealed class UconomyBalanceRepository
{
    private static readonly Regex IdentifierPattern = new("^[A-Za-z0-9_]+$", RegexOptions.Compiled);

    private readonly PluginConfiguration configuration;

    public UconomyBalanceRepository(PluginConfiguration configuration)
    {
        this.configuration = configuration;
    }

    public IReadOnlyList<UconomyBalanceEntry> GetTopBalances(int count)
    {
        var table = EscapeIdentifier(configuration.UconomyTable);
        var steamIdColumn = EscapeIdentifier(configuration.UconomySteamIdColumn);
        var balanceColumn = EscapeIdentifier(configuration.UconomyBalanceColumn);

        var entries = new List<UconomyBalanceEntry>();
        var query = $"SELECT {steamIdColumn}, {balanceColumn} FROM {table} ORDER BY {balanceColumn} DESC LIMIT @count;";

        using var connection = new MySqlConnection(BuildConnectionString());
        using var command = new MySqlCommand(query, connection);
        command.Parameters.AddWithValue("@count", count);

        connection.Open();

        using var reader = command.ExecuteReader();
        while (reader.Read())
        {
            entries.Add(new UconomyBalanceEntry(
                Convert.ToString(reader[0]) ?? "Unknown",
                Convert.ToDecimal(reader[1])));
        }

        return entries;
    }

    private string BuildConnectionString()
    {
        var builder = new MySqlConnectionStringBuilder
        {
            Server = configuration.UconomyHost,
            Port = configuration.UconomyPort,
            Database = configuration.UconomyDatabase,
            UserID = configuration.UconomyUsername,
            Password = configuration.UconomyPassword,
            SslMode = MySqlSslMode.None,
        };

        return builder.ConnectionString;
    }

    private static string EscapeIdentifier(string identifier)
    {
        if (!IdentifierPattern.IsMatch(identifier))
        {
            throw new InvalidOperationException($"Invalid database identifier in config: {identifier}");
        }

        return $"`{identifier}`";
    }
}
