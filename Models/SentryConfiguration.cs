using log4net.Core;
using System;
using System.Collections.Generic;

namespace Contrib.Logging.Sentry.Models
{
    public class SentryConfiguration
    {
        #region Members

        /// <summary>
        /// Get or set the Sentry DSN
        /// </summary>
        public Uri DSN { get; set; }

        /// <summary>
        /// Get or set the list of sentry tags that will be extracted from the
        /// log event's properties
        /// </summary>
        public IList<string> Tags { get; set; }

        /// <summary>
        /// Get or set the date of the last time the config was updated
        /// </summary>
        public DateTime DateUpdated { get; set; }

        /// <summary>
        /// The minimum log level to match for the event to be sent to sentry
        /// </summary>
        public LogLevel LevelMin { get; set; }

        /// <summary>
        /// A mapper between this class's min log level and a log4net event level
        /// </summary>
        public Level Log4netLevelMin
        {
            get
            {
                switch (this.LevelMin)
                {
                    case LogLevel.All:
                        return Level.All;
                    case LogLevel.Debug:
                        return Level.Debug;
                    case LogLevel.Info:
                        return Level.Info;
                    case LogLevel.Warning:
                        return Level.Warn;
                    case LogLevel.Error:
                        return Level.Error;
                    case LogLevel.Fatal:
                        return Level.Fatal;
                    default:
                        return Level.Off;
                }
            }
        }

        /// <summary>
        /// Get or set the pattern layout used by log4net to format the message before
        /// sending it to Sentry
        /// </summary>
        public string Layout { get; set; }

        /// <summary>
        /// True if the configuration is active and can be used to create a new
        /// log appender
        /// </summary>
        public bool IsActive
        {
            get
            {
                return this.DSN != null;
            }
        }

        #endregion

        #region Ctors

        public SentryConfiguration()
        {
        }

        public SentryConfiguration(SentryConfigurationPart part)
        {
            this.DSN = !string.IsNullOrEmpty(part.DSN) ? new Uri(part.DSN, UriKind.Absolute) : null;
            this.Tags = part.Tags;
            this.DateUpdated = part.DateUpdated;
            this.LevelMin = part.LevelMin;
            this.Layout = part.Layout;
        }

        #endregion
    }
}