using Amur8.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media.Animation;

namespace Amur8.Animations
{
    /// <summary>
    /// FlipTileAnimation creates the storyboard to rotate (flip) the tile
    /// </summary>
    public class FlipTileAnimation
    {

        public Storyboard GetStoryboard(FrameworkElement FrontContent, FrameworkElement BackContent,
                                        FlipDetails flipDetails, bool isFront, double flipTime)
        {
            var sb = new Storyboard();
            var animationSpeed = flipTime / 2;
            var duration = new Duration(TimeSpan.FromMilliseconds(animationSpeed));
            FrameworkElement item1;
            FrameworkElement item2;

            if (isFront)
            {
                item1 = FrontContent;
                item2 = BackContent;
            }
            else
            {
                item1 = BackContent;
                item2 = FrontContent;
            }

            var animation = new DoubleAnimation()
            {
                Duration = duration,
                To = flipDetails.FrontAnimationTo
            };
            Storyboard.SetTargetProperty(animation, flipDetails.RotationAxis);
            Storyboard.SetTarget(animation, item1);
            sb.Children.Add(animation);

            var d = new DiscreteObjectKeyFrame() { KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromMilliseconds(animationSpeed)), Value = Visibility.Collapsed };
            var hideGrid = new ObjectAnimationUsingKeyFrames();
            hideGrid.KeyFrames.Add(d);
            Storyboard.SetTargetProperty(hideGrid, Constants.VISIBLITY);
            Storyboard.SetTarget(hideGrid, item1);
            sb.Children.Add(hideGrid);

            var d2 = new DiscreteObjectKeyFrame() { KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromMilliseconds(animationSpeed)), Value = Visibility.Visible };
            var hideGrid2 = new ObjectAnimationUsingKeyFrames();
            hideGrid2.KeyFrames.Add(d2);
            Storyboard.SetTargetProperty(hideGrid2, Constants.VISIBLITY);
            Storyboard.SetTarget(hideGrid2, item2);
            sb.Children.Add(hideGrid2);

            var animation2 = new DoubleAnimationUsingKeyFrames();
            var ease1 = new EasingDoubleKeyFrame() { KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromMilliseconds(animationSpeed)), Value = flipDetails.BackAnimationTo };
            var ease2 = new EasingDoubleKeyFrame() { KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromMilliseconds(flipTime)), Value = 0 };
            animation2.KeyFrames.Add(ease1);
            animation2.KeyFrames.Add(ease2);
            Storyboard.SetTargetProperty(animation2, flipDetails.RotationAxis);
            Storyboard.SetTarget(animation2, item2);
            sb.Children.Add(animation2);

            return sb;
        }
    }
}
