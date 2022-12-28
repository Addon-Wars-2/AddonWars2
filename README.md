# AddonWars2

A tool to improve the experience of using addons and plugins in Guild Wars 2 by handling installation, updates, and file management behind the scenes.
![App View Light](/img/app_view_01.png#gh-light-mode-only)

### Project status

This project is in early development and does not provide the necessary functionality. But is updated frequently.
The project is driven by .NET 7 and WPF (Windows Presentation Foundation) using the MVVM (Model - View - ViewModel) pattern extensively.

### Motivation

This project is a pet-project which was started to improve the author's practical (yet hobby) skills and create a small, but fully finished and functional application, which could serve well for others. The development was inspired by a widely known tool [Guild Wars 2 Unofficial Add-On Manager](https://github.com/gw2-addon-loader/GW2-Addon-Manager), however the decision was made to create a new implementation from scratch rather than improving the existing solution.

### List of TODOs (dev):
#### UI Design
##### General
- [x] A unique look for the background image.
- [ ] Different themes support including the background.
- [ ] Animated appearance in Guild Wars 2 UI style (animated opacity mask?).
##### Control Styles
###### ScrollViewer and ScrollBar
- [ ] Show on mouse move and make visible and thicker on hover.
- [ ] Add a simple animated appearance.
###### ComboBox
- [ ] Hide ComboBox popup when the cursor is further than specified distance.
###### TabControl
- [ ] Add a fancy transfer when moving from one tab to abother.
#### Features
- [x] Support different languages.
- [x] [BETA] "Hot-reload" on language change without restarting the application.
- [x] Logging and wiriting logs locally.
  - [ ] Debug mode through EXE args => different logging levels.
- [x] Application configuration stored locally. Using another application version of starting a different .exe won't break anything since it will look for the same directory anyway and load the configuration from there.
- [ ] Looking for GW2 installation directory and /addons sub-directory automatically rather than selecting it manually.
- [ ] A list of approved add-ons available for installation.
- [ ] Download and install the selected add-ons automatically.
- [ ] Storing and tracking the installed add-ons files dependencies. Required for clean install/uninstall and not breaking other add-ons if any of them was uninstalled.
- [ ] Safe enable/disable and uninstall for an add-on.
- [ ] Auto-update the application on start.
- [ ] Auto-update add-ons on the application start.
#### Miscellaneous
- [ ] THIRDPARTY readme.
#### Other
- [ ] GitHub API limitations.

### Notice of Non-Affiliation and Disclaimer

The author is not affiliated, associated, authorized, endorsed by, or in any way officially connected with the ArenaNet, or any of its subsidiaries or its affiliates. The official ArenaNet website can be found at http://www.arena.net.

All trademarks, registered trademarks, logos, etc are the property of their respective owners. You use this tool at your own risk.