using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace WebApplication1.ViewModels
{
    public class FutureDate : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            DateTime dt;
            var isValid = DateTime.TryParseExact(
                Convert.ToString(value),
                "d MMM yyyy",
                CultureInfo.CurrentCulture,
                DateTimeStyles.None,
                out dt);

            return (isValid && dt > DateTime.Now);
        }
    }
}
