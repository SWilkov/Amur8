Amur8
=====

A toolkit of custom controls for Windows 8 XAML applications

##Included

* CountdownTimer - An hours, minutes, seconds countdown timer that is adjustable using sliders and can notify on timer    completed
* FlipTile - A live tile which flips between Front and Back content
* RollOverTile - A Live tile which rolls over between front and back content
* AddZeroToNumberConverter - If the integer is below 10 will add a proceeding zero
* CountdownTimerEventArgs - Used in TimerPaused, TimerStarted, TimerFinished events to hold start time, pause time
* Amur8 Test App - Test the controls

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

###FlipTile Properties (**Defaults in bold**)

* Command - Enables binding to Command from ViewModel in MVVM (**null**)
* CommandParamter - Enables a parameter to be set in ViewModel in MVVM (**null**)
* FrontContent, BackContent - Set the Front & Back content of the tile (**null**)
* EmptyContentBackground - Set the default background colour of the tile (**light grey**)
* MinimumFlipTime, MaximumFlipTime - If IsRandom is true set the min and max time between flips (**3000, 10000**)
* IsRandomFlip - A random time between flips (**true**)
* FlipTime - How long the tile takes to Flip (**2000**)
* TimeBetweenFlips - Sets an exact time between flips nb. IsRandomFlip needs to be false (**5000**)


##Installation instructions
You can install the toolkit via [NuGet](https://www.nuget.org/). Exact location is [here](https://www.nuget.org/packages/Amur8/)

If you have NuGet installed in Visual Studio in Package manager type **Install-Package Amur8**
