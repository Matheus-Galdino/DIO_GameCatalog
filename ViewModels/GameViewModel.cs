﻿using System;
using System.ComponentModel.DataAnnotations;

namespace APIGamesCatalog.ViewModels
{
    public class GameViewModel
    {
        [Required]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "O nome do jogo deve conter entre 3 e 100 caracteres.")]
        public string Name { get; set; }

        [Required]
        [Range(1, 1000, ErrorMessage = "O preço do jogo deve ser no mínimo 1 real e no máximo 1000 reais")]
        public decimal Price { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "O nome da produtora deve conter entre 3 e 100 caracteres.")]
        public string Producer { get; set; }
    }
}
