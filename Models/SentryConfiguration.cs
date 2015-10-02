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
        /// The minimum log4net event level to match for the event to be sent to sentry
        /// </summary>
        public Level LevelMin { get; set; }

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
            this.DSN = part.DSN;
            this.Tags = part.Tags;
            this.DateUpdated = part.DateUpdated;
            switch (part.LevelMin)
            {
                case LogLevel.All:
                    this.LevelMin = Level.All;
                    break;
                case LogLevel.Debug:
                    this.LevelMin = Level.Debug;
                    break;
                case LogLevel.Info:
                    this.LevelMin = Level.Info;
                    break;
                case LogLevel.Warning:
                    this.LevelMin = Level.Warn;
                    break;
                case LogLevel.Error:
                    this.LevelMin = Level.Error;
                    break;
                case LogLevel.Fatal:
                    this.LevelMin = Level.Fatal;
                    break;
                default:
                    this.LevelMin = Level.Off;
                    break;
            }
            this.Layout = part.Layout;
        }

        #endregion
    }
}