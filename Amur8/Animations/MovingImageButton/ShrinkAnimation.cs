using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media.Animation;

namespace Amur8.Animations
{
    public class ShrinkAnimation : IMovingAnimation
    {
        public Storyboard sbShrink { get; set; }
        private FrameworkElement _element;

        private const double ANIMATION_DURATION = 100.0;
        private const double ELEMENT_SIZE = 0.97;

        public void CreateStoryboard(FrameworkElement element)
        {
            _element = element;
            sbShrink = new Storyboard
            {
                AutoReverse = true
            };

            var xAnimation = new DoubleAnimation { Duration = TimeSpan.FromMilliseconds(ANIMATION_DURATION), To = ELEMENT_SIZE };
            var yAnimation = new DoubleAnimation { Duration = TimeSpan.FromMilliseconds(ANIMATION_DURATION), To = ELEMENT_SIZE };

            Storyboard.SetTargetProperty(xAnimation, Constants.SCALEX);
            Storyboard.SetTarget(xAnimation, _element);

            Storyboard.SetTargetProperty(yAnimation, Constants.SCALEY);
            Storyboard.SetTarget(yAnimation, _element);

            sbShrink.Children.Add(xAnimation);
            sbShrink.Children.Add(yAnimation);
        }

        public void StartStoryboard()
        {
            sbShrink.Begin();
        }


        public void StopStoryboard()
        {
            sbShrink.Stop();
        }
    }
}
