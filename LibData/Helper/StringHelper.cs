using System;

namespace LibData.Helper
{
    public class StringHelper
    {
        public static string TrimIfNotNull(string value)
        {
            if (value != null)
            {
                return value.Trim();
            }
            return null;
        }
    }
}
