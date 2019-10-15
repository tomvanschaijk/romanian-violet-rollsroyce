using System;
using System.Collections.Generic;
using System.Text;

namespace RomanianVioletRollsRoyce.Crosscutting.HealthChecks
{
    public interface IMemoryUsageLoader
    {
        MemoryUsage GetMemoryUsage();
    }
}
