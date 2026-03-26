using Microsoft.EntityFrameworkCore.Migrations;

namespace EFConsole5.Migrations
{
    public partial class AddViewAndStoredProcedure : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // VIEW: Викладачі та їх предмети
            migrationBuilder.Sql(@"
                CREATE VIEW vw_TeacherSubjects AS
                SELECT 
                    t.Id,
                    t.FirstName + ' ' + t.LastName AS TeacherName,
                    STRING_AGG(s.Name, ', ') AS Subjects
                FROM Teachers t
                LEFT JOIN TeacherSubjects ts ON t.Id = ts.TeacherId
                LEFT JOIN Subjects s ON ts.SubjectId = s.Id
                GROUP BY t.Id, t.FirstName, t.LastName;
            ");

            // STORED PROCEDURE: Додати викладача
            migrationBuilder.Sql(@"
                CREATE PROCEDURE sp_AddTeacher
                    @FirstName NVARCHAR(100),
                    @LastName NVARCHAR(100),
                    @BirthDate DATE,
                    @Salary DECIMAL(8,2),
                    @DepartmentId INT
                AS
                BEGIN
                    INSERT INTO Teachers (FirstName, LastName, BirthDate, Salary, DepartmentId)
                    VALUES (@FirstName, @LastName, @BirthDate, @Salary, @DepartmentId);
                END;
            ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP VIEW IF EXISTS vw_TeacherSubjects;");
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS sp_AddTeacher;");
        }
    }
}