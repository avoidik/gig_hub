using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace WebApplication1.ViewModels
{
    public class FutureTime : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            DateTime dt;
            var isValid = DateTime.TryParseExact(
                Convert.ToString(value),
                "HH:mm",
                CultureInfo.CurrentCulture,
                DateTimeStyles.None,
                out dt);

            return (isValid);
        }
    }
}
