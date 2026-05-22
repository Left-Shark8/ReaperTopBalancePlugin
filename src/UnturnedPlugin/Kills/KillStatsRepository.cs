using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using MySql.Data.MySqlClient;

namespace ReaperLeaderboardPlugin.Kills;

public sealed class KillStatsRepository
{
    private static readonly Regex IdentifierPattern = new("^[A-Za-z0-9_]+$", RegexOptions.Compiled);

    private readonly PluginConfiguration configuration;

    public KillStatsRepository(PluginConfiguration configuration)
    {
        this.configuration = configuration;
    }

    public void EnsureTable()
    {
        var table = EscapeIdentifier(configuration.KillStatsTable);
        var query = $@"
CREATE TABLE IF NOT EXISTS {table} (
    steam_id VARCHAR(32) NOT NULL PRIMARY KEY,
    display_name VARCHAR(64) NOT NULL DEFAULT '',
    player_kills INT NOT NULL DEFAULT 0,
    zombie_kills INT NOT NULL DEFAULT 0,
    mega_zombie_kills INT NOT NULL DEFAULT 0
);";

        ExecuteNonQuery(query);
    }

    public void Increment(string steamId, string displayName, KillCategory category)
    {
        var table = EscapeIdentifier(configuration.KillStatsTable);
        var column = GetColumnName(category);
        var query = $@"
INSERT INTO {table} (steam_id, display_name, {column})
VALUES (@steamId, @displayName, 1)
ON DUPLICATE KEY UPDATE display_name = @displayName, {column} = {column} + 1;";

        using var connection = new MySqlConnection(BuildConnectionString());
        using var command = new MySqlCommand(query, connection);
        command.Parameters.AddWithValue("@steamId", steamId);
        command.Parameters.AddWithValue("@displayName", displayName);

        connection.Open();
        command.ExecuteNonQuery();
    }

    public KillStats GetStats(string steamId)
    {
        var table = EscapeIdentifier(configuration.KillStatsTable);
        var query = $@"
SELECT player_kills, zombie_kills, mega_zombie_kills
FROM {table}
WHERE steam_id = @steamId;";

        using var connection = new MySqlConnection(BuildConnectionString());
        using var command = new MySqlCommand(query, connection);
        command.Parameters.AddWithValue("@steamId", steamId);

        connection.Open();

        using var reader = command.ExecuteReader();
        if (!reader.Read())
        {
            return new KillStats(steamId, 0, 0, 0);
        }

        return new KillStats(
            steamId,
            Convert.ToInt32(reader["player_kills"]),
            Convert.ToInt32(reader["zombie_kills"]),
            Convert.ToInt32(reader["mega_zombie_kills"]));
    }

    public IReadOnlyList<KillLeaderboardEntry> GetTop(KillCategory category, int count)
    {
        var table = EscapeIdentifier(configuration.KillStatsTable);
        var column = GetColumnName(category);
        var query = $@"
SELECT steam_id, display_name, {column}
FROM {table}
WHERE {column} > 0
ORDER BY {column} DESC
LIMIT @count;";

        var entries = new List<KillLeaderboardEntry>();

        using var connection = new MySqlConnection(BuildConnectionString());
        using var command = new MySqlCommand(query, connection);
        command.Parameters.AddWithValue("@count", count);

        connection.Open();

        using var reader = command.ExecuteReader();
        while (reader.Read())
        {
            var steamId = Convert.ToString(reader["steam_id"]) ?? "Unknown";
            var displayName = Convert.ToString(reader["display_name"]);

            entries.Add(new KillLeaderboardEntry(
                steamId,
                string.IsNullOrWhiteSpace(displayName) ? steamId : displayName,
                Convert.ToInt32(reader[column])));
        }

        return entries;
    }

    private void ExecuteNonQuery(string query)
    {
        using var connection = new MySqlConnection(BuildConnectionString());
        using var command = new MySqlCommand(query, connection);

        connection.Open();
        command.ExecuteNonQuery();
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

    private static string GetColumnName(KillCategory category)
    {
        return category switch
        {
            KillCategory.Player => "player_kills",
            KillCategory.Zombie => "zombie_kills",
            KillCategory.MegaZombie => "mega_zombie_kills",
            _ => throw new ArgumentOutOfRangeException(nameof(category), category, null),
        };
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
