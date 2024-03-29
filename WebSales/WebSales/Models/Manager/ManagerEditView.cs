﻿using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace WebSales.Models.Manager
{
    public class ManagerEditView
    {
        [HiddenInput(DisplayValue = false)]
        public int Id { get; set; }

        [Required(ErrorMessage = "Enter Name!")]
        [Display(Name = "Name")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Length from 2 to 50!")]
        public string Name { get; set; }

        [Display(Name = "Age")]
        [Required(ErrorMessage = "Enter age!")]
        [Range(1, int.MaxValue, ErrorMessage = "Enter correct data!")]
        public int Age { get; set; }
    }
}