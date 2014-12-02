using Amur8.Animations;
using Amur8.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;

// The Templated Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234235

namespace Amur8.Controls
{
    public enum MoveSpeed { Slow, Medium, Fast }

    [TemplatePart(Name = "PART_MainGrid", Type = typeof(FrameworkElement))]
    [TemplatePart(Name = "PART_MainImage", Type = typeof(FrameworkElement))]
    [TemplatePart(Name = "PART_MainBorder", Type = typeof(FrameworkElement))]
    public sealed class MovingImageButton : Button
    {
        private Border mainBorder { get; set; }
        private Grid mainGrid { get; set; }
        private Image mainImage { get; set; }
        private MoveImageAnimation MoveAnimation { get; set; }
        private Speed SelectedSpeed { get; set; }
        public MovingImageButton()
        {
            this.DefaultStyleKey = typeof(MovingImageButton);

            this.Loaded += (s, args) =>
            {
                this.ValidateBorderThickness();

                if (this.ButtonEffectEnabled)
                    PressedAnimation.CreateStoryboard(mainBorder);

                this.SelectedSpeed = new Speed(this.MoveSpeed);

                this.MoveAnimation = new MoveImageAnimation(mainImage, mainGrid, this.Delay, this.SelectedSpeed);
                this.MoveAnimation.sbMoveImage.Begin();
            };
        }

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            mainBorder = this.GetTemplateChild("PART_MainBorder") as Border;
            mainGrid = this.GetTemplateChild("PART_MainGrid") as Grid;
            mainImage = this.GetTemplateChild("PART_MainImage") as Image;
        }

        protected override void OnPointerPressed(PointerRoutedEventArgs e)
        {
            if (this.ButtonEffectEnabled)
            {
                PressedAnimation.StopStoryboard();
                PressedAnimation.StartStoryboard();
            }
        }

        public double ImageWidth
        {
            get { return (double)GetValue(ImageWidthProperty); }
            set { SetValue(ImageWidthProperty, value); }
        }

        public MoveSpeed MoveSpeed
        {
            get { return (MoveSpeed)GetValue(MoveSpeedProperty); }
            set { SetValue(MoveSpeedProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MoveSpeed.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MoveSpeedProperty =
            DependencyProperty.Register("MoveSpeed",
                                        typeof(MoveSpeed),
                                        typeof(MovingImageButton),
                                        new PropertyMetadata(MoveSpeed.Slow));

        public static void MoveSpeedChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var ctrl = (MovingImageButton)d;
            if (ctrl != null)
                ctrl.SelectedSpeed = new Speed(ctrl.MoveSpeed);
        }

        public double Delay
        {
            get { return (double)GetValue(DelayProperty); }
            set { SetValue(DelayProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Delay.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DelayProperty =
            DependencyProperty.Register("Delay", typeof(double), typeof(MovingImageButton), new PropertyMetadata(0.0));



        // Using a DependencyProperty as the backing store for ImageWidth.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ImageWidthProperty =
            DependencyProperty.Register("ImageWidth",
                                        typeof(double),
                                        typeof(MovingImageButton),
                                        new PropertyMetadata(200));

        private static void ImageWidthChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var ctrl = (MovingImageButton)d;
            if (ctrl.mainGrid != null)
                ctrl.ImageWidth = (ctrl.mainGrid.ActualWidth - ctrl.ImageWidth <= 10)
                                        ? ctrl.mainGrid.ActualWidth + 10
                                        : ctrl.ImageWidth;
        }

        public double ImageHeight
        {
            get { return (double)GetValue(ImageHeightProperty); }
            set { SetValue(ImageHeightProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ImageHeight.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ImageHeightProperty =
            DependencyProperty.Register("ImageHeight",
                                        typeof(double),
                                        typeof(MovingImageButton),
                                        new PropertyMetadata(200, ImageHeightChanged));

        private static void ImageHeightChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var item = (MovingImageButton)d;
            if (item.mainGrid != null)
                item.ImageHeight = (item.mainGrid.ActualHeight - item.ImageHeight <= 10)
                                        ? item.mainGrid.ActualHeight + 10
                                        : item.ImageHeight;
        }

        public string ImageSource
        {
            get { return (string)GetValue(ImageSourceProperty); }
            set { SetValue(ImageSourceProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ImageSource.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ImageSourceProperty =
            DependencyProperty.Register("ImageSource", typeof(string), typeof(MovingImageButton), new PropertyMetadata(null));


        public bool ButtonEffectEnabled
        {
            get { return (bool)GetValue(ButtonEffectEnabledProperty); }
            set { SetValue(ButtonEffectEnabledProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ButtonEffectEnabled.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ButtonEffectEnabledProperty =
            DependencyProperty.Register("ButtonEffectEnabled", typeof(bool), typeof(MovingImageButton), new PropertyMetadata(false));


        public IMovingAnimation PressedAnimation
        {
            get { return (IMovingAnimation)GetValue(PressedAnimationProperty); }
            set { SetValue(PressedAnimationProperty, value); }
        }

        // Using a DependencyProperty as the backing store for PressedAnimation.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PressedAnimationProperty =
            DependencyProperty.Register("PressedAnimation",
                                        typeof(IMovingAnimation),
                                        typeof(MovingImageButton),
                                        new PropertyMetadata(new TiltAnimation()));

        public object FrontContent
        {
            get { return (object)GetValue(FrontContentProperty); }
            set { SetValue(FrontContentProperty, value); }
        }

        // Using a DependencyProperty as the backing store for FrontContent.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty FrontContentProperty =
            DependencyProperty.Register("FrontContent", typeof(object), typeof(MovingImageButton), new PropertyMetadata(null));


        private void ValidateBorderThickness()
        {
            var zeroThickness = new Thickness(0);
            if (this.BorderThickness == zeroThickness)
                this.BorderThickness = new Thickness(1);
        }       
    }
}
