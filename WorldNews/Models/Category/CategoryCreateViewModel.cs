using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WorldNews.Models.Category
{
    public class CategoryCreateViewModel
    {
        [Required]
        public string Name { get; set; }
    }
}