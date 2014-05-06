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
    public interface IRollOverAnimation
    {
        Storyboard GetStoryboard(BaseRollOverTileKeyFrames animationKeyFrames, FrameworkElement frontElement,
                                    TranslateTransform frontTransform, double slideTime,
                                    SlideDirection direction, bool isFront);
    }
}
