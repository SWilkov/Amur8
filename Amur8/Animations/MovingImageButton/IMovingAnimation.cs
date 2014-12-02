using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;

namespace Amur8.Animations
{
    public interface IMovingAnimation
    {
        void CreateStoryboard(FrameworkElement element);
        void StartStoryboard();
        void StopStoryboard();
    }
}
