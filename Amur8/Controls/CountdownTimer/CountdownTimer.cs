using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Amur8.Animations;
using Windows.UI.Notifications;
using Amur8.Events;
using Amur8.Models;
using System.ComponentModel;
using Amur8.Helpers;

// The Templated Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234235

namespace Amur8.Controls
{
    public enum OpeningDirection
    {
        Left,
        Right,
        Up,
        Down
    }

    
    [TemplatePart(Name = TIMER_GRID, Type = typeof(FrameworkElement))]
    [TemplatePart(Name = SETTINGS_GRID, Type = typeof(FrameworkElement))]
    [TemplatePart(Name = CLOSE_SETTINGS_BUTTON, Type = typeof(Button))]
    [TemplatePart(Name = LEFT_GLYPH, Type = typeof(Grid))]
    [TemplatePart(Name = RIGHT_GLYPH, Type = typeof(Grid))]
    [TemplatePart(Name = UP_GLYPH, Type = typeof(Grid))]
    [TemplatePart(Name = DOWN_GLYPH, Type = typeof(Grid))]
    [TemplatePart(Name = SLIDERS_GRID, Type = typeof(Grid))]
    [TemplatePart(Name = COMBO_GRID, Type = typeof(Grid))]
    [TemplatePart(Name = START_TIMER, Type = typeof(Button))]
    [TemplatePart(Name = PAUSE_TIMER, Type = typeof(Button))]
    [TemplatePart(Name = HOURS_SLIDER, Type = typeof(Slider))]
    [TemplatePart(Name = MINUTES_SLIDER, Type = typeof(Slider))]
    [TemplatePart(Name = SECONDS_SLIDER, Type = typeof(Slider))]
    [TemplatePart(Name = HOURS_COMBO, Type = typeof(ComboBox))]
    [TemplatePart(Name = MINUTES_COMBO, Type = typeof(ComboBox))]
    [TemplatePart(Name = SECONDS_COMBO, Type = typeof(ComboBox))]
    [TemplateVisualState(GroupName = SettingsVisualStateGroupName, Name = SliderVisualState)]
    [TemplateVisualState(GroupName = SettingsVisualStateGroupName, Name = ComboVisualState)]
    public sealed class CountdownTimer : Control, INotifyPropertyChanged
    {
        #region Private fields

        private Grid _timerGrid;
        private Grid _settingsGrid;
        private Grid _sliderGrid;
        private Grid _comboGrid;
        private Grid _currentsSettingsGrid;
        private Button _closeSettingsButton;

        private Grid _leftGlypgGrid;
        private Grid _rightGlypgGrid;
        private Grid _upGlypgGrid;
        private Grid _downGlypgGrid;

        private Grid _currentGlyphGrid;

        private Button _startTimer;
        private Button _pauseTimer;

        private Slider _hoursSlider;
        private Slider _minutesSlider;
        private Slider _secondsSlider;

        private ComboBox _hoursCombo;
        private ComboBox _minutesCombo;
        private ComboBox _secondsCombo;

        private OpenCloseSettingsAnimation _openCloseAnimation;

        private ToastNotification _timerFinishedNotification;

        private TimeSpan _startedTime;
        
        #endregion

        #region constants

        //Countdown Timer constants
        public const string TIMER_GRID = "PART_TimerGrid";
        public const string SETTINGS_GRID = "PART_SettingsGrid";
        public const string SLIDERS_GRID = "PART_sliderGrid";
        public const string COMBO_GRID = "PART_comboGrid";
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

        //Combo controls for hours minutes seconds
        public const string HOURS_COMBO = "PART_HoursCombo";
        public const string MINUTES_COMBO = "PART_MinutesCombo";
        public const string SECONDS_COMBO = "PART_SecondsCombo";

        //VisualStates
        public const string SettingsVisualStateGroupName = "SettingStates";
        public const string SliderVisualState = "Slider";
        public const string ComboVisualState = "Combo";

        public const int MAX_HOURS = 12;

       

        private int[] _hours = new int[] { 0, 1, 2, 3, 4, 5, 6 };
        public int[] Hours
        {
            get { return _hours; }
        }

        private int[] _minutes = new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 33, 34, 35, 36, 37, 38, 39, 40, 41, 42, 43, 44, 45, 46, 47, 48, 49, 50, 51, 52, 53, 54, 55, 56, 57, 58, 59 };
        public int[] Minutes
        {
            get { return _minutes; }
        }

