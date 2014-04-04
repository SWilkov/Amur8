using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Amur8
{
    public static class Constants
    {
        //Countdown Timer constants
        public const string TIMER_GRID = "PART_TimerGrid";
        public const string SETTINGS_GRID = "PART_SettingsGrid";
        public const string SLIDERS_GRID = "PART_sliderGrid";
        public const string CLOSE_SETTINGS_BUTTON = "PART_CloseSettingsButton"; 

        //Buttons
        public const string START_TIMER = "PART_StartButton";
        public const string PAUSE_TIMER = "PART_PauseButton";

        // default time when openning the Settings Grid
        public const double DEFAULT_OPENCLOSE_TIME = 750;

        //Settings arrow glyphs
        public const string LEFT_GLYPH = "PART_left";
        public const string RIGHT_GLYPH = "PART_right";
        public const string UP_GLYPH = "PART_up";
        public const string DOWN_GLYPH = "PART_down";

        //Slider controls for hours, minutes, seconds
        public const string HOURS_SLIDER = "PART_HoursSlider";
        public const string MINUTES_SLIDER = "PART_MinutesSlider";
        public const string SECONDS_SLIDER = "PART_SecondsSlider";

        public const double EXPANDED_SCALE_SIZE = 1.3;
        public const int MAX_HOURS = 12;

        //Notification Defaults
        public const string NOTIFY_HEADER = "Timer";
        public const string NOTIFY_TEXT = "timer has finished";
        
    }
}
