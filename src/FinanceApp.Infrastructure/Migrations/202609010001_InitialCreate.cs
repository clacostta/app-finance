using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinanceApp.Infrastructure.Migrations;

public partial class InitialCreate : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.Sql(@"
-- Initial schema should be generated via `dotnet ef migrations add InitialCreate`.
-- This placeholder keeps the repository phase-1 ready in environments sem SDK.
");
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
    }
}
