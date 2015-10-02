using Contrib.Logging.Sentry.Components;
using Contrib.Logging.Sentry.Models;
using log4net;
using log4net.Repository.Hierarchy;
using Orchard.Environment.Configuration;
using Orchard.Tasks;

namespace Contrib.Logging.Sentry.Services
{
    public class Log4netConfigurationTask : IBackgroundTask
    {
        #region Members

        private readonly ShellSettings _shellSettings;
        private readonly ISentryConfigService _sentryConfigService;

        #endregion

        #region Ctors

        public Log4netConfigurationTask(ShellSettings shellSettings, ISentryConfigService sentryConfigService)
        {
            this._shellSettings = shellSettings;
            this._sentryConfigService = sentryConfigService;
        }

        #endregion

        #region Interface members

        /// <summary>
        /// Check for configuration changes and update log4net accordingly
        /// </summary>
        void IBackgroundTask.Sweep()
        {
            // Get log4net repository to check if the appender needs to be added or updated
            Hierarchy hierarchy = (Hierarchy)LogManager.GetRepository();

            // There is 1 appender per tenant using this feature
            // Log events filtering is done internally to prevent one tenant's appender
            // to log an event of another tenant
            string appenderName = SentryAppender.MakeAppenderName(this._shellSettings.Name);

            // Get config
            SentryConfiguration config = this._sentryConfigService.GetConfiguration();
            if (!config.IsActive)
            {
                // Config is invalid: remove any appender associated to this tenant
                hierarchy.Root.RemoveAppender(appenderName);
                return;
            }

            // Get the existing Sentry appender if any
            SentryAppender appender = (SentryAppender)hierarchy.Root.GetAppender(appenderName);

            // If the appender is outdated, remove it
            if (appender != null && appender.ConfigurationDateUpdated < config.DateUpdated)
            {
                hierarchy.Root.RemoveAppender(appender);
                appender = null;
            }

            // Appender needs to be created
            if (appender == null)
            {
                // Re-create the appender
                appender = new SentryAppender(this._shellSettings.Name, config);

                // Add appender
                hierarchy.Root.AddAppender(appender);
            }
        }

        #endregion
    }
}