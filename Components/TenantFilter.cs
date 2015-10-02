using log4net;
using log4net.Core;
using log4net.Filter;
using System;

namespace Contrib.Logging.Sentry.Components
{
    public class TenantFilter : FilterSkeleton
    {
        private readonly string _tenant;

        public TenantFilter(string tenant)
        {
            this._tenant = tenant;
        }

        public override FilterDecision Decide(LoggingEvent loggingEvent)
        {
            // Orchard stores the tenant name in the log4net ThreadContext.Properties
            // static instance whenever possible.
            // There are some issues with this, especially when you hit a breakpoint
            // in debug mode, the property is not always populated.
            // However this is the best guess at finding the current tenant from a
            // log4net filter, except starting to implement dependency injection in there
            // or dealing with HttpContext.Current
            string eventTenant = ThreadContext.Properties["Tenant"] as string;

            // Ignore the event if the appender/filter's tenant doesn't match the current tenant
            if (string.Equals(eventTenant, this._tenant, StringComparison.Ordinal))
            {
                return FilterDecision.Neutral;
            }
            else
            {
                return FilterDecision.Deny;
            }
        }
    }
}