using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PokemonCatcher.Migrations
{
    /// <inheritdoc />
    public partial class addnameToCaughtPokemon : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PokemonName",
                table: "caughtPokemons",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PokemonName",
                table: "caughtPokemons");
        }
    }
}
