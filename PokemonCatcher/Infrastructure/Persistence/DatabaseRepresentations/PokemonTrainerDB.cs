﻿using System.ComponentModel.DataAnnotations;
using Swashbuckle.AspNetCore.Annotations;

namespace PokemonCatcher.Infrastructure.Persistence.DatabaseRepresentations
{
    public class PokemonTrainerDB
    {
        [SwaggerIgnore]
        public int Id { get; set; }
        public string? Name { get; set; }
        public int Level { get; set; }
    }
}