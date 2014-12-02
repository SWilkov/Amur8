using Amur8.Animations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Windows.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;

// The Templated Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234235

namespace Amur8.Controls
{
    public enum SlideDirection
    {
        LeftToRight,
        UpToDown
    }

    [TemplatePart(Name = MAIN_PANEL, Type = typeof(StackPanel))]
    [TemplatePart(Name = FRONT_GRID, Type = typeof(Grid))]
    [TemplatePart(Name = BACK_GRID, Type = typeof(Grid))]
    [TemplatePart(Name = FRONT_TRANSFORM, Type = typeof(TranslateTransform))]
    [TemplatePart(Name = BACK_TRANSFORM, Type = typeof(TranslateTransform))]
    public sealed class RollOverTile : Control
    { 
        #region constants
        //Part names from Template
        private const string MAIN_PANEL = "PART_MainPanel";
        private const string FRONT_GRID = "PART_FrontGrid";
        private const string BACK_GRID = "PART_BackGrid";
        private const string FRONT_TRANSFORM = "PART_FrontTransform";
        private const string BACK_TRANSFORM = "PART_BackTransform";
        //Defaults
        private const double DEF_SLIDE_TIME = 3000;
        private const double DEF_TIME_BETWEEN_SLIDES = 6000;
        private const double DEF_MIN_SLIDETIME = 3000;
        private const double DEF_MAX_SLIDETIME = 6000;
        #endregion

        private StackPanel _mainPanel;
        private Grid _frontGrid;
        private Grid _backGrid;
        private TranslateTransform _frontTransform;
        private TranslateTransform _backTransform;

        private DispatcherTimer _timer;
        private Random _random;        
        private BaseRollOverTileKeyFrames _akf;
        private bool IsFront = false;

        public RollOverTile()
        {
            this.DefaultStyleKey = typeof(RollOverTile);

            Loaded += (s, args) =>
                {  
                                SetDefaultPositions(Direction);

                    //use a seed to randomise the number
                    var randomSeed = (Int32)Windows.Security.Cryptography.CryptographicBuffer.GenerateRandomNumber();
                    _random = new Random(randomSeed);
                    
                    //Validation of time between slides. If the SlideTime is greater than the TimeBetweenSlides then the animation won't work
                    //correctly.
                    ValidateSlideTime();                   
                    SetAnimationKeyFrames(TileAnimation);

                    if (_timer == null)
                        CreateTimer();

                    if (_timer != null)
                    {
                        //Start Timer
                        _timer.Start();
                    }
                };

            Unloaded += (s, args) =>
                {
                    if (_timer != null)
                        _timer.Stop();
                };

            Tapped += (s, args) =>
                {
                    if (Command != null && Command.CanExecute(CommandParameter))
                        Command.Execute(CommandParameter);
                };
        }

        protected override void OnApplyTemplate()
        {
            _mainPanel = this.GetTemplateChild(MAIN_PANEL) as StackPanel;
            _frontGrid = this.GetTemplateChild(FRONT_GRID) as Grid;
            _backGrid = this.GetTemplateChild(BACK_GRID) as Grid;
            _frontTransform = this.GetTemplateChild(FRONT_TRANSFORM) as TranslateTransform;
            _backTransform = this.GetTemplateChild(BACK_TRANSFORM) as TranslateTransform;  
            
        }

        /// <summary>
        /// Sets the default position of the FrontContent based on SlideDirection
        /// 1 is subtracted so that the FrontContent doesnt appear above or to the left of BackContent
        /// </summary>
        /// <param name="direction"></param>
        private void SetDefaultPositions(SlideDirection direction)
        {
            var margin = _frontGrid.Margin;
            if (direction == SlideDirection.LeftToRight)
            {
                var width = _frontGrid.Width - 1;
                margin.Left = -width;
                _mainPanel.Orientation = Orientation.Horizontal;
            }                
            else
            {
                var height = _frontGrid.Height - 1;
                margin.Top = -height;
                _mainPanel.Orientation = Orientation.Vertical;
            }
            _frontGrid.Margin = margin;
        }

        private void SetAnimationKeyFrames(IRollOverAnimation tileAnimation)
        {
            if (tileAnimation.GetType() == typeof(LinearRollOverAnimation))
                _akf = new LinearRollOverTileKeyFrames(SlideTime, Direction, _frontGrid);
            else if (tileAnimation.GetType() == typeof(RollOverTileAnimation))
                _akf = new SplineRollOverTileKeyFrames(SlideTime, Direction, _frontGrid);             
        }

