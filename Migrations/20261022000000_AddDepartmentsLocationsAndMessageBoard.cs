using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SSD_Lab1.Migrations;

[Microsoft.EntityFrameworkCore.Infrastructure.DbContext(typeof(SSD_Lab1.Data.ApplicationDbContext))]
[Migration("20261022000000_AddDepartmentsLocationsAndMessageBoard")]
public partial class AddDepartmentsLocationsAndMessageBoard : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "Departments",
            columns: table => new
            {
                Id = table.Column<int>(type: "int", nullable: false).Annotation("SqlServer:Identity", "1, 1"),
                Name = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: false),
                Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                ManagerName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                Email = table.Column<string>(type: "nvarchar(254)", maxLength: 254, nullable: false),
                Phone = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: true),
                OfficeLocation = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
            },
            constraints: table => table.PrimaryKey("PK_Departments", x => x.Id));

        migrationBuilder.CreateTable(
            name: "OfficeLocations",
            columns: table => new
            {
                Id = table.Column<int>(type: "int", nullable: false).Annotation("SqlServer:Identity", "1, 1"),
                Name = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: false),
                Address = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                City = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: false),
                Province = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                PostalCode = table.Column<string>(type: "nvarchar(12)", maxLength: 12, nullable: false),
                Phone = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false)
            },
            constraints: table => table.PrimaryKey("PK_OfficeLocations", x => x.Id));

        migrationBuilder.CreateTable(
            name: "BoardMessages",
            columns: table => new
            {
                Id = table.Column<int>(type: "int", nullable: false).Annotation("SqlServer:Identity", "1, 1"),
                Content = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                AuthorName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                AuthorId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                CreatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_BoardMessages", x => x.Id);
                table.ForeignKey(
                    name: "FK_BoardMessages_AspNetUsers_AuthorId",
                    column: x => x.AuthorId,
                    principalTable: "AspNetUsers",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateIndex(name: "IX_BoardMessages_AuthorId", table: "BoardMessages", column: "AuthorId");
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(name: "BoardMessages");
        migrationBuilder.DropTable(name: "Departments");
        migrationBuilder.DropTable(name: "OfficeLocations");
    }
}
