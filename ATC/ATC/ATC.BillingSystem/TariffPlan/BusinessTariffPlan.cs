﻿using ATC.Abstractions.BillingSystem.TariffPlan;

namespace ATC.BillingSystem.TariffPlan
{
    public class BusinessTariffPlan : ITariffPlan
    {
        private const decimal indexCost = 0.0083m;

        public decimal GetPrice(int time)
        {
            return indexCost * time;
        }
    }
}
