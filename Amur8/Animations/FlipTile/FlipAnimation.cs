using Amur8.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media.Animation;

namespace Amur8.Animations
{
    /// <summary>
    /// FlipTileAnimation creates the storyboard to rotate (flip) the tile
    /// </summary>
    public class FlipTileAnimation
    {

        public Storyboard sbOpenFront;
        public Storyboard sbCloseFront;
        public Storyboard sbOpenBack;
        public Storyboard sbCloseBack;
        
        private double _animationSpeed;
        private FlipDetails _flipDetails;

        private FrameworkElement _frontContent;
        private FrameworkElement _backContent;


        public FlipTileAnimation()
        {

        }

        public FlipTileAnimation(FrameworkElement frontContent, FrameworkElement backContent,
                                    double flipTime, FlipDetails flipDetails)
        {
            _animationSpeed = flipTime / 2;
            _frontContent = frontContent;
            _backContent = backContent;
            _flipDetails = flipDetails;

            CreateStoryboards();
        }

        private void CreateStoryboards()
        {
            sbCloseFront = new Storyboard();
            var closeFrontAnimation = new DoubleAnimation()
            {
                To = _flipDetails.FrontAnimationTo,
                EnableDependentAnimation = true,
                Duration = new Duration(TimeSpan.FromMilliseconds(_animationSpeed)),
                From = 1
            };
            Storyboard.SetTarget(closeFrontAnimation, _frontContent);
            Storyboard.SetTargetProperty(closeFrontAnimation, _flipDetails.RotationAxis);
            sbCloseFront.Children.Add(closeFrontAnimation);
            sbCloseFront.Completed += (s, args) =>
                {
                    sbOpenBack.Begin();
                };

            sbOpenBack = new Storyboard();
            var openBackAnimation = new DoubleAnimation()
            {
                To = 0,
                EnableDependentAnimation = true,
                Duration = new Duration(TimeSpan.FromMilliseconds(_animationSpeed)),
                From = _flipDetails.BackAnimationTo
            };
            Storyboard.SetTarget(openBackAnimation, _backContent);
            Storyboard.SetTargetProperty(openBackAnimation, _flipDetails.RotationAxis);
            sbOpenBack.Children.Add(openBackAnimation);
            
            sbCloseBack = new Storyboard();
            var closeBackAnimation = new DoubleAnimation()
            {
                To = _flipDetails.BackAnimationTo,
                EnableDependentAnimation = true,
                Duration = new Duration(TimeSpan.FromMilliseconds(_animationSpeed)),
                From = 1
            };
            Storyboard.SetTarget(closeBackAnimation, _backContent);
            Storyboard.SetTargetProperty(closeBackAnimation, _flipDetails.RotationAxis);
            sbCloseBack.Children.Add(closeBackAnimation);
            sbCloseBack.Completed += (s, args) =>
            {
                sbOpenFront.Begin();
            };

            sbOpenFront = new Storyboard();
            var openFrontAnimation = new DoubleAnimation()
            {
                To = 0,
                EnableDependentAnimation = true,
                Duration = new Duration(TimeSpan.FromMilliseconds(_animationSpeed)),
                From = _flipDetails.FrontAnimationTo
            };
            Storyboard.SetTarget(openFrontAnimation, _frontContent);
            Storyboard.SetTargetProperty(openFrontAnimation, _flipDetails.RotationAxis);
            sbOpenFront.Children.Add(openFrontAnimation);
        } 
    }
}
