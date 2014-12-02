using Amur8.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media.Animation;

namespace Amur8.Animations
{
    public class MoveImageAnimation
    {
        public Storyboard sbMoveImage;
        private FrameworkElement _mainImage;
        private FrameworkElement _mainGrid;
        private double imageWidth;
        private double imageHeight;

        private double gridWidth;
        private double gridHeight;
        private double _delayTime;
        private Speed _speed;

        public MoveImageAnimation(FrameworkElement mainImage, FrameworkElement grid, double delay, Speed speed)
        {
            _mainGrid = grid;
            _mainImage = mainImage;

            gridWidth = grid.ActualWidth;
            gridHeight = grid.ActualHeight;
            _delayTime = delay;
            _speed = speed;

            CreateStoryboard();
        }

        private void CreateStoryboard()
        {
            sbMoveImage = new Storyboard()
            {
                AutoReverse = true,
                RepeatBehavior = RepeatBehavior.Forever,
                BeginTime = TimeSpan.FromMilliseconds(_delayTime)
            };

            imageWidth = _mainImage.ActualWidth;
            imageHeight = _mainImage.ActualHeight;

            var xMove = (imageWidth - gridWidth) - 1;
            var xMoveFinal = (imageWidth / 8);
            var yMove = (imageHeight - gridHeight) - 1;
            double bottomY = -10; //so that the image never moves below its size

            var animationForward = new DoubleAnimationUsingKeyFrames();
            var ease1 = new EasingDoubleKeyFrame { KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromSeconds(_speed.speedA)), Value = -xMove };
            var ease2 = new EasingDoubleKeyFrame { KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromSeconds(_speed.speedB)), Value = 0 };
            var ease3 = new EasingDoubleKeyFrame { KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromSeconds(_speed.speedC)), Value = -xMoveFinal };

            animationForward.KeyFrames.Add(ease1);
            animationForward.KeyFrames.Add(ease2);
            animationForward.KeyFrames.Add(ease3);

            Storyboard.SetTargetProperty(animationForward, Constants.TRANSLATEX);
            Storyboard.SetTarget(animationForward, _mainImage);
            sbMoveImage.Children.Add(animationForward);

            var animationBack = new DoubleAnimationUsingKeyFrames();
            var ease4 = new EasingDoubleKeyFrame { KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromSeconds(_speed.speedA)), Value = -yMove };
            var ease5 = new EasingDoubleKeyFrame { KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromSeconds(_speed.speedB)), Value = -yMove };
            var ease6 = new EasingDoubleKeyFrame { KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromSeconds(_speed.speedC)), Value = bottomY };

            animationBack.KeyFrames.Add(ease4);
            animationBack.KeyFrames.Add(ease5);
            animationBack.KeyFrames.Add(ease6);

            Storyboard.SetTargetProperty(animationBack, Constants.TRANSLATEY);
            Storyboard.SetTarget(animationBack, _mainImage);
            sbMoveImage.Children.Add(animationBack);
        }
    }
}
