using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hallearn.Utility
{
    public static class calculeHoras
    {

        public static decimal CalcularHoras(TimeSpan hi, TimeSpan hf)
        {
            return Convert.ToDecimal((hf - hi).TotalHours);
        }

    }
}
