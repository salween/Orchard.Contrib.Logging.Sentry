using Newtonsoft.Json;
using Orchard.ContentManagement;
using System;
using System.Collections.Generic;

namespace Contrib.Logging.Sentry.Models
{
    public class SentryConfigurationPart : ContentPart
    {
        /// <summary>
        /// Get or set the Sentry DSN
        /// </summary>
        public string DSN
        {
            get { return this.Retrieve(x => x.DSN); }
            set { this.Store(x => x.DSN, value); }
        }

        /// <summary>
        /// Get or set the list of sentry tags that will be extracted from the
        /// log event's properties
        /// </summary>
        public IList<string> Tags
        {
            get
            {
                string serializedValue = this.Retrieve<string>("Tags");
                if (!string.IsNullOrEmpty(serializedValue))
                {
                    try
                    {
                        return JsonConvert.DeserializeObject<List<string>>(serializedValue);
                    }
                    catch (Exception)
                    {
                        return new List<string>();
                    }
                }
                else
                {
                    return new List<string>();
                }
            }
            set
            {
                string serializedValue = JsonConvert.SerializeObject(value ?? new List<string>());
                this.Store<string>("Tags", serializedValue);
            }
        }

        /// <summary>
        /// Get or set the date of the last time the config was updated
        /// </summary>
        public DateTime DateUpdated
        {
            get { return this.Retrieve(x => x.DateUpdated); }
            set { this.Store(x => x.DateUpdated, value); }
        }

        /// <summary>
        /// The minimum log4net event level to match for the event to be sent to sentry
        /// </summary>
        public LogLevel LevelMin
        {
            get { return this.Retrieve(x => x.LevelMin); }
            set { this.Store(x => x.LevelMin, value); }
        }

        /// <summary>
        /// Get or set the pattern layout used by log4net to format the message before
        /// sending it to Sentry
        /// </summary>
        public string Layout
        {
            get { return this.Retrieve(x => x.Layout); }
            set { this.Store(x => x.Layout, value); }
        }
    }

    /// <summary>
    /// Represent the error level of a log event
    /// </summary>
    public enum LogLevel { All, Debug, Info, Warning, Error, Fatal, Off }
}