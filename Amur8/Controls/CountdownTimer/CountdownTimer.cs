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
using NotificationsExtensions.ToastContent;

// The Templated Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234235

namespace Amur8.Controls.CountdownTimer
{
    public enum OpeningDirection
    {
        Left,
        Right,
        Up,
        Down
    }

    [TemplatePart(Name = Constants.TIMER_GRID, Type = typeof(FrameworkElement))]
    [TemplatePart(Name = Constants.SETTINGS_GRID, Type = typeof(FrameworkElement))]
    [TemplatePart(Name = Constants.CLOSE_SETTINGS_BUTTON, Type = typeof(Button))]
    [TemplatePart(Name = Constants.LEFT_GLYPH, Type = typeof(Grid))]
    [TemplatePart(Name = Constants.RIGHT_GLYPH, Type = typeof(Grid))]
    [TemplatePart(Name = Constants.UP_GLYPH, Type = typeof(Grid))]
    [TemplatePart(Name = Constants.DOWN_GLYPH, Type = typeof(Grid))]
    [TemplatePart(Name = Constants.SLIDERS_GRID, Type = typeof(Grid))]
    [TemplatePart(Name = Constants.START_TIMER, Type = typeof(Button))]
    [TemplatePart(Name = Constants.PAUSE_TIMER, Type = typeof(Button))]
    [TemplatePart(Name = Constants.HOURS_SLIDER, Type = typeof(Slider))]
    [TemplatePart(Name = Constants.MINUTES_SLIDER, Type = typeof(Slider))]
    [TemplatePart(Name = Constants.SECONDS_SLIDER, Type = typeof(Slider))]
    public sealed class CountdownTimer : Control
    {
        #region Private fields

        private Grid _timerGrid;
        private Grid _settingsGrid;
        private Grid _sliderGrid;
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

        private OpenCloseSettingsAnimation _openCloseAnimation;

        private DispatcherTimer _timer;

        private ToastNotification _timerFinishedNotification;

        private TimeSpan _startedTime;

        #endregion

        #region Constructor

