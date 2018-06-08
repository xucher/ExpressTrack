using System;
using System.Windows;
using System.Windows.Media.Animation;

//TODO: unfinished
namespace ExpressTrack.Style.Animations {
    public class GridLengthAnimation : AnimationTimeline {
        public GridLengthAnimation() {

        }

        public override Type TargetPropertyType => typeof(GridLength);

        protected override Freezable CreateInstanceCore() {
            return new GridLengthAnimation();
        }
        public static readonly DependencyProperty FromProperty = DependencyProperty.Register("From", typeof(GridLength),
          typeof(GridLengthAnimation));

        public GridLength From {
            get {
                return (GridLength)GetValue(FromProperty);
            }
            set {
                SetValue(FromProperty, value);
            }
        }
        public static readonly DependencyProperty ToProperty = DependencyProperty.Register("To", typeof(GridLength),
          typeof(GridLengthAnimation));
        public GridLength To {
            get {
                return (GridLength)GetValue(GridLengthAnimation.ToProperty);
            }
            set {
                SetValue(GridLengthAnimation.ToProperty, value);
            }
        }

        public override object GetCurrentValue(object defaultOriginValue,
          object defaultDestinationValue, AnimationClock animationClock) {
            double fromVal = ((GridLength)GetValue(FromProperty)).Value;

            double toVal = ((GridLength)GetValue(ToProperty)).Value;

            if (fromVal > toVal)
                return new GridLength((1 - animationClock.CurrentProgress.Value) * (fromVal - toVal) + toVal, GridUnitType.Star);
            else
                return new GridLength(animationClock.CurrentProgress.Value * (toVal - fromVal) + fromVal, GridUnitType.Star);
        }
    }
}
