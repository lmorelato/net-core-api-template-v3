using System.Linq;
using Humanizer;
using Microsoft.Extensions.Localization;

namespace Template.Localization
{
    // https://stackoverflow.com/questions/45167350/localization-in-external-class-libraries-in-asp-net-core
    public class SharedResources : ISharedResources
    {
        private readonly IStringLocalizer localizer;

        public SharedResources(IStringLocalizer<SharedResources> localizer)
        {
            this.localizer = localizer;
        }

        public string Get(string key)
        {
            var value = this.localizer.GetString(key);
            return value.ToString().Transform(To.SentenceCase);
        }

        public string GetAndApplyKeys(string key, params string[] keys)
        {
            var parameters = keys.Select(k => this.localizer.GetString(k).Value).ToArray();
            var value = this.localizer.GetString(key, parameters);
            return value.ToString().Transform(To.SentenceCase);
        }

        public string GetAndApplyValues(string key, params object[] values)
        {
            var value = this.localizer.GetString(key, values);
            return value.ToString().Transform(To.SentenceCase);
        }
    }
}
