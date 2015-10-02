using Contrib.Logging.Sentry.Models;
using Contrib.Logging.Sentry.ViewModels;
using Orchard;
using Orchard.Caching.Services;
using Orchard.ContentManagement;
using Orchard.Localization;
using Orchard.Mvc.Extensions;
using Orchard.Security;
using Orchard.UI.Notify;
using SharpRaven;
using System;
using System.Web.Mvc;

namespace Contrib.Logging.Sentry.Controllers
{
    public class AdminController : Controller
    {
        #region Members

        private readonly IOrchardServices _orchardServices;
        private readonly ICacheService _cacheService;

        public Localizer T { get; set; }

        #endregion

        #region Ctors

        public AdminController(
            IOrchardServices orchardServices,
            ICacheService cacheService
        )
        {
            this._orchardServices = orchardServices;
            this._cacheService = cacheService;
            this.T = NullLocalizer.Instance;
        }

        #endregion

        #region Actions

        public ActionResult Settings()
        {
            if (!this._orchardServices.Authorizer.Authorize(StandardPermissions.SiteOwner))
            {
                return new HttpUnauthorizedResult();
            }

            SentryConfigurationPart part = this._orchardServices.WorkContext.CurrentSite.As<SentryConfigurationPart>();
            var model = new SentryConfigurationViewModel(part);
            return this.View(model);
        }

        [HttpPost]
        [ActionName("Settings")]
        public ActionResult SettingsPOST()
        {
            if (!this._orchardServices.Authorizer.Authorize(StandardPermissions.SiteOwner))
            {
                return new HttpUnauthorizedResult();
            }

            SentryConfigurationPart part = this._orchardServices.WorkContext.CurrentSite.As<SentryConfigurationPart>();
            var model = new SentryConfigurationViewModel(part);

            if (!this.TryUpdateModel(model))
            {
                this._orchardServices.Notifier.Error(T("Could not save Settings."));
                return this.View(model);
            }

            // Custom validation
            this.ApplyCustomRules(model);
            if (!this.ModelState.IsValid)
            {
                this._orchardServices.Notifier.Error(T("Could not save Settings."));
                return this.View(model);
            }

            // Save
            model.UpdateContentPart(part);

            // Update cache
            var cacheableSettings = new SentryConfiguration(part);
            this._cacheService.Put(cacheableSettings.GetType().FullName, cacheableSettings);

            this._orchardServices.Notifier.Information(T("Settings saved successfully."));

            return this.RedirectToAction("Settings", "Admin", new { area = "Contrib.Logging.Sentry" });
        }

        #endregion

        #region Utils

        private void ApplyCustomRules(SentryConfigurationViewModel model)
        {
            if (!string.IsNullOrEmpty(model.DSN))
            {
                Uri dummy;
                if (!Uri.TryCreate(model.DSN, UriKind.Absolute, out dummy))
                {
                    this.ModelState.AddModelError("DSN", T("The DSN must be a valid URL"));
                    return;
                }

                try
                {
                    var dsn = new Dsn(model.DSN);
                }
                catch
                {
                    this.ModelState.AddModelError("DSN", T("The DSN is invalid"));
                    return;
                }
            }
        }

        #endregion
    }
}