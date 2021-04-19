﻿using System;
using System.Collections.Generic;
using System.Linq;
using ClickHouse.Client.ADO;

namespace YY.EventLogExportAssistant.ClickHouse.Helpers
{
    public static class ClickHouseHelpers
    {
        public static Dictionary<string, string> GetConnectionParams(string connectionString)
        {
            var connectionParams = connectionString.Split(';', StringSplitOptions.RemoveEmptyEntries)
                .Select(p => p.Split('=', StringSplitOptions.RemoveEmptyEntries))
                .Select(i => new { Name = i[0], Value = i.Length > 1 ? i[1] : string.Empty });

            return connectionParams.ToDictionary(o => o.Name, o => o.Value);
        }
        public static void CreateDatabaseIfNotExist(string connectionSettings)
        {
            var connectionParams = GetConnectionParams(connectionSettings);
            var databaseParam = connectionParams.FirstOrDefault(e => e.Key.ToUpper() == "DATABASE");
            string databaseName = databaseParam.Value;

            if (databaseName != null)
            {
                string connectionStringDefault = connectionSettings.Replace(
                    $"{databaseParam.Key}={databaseParam.Value}",
                    $"Database=default"
                );
                using (var defaultConnection = new ClickHouseConnection(connectionStringDefault))
                {
                    defaultConnection.Open();
                    var cmdDefault = defaultConnection.CreateCommand();
                    cmdDefault.CommandText = $"CREATE DATABASE IF NOT EXISTS {databaseName} Engine=Ordinary;";
                    cmdDefault.ExecuteNonQuery();
                }
            }
        }
        public static void DropDatabaseIfExist(string connectionSettings)
        {
            var connectionParams = GetConnectionParams(connectionSettings);
            var databaseParam = connectionParams.FirstOrDefault(e => e.Key.ToUpper() == "DATABASE");
            string databaseName = databaseParam.Value;

            if (databaseName != null)
            {
                string connectionStringDefault = connectionSettings.Replace(
                    $"{databaseParam.Key}={databaseParam.Value}",
                    $"Database=default"
                );
                using (var defaultConnection = new ClickHouseConnection(connectionStringDefault))
                {
                    defaultConnection.Open();
                    var cmdDefault = defaultConnection.CreateCommand();
                    cmdDefault.CommandText = $"DROP DATABASE IF EXISTS {databaseName}";
                    cmdDefault.ExecuteNonQuery();
                }
            }
        }
    }
}
