﻿using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace RadioButtonCustom.CustomControl
{
    public class RippleRadioButton_SelectedCircle : RadioButton
    {
        #region RippleChecked
        public Color RippleChecked
        {
            get { return (Color)GetValue(RippleCheckedProperty); }
            set { SetValue(RippleCheckedProperty, value); }
        }
        public static Color GetRippleChecked(UIElement element) => (Color)element.GetValue(RippleCheckedProperty);

        public static void SetRippleChecked(UIElement element, Color value) => element.SetValue(RippleCheckedProperty, value);

        public static readonly DependencyProperty RippleCheckedProperty =
            DependencyProperty.Register(nameof(RippleChecked), typeof(Color),
                typeof(RippleRadioButton_SelectedCircle), new PropertyMetadata((Color)ColorConverter.ConvertFromString("#46BCFF")));
        #endregion

        #region RippleUnChecked
        public Color RippleUnChecked
        {
            get { return (Color)GetValue(RippleUnCheckedProperty); }
            set { SetValue(RippleUnCheckedProperty, value); }
        }
        public static Color GetRippleUnChecked(UIElement element) => (Color)element.GetValue(RippleUnCheckedProperty);

        public static void SetRippleUnChecked(UIElement element, Color value) => element.SetValue(RippleUnCheckedProperty, value);

        public static readonly DependencyProperty RippleUnCheckedProperty =
            DependencyProperty.Register(nameof(RippleUnChecked), typeof(Color),
                typeof(RippleRadioButton_SelectedCircle), new PropertyMetadata((Color)ColorConverter.ConvertFromString("#BBBBBB")));
        #endregion

        #region Ripple effect анимация
        public Brush RippleColor
        {
            get { return (Brush)GetValue(RippleColorProperty); }
            set { SetValue(RippleColorProperty, value); }
        }
        public static Brush GetRippleColor(UIElement element) => (Brush)element.GetValue(RippleColorProperty);

        public static void SetRippleColor(UIElement element, Brush value) => element.SetValue(RippleColorProperty, value);

        public static readonly DependencyProperty RippleColorProperty =
            DependencyProperty.Register("RippleColor", typeof(Brush),
                typeof(RippleRadioButton_SelectedCircle), new PropertyMetadata(Brushes.White));

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            this.AddHandler(MouseDownEvent, new RoutedEventHandler(this.OnMouseDown), true);
        }

        public void OnMouseDown(object sender, RoutedEventArgs e)
        {
            Point mousePos = (e as MouseButtonEventArgs).GetPosition(this);

            var ellipse = this.GetTemplateChild("CircleEffect") as Ellipse;

            ellipse.Margin = new Thickness(mousePos.X, mousePos.Y, 0, 0);
            Storyboard storyboard = (this.FindResource("RippleAnimation") as Storyboard).Clone();
            double effectMaxSize = Math.Max(this.ActualWidth, this.ActualHeight) * 3;

            (storyboard.Children[2] as ThicknessAnimation).From =
                new Thickness(mousePos.X, mousePos.Y, 0, 0);
            (storyboard.Children[2] as ThicknessAnimation).To =
                new Thickness(mousePos.X - effectMaxSize / 2, mousePos.Y - effectMaxSize / 2, 0, 0);
            (storyboard.Children[3] as DoubleAnimation).To =
                effectMaxSize;

            ellipse.BeginStoryboard(storyboard);
        }
        #endregion


        #region Логика для переопределения события Checked и UnChecked
        public static readonly DependencyProperty ParentToggleButtonProperty =
         DependencyProperty.RegisterAttached(
             nameof(GetParentToggleButton)[3..],
             typeof(RadioButton),
             typeof(RippleRadioButton_SelectedCircle),
             new PropertyMetadata(null, OnParentToggleButtonChanged));
        public static bool GetParentToggleButton(UIElement element) => (bool)element.GetValue(ParentToggleButtonProperty);

        public static void SetParentToggleButton(UIElement element, bool value) => element.SetValue(ParentToggleButtonProperty, value);

        public static void OnParentToggleButtonChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is not UIElement element)
                throw new NotImplementedException("Реализовано только для UIElement.");
            if (e.NewValue is RadioButton nbutton)
            {
                RoutedEventHandler onChecked = (sender, e) =>
                {
                    RadioButton button = (RadioButton)sender;
                    if (e.OriginalSource != button)
                        return;
                    RoutedEvent @event = button.IsChecked is true ? RadioButton.CheckedEvent : RadioButton.UncheckedEvent;
                    RoutedEventArgs arg = new RoutedEventArgs(@event);
                    element.RaiseEvent(arg);
                };
                var grid = d as Grid;
                if (grid != null)
                {
                    var width = grid.ActualWidth;
                    grid.Tag = width;
                }
                nbutton.Checked += onChecked;
                nbutton.Unchecked += onChecked;
                CheckedHandlers.Add(element, onChecked);
            }
        }
        public static ConditionalWeakTable<UIElement, RoutedEventHandler> CheckedHandlers = new();
        #endregion

    }
}
