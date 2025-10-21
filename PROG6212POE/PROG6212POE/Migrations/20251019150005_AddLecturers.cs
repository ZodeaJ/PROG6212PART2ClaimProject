using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace PROG6212POE.Migrations
{
    /// <inheritdoc />
    public partial class AddLecturers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Lecturers",
                columns: new[] { "LecturerId", "Department", "Email", "Name", "PhoneNumber" },
                values: new object[,]
                {
                    { 1, "Computer Science", "emma.watson@example.com", "Emma Watson", "0712345678" },
                    { 2, "Mathematics", "chris.hemsworth@example.com", "Chris Hemsworth", "0723456789" },
                    { 3, "Physics", "scarlett.johansson@example.com", "Scarlett Johansson", "0734567890" },
                    { 4, "Chemistry", "leonardo.dicaprio@example.com", "Leonardo DiCaprio", "0745678901" },
                    { 5, "Biology", "jennifer.lawrence@example.com", "Jennifer Lawrence", "0756789012" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Lecturers",
                keyColumn: "LecturerId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Lecturers",
                keyColumn: "LecturerId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Lecturers",
                keyColumn: "LecturerId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Lecturers",
                keyColumn: "LecturerId",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Lecturers",
                keyColumn: "LecturerId",
                keyValue: 5);
        }
    }
}
