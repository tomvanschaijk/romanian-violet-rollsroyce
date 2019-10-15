using System;
using System.Collections.Generic;
using System.Text;

namespace RomanianVioletRollsRoyce.Crosscutting.HealthChecks
{
    public struct MemoryUsage
    {
        public double Total { get; }
        public double Used { get; }
        public double Free { get; }

        public MemoryUsage(double total, double used, double free)
        {
            Total = total;
            Used = used;
            Free = free;
        }
    }
}
