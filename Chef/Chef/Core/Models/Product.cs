﻿using Chef.Core.Specifications;
using System;

namespace Chef.Core.Models
{
    public abstract class Product
    {
        private string _name;

        public string Name
        {
            get => _name;
            private set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException("Name can't be empty or null!");
                }

                _name = value;
            }
        }

        public Manufacturer Manufacturer { get; set; }

        public Product(string name, Manufacturer manufacturer)
        {
            Name = name;
            Manufacturer = manufacturer;
        }

        public abstract double CaloriePerHundred { get; }

        protected double CalculateCalories(double fats, double proteins, double carbohydrates)
        {
            return 9 * fats + 4 * proteins + 4 * carbohydrates;
        }
    }
}
