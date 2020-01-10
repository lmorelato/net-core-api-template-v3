using System;
using System.Collections.Generic;
using System.Globalization;
using NToolbox.Extensions.Strings;

namespace Template.Core.Helpers
{
    public static class LocalizationHelper
    {
        public const string DefaultCultureName = "en-US";

        public static readonly Dictionary<string, CultureInfo> SupportedCultures =
            new Dictionary<string, CultureInfo>(StringComparer.OrdinalIgnoreCase)
            {
                { "en-US", new CultureInfo("en-US") },
                { "pt-BR", new CultureInfo("pt-BR") }
            };

        private static readonly Dictionary<string, CultureInfo> FallbackCulturesMap =
            new Dictionary<string, CultureInfo>(StringComparer.OrdinalIgnoreCase)
            {
                { "en", new CultureInfo("en-US") },
                { "pt", new CultureInfo("pt-BR") }
            };

        public static string GetClosestSupportedCultureName()
        {
            var culture = GetClosestSupportedCulture(CultureInfo.CurrentCulture.Name);
            return culture.Name;
        }

        public static string GetQueryStringCultureInfo()
        {
            return $"&culture={CultureInfo.CurrentCulture}&ui-culture={CultureInfo.CurrentUICulture}";
        }

        private static CultureInfo GetClosestSupportedCulture(string code)
        {
            if (code.IsNullOrEmpty())
            {
                return SupportedCultures[DefaultCultureName];
            }

            if (SupportedCultures.ContainsKey(code))
            {
                return SupportedCultures[code];
            }

            var fallback = TryGetNextSupportedCulture(code);
            return fallback == null ? SupportedCultures[DefaultCultureName] : SupportedCultures[fallback];
        }

        private static string TryGetNextSupportedCulture(string code)
        {
            code = code.Truncate(2);
            return FallbackCulturesMap.ContainsKey(code) ? code : null;
        }
    }
}
