using Amur8.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;

namespace Amur8.Animations
{
    public class RollOverTileAnimation : IRollOverAnimation
    {
        private const string Y_Translate_Axis = "Y";
        private const string X_Translate_Axis = "X";

        public Storyboard GetStoryboard(BaseRollOverTileKeyFrames animationKeyFrames, FrameworkElement frontElement, 
                                        TranslateTransform frontTransform, double slideTime, SlideDirection direction, bool isFront)
        {
            string slideAxis = string.Empty;
            if (direction == SlideDirection.LeftToRight)
                slideAxis = X_Translate_Axis;
            else
                slideAxis = Y_Translate_Axis;

            var sb = new Storyboard();

            var animation = new DoubleAnimationUsingKeyFrames()
            {
                BeginTime = TimeSpan.FromMilliseconds(500),
                EnableDependentAnimation = true,
                Duration = new Windows.UI.Xaml.Duration(TimeSpan.FromMilliseconds(slideTime))
            };
            var showHide = new DoubleAnimation()
            {
                Duration = new Duration(TimeSpan.FromMilliseconds(0))
            };

            if (isFront)
            {
                foreach (var frame in animationKeyFrames.CloseAnimations)
                {
                    animation.KeyFrames.Add(
                        new SplineDoubleKeyFrame
                        {
                            KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromMilliseconds(frame.KeyTime)),
                            KeySpline = new KeySpline() { ControlPoint1 = frame.Point1, ControlPoint2 = frame.Point2 },
                            Value = frame.KeyValue
                        });
                }
                showHide.BeginTime = TimeSpan.FromMilliseconds(animationKeyFrames.CloseAnimations.Last().KeyTime + 500);
                showHide.From = 1;
                showHide.To = 0;
            }
            else
            {
                foreach (var frame in animationKeyFrames.OpenAnimations)
                {
                    animation.KeyFrames.Add(
                        new SplineDoubleKeyFrame
                        {
                            KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromMilliseconds(frame.KeyTime)),
                            KeySpline = new KeySpline() { ControlPoint1 = frame.Point1, ControlPoint2 = frame.Point2 },
                            Value = frame.KeyValue
                        });
                }

                showHide.BeginTime = TimeSpan.FromMilliseconds(500);
                showHide.From = 0;
                showHide.To = 1;
            }

            Storyboard.SetTarget(animation, frontTransform);
            Storyboard.SetTargetProperty(animation, slideAxis);
            sb.Children.Add(animation);

            Storyboard.SetTarget(showHide, frontElement);
            Storyboard.SetTargetProperty(showHide, "Opacity");
            sb.Children.Add(showHide);

            return sb;
        }
    }
}
