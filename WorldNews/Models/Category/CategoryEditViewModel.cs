﻿using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace WorldNews.Models.Category
{
    public class CategoryEditViewModel
    {
        [Required]
        [HiddenInput(DisplayValue = false)]
        public string Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Display category")]
        public bool IsEnabled { get; set; }
    }
}