        /// <summary>
        /// Creates the timer used to control when the tile slides
        /// </summary>
        private void CreateTimer()
        {
            _timer = new DispatcherTimer();

            //If IsRandomSlide then use the min and max slidetimes to get a random time
            //Otherwise just use TimeBetweenSlides
            if (IsCustomTime)
            {
                if (IsRandomTime)
                {
                    int maxCustomTime = 0;
                    int minCustomTime = 0;
                    if (FrontTimeBetweenSlides > BackTimeBetweenSlides)
                    {
                        maxCustomTime = FrontTimeBetweenSlides;
                        minCustomTime = BackTimeBetweenSlides;
                        _timer.Interval = TimeSpan.FromMilliseconds(_random.Next(minCustomTime, maxCustomTime));
                    }
                    else if (BackTimeBetweenSlides > FrontTimeBetweenSlides)
                    {
                        maxCustomTime = BackTimeBetweenSlides;
                        minCustomTime = FrontTimeBetweenSlides;
                        _timer.Interval = TimeSpan.FromMilliseconds(_random.Next(minCustomTime, maxCustomTime));
                    }
                    else if (FrontTimeBetweenSlides == BackTimeBetweenSlides)
                    {
                        _timer.Interval = TimeSpan.FromMilliseconds(FrontTimeBetweenSlides);
                    }                   
                }
            }
            else
            {
                if (IsRandomTime)
                   _timer.Interval = TimeSpan.FromMilliseconds(_random.Next(MinTimeBetweenSlides, MaxTimeBetweenSlides));
                else
                   _timer.Interval = TimeSpan.FromMilliseconds(TimeBetweenSlides);
            }
                             

            _timer.Tick += (s, args) =>
            {                
                var sb = TileAnimation.GetStoryboard(_akf, _frontGrid, _frontTransform, 
                                                        SlideTime, Direction, IsFront);
                sb.Completed += (sender, e) =>
                    {
                        if (IsFront)
                        {
                            this.IsFront = false;
                            if (IsCustomTime)
                                _timer.Interval = TimeSpan.FromMilliseconds(BackTimeBetweenSlides);
                        }
                        else
                        {
                            this.IsFront = true;
                            if (IsCustomTime)
                                _timer.Interval = TimeSpan.FromMilliseconds(FrontTimeBetweenSlides);
                        }
                    };

                sb.Begin();
            };
        }

        private void ValidateSlideTime()
        {
            if (SlideTime < 1000)
                SlideTime = 1000;

            if (SlideTime >= TimeBetweenSlides)
                TimeBetweenSlides = SlideTime + 1000;

            if (IsRandomTime)
            {
                if (SlideTime >= MinTimeBetweenSlides)
                    SlideTime = MinTimeBetweenSlides - 1000;
            }

            if (IsCustomTime)
            {
                if (SlideTime >= FrontTimeBetweenSlides)
                    FrontTimeBetweenSlides = Convert.ToInt32(SlideTime) + 1000;
                if (SlideTime >= BackTimeBetweenSlides)
                    BackTimeBetweenSlides = Convert.ToInt32(SlideTime) + 1000;
            }
        }
                
        #region Dependency Properties

        public static DependencyProperty CommandProperty =
                    DependencyProperty.Register("Command", typeof(ICommand), typeof(RollOverTile),
                                                    new PropertyMetadata(null));