        public CountdownTimer()
        {
            this.DefaultStyleKey = typeof(CountdownTimer);
            this.Loaded += (s, args) =>
            {
                _openCloseAnimation = new OpenCloseSettingsAnimation();

                SetArrowOpacity(OpenSettingsDirection);
                _currentGlyphGrid = SetCurrentArrowGlyph(OpenSettingsDirection);

                _pauseTimer.Visibility = Windows.UI.Xaml.Visibility.Collapsed;

                CreateTimer();

                if (IsSettingsOpen)
                    OpenSettings();
            };
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
                        PausedTime = new TimeSpan(Hours, Minutes, Seconds)
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

        /// <summary>
        /// Creates the timer and handles Tick event to update UI
        /// </summary>
        private void CreateTimer()
        {
            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromMilliseconds(1000);
            _timer.Tick += (s, args) =>
            {
                if (Hours == 0 && Minutes == 0 && Seconds == 0)
                {
                    //Stop the timer
                    _timer.Stop();

                    //Raise TimerFinished event
                    OnTimerFinished();

                    ShowStartButton();
                    //Display Notification if EnableNotfication is true
                    if (EnableNotification)
                        DisplayNotification();

                    return;
                }

                if (Seconds == 0 && Minutes > 0)
                {
                    Minutes--;
                    Seconds = 60;
                }

                if (Minutes == 0 && Hours > 0)
                {
                    Hours--;
                    Minutes = 59;
                    Seconds = 60;
                }
                Seconds--;
            };
        }

        protected override void OnApplyTemplate()
        {
            #region retrieve relevant controls

            _timerGrid = this.GetTemplateChild(Constants.TIMER_GRID) as Grid;
            _settingsGrid = this.GetTemplateChild(Constants.SETTINGS_GRID) as Grid;
            _closeSettingsButton = this.GetTemplateChild(Constants.CLOSE_SETTINGS_BUTTON) as Button;
            _sliderGrid = this.GetTemplateChild(Constants.SLIDERS_GRID) as Grid;

            _leftGlypgGrid = this.GetTemplateChild(Constants.LEFT_GLYPH) as Grid;
            _rightGlypgGrid = this.GetTemplateChild(Constants.RIGHT_GLYPH) as Grid;
            _upGlypgGrid = this.GetTemplateChild(Constants.UP_GLYPH) as Grid;
            _downGlypgGrid = this.GetTemplateChild(Constants.DOWN_GLYPH) as Grid;

            _startTimer = this.GetTemplateChild(Constants.START_TIMER) as Button;
            _pauseTimer = this.GetTemplateChild(Constants.PAUSE_TIMER) as Button;

            _hoursSlider = this.GetTemplateChild(Constants.HOURS_SLIDER) as Slider;
            _minutesSlider = this.GetTemplateChild(Constants.MINUTES_SLIDER) as Slider;
            _secondsSlider = this.GetTemplateChild(Constants.SECONDS_SLIDER) as Slider;

            #endregion

            #region Open & close settings event

            if (_timerGrid != null)
            {
                _timerGrid.Tapped += (s, args) =>
                {
                    if (_isSettingsOpen)
                        CloseSettings();
                    else
                        OpenSettings();
                };
            }

            #endregion

            #region start & pause timer events

            if (_startTimer != null)
            {
                _startTimer.Click += (s, args) =>
                {
                    //Start the timer
                    if (_timer != null)
                    {

                        if (Hours == 0 && Minutes == 0 && Seconds == 0)
                        {
                        }
                        else
                        {
                            ShowPauseButton();
                            _startedTime = new TimeSpan(Hours, Minutes, Seconds);
                            _timer.Start();
                            OnTimerStarted();
                        }
                    }
                    else
                    {
                        CreateTimer();
                    }
                };
            }

            if (_pauseTimer != null)
            {
                _pauseTimer.Click += (s, args) =>
                {
                    if (_timer != null)
                    {
                        ShowStartButton();
                        _timer.Stop();
                        OnTimerPaused();
                    }
                };
            }

            #endregion

            #region Slider changed events

            if (_hoursSlider != null)
            {
                _hoursSlider.ValueChanged += (s, args) =>
                {
                    this.Hours = Convert.ToInt32(_hoursSlider.Value);
                };
            }

            if (_minutesSlider != null)
            {
                _minutesSlider.ValueChanged += (s, args) =>
                {
                    this.Minutes = Convert.ToInt32(_minutesSlider.Value);
                };
            }

            if (_secondsSlider != null)
            {
                _secondsSlider.ValueChanged += (s, args) =>
                {
                    this.Seconds = Convert.ToInt32(_secondsSlider.Value);
                };
            }
            #endregion

            #region Arrow grid events

            if (_leftGlypgGrid != null)
            {
                _leftGlypgGrid.Tapped += (s, args) =>
                {
                    CloseSettings();
                };
            }

            if (_rightGlypgGrid != null)
            {
                _rightGlypgGrid.Tapped += (s, args) =>
                {
                    CloseSettings();
                };
            }

            if (_upGlypgGrid != null)
            {
                _upGlypgGrid.Tapped += (s, args) =>
                {
                    CloseSettings();
                };
            }

            if (_downGlypgGrid != null)
            {
                _downGlypgGrid.Tapped += (s, args) =>
                {
                    CloseSettings();
                };
            }

            #endregion

            base.OnApplyTemplate();
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
            var sb = _openCloseAnimation.GetOpenStoryboard(_sliderGrid, _currentGlyphGrid, OpenSettingsDirection, OpenSettingsDuration,
                                                           _settingsGrid.Width, _settingsGrid.Height);

            sb.Completed += (s, args) =>
            {
                this.IsSettingsOpen = true;
            };

            sb.Begin();
        }

        private void CloseSettings()
        {
            var sb = _openCloseAnimation.GetCloseStoryboard(_sliderGrid, _currentGlyphGrid,
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

        #region Dependency Properties for hours, minutes, seconds

        private int Hours
        {
            get { return (int)GetValue(HoursProperty); }
            set { SetValue(HoursProperty, value); }
        }

        // Using a DependencyProperty as the backing store for StartingHour.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty HoursProperty =
            DependencyProperty.Register("Hours", typeof(int), typeof(CountdownTimer), new PropertyMetadata(0));

        private int Minutes
        {
            get { return (int)GetValue(MinutesProperty); }
            set { SetValue(MinutesProperty, value); }
        }

        // Using a DependencyProperty as the backing store for StartingMinute.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MinutesProperty =
            DependencyProperty.Register("Minutes", typeof(int), typeof(CountdownTimer), new PropertyMetadata(0));

        private int Seconds
        {
            get { return (int)GetValue(SecondsProperty); }
            set { SetValue(SecondsProperty, value); }
        }

        // Using a DependencyProperty as the backing store for StartingSecond.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SecondsProperty =
            DependencyProperty.Register("Seconds", typeof(int), typeof(CountdownTimer), new PropertyMetadata(0));

        #endregion

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
            DependencyProperty.Register("MaximumHours", typeof(int), typeof(CountdownTimer), new PropertyMetadata(Constants.MAX_HOURS));

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
                                        new PropertyMetadata(Constants.DEFAULT_OPENCLOSE_TIME));

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
                                        new PropertyMetadata(Constants.DEFAULT_OPENCLOSE_TIME));
        #endregion

        #region Notification properties & methods

        public bool EnableNotification
        {
            get { return (bool)GetValue(EnableNotificationProperty); }
            set { SetValue(EnableNotificationProperty, value); }
        }

        // Using a DependencyProperty as the backing store for EnableNotification.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty EnableNotificationProperty =
            DependencyProperty.Register("EnableNotification",
                                        typeof(bool),
                                        typeof(CountdownTimer),
                                        new PropertyMetadata(false));


        public string NotificationHeading
        {
            get { return (string)GetValue(NotificationHeadingProperty); }
            set { SetValue(NotificationHeadingProperty, value); }
        }

        // Using a DependencyProperty as the backing store for NotificationHeading.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty NotificationHeadingProperty =
            DependencyProperty.Register("NotificationHeading",
                                        typeof(string),
                                        typeof(CountdownTimer),
                                        new PropertyMetadata(Constants.NOTIFY_HEADER));


        public string NotificationBody
        {
            get { return (string)GetValue(NotificationBodyProperty); }
            set { SetValue(NotificationBodyProperty, value); }
        }

        // Using a DependencyProperty as the backing store for NotificationBody.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty NotificationBodyProperty =
            DependencyProperty.Register("NotificationBody",
                                        typeof(string),
                                        typeof(CountdownTimer),
                                        new PropertyMetadata(Constants.NOTIFY_TEXT));


        private void DisplayNotification()
        {
            IToastText02 toast = ToastContentFactory.CreateToastText02();
            toast.TextHeading.Text = NotificationHeading;
            toast.TextBodyWrap.Text = NotificationBody;
            toast.Duration = ToastDuration.Long;
            toast.Audio.Loop = false;

            _timerFinishedNotification = toast.CreateNotification();
            ToastNotificationManager.CreateToastNotifier().Show(_timerFinishedNotification);
        }

        #endregion
    }
}
