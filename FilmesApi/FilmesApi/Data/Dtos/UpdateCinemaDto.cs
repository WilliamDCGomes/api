﻿using System.ComponentModel.DataAnnotations;

namespace FilmesApi.Data.Dtos
{
    public class UpdateCinemaDto
    {
        [Required(ErrorMessage = "O nome do cinema é obrigatório!")]
        public String Nome { get; set; }
    }
}
