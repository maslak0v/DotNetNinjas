using System.Text.Json;
using Microsoft.EntityFrameworkCore.Migrations;
using Wallet.Infrastructure.Data.SeedData;

#nullable disable

namespace Wallet.Infrastructure.Migrations
{
    public partial class SeedDefaultCategoriesWithIds : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var basePath = AppContext.BaseDirectory;
            var jsonPath = Path.Combine(basePath, "Data", "SeedData", "defaultСategories.json");
    
            if (!File.Exists(jsonPath))
                throw new FileNotFoundException($"Seed file not found: {jsonPath}");

            var jsonData = File.ReadAllText(jsonPath);
            var categories = JsonSerializer.Deserialize<List<CategorySeed>>(jsonData);

            // Удаляем старые данные
            var ids = string.Join(",", categories.Select(c => c.CategoryId));
            migrationBuilder.Sql($"DELETE FROM \"Categories\" WHERE \"CategoryId\" IN ({ids})");

            // Вставляем новые
            foreach (var category in categories)
            {
                migrationBuilder.InsertData(
                    table: "Categories",
                    columns: new[] { "CategoryId", "Name", "Icon", "CreatedAt" },
                    values: new object[] { category.CategoryId, category.Name, category.Icon, DateTime.UtcNow });
            }
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            var basePath = AppContext.BaseDirectory;
            var jsonPath = Path.Combine(basePath, "Data", "SeedData", "defaultСategories.json");
            
            if (!File.Exists(jsonPath)) return;

            var jsonData = File.ReadAllText(jsonPath);
            var categories = JsonSerializer.Deserialize<List<CategorySeed>>(jsonData);
    
            var ids = string.Join(",", categories.Select(c => c.CategoryId));
            migrationBuilder.Sql($"DELETE FROM \"Categories\" WHERE \"CategoryId\" IN ({ids})");
        }
    }
}