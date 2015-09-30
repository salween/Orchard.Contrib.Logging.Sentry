Contrib.Logging.Sentry
===================

Orchard CMS module enabling Sentry logging with a per-tenant configuration.

## Setup
First, you need Orchard CMS installed on your machine. You can download it from the [project site](http://www.orchardproject.net/download). The module has been created under Orchard version 1.8.1, any prior version is not guaranteed to work.

You also need a valid [Sentry](https://getsentry.com/) account and [DSN](https://docs.getsentry.com/hosted/quickstart/#configure-the-dsn).

You have many options to install this module:
- Clone this repository on your desktop, copy the source in your Orchard.Web/Modules directory and add the project in the solutions.
- Download the package from [the gallery](http://gallery.orchardproject.net/List/Modules) and either install it from the [command line](http://docs.orchardproject.net/Documentation/Using-the-command-line-interface) or from the admin menu (Modules -> Installed -> Install a module from your computer).
- Search the module in the gallery from your admin menu (Modules -> Gallery) and install it from there.

Once the package is installed, you need to activate the Contrib.Logging.Sentry feature from the admin menu.

## Configuration
Now that the feature is activated, you have a new navigation node under "Settings -> Sentry logging". Click on that link. You can now configure your Sentry settings from there, and save once you are done. If anything is misconfigured, you will be displayed a warning on the admin page after you hit the save button and the page reloads.

##### Sentry DSN (Data Source Name)
The Data Source Name of your Sentry project. See the documentation on the Sentry [project site](https://docs.getsentry.com/hosted/quickstart/#configure-the-dsn).
