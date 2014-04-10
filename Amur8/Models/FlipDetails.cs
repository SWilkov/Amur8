using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Amur8.Models
{
    /// <summary>
    /// Helper class used to set rotation details dependent on direction of rotation
    /// </summary>
    public class FlipDetails
    {
        public double FrontAnimationTo { get; set; }
        public double BackAnimationTo { get; set; }
        public string RotationAxis { get; set; }
    }
}
