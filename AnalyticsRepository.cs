using BoardGamesAnalytics.Models;
using Dapper;
using ExamWorkDapper_2.Models;
using System.Data.SqlClient;

namespace BoardGamesAnalytics.DataAccess
{
    public class AnalyticsRepository
    {
        private readonly string _connectionString;

        public AnalyticsRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        // 1. Вивести всі сесії (гра, учасники, дата, тривалість)
        public List<dynamic> GetAllSessions()
        {
            using SqlConnection con = new SqlConnection(_connectionString);
            con.Open();

            string query = @"
                SELECT 
                    s.Id,
                    g.Title AS GameTitle,
                    s.Date,
                    s.DurationMinutes,
                    STRING_AGG(m.FullName, ', ') AS Members
                FROM Sessions s
                INNER JOIN Games g ON s.GameId = g.Id
                LEFT JOIN MemberSessions ms ON s.Id = ms.SessionId
                LEFT JOIN Members m ON ms.MemberId = m.Id
                GROUP BY s.Id, g.Title, s.Date, s.DurationMinutes
                ORDER BY s.Date DESC
            ";

            return con.Query<dynamic>(query).ToList();
        }

        // 2. Топ-3 ігри за кількістю зіграних годин
        public List<GameStats> GetTop3Games()
        {
            using SqlConnection con = new SqlConnection(_connectionString);
            con.Open();

            string query = @"
                SELECT TOP 3
                    g.Title,
                    SUM(s.DurationMinutes) / 60 AS TotalHours
                FROM Games g
                INNER JOIN Sessions s ON g.Id = s.GameId
                GROUP BY g.Id, g.Title
                ORDER BY TotalHours DESC
            ";

            return con.Query<GameStats>(query).ToList();
        }

        // 3. Рейтинг учасників за загальною кількістю хвилин
        public List<MemberStats> GetMembersRating()
        {
            using SqlConnection con = new SqlConnection(_connectionString);
            con.Open();

            string query = @"
                SELECT 
                    m.FullName,
                    SUM(s.DurationMinutes) AS TotalMinutes
                FROM Members m
                INNER JOIN MemberSessions ms ON m.Id = ms.MemberId
                INNER JOIN Sessions s ON ms.SessionId = s.Id
                GROUP BY m.Id, m.FullName
                ORDER BY TotalMinutes DESC
            ";

            return con.Query<MemberStats>(query).ToList();
        }

        // 4. Загальна статистика
        public GeneralStats GetGeneralStats()
        {
            using SqlConnection con = new SqlConnection(_connectionString);
            con.Open();

            string query = @"
                SELECT 
                    COUNT(DISTINCT s.Id) AS TotalSessions,
                    SUM(s.DurationMinutes) AS TotalMinutes
                FROM Sessions s
            ";

            return con.QueryFirstOrDefault<GeneralStats>(query);
        }

        // 4a. Статистика за період
        public GeneralStats GetGeneralStatsByPeriod(DateTime startDate, DateTime endDate)
        {
            using SqlConnection con = new SqlConnection(_connectionString);
            con.Open();

            string query = @"
                SELECT 
                    COUNT(DISTINCT s.Id) AS TotalSessions,
                    SUM(s.DurationMinutes) AS TotalMinutes
                FROM Sessions s
                WHERE s.Date >= @StartDate AND s.Date <= @EndDate
            ";

            return con.QueryFirstOrDefault<GeneralStats>(query, new { StartDate = startDate, EndDate = endDate });
        }
    }
}