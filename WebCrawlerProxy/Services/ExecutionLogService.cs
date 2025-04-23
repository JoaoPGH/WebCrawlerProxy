using Dapper;
using System.Data;
using Microsoft.Data.Sqlite;

namespace WebCrawlerProxy.Services
{
    public class ExecutionLogService
    {
        private readonly string _connectionString;

        public ExecutionLogService(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void LogExecution(DateTime startTime, DateTime endTime, int pageCount, int lineCount, string jsonPath)
        {
            using var connection = new SqliteConnection(_connectionString);
            connection.Open();

            var query = @"
                INSERT INTO ExecutionLog (StartTime, EndTime, PageCount, LineCount, JsonFilePath)
                VALUES (@StartTime, @EndTime, @PageCount, @LineCount, @JsonFilePath);
            ";

            connection.Execute(query, new
            {
                StartTime = startTime.ToString("yyyy-MM-dd HH:mm:ss"),
                EndTime = endTime.ToString("yyyy-MM-dd HH:mm:ss"),
                PageCount = pageCount,
                LineCount = lineCount,
                JsonFilePath = jsonPath
            });
        }
    }
}
