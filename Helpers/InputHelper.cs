using System;
using System.Linq;

namespace Helpers
{
    public static class InputHelper
    {
        public static TNumber[] ToArray<TNumber>(this string numbers) where TNumber : new()
        {
            return numbers?.Split(new char[] { ' ', ',' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(x =>
                {
                    if (!double.TryParse(x, out double y))
                    {
                        throw new Exception($"Couldn't parse '{x}' to number");
                    }
                    return (TNumber)Convert.ChangeType(y, typeof(TNumber));
                })
                .ToArray();
        }
    }
}