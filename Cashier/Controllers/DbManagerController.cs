using System.Reflection;
using Cashier.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace Cashier.Controllers
{
    public class DbManagerController : Controller
    {
        private readonly string _connectionString;
        private readonly AppDbContext _dbContext;

        public DbManagerController(IConfiguration config, AppDbContext dbContext)
        {
            _connectionString = config.GetConnectionString("ConnectionStrings")!;
            _dbContext = dbContext;
        }

        // 显示 DbSet 列表
        public IActionResult Index()
        {
            var dbSets = _dbContext.GetType()
                .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(p => p.PropertyType.IsGenericType &&
                            p.PropertyType.GetGenericTypeDefinition() == typeof(DbSet<>))
                .Select(p => p.Name)
                .ToList();

            ViewBag.DbSets = dbSets;
            return View();
        }

        // 查看差异
        public IActionResult Compare(string dbSetName)
        {
            var prop = _dbContext.GetType().GetProperty(dbSetName);
            if (prop == null) return NotFound($"DbSet {dbSetName} not found.");

            var entityType = prop.PropertyType.GetGenericArguments()[0];
            var tableName = entityType.GetCustomAttribute<System.ComponentModel.DataAnnotations.Schema.TableAttribute>()?.Name
                ?? entityType.Name;

            var entityProps = entityType.GetProperties(BindingFlags.Public | BindingFlags.Instance);

            var entityCols = entityProps.Select(p => new { Name = p.Name, Type = MapTypeToSqlite(p.PropertyType) }).ToList();

            var existingCols = new List<string>();
            using (var conn = new SqliteConnection(_connectionString))
            {
                conn.Open();
                var cmd = conn.CreateCommand();
                cmd.CommandText = $"PRAGMA table_info({tableName});";
                try
                {
                    using var reader = cmd.ExecuteReader();
                    while (reader.Read()) existingCols.Add(reader["name"].ToString()!);
                }
                catch
                {
                    // 表不存在，差异就是全新创建
                }
            }

            var missingInDb = entityCols.Where(c => !existingCols.Contains(c.Name)).ToList();
            var extraInDb = existingCols.Where(c => !entityCols.Any(e => e.Name == c)).ToList();

            ViewBag.TableName = tableName;
            ViewBag.EntityCols = entityCols;
            ViewBag.ExistingCols = existingCols;
            ViewBag.MissingInDb = missingInDb;
            ViewBag.ExtraInDb = extraInDb;
            ViewBag.DbSetName = dbSetName;

            return View();
        }

        // 执行同步
        [HttpPost]
        public IActionResult SyncOne(string dbSetName)
        {
            var result = new List<string>();

            var prop = _dbContext.GetType().GetProperty(dbSetName);
            if (prop == null) return NotFound($"DbSet {dbSetName} not found.");

            var entityType = prop.PropertyType.GetGenericArguments()[0];
            var tableName = entityType.GetCustomAttribute<System.ComponentModel.DataAnnotations.Schema.TableAttribute>()?.Name
                ?? entityType.Name;

            var entityProps = entityType.GetProperties(BindingFlags.Public | BindingFlags.Instance);

            var entityCols = entityProps.Select(p => new { Name = p.Name, Type = MapTypeToSqlite(p.PropertyType) }).ToList();

            var existingCols = new List<string>();
            using (var conn = new SqliteConnection(_connectionString))
            {
                conn.Open();
                var cmd = conn.CreateCommand();
                cmd.CommandText = $"PRAGMA table_info({tableName});";
                bool tableExists = false;

                try
                {
                    using var reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        existingCols.Add(reader["name"].ToString()!);
                        tableExists = true;
                    }
                }
                catch { }

                if (!tableExists)
                {
                    // 新建整张表
                    var colDefs = entityCols.Select(c => $"{c.Name} {c.Type}");
                    var cmdCreate = conn.CreateCommand();
                    cmdCreate.CommandText = $"CREATE TABLE {tableName} ({string.Join(",", colDefs)});";
                    cmdCreate.ExecuteNonQuery();
                    result.Add($"新建表 {tableName}，包含字段: {string.Join(", ", entityCols.Select(c => c.Name))}");
                }
                else
                {
                    // 补充缺失列
                    foreach (var col in entityCols)
                    {
                        if (!existingCols.Contains(col.Name))
                        {
                            var cmdAdd = conn.CreateCommand();
                            cmdAdd.CommandText = $"ALTER TABLE {tableName} ADD COLUMN {col.Name} {col.Type};";
                            cmdAdd.ExecuteNonQuery();
                            result.Add($"新增列 {col.Name} ({col.Type}) 到表 {tableName}");
                        }
                    }
                }
            }

            TempData["SyncResult"] = string.Join("; ", result);
            return RedirectToAction("Compare", new { dbSetName });
        }

        private string MapTypeToSqlite(Type type)
        {
            type = Nullable.GetUnderlyingType(type) ?? type;
            if (type == typeof(int)) return "INTEGER";
            if (type == typeof(long)) return "INTEGER";
            if (type == typeof(decimal) || type == typeof(double) || type == typeof(float)) return "REAL";
            if (type == typeof(bool)) return "INTEGER";
            if (type == typeof(DateTime)) return "TEXT";
            return "TEXT";
        }
    }
}
