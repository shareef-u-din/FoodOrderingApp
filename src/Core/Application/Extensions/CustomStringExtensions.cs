using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Extensions
{
    public static class CustomStringExtensions
    {
        public static bool IsNullOrEmptyOrWhitespace(this string str)
        {
            return string.IsNullOrEmpty(str) && string.IsNullOrWhiteSpace(str);
        }
    }
}