        /// <summary>
        /// Command uses the Tap Event to enable binding to command in ViewModel
        /// </summary>
        public ICommand Command
        {
            get { return (ICommand)GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }

        public static DependencyProperty CommandParameterProperty =
            DependencyProperty.Register("CommandParameter", typeof(object), typeof(RollOverTile),
            new PropertyMetadata(null));


        public object CommandParameter
        {
            get { return (object)GetValue(CommandParameterProperty); }
            set { SetValue(CommandParameterProperty, value); }
        }

        public IRollOverAnimation TileAnimation
        {
            get { return (IRollOverAnimation)GetValue(TileAnimationProperty); }
            set { SetValue(TileAnimationProperty, value); }
        }

        // Using a DependencyProperty as the backing store for TileAnimation.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TileAnimationProperty =
            DependencyProperty.Register("TileAnimation",
                                        typeof(IRollOverAnimation),
                                        typeof(RollOverTile), 
                                        new PropertyMetadata(new RollOverTileAnimation()));        

        public object FrontContent
        {
            get { return (object)GetValue(FrontContentProperty); }
            set { SetValue(FrontContentProperty, value); }
        }

        // Using a DependencyProperty as the backing store for FrontContent.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty FrontContentProperty =
            DependencyProperty.Register("FrontContent", 
                                        typeof(object),
                                        typeof(RollOverTile),
                                        new PropertyMetadata(null));        

        public object BackContent
        {
            get { return (object)GetValue(BackContentProperty); }
            set { SetValue(BackContentProperty, value); }
        }

        // Using a DependencyProperty as the backing store for BackContent.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty BackContentProperty =
            DependencyProperty.Register("BackContent", 
                                        typeof(object),
                                        typeof(RollOverTile),
                                        new PropertyMetadata(null));


        public SlideDirection Direction
        {
            get { return (SlideDirection)GetValue(DirectionProperty); }
            set { SetValue(DirectionProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Direction.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DirectionProperty =
            DependencyProperty.Register("Direction", 
                                        typeof(SlideDirection),
                                        typeof(RollOverTile), 
                                        new PropertyMetadata(SlideDirection.UpToDown));

        #region Time properties for slides (time between slide animations, animation time)
        
        public double SlideTime
        {
            get { return (double)GetValue(SlideTimeProperty); }
            set { SetValue(SlideTimeProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SlideTime.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SlideTimeProperty =
            DependencyProperty.Register("SlideTime", 
                                        typeof(double),
                                        typeof(RollOverTile),
                                        new PropertyMetadata(DEF_SLIDE_TIME));


        public double TimeBetweenSlides
        {
            get { return (double)GetValue(TimeBetweenSlidesProperty); }
            set { SetValue(TimeBetweenSlidesProperty, value); }
        }

        // Using a DependencyProperty as the backing store for TimeBetweenSlides.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TimeBetweenSlidesProperty =
            DependencyProperty.Register("TimeBetweenSlides",
                                        typeof(double),
                                        typeof(RollOverTile),
                                        new PropertyMetadata(DEF_TIME_BETWEEN_SLIDES));



        public bool IsCustomTime
        {
            get { return (bool)GetValue(IsCustomTimeProperty); }
            set { SetValue(IsCustomTimeProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsCustomTime.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsCustomTimeProperty =
            DependencyProperty.Register("IsCustomTime", typeof(bool), typeof(RollOverTile), new PropertyMetadata(false));

        public int FrontTimeBetweenSlides
        {
            get { return (int)GetValue(FrontTimeBetweenSlidesProperty); }
            set { SetValue(FrontTimeBetweenSlidesProperty, value); }
        }

        // Using a DependencyProperty as the backing store for FrontSlideTime.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty FrontTimeBetweenSlidesProperty =
            DependencyProperty.Register("FrontTimeBetweenSlides", typeof(int), typeof(RollOverTile), new PropertyMetadata(DEF_TIME_BETWEEN_SLIDES));


        public int BackTimeBetweenSlides
        {
            get { return (int)GetValue(BackTimeBetweenSlidesProperty); }
            set { SetValue(BackTimeBetweenSlidesProperty, value); }
        }

        // Using a DependencyProperty as the backing store for BackTimeBetweenSlides.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty BackTimeBetweenSlidesProperty =
            DependencyProperty.Register("BackTimeBetweenSlides", typeof(int), typeof(RollOverTile), new PropertyMetadata(DEF_TIME_BETWEEN_SLIDES));
    

        public bool IsRandomTime
        {
            get { return (bool)GetValue(IsRandomTimeProperty); }
            set { SetValue(IsRandomTimeProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsRandomTime.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsRandomTimeProperty =
            DependencyProperty.Register("IsRandomTime",
                                        typeof(bool),
                                        typeof(RollOverTile),
                                        new PropertyMetadata(false));

        public int MinTimeBetweenSlides
        {
            get { return (int)GetValue(MinTimeBetweenSlidesProperty); }
            set { SetValue(MinTimeBetweenSlidesProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MinTimeBetweenSlides.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MinTimeBetweenSlidesProperty =
            DependencyProperty.Register("MinTimeBetweenSlides",
                                        typeof(int),
                                        typeof(RollOverTile),
                                        new PropertyMetadata(DEF_MIN_SLIDETIME));        

        public int MaxTimeBetweenSlides
        {
            get { return (int)GetValue(MaxTimeBetweenSlidesProperty); }
            set { SetValue(MaxTimeBetweenSlidesProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MaxTimeBetweenSlides.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MaxTimeBetweenSlidesProperty =
            DependencyProperty.Register("MaxTimeBetweenSlides", 
                                        typeof(int),
                                        typeof(RollOverTile),
                                        new PropertyMetadata(DEF_MAX_SLIDETIME));
        #endregion

        #endregion
    }
}
