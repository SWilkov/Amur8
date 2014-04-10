﻿using Amur8.Animations;
using Amur8.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace Amur8.Controls
{
    public enum FlipDirection
    {
        Left,
        Right,
        Up,
        Down
    }

    [TemplatePart(Name = FRONT_CONTENT, Type = typeof(FrameworkElement))]
    [TemplatePart(Name = BACK_CONTENT, Type = typeof(FrameworkElement))]
    public sealed class FlipTile : Control
    {
        #region Constants

        const string FRONT_CONTENT = "PART_Front";
        const string BACK_CONTENT = "PART_Back";
        const int MIN_FLIPTIME = 3000;
        const int MAX_FLIPTIME = 10000;
        const double DEFAULT_FLIPTIME = 2000;
        const double DEFAULT_TIME_BETWEEN_FLIPS = 5000;
      
        #endregion

        #region private fields

        private DispatcherTimer _timer;
        private Random _random;
        private FlipTileAnimation _flipAnimation;
        private FlipDetails _flipDetails;

        #endregion

        //Dont want the Background to be set on control as it will break flip animation
        [EditorBrowsable(EditorBrowsableState.Never)]
        public new Brush Background { get; set; }

        #region constructor

        public FlipTile()
        {
            this.DefaultStyleKey = typeof(FlipTile);

            #region events

            this.Loaded += (s, args) =>
            {
                _flipAnimation = new FlipTileAnimation();
                //use a seed to randomise the number
                var randomSeed = (Int32)Windows.Security.Cryptography.CryptographicBuffer.GenerateRandomNumber();
                _random = new Random(randomSeed);

                ValidateFlipTime();

                if (_timer != null)
                {
                    _timer.Start();
                }
                else
                {
                    CreateTimer();
                    _timer.Start();
                }
            };

            this.Unloaded += (s, args) =>
            {
                if (_timer != null)
                {
                    _timer.Stop();
                }
            };

            this.Tapped += (s, args) =>
            {

                if (Command != null && Command.CanExecute(CommandParameter))
                    this.Command.Execute(CommandParameter);
            };
            #endregion
        }

        #endregion

        #region Dependency Properties

        public static DependencyProperty CommandProperty =
                    DependencyProperty.Register("Command", typeof(ICommand), typeof(FlipTile),
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
            DependencyProperty.Register("CommandParameter", typeof(object), typeof(FlipTile),
            new PropertyMetadata(null));


        public object CommandParameter
        {
            get { return (object)GetValue(CommandParameterProperty); }
            set { SetValue(CommandParameterProperty, value); }
        }

        public object FrontContent
        {
            get { return (object)GetValue(FrontContentProperty); }
            set { SetValue(FrontContentProperty, value); }
        }

        // Using a DependencyProperty as the backing store for FrontContent.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty FrontContentProperty =
            DependencyProperty.Register("FrontContent", typeof(object), typeof(FlipTile), new PropertyMetadata(null));


        public object BackContent
        {
            get { return (object)GetValue(BackContentProperty); }
            set { SetValue(BackContentProperty, value); }
        }

        // Using a DependencyProperty as the backing store for BackContent.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty BackContentProperty =
            DependencyProperty.Register("BackContent", typeof(object), typeof(FlipTile), new PropertyMetadata(null));


        public Brush EmptyContentBackground
        {
            get { return (Brush)GetValue(EmptyContentBackgroundProperty); }
            set { SetValue(EmptyContentBackgroundProperty, value); }
        }

        // Using a DependencyProperty as the backing store for EmptyContentBackground.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty EmptyContentBackgroundProperty =
            DependencyProperty.Register("EmptyContentBackground",
                                        typeof(Brush),
                                        typeof(FlipTile),
                                        new PropertyMetadata(new SolidColorBrush(Color.FromArgb(255, 60, 68, 68))));


        public int MinimumFlipTime
        {
            get { return (int)GetValue(MinimumFlipTimeProperty); }
            set { SetValue(MinimumFlipTimeProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MinimumFlipTime.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MinimumFlipTimeProperty =
            DependencyProperty.Register("MinimumFlipTime",
                                        typeof(int),
                                        typeof(FlipTile),
                                        new PropertyMetadata(MIN_FLIPTIME));


        public int MaximumFlipTime
        {
            get { return (int)GetValue(MaximumFlipTimeProperty); }
            set { SetValue(MaximumFlipTimeProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MaximumFlipTime.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MaximumFlipTimeProperty =
            DependencyProperty.Register("MaximumFlipTime",
                                        typeof(int),
                                        typeof(FlipTile),
                                        new PropertyMetadata(MAX_FLIPTIME));

        public bool IsRandomFlip
        {
            get { return (bool)GetValue(IsRandomFlipProperty); }
            set { SetValue(IsRandomFlipProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsRandomFlip.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsRandomFlipProperty =
            DependencyProperty.Register("IsRandomFlip",
                                        typeof(bool),
                                        typeof(FlipTile),
                                        new PropertyMetadata(true));


        public double FlipTime
        {
            get { return (double)GetValue(FlipTimeProperty); }
            set { SetValue(FlipTimeProperty, value); }
        }

        // Using a DependencyProperty as the backing store for FlipTime.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty FlipTimeProperty =
            DependencyProperty.Register("FlipTime",
                                        typeof(double),
                                        typeof(FlipTile),
                                        new PropertyMetadata(DEFAULT_FLIPTIME));

        public double TimeBetweenFlips
        {
            get { return (double)GetValue(TimeBetweenFlipsProperty); }
            set { SetValue(TimeBetweenFlipsProperty, value); }
        }

        // Using a DependencyProperty as the backing store for TimeBetweenFlips.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TimeBetweenFlipsProperty =
            DependencyProperty.Register("TimeBetweenFlips",
                                        typeof(double),
                                        typeof(FlipTile),
                                        new PropertyMetadata(DEFAULT_TIME_BETWEEN_FLIPS));

        #endregion

        public FrameworkElement FrontContentPresenter { get; set; }
        public FrameworkElement BackContentPresenter { get; set; }

        private bool _isFront = true;
        public bool IsFront
        {
            get { return _isFront; }
            set
            {
                if (_isFront == value)
                    return;
                _isFront = value;
            }
        }

        private FlipDirection _direction = FlipDirection.Left;
        public FlipDirection Direction
        {
            get { return _direction; }
            set
            {
                if (_direction == value)
                    return;
                _direction = value;
            }
        }

        protected override void OnApplyTemplate()
        {
            this.FrontContentPresenter = this.GetTemplateChild(FRONT_CONTENT) as FrameworkElement;
            this.BackContentPresenter = this.GetTemplateChild(BACK_CONTENT) as FrameworkElement;

            SetPlaneProjection(FrontContentPresenter);
            SetPlaneProjection(Direction, BackContentPresenter);

            _flipDetails = SetFlipDetails(Direction);

            base.OnApplyTemplate();
        }

        /// <summary>
        /// Creates the timer used to control when the tile flips
        /// </summary>
        private void CreateTimer()
        {
            _timer = new DispatcherTimer();

            //If IsRandomFlip then use the min and max fliptimes to get a random time
            //Otherwise just use TimeBetweenFlips
            if (IsRandomFlip)
                _timer.Interval = TimeSpan.FromMilliseconds(_random.Next(MinimumFlipTime, MaximumFlipTime));
            else
                _timer.Interval = TimeSpan.FromMilliseconds(TimeBetweenFlips);

            _timer.Tick += (s, args) =>
            {
                //Create the storyboard 
                var sb = _flipAnimation.GetStoryboard(FrontContentPresenter, BackContentPresenter,
                                                       _flipDetails, IsFront, FlipTime);

                //Its random so we need a random time between min and max
                if (IsRandomFlip)
                    _timer.Interval = TimeSpan.FromMilliseconds(_random.Next(MinimumFlipTime, MaximumFlipTime));

                sb.Completed += (sender, e) =>
                {
                    if (IsFront)
                        IsFront = false;
                    else
                        IsFront = true;
                };

                sb.Begin();
            };

        }

        /// <summary>
        /// Creates a PlaneProjection for UIElement
        /// </summary>
        /// <param name="element"></param>
        private void SetPlaneProjection(UIElement element)
        {
            var projection = new PlaneProjection();
            element.Projection = projection;
        }

        /// <summary>
        /// Creates a PlaneProjection for UIElement.
        /// Sets the starting rotation of the back tile. Ready for animation
        /// </summary>
        /// <param name="direction"></param>
        /// <param name="element"></param>
        private void SetPlaneProjection(FlipDirection direction, UIElement element)
        {
            var projection = new PlaneProjection();

            switch (direction)
            {
                case FlipDirection.Left:
                    projection.RotationY = -90;
                    break;
                case FlipDirection.Right:
                    projection.RotationY = 90;
                    break;
                case FlipDirection.Up:
                    projection.RotationX = 90;
                    break;
                case FlipDirection.Down:
                    projection.RotationX = -90;
                    break;
                default:
                    projection.RotationY = -90;
                    break;
            }

            element.Projection = projection;
        }


        /// <summary>
        /// Validates that the FlipTime is not greater than the TimeBetweenFlips
        /// this is to stop the animation performing incorrectly
        /// </summary>
        private void ValidateFlipTime()
        {
            if (FlipTime >= TimeBetweenFlips)
                TimeBetweenFlips = FlipTime + 1000;
        }

        /// <summary>
        /// Sets some conditions for the animation based on what direction the tile is going to flip 
        /// </summary>
        /// <param name="direction"></param>
        /// <returns></returns>
        private FlipDetails SetFlipDetails(FlipDirection direction)
        {
            var flipDetails = new FlipDetails();
            switch (direction)
            {
                case FlipDirection.Left:
                    flipDetails.FrontAnimationTo = 90;
                    flipDetails.BackAnimationTo = -90;
                    flipDetails.RotationAxis = Constants.ROTATIONY;
                    break;
                case FlipDirection.Right:
                    flipDetails.FrontAnimationTo = -90;
                    flipDetails.BackAnimationTo = 90;
                    flipDetails.RotationAxis = Constants.ROTATIONY;
                    break;
                case FlipDirection.Up:
                    flipDetails.FrontAnimationTo = -90;
                    flipDetails.BackAnimationTo = 90;
                    flipDetails.RotationAxis = Constants.ROTATIONX;
                    break;
                case FlipDirection.Down:
                    flipDetails.FrontAnimationTo = 90;
                    flipDetails.BackAnimationTo = -90;
                    flipDetails.RotationAxis = Constants.ROTATIONX;
                    break;
                default:
                    flipDetails.FrontAnimationTo = 90;
                    flipDetails.BackAnimationTo = -90;
                    flipDetails.RotationAxis = Constants.ROTATIONY;
                    break;
            }
            return flipDetails;
        }

    }

   
}