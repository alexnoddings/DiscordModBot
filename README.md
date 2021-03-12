# Elvet - A personal Discord guild bot.

## [Core](Core/)
The Core projects holds the abstractions of how bot services/plugins are registered.

---

## [Host](Host/)
The Host project acts as the startup and service/plugin-registration point.

---

## Plugins
The plugin projects introduce all of the user-facing functionality of the bot.

### [Friday Night Live](Plugins.FridayNightLive/)
A plugin to track the host and winner(s) of our weekly Friday night activities.

### [Parrot](Plugins.Parrot/)
A plugin to send reminders to users.

### [Role Back](Plugins.RoleBack/)
A plugin to grant roles back to a user if they leave and re-join the guild.

---

## [Plugin Template](PluginTemplate/)
The plugin template makes it quicker to scaffold a new plugin which includes a Plugin, Config, Module, DbContext, and Service Collection extension.

The contents of the folder can be zipped and placed under `%userprofile%\Documents\Visual Studio 2019\Templates\ProjectTemplates` to provide a template when adding a new project to the solution.
