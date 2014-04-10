Amur8
=====

A toolkit of custom controls for Windows 8 XAML applications

##Included

* CountdownTimer - An hours, minutes, seconds countdown timer that is adjustable using sliders and can notify on timer    completed
* FlipTile - A live tile which flips between Front and Back content
* AddZeroToNumberConverter - If the integer is below 10 will add a proceeding zero
* CountdownTimerEventArgs - Used in TimerPaused, TimerStarted, TimerFinished events to hold start time, pause time

###CountdownTimer Properties (**Defaults in bold**)

* EnableNotification - Enables a toast notification message when the countdown finishes (**false**).
                     Note: Requires Toast Enabled in Packageappxmanifest
* NotificationHeading - Sets the heading on the Toast Notification when Timer finishes
* NotificationBody - Sets the body text on the Toast Notification when Timer finishes
* IsSettingsOpen - Can enable the settings panel to be open on startup (**false**)
* OpenSettingsDirection - Can enable the settings panel to open Up, Down, Left or Right (**Left**)
* OpenSettingsDuration - Sets the speed the Settings panel opens (**750ms**)
* CloseSettingsDuration - Sets the speed the settings panel closes (**750**)
* MaximumHours - Sets the max integer in the hours slider (**12**)
* MinimumHours - Sets the min integer in the hours slider (**0**)

##Installation instructions
You can install the toolkit via [NuGet](https://www.nuget.org/). Exact location is [here](https://www.nuget.org/packages/Amur8/)

If you have NuGet installed in Visual Studio in Package manager type **Install-Package Amur8**
