﻿using System;

namespace Chef.Core.Models
{
    public class Ingredient
    {
        private int _amount;

        private double _weight;

        public Product Product;

        public int Amount
        {
            get => _amount;
            private set
            {
                if (value <= 0)
                {
                    throw new ArgumentException();
                }

                _amount = value;
            }
        }

        public double Weight
        {
            get => _weight;
            private set
            {
                if (value <= 0)
                {
                    throw new ArgumentException();
                }

                _weight = value;
            }
        }

        public double CaloricPerIngredient => Product.CaloriePerHundred * Weight / 100 * Amount;

        public Ingredient(Product product, double weight, int amount)
        {
            if (product == null)
            {
                throw new NullReferenceException("Porduct can't be null");
            }

            Product = product;
            Weight = weight;
            Amount = amount;
        }
    }
}
