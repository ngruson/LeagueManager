using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeagueManager.Infrastructure
{
    public class DateTimeFormatter
    {
        public static DateTime? Format(DateTime? dt)
        {
            if (dt.HasValue)
            {
                string value = dt.Value.ToString(CultureInfo.InvariantCulture);
                var dt2 = DateTime.Parse(value, CultureInfo.InvariantCulture);
                return DateTime.Parse(value, CultureInfo.InvariantCulture);
            }
            return null;
        }
    }
}