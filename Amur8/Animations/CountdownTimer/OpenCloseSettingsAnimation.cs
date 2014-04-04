using Amur8.Controls.CountdownTimer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;

namespace Amur8.Animations
{
    /// <summary>
    /// OpenCloseSettingsAnimation handles opening and closing the settings panel using storyboards
    /// </summary>
    public class OpenCloseSettingsAnimation
    {
        #region Constants used for Storyboard.TargetProperty

        private const string GLOBAL_OFFSETY = "(UIElement.Projection).(PlaneProjection.GlobalOffsetY)";
        private const string TRANSLATEX = "(UIElement.RenderTransform).(CompositeTransform.TranslateX)";
        private const string TRANSLATEY = "(UIElement.RenderTransform).(CompositeTransform.TranslateY)";
        private const string SCALEX = "(UIElement.RenderTransform).(CompositeTransform.ScaleX)";
        private const string SCALEY = "(UIElement.RenderTransform).(CompositeTransform.ScaleY)";

        #endregion

        #region private fields

        private string _currentTranslate;
        private string _currentsScale;
        private double _currentOffset;

        #endregion

        /// <summary>
        /// Creates the Opening Settings Storyboard and animations
        /// </summary>
        /// <param name="sliderGrid">Settings Grid conatining slider controls</param>
        /// <param name="arrowGlyph">Arrow box used to cloe settings grid</param>
        /// <param name="direction">user defined direction control opens</param>
        /// <param name="closeDuration">user defined opening time of settings grid</param>
        /// <returns></returns>
        public Storyboard GetOpenStoryboard(Grid sliderGrid, Grid arrowGlyph,
                                            OpeningDirection direction, double openDuration,
                                            double width, double height)
        {
            //Set the translate, scale and distance by Direction
            SetTranslateScaleSettings(direction, width, height);

            //we need to account half the time to each animation so divide users time by 2
            double animationTime = openDuration / 2;
            
            var sb = new Storyboard();
            
            var duration = new Duration(TimeSpan.FromMilliseconds(animationTime));            
            
            
            var animation = new DoubleAnimation()
            {
                To = _currentOffset,
                Duration = duration
            };
            Storyboard.SetTargetProperty(animation, _currentTranslate);
            Storyboard.SetTarget(animation, arrowGlyph);
            sb.Children.Add(animation);

            var animation2 = new DoubleAnimation()
            {
                To = _currentOffset,
                Duration = duration
            };
            Storyboard.SetTargetProperty(animation2, _currentTranslate);
            Storyboard.SetTarget(animation2, sliderGrid);
            sb.Children.Add(animation2);

            var expandedAnimation = new DoubleAnimationUsingKeyFrames();
            expandedAnimation.KeyFrames.Add(new EasingDoubleKeyFrame()
            {
                KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromMilliseconds(animationTime)),
                Value = 1
            });
            expandedAnimation.KeyFrames.Add(new EasingDoubleKeyFrame()
            {
                KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromMilliseconds(openDuration)),
                Value = Constants.EXPANDED_SCALE_SIZE
            });

            Storyboard.SetTargetProperty(expandedAnimation, _currentsScale);
            Storyboard.SetTarget(expandedAnimation, sliderGrid);
            sb.Children.Add(expandedAnimation);
            return sb;
        }

        /// <summary>
        /// Creates the Close Settings Storyboard and animations
        /// </summary>
        /// <param name="sliderGrid">Settings Grid conatining slider controls</param>
        /// <param name="arrowGlyph">Arrow box used to cloe settings grid</param>
        /// <param name="direction">user defined direction control opens</param>
        /// <param name="closeDuration">user defined closing time of settings grid</param>
        /// <returns></returns>
        public Storyboard GetCloseStoryboard(Grid sliderGrid, Grid arrowGlyph,
                                            OpeningDirection direction, double closeDuration)
        {
            //offset is 0 as we are closing the grids
            double returnOffset = 0.0;
           
            //we need to account half the time to each animation so divide users time by 2
            double animationTime = closeDuration / 2;
            
            var sb = new Storyboard();                      

            var duration = new Duration(TimeSpan.FromMilliseconds(animationTime));
            
            //when closing first animation is shrinking the Scale
            var animation = new DoubleAnimation()
            {
                To = 1,
                Duration = duration
            };
            Storyboard.SetTargetProperty(animation, _currentsScale);
            Storyboard.SetTarget(animation, sliderGrid);
            sb.Children.Add(animation);

            var expandedAnimation = new DoubleAnimationUsingKeyFrames();
            expandedAnimation.KeyFrames.Add(new EasingDoubleKeyFrame()
            {
                KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromMilliseconds(animationTime)),
                Value = _currentOffset
            });
            expandedAnimation.KeyFrames.Add(new EasingDoubleKeyFrame()
            {
                KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromMilliseconds(closeDuration)),
                Value = returnOffset
            });

            Storyboard.SetTargetProperty(expandedAnimation, _currentTranslate);
            Storyboard.SetTarget(expandedAnimation, arrowGlyph);
            sb.Children.Add(expandedAnimation);

            var expandedAnimation2 = new DoubleAnimationUsingKeyFrames();
            expandedAnimation2.KeyFrames.Add(new EasingDoubleKeyFrame()
            {
                KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromMilliseconds(animationTime)),
                Value = _currentOffset
            });
            expandedAnimation2.KeyFrames.Add(new EasingDoubleKeyFrame()
            {
                KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromMilliseconds(closeDuration)),
                Value = returnOffset
            });
            Storyboard.SetTargetProperty(expandedAnimation2, _currentTranslate);
            Storyboard.SetTarget(expandedAnimation2, sliderGrid);
            sb.Children.Add(expandedAnimation2);

            return sb;
        }

        /// <summary>
        /// Sets the Translate and Scale axis dependent on control OpeningDirection setting
        /// </summary>
        /// <param name="direction">the direction the settings panel will open</param>
        /// <param name="width">width of the panel</param>
        /// <param name="height">height of the panel</param>
        private void SetTranslateScaleSettings(OpeningDirection direction, double width, double height)
        {
            switch (direction)
            {
                case OpeningDirection.Left:
                    _currentTranslate = TRANSLATEX;
                    _currentsScale = SCALEY;
                    _currentOffset = -width;
                    break;
                case OpeningDirection.Right:
                    _currentTranslate = TRANSLATEX;
                    _currentsScale = SCALEY;
                    _currentOffset = width;
                    break;
                case OpeningDirection.Up:
                    _currentTranslate = TRANSLATEY;
                    _currentsScale = SCALEX;
                    _currentOffset = -height;
                    break;                  
                case OpeningDirection.Down:
                    _currentTranslate = TRANSLATEY;
                    _currentsScale = SCALEX;
                    _currentOffset = height;
                    break;                         
                default:
                    _currentTranslate = TRANSLATEX;
                    _currentsScale = SCALEY;
                    _currentOffset = -width;
                    break;                        
            }
        }       
       
    }
}
