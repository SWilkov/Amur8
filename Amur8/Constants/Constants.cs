using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Amur8
{
    public static class Constants
    {
        public const string GLOBAL_OFFSETY = "(UIElement.Projection).(PlaneProjection.GlobalOffsetY)";
        public const string TRANSLATEX = "(UIElement.RenderTransform).(CompositeTransform.TranslateX)";
        public const string TRANSLATEY = "(UIElement.RenderTransform).(CompositeTransform.TranslateY)";
        public const string SCALEX = "(UIElement.RenderTransform).(CompositeTransform.ScaleX)";
        public const string SCALEY = "(UIElement.RenderTransform).(CompositeTransform.ScaleY)";
        public const string ROTATIONY = "(UIElement.Projection).(PlaneProjection.RotationY)";
        public const string ROTATIONX = "(UIElement.Projection).(PlaneProjection.RotationX)";
        public const string VISIBLITY = "(UIElement.Visibility)";

        public const double SLOW_SPEED_A = 30;
        public const double SLOW_SPEED_B = 60;
        public const double SLOW_SPEED_C = 90;

        public const double MEDIUM_SPEED_A = 20;
        public const double MEDIUM_SPEED_B = 40;
        public const double MEDIUM_SPEED_C = 60;

        public const double FAST_SPEED_A = 10;
        public const double FAST_SPEED_B = 20;
        public const double FAST_SPEED_C = 30;
    }
}
