using Contrib.Logging.Sentry.Models;
using Orchard;

namespace Contrib.Logging.Sentry.Services
{
    public interface ISentryConfigService : IDependency
    {
        /// <summary>
        /// Get the Sentry configuration of this tenant
        /// </summary>
        /// <returns></returns>
        SentryConfiguration GetConfiguration();
    }
}