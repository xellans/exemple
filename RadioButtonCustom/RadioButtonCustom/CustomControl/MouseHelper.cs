using System.Windows;
using System.Windows.Input;

namespace RadioButtonCustom.CustomControl
{
    public static class MouseHelper
    {
        private static object trueBox = true;
        private static object falseBox = false;
        public static bool GetIsDownPosition(UIElement uie)
        {
            return (bool)uie.GetValue(IsDownPositionProperty);
        }

        public static void SetIsDownPosition(UIElement uie, bool value)
        {
            uie.SetValue(IsDownPositionProperty, value ? trueBox : falseBox);
        }

        // Using a DependencyProperty as the backing store for IsDownPosition.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsDownPositionProperty =
            DependencyProperty.RegisterAttached(
                "IsDownPosition",
                typeof(bool),
                typeof(MouseHelper),
                new PropertyMetadata(falseBox)
                {
                    PropertyChangedCallback = IsDownPositionChanged
                });

        private static void IsDownPositionChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is not UIElement uie)
            {
                throw new Exception("Только для UIElement.");
            }
            if (e.OldValue != e.NewValue)
            {
                if (Equals(e.NewValue, trueBox))
                {
                    uie.AddHandler(UIElement.MouseDownEvent, (MouseButtonEventHandler)OnMouseDown, true);
                    //uie.MouseDown += OnMouseDown;
                }
                else
                {
                    uie.RemoveHandler(UIElement.MouseDownEvent, (MouseButtonEventHandler)OnMouseDown);
                    //uie.MouseDown -= OnMouseDown;
                    uie.ClearValue(LastDownPositionPropertyKey);
                }
            }
        }

        private static void OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            UIElement uie = (UIElement)sender;
            Point mousePos = e.GetPosition(uie);
            SetLastDownPosition(uie, mousePos);
        }

        public static Point? GetLastDownPosition(UIElement uie)
        {
            return (Point?)uie.GetValue(LastDownPositionProperty);
        }

        private static void SetLastDownPosition(UIElement uie, Point? value)
        {
            uie.SetValue(LastDownPositionPropertyKey, value);
        }

        // Using a DependencyProperty as the backing store for DownPosition.  This enables animation, styling, binding, etc...
        public static readonly DependencyPropertyKey LastDownPositionPropertyKey =
            DependencyProperty.RegisterAttachedReadOnly("LastDownPosition", typeof(Point?), typeof(MouseHelper), new PropertyMetadata((Point?)null));
        public static readonly DependencyProperty LastDownPositionProperty = LastDownPositionPropertyKey.DependencyProperty;

    }
}
