using Contrib.Logging.Sentry.Models;
using Orchard;
using Orchard.Caching.Services;
using Orchard.ContentManagement;
using Orchard.Settings;

namespace Contrib.Logging.Sentry.Services
{
    public class DefaultSentryConfigService : ISentryConfigService
    {
        #region Members

        private readonly IOrchardServices _orchardServices;
        private readonly ICacheService _cacheService;

        #endregion

        #region Ctors

        public DefaultSentryConfigService(IOrchardServices orchardServices, ICacheService cacheService)
        {
            this._orchardServices = orchardServices;
            this._cacheService = cacheService;
        }

        #endregion

        #region ISentryConfigService members

        SentryConfiguration ISentryConfigService.GetConfiguration()
        {
            return this._cacheService.Get<SentryConfiguration>(
                key: typeof(SentryConfiguration).FullName,
                factory: () =>
                {
                    ISite siteSettings = this._orchardServices.WorkContext.CurrentSite;
                    SentryConfigurationPart part = siteSettings.As<SentryConfigurationPart>();
                    return new SentryConfiguration(part);
                }
            );
        }

        #endregion
    }
}