        private int[] _seconds = new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 33, 34, 35, 36, 37, 38, 39, 40, 41, 42, 43, 44, 45, 46, 47, 48, 49, 50, 51, 52, 53, 54, 55, 56, 57, 58, 59 };
        public int[] Seconds
        {
            get { return _seconds; }
        }

        #endregion

        private TimeHelper _timeHelper;
        public TimeHelper TimeHelper
        {
            get { return _timeHelper; }
            set
            {
                _timeHelper = value;
                OnPropertyChanged("TimeHelper");
            }
        }

        #region PropertyChanged Event

        public event PropertyChangedEventHandler PropertyChanged;

        // Create the OnPropertyChanged method to raise the event 
        private void OnPropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }

        #endregion

        #region Constructor

        public CountdownTimer()
        {
            this.DefaultStyleKey = typeof(CountdownTimer);
            this.Loaded += (s, args) =>
            {
                RegisterEvents();
                _openCloseAnimation = new OpenCloseSettingsAnimation();

                SetArrowOpacity(OpenSettingsDirection);
                _currentGlyphGrid = SetCurrentArrowGlyph(OpenSettingsDirection);

                _pauseTimer.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                this.TimeHelper = TimeHelper.Instance;
                this.TimeHelper.TimerFinishedEvent += OnTimeHelperFinished;

                if (IsSettingsOpen)
                    OpenSettings();

                this.TimeHelper.LoadedCount++;
            };

            this.Unloaded += (s, args) =>
            {
                DeRegisterEvents();
                if (this.TimeHelper.LoadedCount > 1)
                    this.TimeHelper.TimerFinishedEvent -= OnTimeHelperFinished;
            };
        }

        #endregion

        private void UpdateSettingsGrid(bool useTransitions)
        {
            if (IsSlider)
            {
                VisualStateManager.GoToState(this, SliderVisualState, useTransitions);
                _currentsSettingsGrid = _sliderGrid;
            }
            else
            {
                VisualStateManager.GoToState(this, ComboVisualState, useTransitions);
                _currentsSettingsGrid = _comboGrid;
            }
        }

        #region hook up TimeHelper events

        private void OnTimeHelperFinished(object sender, EventArgs e)
        {
            ShowStartButton();
          
            OnTimerFinished();
        }

        #endregion

        #region Timer started, paused and finished events

        public delegate void CountdownEventHandler(object sender, CountdownTimerEventArgs e);
        public event CountdownEventHandler TimerStarted;
        public event CountdownEventHandler TimerPaused;
        public event CountdownEventHandler TimerFinished;

        private void OnTimerStarted()
        {
            CountdownEventHandler eh = TimerStarted;
            if (eh != null)
            {
                TimerStarted(this,
                    new CountdownTimerEventArgs()
                    {
                        StartedTime = _startedTime
                    });
            }
        }

        private void OnTimerPaused()
        {
            CountdownEventHandler eh = TimerPaused;
            if (eh != null)
            {
                TimerPaused(this,
                    new CountdownTimerEventArgs()
                    {
                        StartedTime = _startedTime,
                        PausedTime = new TimeSpan(this.TimeHelper.TimeDetails.Hours,
                            this.TimeHelper.TimeDetails.Minutes, this.TimeHelper.TimeDetails.Seconds)
                    });
            }
        }

        private void OnTimerFinished()
        {
            CountdownEventHandler eh = TimerFinished;
            if (eh != null)
            {
                TimerFinished(this,
                    new CountdownTimerEventArgs()
                    {
                        StartedTime = _startedTime
                    });
            }
        }

        #endregion        

        protected override void OnApplyTemplate()
        {
            #region retrieve relevant controls

            _timerGrid = this.GetTemplateChild(TIMER_GRID) as Grid;
            _settingsGrid = this.GetTemplateChild(SETTINGS_GRID) as Grid;
            _closeSettingsButton = this.GetTemplateChild(CLOSE_SETTINGS_BUTTON) as Button;
            

            _leftGlypgGrid = this.GetTemplateChild(LEFT_GLYPH) as Grid;
            _rightGlypgGrid = this.GetTemplateChild(RIGHT_GLYPH) as Grid;
            _upGlypgGrid = this.GetTemplateChild(UP_GLYPH) as Grid;
            _downGlypgGrid = this.GetTemplateChild(DOWN_GLYPH) as Grid;

            _startTimer = this.GetTemplateChild(START_TIMER) as Button;
            _pauseTimer = this.GetTemplateChild(PAUSE_TIMER) as Button;

            if (this.IsSlider)
            {
                _sliderGrid = this.GetTemplateChild(SLIDERS_GRID) as Grid;
                _hoursSlider = this.GetTemplateChild(HOURS_SLIDER) as Slider;
                _minutesSlider = this.GetTemplateChild(MINUTES_SLIDER) as Slider;
                _secondsSlider = this.GetTemplateChild(SECONDS_SLIDER) as Slider;
            }
            else
            {
                _comboGrid = this.GetTemplateChild(COMBO_GRID) as Grid;
                _hoursCombo = this.GetTemplateChild(HOURS_COMBO) as ComboBox;
                _minutesCombo = this.GetTemplateChild(MINUTES_COMBO) as ComboBox;
                _secondsCombo = this.GetTemplateChild(SECONDS_COMBO) as ComboBox;
            }
            UpdateSettingsGrid(false);
            #endregion          

            base.OnApplyTemplate();
        }

        public void DeRegisterEvents()
        {
            if (_timerGrid != null)
                _timerGrid.Tapped -= _timerGrid_Tapped;

            if (_startTimer != null)
                _startTimer.Click -= _startTimer_Click;

            if (_pauseTimer != null)
                _pauseTimer.Click -= _pauseTimer_Click;

            if (_hoursSlider != null)
                _hoursSlider.ValueChanged -= _hoursSlider_ValueChanged;

            if (_minutesSlider != null)
                _minutesSlider.ValueChanged -= _minutesSlider_ValueChanged;

            if (_secondsSlider != null)
                _secondsSlider.ValueChanged -= _secondsSlider_ValueChanged;

            if (_hoursCombo != null)
                _hoursCombo.SelectionChanged -= _hoursCombo_SelectionChanged;

            if (_minutesCombo != null)
                _minutesCombo.SelectionChanged -= _minutesCombo_SelectionChanged;

            if (_secondsCombo != null)
                _secondsCombo.SelectionChanged -= _secondsCombo_SelectionChanged;

            if (_leftGlypgGrid != null)
                _leftGlypgGrid.Tapped -= _leftGlypgGrid_Tapped;

            if (_rightGlypgGrid != null)
                _rightGlypgGrid.Tapped -= _rightGlypgGrid_Tapped;

            if (_downGlypgGrid != null)
                _downGlypgGrid.Tapped -= _downGlypgGrid_Tapped;

            if (_upGlypgGrid != null)
                _upGlypgGrid.Tapped -= _upGlypgGrid_Tapped;
        }

        public void RegisterEvents()
        {
            #region Open & close settings event

            if (_timerGrid != null)
              _timerGrid.Tapped += _timerGrid_Tapped;
            
            #endregion

            #region start & pause timer events

            if (_startTimer != null)
              _startTimer.Click += _startTimer_Click;            

            if (_pauseTimer != null)
              _pauseTimer.Click += _pauseTimer_Click;
            
            #endregion

            #region Slider changed events

            if (_hoursSlider != null)
                _hoursSlider.ValueChanged += _hoursSlider_ValueChanged;
            
            if (_minutesSlider != null)
                _minutesSlider.ValueChanged += _minutesSlider_ValueChanged;
                
            if (_secondsSlider != null)
                _secondsSlider.ValueChanged += _secondsSlider_ValueChanged;
                
            #endregion

            #region ComboBox changed events

            if (_hoursCombo != null)
                _hoursCombo.SelectionChanged += _hoursCombo_SelectionChanged;
                
            if (_minutesCombo != null)
                 _minutesCombo.SelectionChanged += _minutesCombo_SelectionChanged;
                
            if (_secondsCombo != null)
                _secondsCombo.SelectionChanged += _secondsCombo_SelectionChanged;
                
            #endregion

            #region Arrow grid events

            if (_leftGlypgGrid != null)
                _leftGlypgGrid.Tapped += _leftGlypgGrid_Tapped;
                
            if (_rightGlypgGrid != null)
                _rightGlypgGrid.Tapped += _rightGlypgGrid_Tapped;
                
            if (_upGlypgGrid != null)
                _upGlypgGrid.Tapped += _upGlypgGrid_Tapped;
                
            if (_downGlypgGrid != null)
                _downGlypgGrid.Tapped += _downGlypgGrid_Tapped;
                
            #endregion
        }

        void _downGlypgGrid_Tapped(object sender, TappedRoutedEventArgs e)
        {
            CloseSettings();
        }

        void _upGlypgGrid_Tapped(object sender, TappedRoutedEventArgs e)
        {
            CloseSettings();
        }

        void _rightGlypgGrid_Tapped(object sender, TappedRoutedEventArgs e)
        {
            CloseSettings();
        }

        void _leftGlypgGrid_Tapped(object sender, TappedRoutedEventArgs e)
        {
            CloseSettings();
        }

        void _secondsCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.TimeHelper.TimeDetails.Seconds = Convert.ToInt32(_secondsCombo.SelectedItem);
        }

        void _minutesCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.TimeHelper.TimeDetails.Minutes = Convert.ToInt32(_minutesCombo.SelectedItem);
        }

        void _hoursCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.TimeHelper.TimeDetails.Hours = Convert.ToInt32(_hoursCombo.SelectedItem);
        }

        void _secondsSlider_ValueChanged(object sender, Windows.UI.Xaml.Controls.Primitives.RangeBaseValueChangedEventArgs e)
        {
            this.TimeHelper.TimeDetails.Seconds = Convert.ToInt32(_secondsSlider.Value);
        }

        void _minutesSlider_ValueChanged(object sender, Windows.UI.Xaml.Controls.Primitives.RangeBaseValueChangedEventArgs e)
        {
            this.TimeHelper.TimeDetails.Minutes = Convert.ToInt32(_minutesSlider.Value);
        }

        void _hoursSlider_ValueChanged(object sender, Windows.UI.Xaml.Controls.Primitives.RangeBaseValueChangedEventArgs e)
        {
            this.TimeHelper.TimeDetails.Hours = Convert.ToInt32(_hoursSlider.Value);
        }

        void _pauseTimer_Click(object sender, RoutedEventArgs e)
        {
            ShowStartButton();
            this.TimeHelper.PauseTimer();
            OnTimerPaused();
        }

        void _startTimer_Click(object sender, RoutedEventArgs e)
        {
            this.TimeHelper.StartTimer();

            if (this.TimeHelper.TimeDetails.Hours == 0 && this.TimeHelper.TimeDetails.Minutes == 0
                && this.TimeHelper.TimeDetails.Seconds == 0)
            {
            }
            else
            {
                ShowPauseButton();
                _startedTime = new TimeSpan(this.TimeHelper.TimeDetails.Hours, this.TimeHelper.TimeDetails.Minutes,
                                            this.TimeHelper.TimeDetails.Seconds);

                OnTimerStarted();
            }
        }

        void _timerGrid_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (_isSettingsOpen)
                CloseSettings();
            else
                OpenSettings();
        }

        #region Show and hide start/pause buttons

        private void ShowStartButton()
        {
            _startTimer.Visibility = Windows.UI.Xaml.Visibility.Visible;
            _pauseTimer.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
        }

        private void ShowPauseButton()
        {
            _startTimer.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
            _pauseTimer.Visibility = Windows.UI.Xaml.Visibility.Visible;
        }

        #endregion

        #region Open Close Settings Grid

        private bool _isSettingsOpen = false;
        public bool IsSettingsOpen
        {
            get { return _isSettingsOpen; }
            set
            {
                if (_isSettingsOpen == value)
                    return;

                _isSettingsOpen = value;
            }
        }

        private void OpenSettings()
        {
            var sb = _openCloseAnimation.GetOpenStoryboard(_currentsSettingsGrid, _currentGlyphGrid, OpenSettingsDirection, OpenSettingsDuration,
                                                           _settingsGrid.ActualWidth, _settingsGrid.ActualHeight);
           
            sb.Completed += (s, args) =>
            {
                this.IsSettingsOpen = true;
            };

            sb.Begin();
        }

        private void CloseSettings()
        {
            var sb = _openCloseAnimation.GetCloseStoryboard(_currentsSettingsGrid, _currentGlyphGrid,
                                                            OpenSettingsDirection, CloseSettingsDuration);

            sb.Completed += (s, args) =>
            {
                this.IsSettingsOpen = false;
            };
            sb.Begin();
        }

        private void SetArrowOpacity(OpeningDirection direction)
        {
            switch (direction)
            {
                case OpeningDirection.Left:
                    _rightGlypgGrid.Opacity = 1.0;
                    _leftGlypgGrid.Opacity = _upGlypgGrid.Opacity = _downGlypgGrid.Opacity = 0.0;
                    break;
                case OpeningDirection.Right:
                    _leftGlypgGrid.Opacity = 1.0;
                    _rightGlypgGrid.Opacity = _upGlypgGrid.Opacity = _downGlypgGrid.Opacity = 0.0;
                    break;
                case OpeningDirection.Up:
                    _downGlypgGrid.Opacity = 1.0;
                    _leftGlypgGrid.Opacity = _rightGlypgGrid.Opacity = _upGlypgGrid.Opacity = 0.0;
                    break;
                case OpeningDirection.Down:
                    _upGlypgGrid.Opacity = 1.0;
                    _leftGlypgGrid.Opacity = _rightGlypgGrid.Opacity = _downGlypgGrid.Opacity = 0.0;
                    break;
            }
        }

        private Grid SetCurrentArrowGlyph(OpeningDirection direction)
        {
            switch (direction)
            {
                case OpeningDirection.Left:
                    return _rightGlypgGrid;
                case OpeningDirection.Right:
                    return _leftGlypgGrid;
                case OpeningDirection.Up:
                    return _downGlypgGrid;
                case OpeningDirection.Down:
                    return _upGlypgGrid;
                default:
                    return _rightGlypgGrid;
            }
        }

        #endregion
        
        public double SettingsHeight
        {
            get { return (double)GetValue(SettingsHeightProperty); }
            set { SetValue(SettingsHeightProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SettingsHeight.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SettingsHeightProperty =
            DependencyProperty.Register("SettingsHeight", typeof(double), typeof(CountdownTimer), new PropertyMetadata(80.0));
        

        #region properties for sliders (hours, minutes and seconds )

        public int MinimumHours
        {
            get { return (int)GetValue(MinimumHoursProperty); }
            set { SetValue(MinimumHoursProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MinimumHours.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MinimumHoursProperty =
            DependencyProperty.Register("MinimumHours", typeof(int), typeof(CountdownTimer), new PropertyMetadata(0));

        public int MaximumHours
        {
            get { return (int)GetValue(MaximumHoursProperty); }
            set { SetValue(MaximumHoursProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MaximumHours.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MaximumHoursProperty =
            DependencyProperty.Register("MaximumHours", typeof(int), typeof(CountdownTimer), new PropertyMetadata(MAX_HOURS));

        #endregion

        /// <summary>
        /// Set which way the settings panel opens
        /// </summary>
        public OpeningDirection OpenSettingsDirection
        {
            get { return (OpeningDirection)GetValue(OpenSettingsDirectionProperty); }
            set { SetValue(OpenSettingsDirectionProperty, value); }
        }

        // Using a DependencyProperty as the backing store for OpenSettingsDirection.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty OpenSettingsDirectionProperty =
            DependencyProperty.Register("OpenSettingsDirection",
                                        typeof(OpeningDirection),
                                        typeof(CountdownTimer),
                                        new PropertyMetadata(OpeningDirection.Left));

        #region Settings opening and closing duration properties

        public double OpenSettingsDuration
        {
            get { return (double)GetValue(OpenSettingsDurationProperty); }
            set { SetValue(OpenSettingsDurationProperty, value); }
        }

        // Using a DependencyProperty as the backing store for OpenSettingsDuration.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty OpenSettingsDurationProperty =
            DependencyProperty.Register("OpenSettingsDuration",
                                        typeof(double),
                                        typeof(CountdownTimer),
                                        new PropertyMetadata(DEFAULT_OPENCLOSE_TIME));

        public double CloseSettingsDuration
        {
            get { return (double)GetValue(CloseSettingsDurationProperty); }
            set { SetValue(CloseSettingsDurationProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CloseSettingsDuration.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CloseSettingsDurationProperty =
            DependencyProperty.Register("CloseSettingsDuration",
                                        typeof(double),
                                        typeof(CountdownTimer),
                                        new PropertyMetadata(DEFAULT_OPENCLOSE_TIME));
        #endregion

        public bool IsSlider
        {
            get { return (bool)GetValue(IsSliderProperty); }
            set { SetValue(IsSliderProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsSlider.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsSliderProperty =
            DependencyProperty.Register("IsSlider", typeof(bool), typeof(CountdownTimer), new PropertyMetadata(false));
       
    }
}
