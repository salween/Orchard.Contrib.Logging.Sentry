using Contrib.Logging.Sentry.Models;
using System;
using System.Collections.Generic;
using System.IO;

namespace Contrib.Logging.Sentry.ViewModels
{
    public class SentryConfigurationViewModel
    {
        #region Members

        /// <summary>
        /// Get or set the Sentry DSN
        /// </summary>
        public string DSN { get; set; }

        /// <summary>
        /// Get or set the list of sentry tags that will be extracted from the
        /// log event's properties
        /// </summary>
        public string Tags { get; set; }

        /// <summary>
        /// The minimum log4net event level to match for the event to be sent to sentry
        /// </summary>
        public LogLevel LevelMin { get; set; }

        /// <summary>
        /// Get or set the pattern layout used by log4net to format the message before
        /// sending it to Sentry
        /// </summary>
        public string Layout { get; set; }

        /// <summary>
        /// Get the list of all available log levels
        /// </summary>
        public LogLevel[] LogLevels
        {
            get { return (LogLevel[])Enum.GetValues(typeof(LogLevel)); }
        }

        #endregion

        #region Ctors

        public SentryConfigurationViewModel()
        {
        }

        public SentryConfigurationViewModel(SentryConfigurationPart part)
        {
            this.DSN = part.DSN != null ? part.DSN.OriginalString : null;
            this.Tags = part.Tags != null ? string.Join(Environment.NewLine, part.Tags) : "";
            this.LevelMin = part.LevelMin;
            this.Layout = part.Layout;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Update the content part using the current view model data
        /// </summary>
        /// <param name="part">The content part to update</param>
        public void UpdateContentPart(SentryConfigurationPart part)
        {
            part.DSN = !string.IsNullOrEmpty(this.DSN) ? new Uri(this.DSN, UriKind.Absolute) : null;
            part.Tags = this.ParseTags(this.Tags);
            part.LevelMin = this.LevelMin;
            part.Layout = this.Layout;
            part.DateUpdated = DateTime.UtcNow;
        }

        #endregion

        #region Utils

        private IList<string> ParseTags(string lines)
        {
            var tags = new List<string>();
            if (!string.IsNullOrEmpty(lines))
            {
                using (var sr = new StringReader(lines))
                {
                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        tags.Add(line.Trim());
                    }
                }
            }

            return tags;
        }

        #endregion
    }
}