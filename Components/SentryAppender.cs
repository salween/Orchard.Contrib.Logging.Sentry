using Contrib.Logging.Sentry.Models;
using log4net.Appender;
using log4net.Core;
using log4net.Filter;
using log4net.Layout;
using SharpRaven;
using SharpRaven.Data;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Contrib.Logging.Sentry.Components
{
    public sealed class SentryAppender : AppenderSkeleton
    {
        #region Members

        private readonly object _lock = new object();
        private readonly IRavenClient _client;
        private readonly string _tenant;
        private readonly IList<string> _tags;

        /// <summary>
        /// The date of the configuration's last update
        /// </summary>
        public DateTime ConfigurationDateUpdated { get; private set; }

        #endregion

        #region Ctors

        /// <summary>
        /// Create a new log4net SentryAppender
        /// </summary>
        /// <param name="tenant"></param>
        /// <param name="config"></param>
        public SentryAppender(string tenant, SentryConfiguration config)
        {
            // Base members
            this.Name = SentryAppender.MakeAppenderName(tenant);

            // Current class members
            var dsn = new Dsn(config.DSN.AbsoluteUri);
            this._client = new RavenClient(dsn);
            this._tenant = tenant;
            this._tags = config.Tags;
            this.ConfigurationDateUpdated = config.DateUpdated;

            // This filter prevents one tenant's appender to log an event of another tenant
            this.AddFilter(new TenantFilter(tenant));

            // Error level filter
            if (config.LevelMin == Level.Off)
            {
                this.AddFilter(new DenyAllFilter());
            }
            else if (config.LevelMin != Level.All)
            {
                this.AddFilter(new LevelRangeFilter() { LevelMin = config.LevelMin, LevelMax = Level.Off });
            }

            // Layout is created only if a pattern is defined
            if (!string.IsNullOrEmpty(config.Layout))
            {
                this.Layout = new PatternLayout(config.Layout);
            }
        }

        #endregion

        #region AppenderSkeleton members

        protected override void Append(LoggingEvent loggingEvent)
        {
            try
            {
                // Tags are searchable contextual data used to sort and filter events in sentry
                var tags = new Dictionary<string, string>();
                if (this._tags != null)
                {
                    foreach (string tagKey in this._tags)
                    {
                        if (loggingEvent.Properties.Contains(tagKey))
                        {
                            tags[tagKey] = loggingEvent.Properties[tagKey] as string;
                        }
                    }
                }

                // Extra data (everything in the log event properties except tags)
                var extras = new Dictionary<string, object>();
                foreach (string propertyKey in loggingEvent.Properties.GetKeys().Where(x => !tags.ContainsKey(x)))
                {
                    extras[propertyKey] = loggingEvent.Properties[propertyKey];
                }

                // As the sentry client is a shared instance and its logger name must match the
                // logging event, setting the name and sending the message must be an atomic operation
                lock (this._lock)
                {
                    this._client.Logger = loggingEvent.LoggerName;
                    var sentryMessage = new SentryMessage(loggingEvent.RenderedMessage);

                    // If an exception is provided, use Sentry's CaptureException instead of
                    // CaptureMessage method
                    if (loggingEvent.ExceptionObject != null)
                    {
                        this._client.CaptureException(
                            exception: loggingEvent.ExceptionObject,
                            message: sentryMessage,
                            level: this.MapLogLevel(loggingEvent.Level),
                            tags: tags,
                            extra: extras
                        );
                    }
                    else
                    {
                        this._client.CaptureMessage(
                            message: sentryMessage,
                            level: this.MapLogLevel(loggingEvent.Level),
                            tags: tags,
                            extra: extras
                        );
                    }
                }
            }
            catch (Exception)
            {
                // Silently fail (not so easy to log failed log attempts!)
            }
        }

        #endregion

        #region Utils

        /// <summary>
        /// Generate the appender name from the Orchard's tenant name
        /// </summary>
        /// <param name="tenant">The Orchard's tenant name</param>
        /// <returns></returns>
        public static string MakeAppenderName(string tenant)
        {
            return string.Format("Contrib.Logging.Sentry-{0}", tenant);
        }

        /// <summary>
        /// Map of log4net log levels to Raven/Sentry error levels
        /// </summary>
        private ErrorLevel MapLogLevel(Level level)
        {
            if (level <= Level.Debug)
            {
                return ErrorLevel.Debug;
            }
            else if (level <= Level.Info)
            {
                return ErrorLevel.Info;
            }
            else if (level <= Level.Warn)
            {
                return ErrorLevel.Warning;
            }
            else if (level <= Level.Error)
            {
                return ErrorLevel.Error;
            }
            else
            {
                return ErrorLevel.Fatal;
            }
        }

        #endregion
    }
}