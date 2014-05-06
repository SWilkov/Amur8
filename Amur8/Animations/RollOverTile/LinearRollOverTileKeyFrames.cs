using Amur8.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.Xaml;

namespace Amur8.Animations
{
    public class LinearRollOverTileKeyFrames : BaseRollOverTileKeyFrames
    {
        public LinearRollOverTileKeyFrames(double slideTime, SlideDirection direction, FrameworkElement element) 
            : base(slideTime, direction, element)
        {
            this.SetFrames();
        }
        protected override void SetFrames()
        {
            this.OpenAnimations = new List<RollOverAnimationDetails>()
            {
                new RollOverAnimationDetails { KeyTime = START_TIME, KeyValue = 0 },
                new RollOverAnimationDetails { Point1 = new Point(0.3,0), Point2 = new Point(0.3,0), KeyTime = SlideTime, KeyValue = EndPointValue }
            };
            this.CloseAnimations = new List<RollOverAnimationDetails>()
            {
                new RollOverAnimationDetails { KeyTime = START_TIME, KeyValue = EndPointValue },
                new RollOverAnimationDetails { Point1 = new Point(0.3,0), Point2 = new Point(0.3,0), KeyTime = SlideTime, KeyValue = 0 }
            };
        }    
    }
}
