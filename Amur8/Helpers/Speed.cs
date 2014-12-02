using Amur8.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Amur8.Helpers
{
    public class Speed
    {
        public double speedA;
        public double speedB;
        public double speedC;

        public Speed(MoveSpeed speed)
        {
            this.SetSpeed(speed);
        }

        private void SetSpeed(MoveSpeed moveSpeed)
        {
            switch (moveSpeed)
            {
                case MoveSpeed.Slow:
                    speedA = Constants.SLOW_SPEED_A;
                    speedB = Constants.SLOW_SPEED_B;
                    speedC = Constants.SLOW_SPEED_C;
                    break;
                case MoveSpeed.Medium:
                    speedA = Constants.MEDIUM_SPEED_A;
                    speedB = Constants.MEDIUM_SPEED_B;
                    speedC = Constants.MEDIUM_SPEED_C;
                    break;
                case MoveSpeed.Fast:
                    speedA = Constants.FAST_SPEED_A;
                    speedB = Constants.FAST_SPEED_B;
                    speedC = Constants.FAST_SPEED_C;
                    break;
            }
        }
    }
}
