using Orchard.Localization;
using Orchard.Security;
using Orchard.UI.Navigation;

namespace Contrib.Logging.Sentry.Navigation
{
    public class AdminMenu : INavigationProvider
    {
        #region Members

        public Localizer T { get; set; }

        #endregion Members

        #region Ctors

        public AdminMenu()
        {
            this.T = NullLocalizer.Instance;
        }

        #endregion Ctors

        #region INavigationProvider Members

        string INavigationProvider.MenuName
        {
            get { return "admin"; }
        }

        void INavigationProvider.GetNavigation(NavigationBuilder builder)
        {
            // Site settings menu
            builder.Add(
                T("Sentry Logging"),
                "13",
                item => item
                    .Action("Settings", "Admin", new { area = "Contrib.Logging.Sentry" })
                    .Permission(StandardPermissions.SiteOwner)
            );
        }

        #endregion
    }
}