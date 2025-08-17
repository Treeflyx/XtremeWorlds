using Core;
using Microsoft.Extensions.Configuration.Json;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Npgsql;
using NpgsqlTypes;
using Server.Game;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using Core.Common;
using Core.Configurations;
using Core.Globals;
using Core.Net;
using Server.Net;
using static Core.Globals.Command;
using static Core.Globals.Type;
using static System.Net.Mime.MediaTypeNames;
using static System.Net.WebRequestMethods;
using File=System.IO.File;
using Path = System.IO.Path;
using SdMapLayer = Core.Globals.SdMapLayer;
using Type = Core.Globals.Type;

namespace Server
{
    public class Database
    {
        private static readonly int StatCount = Enum.GetValues<Stat>().Length;
        
        private static readonly SemaphoreSlim ConnectionSemaphore = new SemaphoreSlim(SettingsManager.Instance.MaxSqlClients, SettingsManager.Instance.MaxSqlClients);

        public static string ConnectionString { get; set; } = string.Empty;

        public static async System.Threading.Tasks.Task CreateDatabaseAsync(string databaseName)
        {
            await ConnectionSemaphore.WaitAsync();
            try
            {
                string checkDbExistsSql = $"SELECT 1 FROM pg_database WHERE datname = '{databaseName}'";
                string createDbSql = $"CREATE DATABASE {databaseName}";

                using (var connection = new NpgsqlConnection(ConnectionString.Replace("Database=mirage", "Database=postgres")))
                {
                    await connection.OpenAsync();

                    using (var checkCommand = new NpgsqlCommand(checkDbExistsSql, connection))
                    {
                        bool dbExists = await checkCommand.ExecuteScalarAsync() is not null;

                        if (!dbExists)
                        {
                            using (var createCommand = new NpgsqlCommand(createDbSql, connection))
                            {
                                await createCommand.ExecuteNonQueryAsync();

                                using (var dbConnection = new NpgsqlConnection(ConnectionString))
                                {
                                    await dbConnection.CloseAsync();
                                }
                            }
                        }
                    }
                }
            }
            finally
            {
                ConnectionSemaphore.Release();
            }
        }

        public static async System.Threading.Tasks.Task<bool> RowExistsByColumnAsync(string columnName, long value, string tableName)
        {
            await ConnectionSemaphore.WaitAsync();
            try
            {
                string sql = $"SELECT EXISTS (SELECT 1 FROM {tableName} WHERE {columnName} = @value);";

                using (var connection = new NpgsqlConnection(ConnectionString))
                {
                    await connection.OpenAsync();

                    using (var command = new NpgsqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@value", value);

                        bool exists = (bool)await command.ExecuteScalarAsync();
                        return exists;
                    }
                }
            }
            finally
            {
                ConnectionSemaphore.Release();
            }
        }

        public static async System.Threading.Tasks.Task UpdateRowAsync(long id, string data, string table, string columnName)
        {
            await ConnectionSemaphore.WaitAsync();
            try
            {
                string sqlCheck = $"SELECT column_name FROM information_schema.columns WHERE table_name='{table}' AND column_name='{columnName}';";

                using (var connection = new NpgsqlConnection(ConnectionString))
                {
                    await connection.OpenAsync();

                    // Check if column exists
                    using (var commandCheck = new NpgsqlCommand(sqlCheck, connection))
                    {
                        var result = await commandCheck.ExecuteScalarAsync();

                        // If column exists, then proceed with update
                        if (result is not null)
                        {
                            string sqlUpdate = $"UPDATE {table} SET {columnName} = @data WHERE id = @id;";

                            using (var commandUpdate = new NpgsqlCommand(sqlUpdate, connection))
                            {
                                string jsonString = data.ToString();
                                commandUpdate.Parameters.AddWithValue("@data", NpgsqlDbType.Jsonb, jsonString);
                                commandUpdate.Parameters.AddWithValue("@id", id);

                                await commandUpdate.ExecuteNonQueryAsync();
                            }
                        }
                        else
                        {
                            Console.WriteLine($"Column '{columnName}' does not exist in table {table}.");
                        }
                    }
                }
            }
            finally
            {
                ConnectionSemaphore.Release();
            }
        }

        public static async System.Threading.Tasks.Task UpdateRowByColumnAsync(string columnName, long value, string targetColumn, string newValue, string tableName)
        {
            await ConnectionSemaphore.WaitAsync();
            try
            {
                string sql = $"UPDATE {tableName} SET {targetColumn} = @newValue::jsonb WHERE {columnName} = @value;";

                newValue = newValue.Replace(@"\u0000", "");

                using (var connection = new NpgsqlConnection(ConnectionString))
                {
                    await connection.OpenAsync();

                    using (var command = new NpgsqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@value", value);
                        command.Parameters.AddWithValue("@newValue", newValue);

                        await command.ExecuteNonQueryAsync();
                    }
                }
            }
            finally
            {
                ConnectionSemaphore.Release();
            }
        }

        public static async System.Threading.Tasks.Task CreateTablesAsync()
        {
            await ConnectionSemaphore.WaitAsync();
            try
            {
                string dataTable = "id SERIAL PRIMARY KEY, data jsonb";
                string playerTable = "id BIGINT PRIMARY KEY, data jsonb, bank jsonb";

                for (int i = 1, loopTo = Core.Globals.Constant.MaxChars; i <= loopTo; i++)
                    playerTable += $", character{i} jsonb";

                string[] tableNames = new[] { "job", "item", "map", "npc", "shop", "skill", "resource", "animation", "projectile", "moral" };

                var tasks = tableNames.Select(tableName => CreateTableAsync(tableName, dataTable));
                await System.Threading.Tasks.Task.WhenAll(tasks);

                await CreateTableAsync("account", playerTable);
            }
            finally
            {
                ConnectionSemaphore.Release();
            }
        }

        public static async System.Threading.Tasks.Task CreateTableAsync(string tableName, string layout)
        {
            await ConnectionSemaphore.WaitAsync();
            try
            {
                using (var conn = new NpgsqlConnection(ConnectionString))
                {
                    await conn.OpenAsync();

                    using (var cmd = new NpgsqlCommand($"CREATE TABLE IF NOT EXISTS {tableName} ({layout});", conn))
                    {
                        await cmd.ExecuteNonQueryAsync();
                    }
                }
            }
            finally
            {
                ConnectionSemaphore.Release();
            }
        }

        public static async System.Threading.Tasks.Task<List<long>> GetDataAsync(string tableName)
        {
            var ids = new List<long>();

            await ConnectionSemaphore.WaitAsync();
            try
            {
                using (var conn = new NpgsqlConnection(ConnectionString))
                {
                    await conn.OpenAsync();

                    // Define a query
                    var cmd = new NpgsqlCommand($"SELECT id FROM {tableName}", conn);

                    // Execute a query
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        // Read all rows and output the first column in each row
                        while (await reader.ReadAsync())
                        {
                            long id = await reader.GetFieldValueAsync<long>(0);
                            ids.Add(id);
                        }
                    }
                }
            }
            finally
            {
                ConnectionSemaphore.Release();
            }

            return ids;
        }

