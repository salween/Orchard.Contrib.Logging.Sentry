using Contrib.Logging.Sentry.Models;
using Orchard.ContentManagement.Handlers;
using System;
using System.Collections.Generic;

namespace Contrib.Logging.Sentry.Handlers
{
    public class SentryConfigurationHandler : ContentHandler
    {
        public SentryConfigurationHandler()
        {
            // Bind the SentryConfigurationPart content part to the "Site" content type
            this.Filters.Add(new ActivatingFilter<SentryConfigurationPart>("Site"));

            // Default settings values (typically on module activation)
            this.OnInitializing<SentryConfigurationPart>(
                (context, part) =>
                {
                    part.DSN = null;
                    part.Tags = new List<string>();
                    part.DateUpdated = DateTime.UtcNow;
                    part.LevelMin = Contrib.Logging.Sentry.Models.LogLevel.All;
                    part.Layout = "";
                }
            );
        }
    }
}