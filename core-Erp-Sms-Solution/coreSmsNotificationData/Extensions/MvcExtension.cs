using coreSmsNotificationData.Abstractions;
using coreSmsNotificationData.Helpers;
using coreSmsNotificationData.Repositories;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace coreSmsNotificationData.Extensions
{
    public static class MvcExtension
    {
        /// <summary>
        /// Adds services for SMS data access layer
        /// </summary>
        /// <param name="services"></param>
        /// <returns name="Returns:">The original services so that additional operations can be chained</returns>
        public static IServiceCollection AddSmsDataServices(this IServiceCollection services)
        {
            services.AddTransient<ISmsDbHelper, SmsDbHelper>();
            services.AddTransient<ISmsDataRepository, SmsDataRepository>();
            services.AddTransient<ISmsHttpHelper, SmsHttpHelper>();
            return services;
        }


        public static string RemoveIntlCode(this string msisdn)
        {
            return $"0{msisdn[3..]}";
        }

        public static string ToIntlFormat(this string input)
        {
            var regex = @"^[0-9-\)\( ]{9,15}$";
            if (Regex.Match(input, regex).Success)
            {
                string phoneNumber = input.Replace("-", "").Replace(" ", "").Replace("+", "")
                    .Replace("(", "").Replace(")", "").TrimStart('0');
                if (phoneNumber.Length >= 8 && phoneNumber.Length <= 9)
                {
                    phoneNumber = "233" + phoneNumber;
                }
                return phoneNumber;
            }
            throw new InvalidOperationException($"The number {input} is an invalid phone number passed");
        }
    }
}
