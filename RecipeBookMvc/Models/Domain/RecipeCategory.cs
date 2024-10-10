﻿using System.ComponentModel.DataAnnotations;

namespace RecipeBookMvc.Models.Domain
{
    public class RecipeCategory
    {
        public int Id { get; set; }

        public int RecipeId { get; set; }

        public int CategoryId { get; set; }

    }
}

