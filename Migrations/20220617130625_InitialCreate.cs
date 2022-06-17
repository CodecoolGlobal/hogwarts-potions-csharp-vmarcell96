using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HogwartsPotions.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Room",
                columns: table => new
                {
                    ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Capacity = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Room", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Student",
                columns: table => new
                {
                    ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HouseType = table.Column<byte>(type: "tinyint", nullable: false),
                    PetType = table.Column<byte>(type: "tinyint", nullable: false),
                    RoomID = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Student", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Student_Room_RoomID",
                        column: x => x.RoomID,
                        principalTable: "Room",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "Recipe",
                columns: table => new
                {
                    ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BrewerID = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Recipe", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Recipe_Student_BrewerID",
                        column: x => x.BrewerID,
                        principalTable: "Student",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "Potion",
                columns: table => new
                {
                    ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BrewerID = table.Column<long>(type: "bigint", nullable: true),
                    Status = table.Column<byte>(type: "tinyint", nullable: false),
                    RecipeID = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Potion", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Potion_Recipe_RecipeID",
                        column: x => x.RecipeID,
                        principalTable: "Recipe",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_Potion_Student_BrewerID",
                        column: x => x.BrewerID,
                        principalTable: "Student",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "Ingredient",
                columns: table => new
                {
                    ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PotionID = table.Column<long>(type: "bigint", nullable: true),
                    RecipeID = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ingredient", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Ingredient_Potion_PotionID",
                        column: x => x.PotionID,
                        principalTable: "Potion",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_Ingredient_Recipe_RecipeID",
                        column: x => x.RecipeID,
                        principalTable: "Recipe",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Ingredient_PotionID",
                table: "Ingredient",
                column: "PotionID");

            migrationBuilder.CreateIndex(
                name: "IX_Ingredient_RecipeID",
                table: "Ingredient",
                column: "RecipeID");

            migrationBuilder.CreateIndex(
                name: "IX_Potion_BrewerID",
                table: "Potion",
                column: "BrewerID");

            migrationBuilder.CreateIndex(
                name: "IX_Potion_RecipeID",
                table: "Potion",
                column: "RecipeID");

            migrationBuilder.CreateIndex(
                name: "IX_Recipe_BrewerID",
                table: "Recipe",
                column: "BrewerID");

            migrationBuilder.CreateIndex(
                name: "IX_Student_RoomID",
                table: "Student",
                column: "RoomID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Ingredient");

            migrationBuilder.DropTable(
                name: "Potion");

            migrationBuilder.DropTable(
                name: "Recipe");

            migrationBuilder.DropTable(
                name: "Student");

            migrationBuilder.DropTable(
                name: "Room");
        }
    }
}
