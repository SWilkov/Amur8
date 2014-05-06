using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;

namespace Amur8.Animations
{
    public class RollOverAnimationDetails
    {
        public double KeyTime { get; set; }
        public double KeyValue { get; set; }

        private Point _point1 = new Point(0, 0);
        public Point Point1
        {
            get { return _point1; }
            set
            {
                if (_point1 == value)
                    return;
                _point1 = value;
            }
        }

        private Point _point2 = new Point(1, 1);
        public Point Point2
        {
            get { return _point2; }
            set
            {
                if (_point2 == value)
                    return;
                _point2 = value;
            }
        }
    }
}
