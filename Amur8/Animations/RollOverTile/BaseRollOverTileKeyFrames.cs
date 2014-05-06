using Amur8.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;

namespace Amur8.Animations
{
    public abstract class BaseRollOverTileKeyFrames
    {
         public const double START_TIME = 100;
        public double PauseTime;
        public double HalfSlideTime;
        public double SlideTime;

        public double MidPointValue;
        public double EndPointValue;

        public List<RollOverAnimationDetails> OpenAnimations { get; set; }
        public List<RollOverAnimationDetails> CloseAnimations { get; set; }

        public BaseRollOverTileKeyFrames(double slideTime, SlideDirection direction, FrameworkElement element)
        {
            this.SlideTime = slideTime;
            this.HalfSlideTime = slideTime / 2;
            this.PauseTime = slideTime / 5;

            this.SetPointValues(direction, element);
        }

        protected virtual void SetFrames() {}
        private void SetPointValues(SlideDirection direction, FrameworkElement frontElement)
        {
            if (direction == SlideDirection.LeftToRight)
            {
                this.MidPointValue = frontElement.Width / 2;
                this.EndPointValue = frontElement.Width;
            }
            else
            {
                this.MidPointValue = frontElement.Height / 2;
                this.EndPointValue = frontElement.Height;
            }
        }
    }
}
