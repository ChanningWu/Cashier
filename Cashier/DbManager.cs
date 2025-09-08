namespace Cashier
{
    using System;
    using System.Linq;
    using Cashier.Data;
    using Microsoft.Data.Sqlite;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata;

    public static class DbManager
    {
        public static void EnsureDatabaseSynchronized(AppDbContext context)
        {
            using var conn = new SqliteConnection(context.Database.GetConnectionString());
            conn.Open();

            foreach (var entityType in context.Model.GetEntityTypes())
            {
                var tableName = entityType.GetTableName();
                if (tableName is not null)
                {
                    EnsureTable(conn, tableName, entityType);
                }
            }
        }

        private static void EnsureTable(SqliteConnection conn, string tableName, IEntityType entityType)
        {
            using var checkTableCmd = conn.CreateCommand();
            checkTableCmd.CommandText = $"SELECT name FROM sqlite_master WHERE type='table' AND name='{tableName}'";
            var exists = checkTableCmd.ExecuteScalar() != null;

            if (!exists)
            {
                // 如果表不存在，直接创建
                using var createCmd = conn.CreateCommand();
                createCmd.CommandText = entityType.GetCreateTableSql();
                createCmd.ExecuteNonQuery();
                return;
            }

            // 如果表存在，则检查缺失的字段
            var existingColumns = GetExistingColumns(conn, tableName);

            foreach (var prop in entityType.GetProperties())
            {
                var columnName = prop.GetColumnName(StoreObjectIdentifier.Table(tableName, null));
                if (!existingColumns.Contains(columnName, StringComparer.OrdinalIgnoreCase))
                {
                    var columnType = prop.GetColumnType() ?? prop.ClrType.ToSqliteType();
                    using var alterCmd = conn.CreateCommand();
                    alterCmd.CommandText = $"ALTER TABLE `{tableName}` ADD COLUMN `{columnName}` {columnType}";
                    alterCmd.ExecuteNonQuery();
                }
            }
        }

        // 扩展：从 IEntityType 生成建表 SQL（带 IF NOT EXISTS + 反引号转义）
        private static string GetCreateTableSql(this IEntityType entityType)
        {
            var tableName = entityType.GetTableName();
            var columns = entityType.GetProperties()
                .Select(p =>
                {
                    var columnName = p.GetColumnName(StoreObjectIdentifier.Table(tableName, null));
                    var columnType = p.GetColumnType() ?? p.ClrType.ToSqliteType();
                    return $"`{columnName}` {columnType}";
                });

            return $"CREATE TABLE IF NOT EXISTS `{tableName}` ({string.Join(", ", columns)})";
        }


        private static string[] GetExistingColumns(SqliteConnection conn, string tableName)
        {
            using var pragmaCmd = conn.CreateCommand();
            pragmaCmd.CommandText = $"PRAGMA table_info({tableName})";
            using var reader = pragmaCmd.ExecuteReader();

            var columns = new System.Collections.Generic.List<string>();
            while (reader.Read())
            {
                columns.Add(reader.GetString(1)); // 第2列是列名
            }
            return columns.ToArray();
        }

        // 扩展：根据 CLR 类型推断 SQLite 类型
        private static string ToSqliteType(this Type clrType)
        {
            if (clrType == typeof(int) || clrType == typeof(long)) return "INTEGER";
            if (clrType == typeof(double) || clrType == typeof(float) || clrType == typeof(decimal)) return "REAL";
            if (clrType == typeof(string)) return "TEXT";
            if (clrType == typeof(bool)) return "INTEGER"; // SQLite 没有布尔，常用 INTEGER
            if (clrType == typeof(byte[])) return "BLOB";
            return "TEXT";
        }
    }

}
