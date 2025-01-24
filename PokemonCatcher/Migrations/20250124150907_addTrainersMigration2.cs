using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PokemonCatcher.Migrations
{
    /// <inheritdoc />
    public partial class addTrainersMigration2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PokemonId",
                table: "PokemonTypes",
                newName: "PokemonDexId");

            migrationBuilder.RenameColumn(
                name: "PokemonId",
                table: "PokemonAbilities",
                newName: "PokemonDexId");

            migrationBuilder.RenameColumn(
                name: "PokemonId",
                table: "caughtPokemons",
                newName: "PokemonDexId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PokemonDexId",
                table: "PokemonTypes",
                newName: "PokemonId");

            migrationBuilder.RenameColumn(
                name: "PokemonDexId",
                table: "PokemonAbilities",
                newName: "PokemonId");

            migrationBuilder.RenameColumn(
                name: "PokemonDexId",
                table: "caughtPokemons",
                newName: "PokemonId");
        }
    }
}
