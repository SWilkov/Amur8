using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media.Animation;

namespace Amur8.Animations
{
    public class TiltAnimation : IMovingAnimation
    {
        public Storyboard sbTilt { get; set; }
        private FrameworkElement _element;

        private const double ANIMATION_DURATION = 100.0;
        private const double TILT_ANGLE = -20.0;

        public TiltAnimation()
        {

        }

        public void CreateStoryboard(FrameworkElement element)
        {
            _element = element;
            sbTilt = new Storyboard
            {
                AutoReverse = true
            };

            var animation = new DoubleAnimation() { Duration = TimeSpan.FromMilliseconds(ANIMATION_DURATION), To = TILT_ANGLE };
            Storyboard.SetTargetProperty(animation, Constants.ROTATIONY);
            Storyboard.SetTarget(animation, _element);
            sbTilt.Children.Add(animation);
        }

        public void StartStoryboard()
        {
            sbTilt.Begin();
        }


        public void StopStoryboard()
        {
            sbTilt.Stop();
        }
    }
}
