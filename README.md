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
Now that the feature is activated, you have a new navigation node called "Sentry Logging". Click on that link. You can now configure your Sentry settings from there, and save them once you are done.

##### Sentry DSN (Data Source Name)
The Data Source Name of your Sentry project. See the documentation on the Sentry [project site](https://docs.getsentry.com/hosted/quickstart/#configure-the-dsn). Leave this field empty to disable Sentry logging for this tenant.

##### Tags
A list of tag keys (one per line) that will be sent to Sentry as [tags](https://docs.getsentry.com/hosted/learn/context/).
The tag value will be extracted from the log4net log event properties, if provided. The instance emitting the log event is responsible for setting the property with the appropriate key.
Any log event property that is not sent as a tag will be sent as a log extra.
Note that only log event properties will be processed this way, not thread or global properties, so you might want to add tenant/request information into this also.

##### LevelMin
This is the minimum level that a log event must match in order to be processed by the Sentry appender. Anything strictly below that will be ignored by the log appender.

##### Layout
A custom pattern layout, as defined by the [log4net documentation](https://logging.apache.org/log4net/release/sdk/log4net.Layout.PatternLayout.html). Leave empty to send the raw information.