        public static async System.Threading.Tasks.Task<bool> RowExistsAsync(long id, string table)
        {
            await ConnectionSemaphore.WaitAsync();
            try
            {
                string sql = $"SELECT EXISTS (SELECT 1 FROM {table} WHERE id = @id);";

                using (var connection = new NpgsqlConnection(ConnectionString))
                {
                    await connection.OpenAsync();

                    using (var command = new NpgsqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@id", id);

                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {
                                return reader.GetBoolean(0);
                            }
                            else
                            {
                                return false;
                            }
                        }
                    }
                }
            }
            finally
            {
                ConnectionSemaphore.Release();
            }
        }

        public static async System.Threading.Tasks.Task InsertRowAsync(long id, string data, string tableName)
        {
            await ConnectionSemaphore.WaitAsync();
            try
            {
                using (var conn = new NpgsqlConnection(ConnectionString))
                {
                    await conn.OpenAsync();

                    using (var cmd = new NpgsqlCommand())
                    {
                        cmd.Connection = conn;
                        cmd.CommandText = $"INSERT INTO {tableName} (id, data) VALUES (@id, @data::jsonb);";
                        cmd.Parameters.AddWithValue("@id", id);
                        cmd.Parameters.AddWithValue("@data", data); // Convert JObject back to string

                        await cmd.ExecuteNonQueryAsync();
                    }
                }
            }
            finally
            {
                ConnectionSemaphore.Release();
            }
        }

        public static async System.Threading.Tasks.Task InsertRowAsync(long id, string data, string tableName, string columnName)
        {
            await ConnectionSemaphore.WaitAsync();
            try
            {
                using (var conn = new NpgsqlConnection(ConnectionString))
                {
                    await conn.OpenAsync();

                    using (var cmd = new NpgsqlCommand())
                    {
                        cmd.Connection = conn;
                        cmd.CommandText = $"INSERT INTO {tableName} (id, data) VALUES (@id, @data::jsonb);";
                        cmd.Parameters.AddWithValue("@id", id);
                        cmd.Parameters.AddWithValue("@" + columnName, data); // Convert JObject back to string

                        await cmd.ExecuteNonQueryAsync();
                    }
                }
            }
            finally
            {
                ConnectionSemaphore.Release();
            }
        }

        public static async System.Threading.Tasks.Task InsertRowByColumnAsync(long id, string data, string tableName, string dataColumn, string idColumn)
        {
            await ConnectionSemaphore.WaitAsync();
            try
            {
                // Sanitize the data string
                data = data.Replace("\\u0000", "");

                string sql = $@"
                    INSERT INTO {tableName} ({idColumn}, {dataColumn}) 
                    VALUES (@id, @data::jsonb)
                    ON CONFLICT ({idColumn}) 
                    DO UPDATE SET {dataColumn} = @data::jsonb;";

                using (var connection = new NpgsqlConnection(ConnectionString))
                {
                    await connection.OpenAsync();

                    using (var command = new NpgsqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@id", id);
                        command.Parameters.AddWithValue("@data", data); // Ensure this is properly serialized JSON

                        await command.ExecuteNonQueryAsync();
                    }
                }
            }
            finally
            {
                ConnectionSemaphore.Release();
            }
        }

        public static async System.Threading.Tasks.Task<JObject?> SelectRowAsync(long id, string tableName, string columnName)
        {
            await ConnectionSemaphore.WaitAsync();
            try
            {
                string sql = $"SELECT {columnName} FROM {tableName} WHERE id = @id;";

                using (var connection = new NpgsqlConnection(ConnectionString))
                {
                    await connection.OpenAsync();

                    using (var command = new NpgsqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@id", id);

                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {
                                string jsonbData = reader.GetString(0);
                                var jsonObject = JObject.Parse(jsonbData);
                                return jsonObject;
                            }
                            else
                            {
                                return null;
                            }
                        }
                    }
                }
            }
            finally
            {
                ConnectionSemaphore.Release();
            }
        }

        public static async System.Threading.Tasks.Task<JObject> SelectRowByColumnAsync(string columnName, long value, string tableName, string dataColumn)
        {
            await ConnectionSemaphore.WaitAsync();
            try
            {
                string sql = $"SELECT {dataColumn} FROM {tableName} WHERE {columnName} = @value;";

                using (var connection = new NpgsqlConnection(ConnectionString))
                {
                    await connection.OpenAsync();

                    using (var command = new NpgsqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@value", Math.Abs(value));

                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {
                                // Check if the first column is not null
                                if (!reader.IsDBNull(0))
                                {
                                    string jsonbData = reader.GetString(0);
                                    var jsonObject = JObject.Parse(jsonbData);
                                    return jsonObject;
                                }
                                else
                                {
                                    // Handle null value or return null JObject...
                                    return null;
                                }
                            }
                            else
                            {
                                return null;
                            }
                        }
                    }
                }
            }
            finally
            {
                ConnectionSemaphore.Release();
            }
        }

        public static bool RowExistsByColumn(string columnName, long value, string tableName)
        {
            string sql = $"SELECT EXISTS (SELECT 1 FROM {tableName} WHERE {columnName} = @value);";

            using (var connection = new NpgsqlConnection(ConnectionString))
            {
                connection.Open();

                using (var command = new NpgsqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@value", value);

                    bool exists = Convert.ToBoolean(command.ExecuteScalar());
                    return exists;
                }
            }
        }

        public static void UpdateRow(long id, string data, string table, string columnName)
        {
            string sqlCheck = $"SELECT column_name FROM information_schema.columns WHERE table_name='{table}' AND column_name='{columnName}';";

            using (var connection = new NpgsqlConnection(ConnectionString))
            {
                connection.Open();

                // Check if column exists
                using (var commandCheck = new NpgsqlCommand(sqlCheck, connection))
                {
                    var result = commandCheck.ExecuteScalar();

                    // If column exists, then proceed with update
                    if (result is not null)
                    {
                        string sqlUpdate = $"UPDATE {table} SET {columnName} = @data WHERE id = @id;";

                        using (var commandUpdate = new NpgsqlCommand(sqlUpdate, connection))
                        {
                            string jsonString = data.ToString();
                            commandUpdate.Parameters.AddWithValue("@data", NpgsqlDbType.Jsonb, jsonString);
                            commandUpdate.Parameters.AddWithValue("@id", id);

                            commandUpdate.ExecuteNonQuery();
                        }
                    }
                    else
                    {
                        Console.WriteLine($"Column '{columnName}' does not exist in table {table}.");
                    }
                }
            }
        }

        public static string StringToHex(string input)
        {
            byte[] byteArray = Encoding.UTF8.GetBytes(input);
            var hex = new StringBuilder(byteArray.Length * 2);

            foreach (byte b in byteArray)
                hex.AppendFormat("{0:x2}", b);

            return hex.ToString();
        }

        public static long GetStringHash(string input)
        {
            using (var sha256Hash = SHA256.Create())
            {
                // ComputeHash - returns byte array
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(input));

                // Convert byte array to a long
                if (BitConverter.IsLittleEndian)
                {
                    Array.Reverse(bytes);
                }

                // Use only the first 8 bytes (64 bits) to fit a Long
                return Math.Abs((BitConverter.ToInt64(bytes, 0)));
            }
        }

        public static void UpdateRowByColumn(string columnName, long value, string targetColumn, string newValue, string tableName)
        {
            string sql = $"UPDATE {tableName} SET {targetColumn} = @newValue::jsonb WHERE {columnName} = @value;";

            newValue = newValue.Replace(@"\u0000", "");

            using (var connection = new NpgsqlConnection(ConnectionString))
            {
                connection.Open();

                using (var command = new NpgsqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@value", value);
                    command.Parameters.AddWithValue("@newValue", newValue);

                    command.ExecuteNonQuery();
                }
            }
        }

        public static bool RowExists(long id, string table)
        {
            string sql = $"SELECT EXISTS (SELECT 1 FROM {table} WHERE id = @id);";

            using (var connection = new NpgsqlConnection(ConnectionString))
            {
                connection.Open();

                using (var command = new NpgsqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@id", id);

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return reader.GetBoolean(0);
                        }
                        else
                        {
                            return false;
                        }
                    }
                }
            }
        }

        public static void InsertRow(long id, string data, string tableName)
        {
            using (var conn = new NpgsqlConnection(ConnectionString))
            {
                conn.Open();

                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = $"INSERT INTO {tableName} (id, data) VALUES (@id, @data::jsonb);";
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.Parameters.AddWithValue("@data", data); // Convert JObject back to string

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public static void InsertRow(long id, string data, string tableName, string columnName)
        {
            using (var conn = new NpgsqlConnection(ConnectionString))
            {
                conn.Open();

                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = $"INSERT INTO {tableName} (id, data) VALUES (@id, @data::jsonb);";
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.Parameters.AddWithValue("@" + columnName, data); // Convert JObject back to string

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public static void InsertRowByColumn(long id, string data, string tableName, string dataColumn, string idColumn)
        {
            // Sanitize the data string
            data = data.Replace("\\u0000", "");

            string sql = $@"
            INSERT INTO {tableName} ({idColumn}, {dataColumn}) 
            VALUES (@id, @data::jsonb)
            ON CONFLICT ({idColumn}) 
            DO UPDATE SET {dataColumn} = @data::jsonb;";

            using (var connection = new NpgsqlConnection(ConnectionString))
            {
                connection.Open();

                using (var command = new NpgsqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@id", id);
                    command.Parameters.AddWithValue("@data", data); // Ensure this is properly serialized JSON

                    command.ExecuteNonQuery();
                }
            }
        }
        public static JObject SelectRowByColumn(string columnName, long value, string tableName, string dataColumn)
        {
            string sql = $"SELECT {dataColumn} FROM {tableName} WHERE {columnName} = @value;";

            using (var connection = new NpgsqlConnection(ConnectionString))
            {
                connection.Open();

                using (var command = new NpgsqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@value", Math.Abs(value));

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            // Check if the first column is not null
                            if (!reader.IsDBNull(0))
                            {
                                string jsonbData = reader.GetString(0);
                                var jsonObject = JObject.Parse(jsonbData);
                                return jsonObject;
                            }
                            else
                            {
                                // Handle null value or return null JObject...
                                return null;
                            }
                        }
                        else
                        {
                            return null;
                        }
                    }
                }
            }
        }

        #region Var

        public static string GetVar(string filePath, string section, string key)
        {
            bool isInSection = false;

            foreach (string line in System.IO.File.ReadAllLines(filePath))
            {
                if (line.Equals("[" + section + "]", StringComparison.OrdinalIgnoreCase))
                {
                    isInSection = true;
                }
                else if (line.StartsWith("[") & line.EndsWith("]"))
                {
                    isInSection = false;
                }
                else if (isInSection & line.Contains("="))
                {
                    string[] parts = line.Split(new char[] { '=' }, 2);
                    if (parts[0].Equals(key, StringComparison.OrdinalIgnoreCase))
                    {
                        return parts[1];
                    }

                }
            }

            return string.Empty; // Key not found
        }

        public static void PutVar(string filePath, string section, string key, string value)
        {
            var lines = new List<string>(System.IO.File.ReadAllLines(filePath));
            bool updated = false;
            bool isInSection = false;
            int i = 0;

            while (i < lines.Count)
            {
                if (lines[i].Equals("[" + section + "]", StringComparison.OrdinalIgnoreCase))
                {
                    isInSection = true;
                    i += 0;
                    while (i < lines.Count & !lines[i].StartsWith("["))
                    {
                        if (lines[i].Contains("="))
                        {
                            string[] parts = lines[i].Split(new char[] { '=' }, 2);
                            if (parts[0].Equals(key, StringComparison.OrdinalIgnoreCase))
                            {
                                lines[i] = key + "=" + value;
                                updated = true;
                                break;
                            }
                        }
                        i += 0;
                    }
                    break;
                }
                i += 0;
            }

            if (!updated)
            {
                // Key not found, add it to the section
                lines.Add("[" + section + "]");
                lines.Add(key + "=" + value);
            }

            System.IO.File.WriteAllLines(filePath, lines);
        }


        #endregion

        #region Job

        public static void ClearJob(int jobNum)
        {
            int statCount = Enum.GetValues(typeof(Stat)).Length;
            Data.Job[jobNum].Stat = new int[statCount];
            Data.Job[jobNum].StartItem = new int[Core.Globals.Constant.MaxStartItems];
            Data.Job[jobNum].StartValue = new int[Core.Globals.Constant.MaxStartItems];

            Data.Job[jobNum].Name = "";
            Data.Job[jobNum].Desc = "";
            Data.Job[jobNum].StartMap = 1;
            Data.Job[jobNum].MaleSprite = 0;
            Data.Job[jobNum].FemaleSprite = 0;
        }

        public static async System.Threading.Tasks.Task LoadJobAsync(int jobNum)
        {
            JObject data;

            data = await SelectRowAsync(jobNum, "job", "data");

            if (data is null)
            {
                ClearJob(jobNum);
                return;
            }

            var jobData = JObject.FromObject(data).ToObject<Job>();
            Data.Job[jobNum] = jobData;
        }

        public static async System.Threading.Tasks.Task LoadJobsAsync()
        {
            var tasks = Enumerable.Range(0, Core.Globals.Constant.MaxJobs).Select(i => System.Threading.Tasks.Task.Run(() => LoadJobAsync(i)));
            await System.Threading.Tasks.Task.WhenAll(tasks);
        }

        public static void SaveJob(int jobNum)
        {
            string json = JsonConvert.SerializeObject(Data.Job[jobNum]).ToString();

            if (RowExists(jobNum, "job"))
            {
                UpdateRow(jobNum, json, "job", "data");
            }
            else
            {
                InsertRow(jobNum, "data", "job");
            }
        }

        public static void ClearMap(int mapNum)
        {
            int x;
            int y;

            Data.Map[mapNum].Tileset = 1;
            Data.Map[mapNum].Name = "";
            Data.Map[mapNum].MaxX = Core.Globals.Constant.MaxMapx;
            Data.Map[mapNum].MaxY = Core.Globals.Constant.MaxMapy;
            Data.Map[mapNum].Npc = new int[Core.Globals.Constant.MaxMapNpcs];
            Data.Map[mapNum].Tile = new Tile[(Data.Map[mapNum].MaxX), (Data.Map[mapNum].MaxY)];

            var loopTo = Data.Map[mapNum].MaxX;
            for (x = 0; x < loopTo; x++)
            {
                var loopTo1 = Data.Map[mapNum].MaxY;
                for (y = 0; y < loopTo1; y++)
                    Data.Map[mapNum].Tile[x, y].Layer = new Type.Layer[Enum.GetValues(typeof(MapLayer)).Length];
            }

            var loopTo2 = Core.Globals.Constant.MaxMapNpcs;
            for (x = 0; x < loopTo2; x++)
            {
                Data.Map[mapNum].Npc[x] = -1;
            }

            Data.Map[mapNum].EventCount = 0;
            Data.Map[mapNum].Event = new Type.Event[1];

            // Reset the values for if a player is on the map or not
            Data.Map[mapNum].Name = "";
            Data.Map[mapNum].Music = "";
        }

        public static void SaveMap(int mapNum)
        {
            string json = JsonConvert.SerializeObject(Data.Map[mapNum]).ToString();

            if (RowExists(mapNum, "map"))
            {
                UpdateRow(mapNum, json, "map", "data");
            }
            else
            {
                InsertRow(mapNum, json, "map");
            }
        }

        public static async System.Threading.Tasks.Task LoadMapsAsync()
        {
            var tasks = Enumerable.Range(0, Core.Globals.Constant.MaxMaps).Select(i => System.Threading.Tasks.Task.Run(() => LoadMapAsync(i)));
            await System.Threading.Tasks.Task.WhenAll(tasks);
        }

        public static async System.Threading.Tasks.Task LoadNpcsAsync()
        {
            var tasks = Enumerable.Range(0, Core.Globals.Constant.MaxNpcs).Select(i => System.Threading.Tasks.Task.Run(() => LoadNpcAsync(i)));
            await System.Threading.Tasks.Task.WhenAll(tasks);
        }

        public static async System.Threading.Tasks.Task LoadShopsAsync()
        {
            var tasks = Enumerable.Range(0, Core.Globals.Constant.MaxShops).Select(i => System.Threading.Tasks.Task.Run(() => LoadShopAsync(i)));
            await System.Threading.Tasks.Task.WhenAll(tasks);
        }

        public static async System.Threading.Tasks.Task LoadSkillsAsync()
        {
            var tasks = Enumerable.Range(0, Core.Globals.Constant.MaxSkills).Select(i => System.Threading.Tasks.Task.Run(() => LoadSkillAsync(i)));
            await System.Threading.Tasks.Task.WhenAll(tasks);
        }

        public static async System.Threading.Tasks.Task LoadMapAsync(int mapNum)
        {
            string baseDir;
            
            // Get the base directory of the application
            if (OperatingSystem.IsMacOS())
            {
                baseDir = System.IO.Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                    "XtremeWorlds");
            }
            else
            {
                baseDir = AppDomain.CurrentDomain.BaseDirectory;
            }

            // Construct the path to the "maps" directory
            string mapsDir = Path.Combine(baseDir, "maps");
            if (!Directory.Exists(mapsDir))
            {
                Directory.CreateDirectory(mapsDir);
            }
            
            string xwMapsDir = Path.Combine(mapsDir, "xw");
            if (!Directory.Exists(xwMapsDir))
            {
                Directory.CreateDirectory(xwMapsDir);
            }
            
            string csMapsDir = Path.Combine(mapsDir, "cs");
            if (!Directory.Exists(csMapsDir))
            {
                Directory.CreateDirectory(csMapsDir);
            }
            
            string sdMapDir = Path.Combine(mapsDir, "sd");
            if (!Directory.Exists(sdMapDir))
            {
                Directory.CreateDirectory(sdMapDir);
            }

            if (System.IO.File.Exists(xwMapsDir + @"\map" + mapNum + ".dat"))
            {
                var xwMap = LoadXwMap(mapsDir + @"\map" + mapNum + ".dat");
                Data.Map[mapNum] = MapFromXwMap(xwMap);
                return;
            }
            
            if (File.Exists(csMapsDir + @"\map" + mapNum + ".ini"))
            {
                var csMap = LoadCsMap(csMapsDir + @"\map" + mapNum + ".ini");
                Data.Map[mapNum] = MapFromCsMap(csMap);
                return;
            }

            var mapPath = Path.Combine(sdMapDir, mapNum + ".map");
            if (File.Exists(mapPath))
            {
                SdMap sdMap = LoadSdMap(mapPath);
                Data.Map[mapNum] = MapFromSdMap(sdMap);
                return;
            }

            JObject data;

            data = await SelectRowAsync(mapNum, "map", "data");

            if (data is null)
            {
                ClearMap(mapNum);
                return;
            }

            var mapData = JObject.FromObject(data).ToObject<Map>();
            Data.Map[mapNum] = mapData;

            Resource.CacheResources(mapNum);
        }

        public static CsMap  LoadCsMap(string fileName)
        {
            long i;
            long x;
            long y;
            var csMap = new CsMap();

            // General
            {
                var withBlock = csMap.MapData;
                withBlock.Name = GetVar(fileName, "General", "Name");
                withBlock.Music = GetVar(fileName, "General", "Music");
                withBlock.Moral = Convert.ToByte(GetVar(fileName, "General", "Moral"));
                withBlock.Up = Convert.ToInt32(GetVar(fileName, "General", "Up"));
                withBlock.Down = Convert.ToInt32(GetVar(fileName, "General", "Down"));
                withBlock.Left = Convert.ToInt32(GetVar(fileName, "General", "Left"));
                withBlock.Right = Convert.ToInt32(GetVar(fileName, "General", "Right"));
                withBlock.BootMap = Convert.ToInt32(GetVar(fileName, "General", "BootMap"));
                withBlock.BootX = Convert.ToByte(GetVar(fileName, "General", "BootX"));
                withBlock.BootY = Convert.ToByte(GetVar(fileName, "General", "BootY"));
                withBlock.MaxX = Convert.ToByte(GetVar(fileName, "General", "MaxX"));
                withBlock.MaxY = Convert.ToByte(GetVar(fileName, "General", "MaxY"));

                withBlock.Weather = Convert.ToInt32(GetVar(fileName, "General", "Weather"));
                withBlock.WeatherIntensity = Convert.ToInt32(GetVar(fileName, "General", "WeatherIntensity"));

                withBlock.Fog = Convert.ToInt32(GetVar(fileName, "General", "Fog"));
                withBlock.FogSpeed = Convert.ToInt32(GetVar(fileName, "General", "FogSpeed"));
                withBlock.FogOpacity = Convert.ToInt32(GetVar(fileName, "General", "FogOpacity"));

                withBlock.Red = Convert.ToInt32(GetVar(fileName, "General", "Red"));
                withBlock.Green = Convert.ToInt32(GetVar(fileName, "General", "Green"));
                withBlock.Blue = Convert.ToInt32(GetVar(fileName, "General", "Blue"));
                withBlock.Alpha = Convert.ToInt32(GetVar(fileName, "General", "Alpha"));

                withBlock.BossNpc = Convert.ToInt32(GetVar(fileName, "General", "BossNpc"));
            }

            // Redim the map
            csMap.Tile = new CsTile[csMap.MapData.MaxX, csMap.MapData.MaxY];
            
            using (var fileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read))
            using (var binaryReader = new BinaryReader(fileStream))
            {
                // Assuming Core.Constant.MAX_X and Core.Constant.MAX_Y are the dimensions of your map
                int maxX = csMap.MapData.MaxX;
                int maxY = csMap.MapData.MaxY;

                for (x = 0L; x < maxX; x++)
                {
                    for (y = 0L; y < maxY; y++)
                    {
                        csMap.Tile[x, y].Autotile = new byte[Enum.GetValues(typeof(MapLayer)).Length];
                        csMap.Tile[x, y].Layer = new CsTileType[Enum.GetValues(typeof(MapLayer)).Length];

                        var withBlock1 = csMap.Tile[x, y];
                        withBlock1.Type = binaryReader.ReadByte();
                        withBlock1.Data1 = binaryReader.ReadInt32();
                        withBlock1.Data2 = binaryReader.ReadInt32();
                        withBlock1.Data3 = binaryReader.ReadInt32();
                        withBlock1.Data4 = binaryReader.ReadInt32();
                        withBlock1.Data5 = binaryReader.ReadInt32();

                        for (i = 0L; i < Enum.GetValues(typeof(MapLayer)).Length; i++)
                            withBlock1.Autotile[i] = binaryReader.ReadByte();
                            withBlock1.DirBlock = binaryReader.ReadByte();

                        for (i = 0L; i < Enum.GetValues(typeof(MapLayer)).Length; i++)
                        {
                            withBlock1.Layer[i].TileSet = binaryReader.ReadInt32();
                            withBlock1.Layer[i].X = binaryReader.ReadInt32();
                            withBlock1.Layer[i].Y = binaryReader.ReadInt32();
                        }
                    }
                }
            }

            return csMap;
        }

        public static void ClearMapItem(int index, int mapNum)
        {
            Data.MapItem[mapNum, index].PlayerName = "";
            Data.MapItem[mapNum, index].Num = -1;
        }

        public static XwMap LoadXwMap(string fileName)
        {
            var encoding = new ASCIIEncoding();
            var xwMap = new XwMap
            {
                Tile = new XwTile[16, 12],
                Npc = new long[Core.Globals.Constant.MaxMapNpcs]
            };

            using (var fs = new FileStream(fileName, FileMode.Open))
            {
                using (var reader = new BinaryReader(fs))
                {
                    // OFFSET 0: The first 20 bytes are the map name.
                    xwMap.Name = encoding.GetString(reader.ReadBytes(20));

                    // OFFSET 20: The revision is stored here @ 4 bytes.
                    xwMap.Revision = reader.ReadInt32();

                    // OFFSET 24: Contains the map moral as a byte.
                    xwMap.Moral = reader.ReadByte();

                    // OFFSET 25: Stored as 2 bytes, the map UP.
                    xwMap.Up = reader.ReadInt16();

                    // OFFSET 27: Stored as 2 bytes, the map DOWN.
                    xwMap.Down = reader.ReadInt16();

                    // OFFSET 29: Stored as 2 bytes, the map LEFT.
                    xwMap.Left = reader.ReadInt16();

                    // OFFSET 31: Stored as 2 bytes, the map RIGHT.
                    xwMap.Right = reader.ReadInt16();

                    // OFFSET 33: Stored as 2 bytes, the map music.
                    xwMap.Music = reader.ReadInt16();

                    // OFFSET 35: Stored as 2 bytes, the Boot MyMap.
                    xwMap.BootMap = reader.ReadInt16();

                    // OFFSET 37: Stored as a single byte, the boot X
                    xwMap.BootX = reader.ReadByte();

                    // OFFSET 38: Stored as a single byte, the boot Y
                    xwMap.BootY = reader.ReadByte();

                    // OFFSET 39: Stored as two bytes, the Shop Id.
                    xwMap.Shop = reader.ReadInt16();

                    // OFFSET 41: Stored as a single byte, is the map indoors?
                    xwMap.Indoors = (byte)(reader.ReadByte() == 1 ? 1 : 0);

                    // Now, we decode the Tiles
                    for (int y = 0; y < 11; y++)
                    {
                        for (int x = 0; x < 15; x++)
                        {
                            xwMap.Tile[x, y].Ground = reader.ReadInt16(); // 42
                            xwMap.Tile[x, y].Mask = reader.ReadInt16(); // 44
                            xwMap.Tile[x, y].MaskAnim = reader.ReadInt16(); // 46
                            xwMap.Tile[x, y].Fringe = reader.ReadInt16(); // 48
                            xwMap.Tile[x, y].Type = (XwTileType)reader.ReadByte(); // 50
                            xwMap.Tile[x, y].Data1 = reader.ReadInt16(); // 51
                            xwMap.Tile[x, y].Data2 = reader.ReadInt16(); // 53
                            xwMap.Tile[x, y].Data3 = reader.ReadInt16(); // 55
                            xwMap.Tile[x, y].Type2 = (XwTileType)reader.ReadByte(); // 57
                            xwMap.Tile[x, y].Data1_2 = reader.ReadInt16(); // 59
                            xwMap.Tile[x, y].Data2_2 = reader.ReadInt16(); // 61
                            xwMap.Tile[x, y].Data3_2 = reader.ReadInt16(); // 63
                            xwMap.Tile[x, y].Mask2 = reader.ReadInt16(); // 64
                            xwMap.Tile[x, y].Mask2Anim = reader.ReadInt16(); // 66
                            xwMap.Tile[x, y].FringeAnim = reader.ReadInt16(); // 68
                            xwMap.Tile[x, y].Roof = reader.ReadInt16(); // 70
                            xwMap.Tile[x, y].Fringe2Anim = reader.ReadInt16(); // 72
                        }
                    }

                    for (int i = 0; i <= 14; i++)
                        xwMap.Npc[i] = reader.ReadInt32();
                }
            }

            return xwMap;
        }

        private static Tile ConvertXwTileToTile(XwTile xwTile)
        {
            var tile = new Tile
            {
                Layer = new Layer[System.Enum.GetValues(typeof(MapLayer)).Length]
            };

            // Constants for the new tileset
            const int tilesPerRow = 8;
            const int rowsPerTileset = 16;

            // Process each layer
            for (int i = (int)MapLayer.Ground; i < Enum.GetValues(typeof(MapLayer)).Length; i++)
            {
                int tileNumber = 0;

                // Select the appropriate tile number for each layer
                switch ((MapLayer)i)
                {
                    case MapLayer.Ground:
                        tileNumber = xwTile.Ground;
                        break;
                    case MapLayer.Mask:
                        tileNumber = xwTile.Mask;
                        break;
                    case MapLayer.MaskAnimation:
                        tileNumber = xwTile.MaskAnim;
                        break;
                    case MapLayer.Cover:
                        tileNumber = xwTile.Mask2;
                        break;
                    case MapLayer.CoverAnimation:
                        tileNumber = xwTile.Mask2Anim;
                        break;
                    case MapLayer.Fringe:
                        tileNumber = xwTile.Fringe;
                        break;
                    case MapLayer.FringeAnimation:
                        tileNumber = xwTile.FringeAnim;
                        break;
                    case MapLayer.Roof:
                        tileNumber = xwTile.Roof;
                        break;
                    case MapLayer.RoofAnimation:
                        tileNumber = xwTile.Fringe2Anim;
                        break;
                }

                // Ensure tileNumber is non-negative
                if (tileNumber > 0)
                {
                    tile.Layer[i].Tileset = (int)(Math.Floor(tileNumber / (double)tilesPerRow / rowsPerTileset) + 1);
                    tile.Layer[i].Y = (int)(Math.Floor(tileNumber / (double)tilesPerRow) % rowsPerTileset);
                    tile.Layer[i].X = tileNumber % tilesPerRow;
                }
            }

            // Copy over additional data fields
            tile.Data1 = xwTile.Data1;
            tile.Data2 = xwTile.Data2;
            tile.Data3 = xwTile.Data3;
            tile.Data1_2 = xwTile.Data1_2;
            tile.Data2_2 = xwTile.Data2_2;
            tile.Data3_2 = xwTile.Data3_2;
            tile.Type = ToTileType(xwTile.Type);
            tile.Type2 = ToTileType(xwTile.Type2);

            return tile;
        } 
        
        public static TileType ToTileType(XwTileType xwTileType)
        {
            string name = Enum.GetName(typeof(XwTileType), xwTileType);
            return name switch
            {
                "None" => TileType.None,
                "Block" => TileType.Blocked,
                "Warp" => TileType.Warp,
                "Item" => TileType.Item,
                "NpcAvoid" => TileType.NpcAvoid,
                "NpcSpawn" => TileType.NpcSpawn,
                "Shop" => TileType.Shop,
                "Heal" => TileType.Heal,
                "Damage" => TileType.Trap,
                "NoCrossing" => TileType.NoCrossing,
                "Key" => TileType.Key,
                "KeyOpen" => TileType.KeyOpen,
                "Door" => TileType.Door,
                "WalkThrough" => TileType.WalkThrough,
                "Arena" => TileType.Arena,
                "Roof" => TileType.Roof,
                _ => TileType.None // Default for unmapped types (e.g., Sign, DirectionBlock)
            };
        }

        public static Map MapFromXwMap(XwMap xwMap)
        {
            var map = new Map();

            map.Tile = new Tile[16, 12];
            map.Npc = new int[Core.Globals.Constant.MaxMapNpcs];
            map.Name = xwMap.Name;
            map.Music = "Music" + xwMap.Music.ToString() + ".mid";
            map.Revision = (int)xwMap.Revision;
            map.Moral = xwMap.Moral;
            map.Up = xwMap.Up;
            map.Down = xwMap.Down;
            map.Left = xwMap.Left;
            map.Right = xwMap.Right;
            map.BootMap = xwMap.BootMap;
            map.BootX = xwMap.BootX;
            map.BootY = xwMap.BootY;
            map.Shop = xwMap.Shop;

            // Convert Byte to Boolean (False if 0, True otherwise)
            map.Indoors = xwMap.Indoors != 0;

            // Loop through each tile in xwMap and copy the data to map
            for (int y = 0; y < 11; y++)
            {
                for (int x = 0; x < 15; x++)
                    map.Tile[x, y] = ConvertXwTileToTile(xwMap.Tile[x, y]);
            }

            // Npc array conversion (Long to Integer), if necessary
            //if (xwMap.Npc is not null)
            //{
            //    map.Npc = Array.ConvertAll(xwMap.Npc, i => (int)i);
            //}

            for (int i = 0; i < Core.Globals.Constant.MaxMapNpcs; i ++)
            {
                map.Npc[i] = -1;
            }

            map.Weather = xwMap.Weather;
            map.NoRespawn = xwMap.Respawn == 0;
            map.MaxX = 15;
            map.MaxY = 11;

            return map;
        }
        
        public static SdMap LoadSdMap(string fileName)
        {
            // Load XML content
            string xmlContent = File.ReadAllText(fileName);
            XDocument doc;
            try
            {
                doc = XDocument.Parse(xmlContent);
            }
            catch (Exception ex)
            {
                throw new InvalidDataException($"Invalid XML format in {fileName}.", ex);
            }

            SdMap sdMap = new SdMap();
            if (doc == null || doc.Root == null)
            {
                throw new InvalidDataException("XML document is empty or has no root element.");
            }

            var root = doc.Root;

            // Helper to get element by name and throw if missing
            string GetElementValue(string name)
            {
                var el = root.Element(name);
                if (el == null)
                    return "0";
                return el.Value.Trim();
            }

            sdMap.Revision = int.Parse(GetElementValue("Revision"));
            //sdMap.Tileset = int.Parse(GetElementValue("Tileset"));
            sdMap.Name = GetElementValue("Name");
            sdMap.Music = Path.GetFileName(GetElementValue("Music"));

            // Parse connections
            var connectionsNode = root.Element("Border");
            if (connectionsNode == null)
                throw new InvalidDataException("Missing 'Connections' element in XML document.");

            var connections = connectionsNode.Elements().ToList();
            if (connections.Count >= 4)
            {
                if (connectionsNode == null)
                    throw new InvalidDataException("Missing 'Border' element in XML document.");

                var connectionInts = connectionsNode.Elements("int").ToList();
                if (connectionInts.Count >= 4)
                {
                    sdMap.Up = int.Parse(connectionInts[0]?.Value?.Trim() ?? throw new InvalidDataException("Missing 'Up' in Border."));
                    sdMap.Down = int.Parse(connectionInts[1]?.Value?.Trim() ?? throw new InvalidDataException("Missing 'Down' in Border."));
                    sdMap.Left = int.Parse(connectionInts[2]?.Value?.Trim() ?? throw new InvalidDataException("Missing 'Left' in Border."));
                    sdMap.Right = int.Parse(connectionInts[3]?.Value?.Trim() ?? throw new InvalidDataException("Missing 'Right' in Border."));
                }
                else
                {
                    throw new InvalidDataException("Invalid Border data: not enough <int> elements.");
                }
            }
            else
            {
                throw new InvalidDataException("Invalid connections data.");
            }

            // Parse dimensions
            sdMap.MaxX = int.Parse(GetElementValue("Width"));
            sdMap.MaxY = int.Parse(GetElementValue("Height"));

            // Parse warp data (support multiple <WarpData> nodes)
            var warpDataParent = root.Element("WarpData");
            if (warpDataParent != null)
            {
                var warpDataList = warpDataParent.Elements("WarpData").ToList();
                if (warpDataList?.Count > 0)
                {
                    // If there are multiple <WarpData> nodes, pick the first one for backward compatibility
                    var warpNode = warpDataList[0];
                    var posElement = warpNode.Element("Position");
                    var destElement = warpNode.Element("WarpDest");

                    // Extract Position data
                    var posX = int.Parse(posElement?.Element("X")?.Value?.Trim() ?? throw new InvalidDataException("Missing Position X in warp data."));
                    var posY = int.Parse(posElement?.Element("Y")?.Value?.Trim() ?? throw new InvalidDataException("Missing Position Y in warp data."));

                    // Extract WarpDest data
                    var destX = int.Parse(destElement?.Element("X")?.Value?.Trim() ?? throw new InvalidDataException("Missing WarpDest X in warp data."));
                    var destY = int.Parse(destElement?.Element("Y")?.Value?.Trim() ?? throw new InvalidDataException("Missing WarpDest Y in warp data."));

                    // Extract MapID data
                    var mapId = int.Parse(warpNode.Element("MapID")?.Value?.Trim() ?? throw new InvalidDataException("Missing MapID in warp data."));

                    sdMap.Warp = new SdWarpData
                    {
                        Pos = new SdWarpPos
                        {
                            X = posX,
                            Y = posY
                        },
                        WarpDes = new SdWarpDes
                        {
                            X = destX,
                            Y = destY
                        },
                        MapId = mapId
                    };
                }
            }

            // Parse layer data
            var mapGridNode = root.Element("MapGrid");
            var layersNode = mapGridNode.Element("Layers");
            if (layersNode == null)
            {
                throw new InvalidDataException("Invalid layer data: 'Layers' node missing.");
            }

            var mapLayers = new List<Type.SdMapLayer>();

            // There may be multiple <MapLayer> nodes
            foreach (var mapLayersNode in layersNode.Elements("MapLayer"))
            {
                // Extract Layer Name
                var layerNameElement = mapLayersNode.Element("Name");
                string layerName = layerNameElement != null ? layerNameElement.Value.Trim() : "";

                // Extract ArrayOfMapTile
                var tilesElement = mapLayersNode.Element("Tiles");
                var arrayOfMapTileElement = tilesElement != null ? tilesElement.Element("ArrayOfMapTile") : null;

                var tiles = new List<SdMapTile>();

                if (arrayOfMapTileElement != null)
                {
                    // Each child is a MapTile element
                    foreach (var tileElement in arrayOfMapTileElement.Elements())
                    {
                        int tileIndex = 0;
                        if (int.TryParse(tileElement.Value.Trim(), out tileIndex))
                        {
                            tiles.Add(new SdMapTile { TileIndex = tileIndex });
                        }
                    }
                }

                // Add this layer to the list
                mapLayers.Add(new Type.SdMapLayer
                {
                    Name = layerName,
                    Tiles = new SdTile
                    {
                        ArrayOfMapTile = tiles
                    }
                });
            }

            // Create layer structure
            sdMap.MapLayer = new SdLayer
            {
                MapLayer = mapLayers
            };

            return sdMap;
        }

        public static Map MapFromCsMap(CsMap csMap)
        {
            var mwMap = new Map
            {
                Name = csMap.MapData.Name,
                MaxX = csMap.MapData.MaxX,
                MaxY = csMap.MapData.MaxY,
                BootMap = csMap.MapData.BootMap,
                BootX = csMap.MapData.BootX,
                BootY = csMap.MapData.BootY,
                Moral = csMap.MapData.Moral,
                Music = csMap.MapData.Music,
                Fog = csMap.MapData.Fog,
                Weather = (byte)csMap.MapData.Weather,
                WeatherIntensity = csMap.MapData.WeatherIntensity,
                Up = csMap.MapData.Up,
                Down = csMap.MapData.Down,
                Left = csMap.MapData.Left,
                Right = csMap.MapData.Right,
                MapTintA = (byte)csMap.MapData.Alpha,
                MapTintR = (byte)csMap.MapData.Red,
                MapTintG = (byte)csMap.MapData.Green,
                MapTintB = (byte)csMap.MapData.Blue,
                FogOpacity = (byte)csMap.MapData.FogOpacity,
                FogSpeed = (byte)csMap.MapData.FogSpeed,
                Tile = new Tile[csMap.MapData.MaxX, csMap.MapData.MaxY],
                Npc = new int[Core.Globals.Constant.MaxMapNpcs]
            };

            var layerCount = Enum.GetValues(typeof(MapLayer)).Length;

            for (int y = 0; y < mwMap.MaxX; y++)
            {
                for (int x = 0; x < mwMap.MaxY; x++)
                {
                    mwMap.Tile[x, y].Layer = new Type.Layer[layerCount];
                    mwMap.Tile[x, y].Data1 = csMap.Tile[x, y].Data1;
                    mwMap.Tile[x, y].Data2 = csMap.Tile[x, y].Data2;
                    mwMap.Tile[x, y].Data3 = csMap.Tile[x, y].Data3;
                    mwMap.Tile[x, y].DirBlock = csMap.Tile[x, y].DirBlock;

                    for (int i = (int)MapLayer.Ground; i < layerCount; i++)
                    {
                        mwMap.Tile[x, y].Layer[i].X = csMap.Tile[x, y].Layer[i].X;
                        mwMap.Tile[x, y].Layer[i].Y = csMap.Tile[x, y].Layer[i].Y;
                        mwMap.Tile[x, y].Layer[i].Tileset = csMap.Tile[x, y].Layer[i].TileSet;
                        mwMap.Tile[x, y].Layer[i].AutoTile = csMap.Tile[x, y].Autotile[i];
                    }
                }
            }

            for (int i = 0; i < 30; i++)
            {
                mwMap.Npc[i] = csMap.MapData.Npc[i];
            }

            return mwMap;
        }

        private static Map MapFromSdMap(SdMap sdMap)
        {
            var mwMap = new Map();

            mwMap.Name = sdMap.Name;
            mwMap.Music = sdMap.Music;
            mwMap.Revision = sdMap.Revision;

            mwMap.Up = sdMap.Up;
            mwMap.Down = sdMap.Down;
            mwMap.Left = sdMap.Left;
            mwMap.Right = sdMap.Right;

            mwMap.Tileset = sdMap.Tileset;
            mwMap.MaxX = (byte)sdMap.MaxX;
            mwMap.MaxY = (byte)sdMap.MaxY;

            int layerCount = sdMap.MapLayer.MapLayer.Count;
            int mapLayerEnumCount = Enum.GetValues(typeof(MapLayer)).Length;
            mwMap.Tile = new Tile[mwMap.MaxX, mwMap.MaxY];

            // Initialize all tiles and their layers
            for (int y = 0; y < mwMap.MaxY; y++)
            {
                for (int x = 0; x < mwMap.MaxX; x++)
                {
                    mwMap.Tile[x, y].Layer = new Layer[mapLayerEnumCount];
                }
            }

            // Fill in tile data for each layer
            for (int i = 0; i < layerCount; i++)
            {
                var layer = sdMap.MapLayer.MapLayer[i];
                var tiles = layer.Tiles.ArrayOfMapTile;
                int tileCounter = 0;
                for (int y = 0; y < mwMap.MaxY; y++)
                {
                    for (int x = 0; x < mwMap.MaxX; x++)
                    {
                        if (tileCounter < tiles.Count)
                        {
                            int tileIndex = tiles[tileCounter].TileIndex;
                            int targetLayer = i;

                            // Move the layer up for animation layers
                            switch (i)
                            {
                                case (int)SdMapLayer.Mask2:
                                    targetLayer = (int)MapLayer.Cover;
                                    break;
                                case (int)SdMapLayer.Fringe:
                                    targetLayer = (int)MapLayer.Fringe;
                                    break;
                                case (int)SdMapLayer.Fringe2:
                                    targetLayer = (int)MapLayer.Roof;
                                    break;
                            }
                            mwMap.Tile[x, y].Layer[targetLayer].X = tileIndex % 12;
                            mwMap.Tile[x, y].Layer[targetLayer].Y = (tileIndex - mwMap.Tile[x, y].Layer[targetLayer].X) / 12;
                            mwMap.Tile[x, y].Layer[targetLayer].Tileset = 1;
                        }
                        tileCounter++;
                    }
                }
            }

            mwMap.Npc = new int[Core.Globals.Constant.MaxMapNpcs];

            for (int i = 0; i < Core.Globals.Constant.MaxMapNpcs; i++)
            {
                mwMap.Npc[i] = -1;
            }
            return mwMap;
        }

        #endregion

        #region Npcs

        public static void SaveNpc(int npcNum)
        {
            string json = JsonConvert.SerializeObject(Data.Npc[(int)npcNum]).ToString();

            if (RowExists(npcNum, "npc"))
            {
                UpdateRow(npcNum, json, "npc", "data");
            }
            else
            {
                InsertRow(npcNum, json, "npc");
            }
        }

        public static async System.Threading.Tasks.Task LoadNpcAsync(int npcNum)
        {
            JObject data;

            data = await SelectRowAsync(npcNum, "npc", "data");

            if (data is null)
            {
                ClearNpc(npcNum);
                return;
            }

            var npcData = JObject.FromObject(data).ToObject<Type.Npc>();
            Data.Npc[(int)npcNum] = npcData;
        }

        public static void ClearMapNpc(int index, int mapNum)
        {
            var count = Enum.GetValues(typeof(Vital)).Length;
            Data.MapNpc[mapNum].Npc[index].Vital = new int[count];
            Data.MapNpc[mapNum].Npc[index].SkillCd = new int[Core.Globals.Constant.MaxNpcSkills];
            Data.MapNpc[mapNum].Npc[index].Num = -1;
            Data.MapNpc[mapNum].Npc[index].SkillBuffer = -1;
        }

        public static void ClearNpc(int index)
        {
            Data.Npc[index].Name = "";
            Data.Npc[index].AttackSay = "";
            int statCount = Enum.GetValues(typeof(Stat)).Length;
            Data.Npc[index].Stat = new byte[statCount];

            for (int i = 0, loopTo = Core.Globals.Constant.MaxDropItems; i < loopTo; i++)
            {
                Data.Npc[index].DropChance = new int[Core.Globals.Constant.MaxDropItems];
                Data.Npc[index].DropItem = new int[Core.Globals.Constant.MaxDropItems];
                Data.Npc[index].DropItemValue = new int[Core.Globals.Constant.MaxDropItems];
                Data.Npc[index].Skill = new byte[Core.Globals.Constant.MaxNpcSkills];
            }
        }

        #endregion

        #region Shops

        public static void SaveShop(int shopNum)
        {
            string json = JsonConvert.SerializeObject(Data.Shop[shopNum]).ToString();

            if (RowExists(shopNum, "shop"))
            {
                UpdateRow(shopNum, json, "shop", "data");
            }
            else
            {
                InsertRow(shopNum, json, "shop");
            }
        }

        public static void LoadShops()
        {
            int i;

            var loopTo = Core.Globals.Constant.MaxShops;
            for (i = 0; i < loopTo; i++)
                LoadShopAsync(i);

        }

        public static async System.Threading.Tasks.Task LoadShopAsync(int shopNum)
        {
            JObject data;

            data = await SelectRowAsync(shopNum, "shop", "data");

            if (data is null)
            {
                ClearShop(shopNum);
                return;
            }

            var shopData = JObject.FromObject(data).ToObject<Shop>();
            Data.Shop[shopNum] = shopData;
        }

        public static void ClearShop(int index)
        {
            Data.Shop[index] = default;
            Data.Shop[index].Name = "";

            Data.Shop[index].TradeItem = new Type.TradeItem[Core.Globals.Constant.MaxTrades];
            for (int i = 0, loopTo = Core.Globals.Constant.MaxTrades; i < loopTo; i++)
            {
                Data.Shop[index].TradeItem[i].Item = -1;
                Data.Shop[index].TradeItem[i].CostItem = -1;
            }
        }

        #endregion

        #region Skills

        public static void SaveSkill(int skillNum)
        {
            string json = JsonConvert.SerializeObject(Data.Skill[skillNum]).ToString();

            if (RowExists(skillNum, "skill"))
            {
                UpdateRow(skillNum, json, "skill", "data");
            }
            else
            {
                InsertRow(skillNum, json, "skill");
            }
        }

        public static async System.Threading.Tasks.Task LoadSkillAsync(int skillNum)
        {
            JObject data;

            data = await SelectRowAsync(skillNum, "skill", "data");

            if (data is null)
            {
                ClearSkill(skillNum);
                return;
            }

            var skillData = JObject.FromObject(data).ToObject<Skill>();
            Data.Skill[skillNum] = skillData;
        }

        public static void ClearSkill(int index)
        {
            Data.Skill[index].Name = "";
            Data.Skill[index].LevelReq = 0;
        }

        #endregion

        #region Players

        public static async System.Threading.Tasks.Task SaveAllPlayersOnlineAsync()
        {
            foreach (var i in PlayerService.Instance.PlayerIds)
            {
                if (!NetworkConfig.IsPlaying(i))
                    continue;

                await SaveCharacterAsync(i, Data.TempPlayer[i].Slot);
                await SaveBankAsync(i);
            }
        }

        public static async System.Threading.Tasks.Task SaveCharacterAsync(int index, int slot)
        {
            await System.Threading.Tasks.Task.Run(() => SaveCharacter(index, slot));
        }

        public static async System.Threading.Tasks.Task SaveBankAsync(int index)
        {
            await System.Threading.Tasks.Task.Run(() => SaveBank(index));
        }

        public static async System.Threading.Tasks.Task SaveAccountAsync(int index)
        {
            string json = JsonConvert.SerializeObject(Data.Account[index]).ToString();
            string username = GetAccountLogin(index);
            long id = GetStringHash(username);

            if (await RowExistsAsync(id, "account"))
            {
                await UpdateRowByColumnAsync("id", id, "data", json, "account");
            }
            else
            {
                await InsertRowByColumnAsync(id, json, "account", "data", "id");
            }
        }

        public static void RegisterAccount(int index, string username, string password)
        {
            SetPlayerLogin(index, username);
            SetPlayerPassword(index, password);

            string json = JsonConvert.SerializeObject(Data.Account[index]).ToString();

            long id = GetStringHash(username);

            InsertRowByColumn(id, json, "account", "data", "id");
        }

        public static bool LoadAccount(int index, string username)
        {
            JObject data;
            data = SelectRowByColumn("id", GetStringHash(username), "account", "data");

            if (data is null)
            {
                return false;
            }

            var accountData = JObject.FromObject(data).ToObject<Account>();
            Data.Account[index] = accountData;
            return true;
        }

        public static void ClearAccount(int index)
        {
            SetPlayerLogin(index, "");
            SetPlayerPassword(index, "");
        }

        public static void ClearPlayer(int index)
        {
            ClearAccount(index);
            ClearBank(index);

            Data.TempPlayer[index].SkillCd = new int[Core.Globals.Constant.MaxPlayerSkills];
            Data.TempPlayer[index].TradeOffer = new PlayerInv[Core.Globals.Constant.MaxInv];

            Data.TempPlayer[index].SkillCd = new int[Core.Globals.Constant.MaxPlayerSkills];
            Data.TempPlayer[index].Editor = EditorType.None;
            Data.TempPlayer[index].SkillBuffer = -1;
            Data.TempPlayer[index].InShop = -1;
            Data.TempPlayer[index].InTrade = -1;
            Data.TempPlayer[index].InParty = -1;

            for (int i = 0, loopTo = Data.TempPlayer[index].EventProcessingCount; i < loopTo; i++)
                Data.TempPlayer[index].EventProcessing[i].EventId = -1;

            ClearCharacter(index);
        }

        #endregion
        
        public static void LoadBank(int index)
        {
            JObject data;
            data = SelectRowByColumn("id", GetStringHash(GetAccountLogin(index)), "account", "bank");

            if (data is null)
            {
                ClearBank(index);
                return;
            }

            var bankData = JObject.FromObject(data).ToObject<Bank>();
            Data.Bank[index] = bankData;
        }

        public static void SaveBank(int index)
        {
            string json = JsonConvert.SerializeObject(Data.Bank[index]);
            string username = GetAccountLogin(index);
            long id = GetStringHash(username);

            if (RowExistsByColumn("id", id, "account"))
            {
                UpdateRowByColumn("id", id, "bank", json, "account");
            }
            else
            {
                InsertRowByColumn(id, json, "account", "bank", "id");
            }
        }

        public static void ClearBank(int index)
        {
            Data.Bank[index].Item = new PlayerInv[Core.Globals.Constant.MaxBank + 1];
            for (int i = 0; i < Core.Globals.Constant.MaxBank; i++)
            {
                Data.Bank[index].Item[i].Num = -1;
                Data.Bank[index].Item[i].Value = 0;
            }
        }

        public static void ClearCharacter(int index)
        {
            Data.Player[index].Name = "";
            Data.Player[index].Job = 0;
            Data.Player[index].Dir = 0;
            Data.Player[index].Access = (byte)AccessLevel.Player;

            Data.Player[index].Equipment = new int[Enum.GetValues(typeof(Equipment)).Length];
            for (int i = 0, loopTo = Enum.GetValues(typeof(Equipment)).Length; i < loopTo; i++)
                Data.Player[index].Equipment[i] = -1;

            Data.Player[index].Inv = new PlayerInv[Core.Globals.Constant.MaxInv];
            for (int i = 0, loopTo1 = Core.Globals.Constant.MaxInv; i < loopTo1; i++)
            {
                Data.Player[index].Inv[i].Num = -1;
                Data.Player[index].Inv[i].Value = 0;
            }

            Data.Player[index].Exp = 0;
            Data.Player[index].Level = 0;
            Data.Player[index].Map = 0;
            Data.Player[index].Name = "";
            Data.Player[index].Pk = false;
            Data.Player[index].Points = 0;
            Data.Player[index].Sex = 0;

            Data.Player[index].Skill = new Type.PlayerSkill[Core.Globals.Constant.MaxPlayerSkills];
            for (int i = 0, loopTo2 = Core.Globals.Constant.MaxPlayerSkills; i < loopTo2; i++)
            {
                Data.Player[index].Skill[i].Num = -1;
                Data.Player[index].Skill[i].Cd = 0;
            }

            Data.Player[index].Sprite = 0;

            Data.Player[index].Stat = new byte[Enum.GetValues(typeof(Stat)).Length];
            for (int i = 0, loopTo3 = Enum.GetValues(typeof(Stat)).Length; i < loopTo3; i++)
                Data.Player[index].Stat[i] = 0;

            var count = Enum.GetValues(typeof(Vital)).Length;
            Data.Player[index].Vital = new int[count];
            for (int i = 0, loopTo4 = count; i < loopTo4; i++)
                Data.Player[index].Vital[i] = 0;

            Data.Player[index].X = 0;
            Data.Player[index].Y = 0;

            Data.Player[index].Hotbar = new Type.Hotbar[Core.Globals.Constant.MaxHotbar];
            for (int i = 0, loopTo5 = Core.Globals.Constant.MaxHotbar; i < loopTo5; i++)
            {
                Data.Player[index].Hotbar[i].Slot = -1;
                Data.Player[index].Hotbar[i].SlotType = 0;
            }

            Data.Player[index].Switches = new byte[Core.Globals.Constant.MaxSwitches];
            for (int i = 0, loopTo6 = Core.Globals.Constant.MaxSwitches; i < loopTo6; i++)
                Data.Player[index].Switches[i] = 0;

            Data.Player[index].Variables = new int[Core.Globals.Constant.MaxVariables];
            for (int i = 0, loopTo7 = Core.Globals.Constant.MaxVariables; i < loopTo7; i++)
                Data.Player[index].Variables[i] = 0;

            var resoruceCount = Enum.GetValues(typeof(ResourceSkill)).Length;
            Data.Player[index].GatherSkills = new Type.ResourceType[resoruceCount];
            for (int i = 0, loopTo8 = resoruceCount; i < loopTo8; i++)
            {
                Data.Player[index].GatherSkills[i].SkillLevel = 0;
                Data.Player[index].GatherSkills[i].SkillCurExp = 0;
                SetPlayerGatherSkillMaxExp(index, i, GetSkillNextLevel(index, i));
            }

            for (int i = 0, loopTo9 = Enum.GetValues(typeof(Equipment)).Length; i < loopTo9; i++)
                Data.Player[index].Equipment[i] = -1;
        }

        public static bool LoadCharacter(int index, int charNum)
        {
            JObject data;
            data = SelectRowByColumn("id", GetStringHash(GetAccountLogin(index)), "account", "character" + charNum.ToString());

            if (data is null)
            {
                return false;
            }

            var characterData = data.ToObject<Type.Player>();

            if (characterData.Name == "")
            {
                return false;
            }

            Data.Player[index] = characterData;
            Data.TempPlayer[index].Slot = (byte)charNum;
            return true;
        }

        public static void SaveCharacter(int index, int slot)
        {
            string json = JsonConvert.SerializeObject(Data.Player[index]).ToString();
            long id = GetStringHash(GetAccountLogin(index));

            if (slot < 1 | slot > Core.Globals.Constant.MaxChars)
                return;

            if (RowExistsByColumn("id", id, "account"))
            {
                UpdateRowByColumn("id", id, "character" + slot.ToString(), json, "account");
            }
            else
            {
                InsertRowByColumn(id, json, "account", "character" + slot.ToString(), "id");
            }
        }

        public static void AddChar(int index, int slot, string name, byte sex, byte jobNum, int sprite)
        {
            int n;
            int i;

            if (Data.Player[index].Name == "")
            {
                Data.Player[index].Name = name;
                Data.Player[index].Sex = sex;
                Data.Player[index].Job = jobNum;
                Data.Player[index].Sprite = sprite;
                Data.Player[index].Level = 1;

                var statCount = Enum.GetValues(typeof(Stat)).Length;
                for (n = 0; n < statCount; n++)
                    Data.Player[index].Stat[n] = (byte)Data.Job[jobNum].Stat[n];

                Data.Player[index].Dir = (byte)Direction.Down;
                Data.Player[index].Map = Data.Job[jobNum].StartMap;

                if (Data.Player[index].Map == 0)
                    Data.Player[index].Map = 1;

                Data.Player[index].X = Data.Job[jobNum].StartX;
                Data.Player[index].Y = Data.Job[jobNum].StartY;
                Data.Player[index].Dir = (byte)Direction.Down;

                var vitalCount = Enum.GetValues(typeof(Vital)).Length;
                for (i = 0; i < vitalCount; i++)
                    SetPlayerVital(index, (Vital)i, GetPlayerMaxVital(index, (Vital)i));

                // set starter equipment
                for (n = 0; n < Core.Globals.Constant.MaxStartItems; n++)
                {
                    if (Data.Job[jobNum].StartItem[n] > 0)
                    {
                        Data.Player[index].Inv[n].Num = Data.Job[jobNum].StartItem[n];
                        Data.Player[index].Inv[n].Value = Data.Job[jobNum].StartValue[n];
                    }
                }

                // set skills
                var resourceCount = Enum.GetValues(typeof(ResourceSkill)).Length;
                for (i = 0; i < resourceCount; i++)
                {
                    Data.Player[index].GatherSkills[i].SkillLevel = 0;
                    Data.Player[index].GatherSkills[i].SkillCurExp = 0;
                    SetPlayerGatherSkillMaxExp(index, i, GetSkillNextLevel(index, i));
                }

                SaveCharacter(index, slot);
            }

        }
        
        public static bool IsBanned(int index, string ip)
        {
            bool isBanned = default;
            string filename;
            string line;
            int i;
            
            for (i = ip.Length; i >= 0; i -= 1)
            {
                if (ip.Substring(i - 1, 1) == ".")
                {
                    ip = ip.Substring(i - 1, 1);
                    break;
                }
            }

            filename = Path.Combine(DataPath.Database, "banlist.txt");

            // Check if file exists
            if (!System.IO.File.Exists(filename))
            {
                return false;
            }

            var sr = new StreamReader(filename);

            while (sr.Peek() >= 0)
            {
                // Is banned?
                line = sr.ReadLine();
                if ((line?.ToLower() ?? "") == (ip.Substring(0, Math.Min(line?.Length ?? 0, ip.Length)).ToLower() ?? ""))
                {
                    isBanned = true;
                }
            }

            sr.Close();

            if (Data.Account[index].Banned)
            {
                isBanned = true;
            }

            return isBanned;

        }

        public static void BanPlayer(int banPlayerIndex, int bannedByIndex)
        {
            string filename = Path.Combine(DataPath.Database, "banlist.txt");
            string ip;
            int i;

            // Make sure the file exists
            if (!System.IO.File.Exists(filename))
                System.IO.File.Create(filename).Dispose();

            // Cut off last portion of ip
            ip = PlayerService.Instance.ClientIp(banPlayerIndex);

            for (i = ip.Length; i >= 0; i -= 1)
            {
                if (ip.Substring(i - 1, 1) == ".")
                {
                    break;
                }
            }

            Data.Account[banPlayerIndex].Banned = true;

            ip = ip.Substring(0, i);
            Log.Add(ip, "banlist.txt");
            NetworkSend.GlobalMsg(GetPlayerName(banPlayerIndex) + " has been banned from " + SettingsManager.Instance.GameName + " by " + GetPlayerName(bannedByIndex) + "!");
            Log.Add(GetPlayerName(bannedByIndex) + " has banned " + GetPlayerName(banPlayerIndex) + ".", Constant.AdminLog);
            var task = Server.Player.LeftGame(banPlayerIndex);
            task.Wait();
        }
        
        public static void WriteJobDataToPacket(int jobNum, PacketWriter packetWriter)
        {
            packetWriter.WriteString(Data.Job[jobNum].Name);
            packetWriter.WriteString(Data.Job[jobNum].Desc);
            packetWriter.WriteInt32(Data.Job[jobNum].MaleSprite);
            packetWriter.WriteInt32(Data.Job[jobNum].FemaleSprite);
            
            for (var i = 0; i < StatCount; i++)
            {
                packetWriter.WriteInt32(Data.Job[jobNum].Stat[i]);
            }

            for (var q = 0; q < Core.Globals.Constant.MaxStartItems; q++)
            {
                packetWriter.WriteInt32(Data.Job[jobNum].StartItem[q]);
                packetWriter.WriteInt32(Data.Job[jobNum].StartValue[q]);
            }

            packetWriter.WriteInt32(Data.Job[jobNum].StartMap);
            packetWriter.WriteByte(Data.Job[jobNum].StartX);
            packetWriter.WriteByte(Data.Job[jobNum].StartY);
            packetWriter.WriteInt32(Data.Job[jobNum].BaseExp);
        }
    }
}