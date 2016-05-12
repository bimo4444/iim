using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace MVPLight
{
    public class ImgButton : Button
    {
        public static readonly DependencyProperty MyPicture =
            DependencyProperty.Register("Picture", typeof(string), typeof(ImgButton), new PropertyMetadata(string.Empty, ChangedPic));
        public static readonly DependencyProperty MyProperty1 =
            DependencyProperty.Register("Text1", typeof(string), typeof(ImgButton), new PropertyMetadata(string.Empty, Changed1));
        public static readonly DependencyProperty MyProperty2 =
            DependencyProperty.Register("Text2", typeof(string), typeof(ImgButton), new PropertyMetadata(string.Empty, Changed2));
        public static readonly DependencyProperty MyMargin =
            DependencyProperty.Register("StackMargin", typeof(Thickness), typeof(ImgButton), new PropertyMetadata(new Thickness(), ChangedMargin));
        public static readonly DependencyProperty MyHeight =
            DependencyProperty.Register("StackHeight", typeof(double), typeof(ImgButton), new PropertyMetadata((double)0.0, ChangedHeight));
        public static readonly DependencyProperty MyWidth =
            DependencyProperty.Register("StackWidth", typeof(double), typeof(ImgButton), new PropertyMetadata((double)0.0, ChangedWidth));


        TextBlock block1 = new TextBlock();
        TextBlock block2 = new TextBlock();
        Image image = new Image();
        StackPanel stack = new StackPanel();
        public bool RightSide 
        {
            set 
            {
                stack.Children.Clear();
                if (value)
                {
                    stack.Children.Add(block1);
                    stack.Children.Add(block2);
                    stack.Children.Add(new Border() { BorderThickness = new Thickness((double)2) });
                    stack.Children.Add(image);
                }
                else
                {
                    stack.Children.Add(image);
                    stack.Children.Add(new Border() { BorderThickness = new Thickness((double)2) });
                    stack.Children.Add(block1);
                    stack.Children.Add(block2);
                }
            }
        }
        [BindableAttribute(true)]
        public double StackWidth
        {
            get { return (double)GetValue(MyWidth); }
            set { SetValue(MyWidth, value); }
        }
        [BindableAttribute(true)]
        public double StackHeight
        {
            get { return (double)GetValue(MyHeight); }
            set { SetValue(MyHeight, value); }
        }
        [BindableAttribute(true)]
        public Thickness StackMargin
        {
            get { return (Thickness)GetValue(MyMargin); }
            set { SetValue(MyMargin, value); }
        }
        [BindableAttribute(true)]
        public string Picture
        {
            get { return (string)GetValue(MyPicture); }
            set { SetValue(MyPicture, value); }
        }
        [BindableAttribute(true)]
        public string Text2
        {
            get { return (string)GetValue(MyProperty2); }
            set { SetValue(MyProperty2, value); }
        }
        [BindableAttribute(true)]
        public string Text1
        {
            get { return (string)GetValue(MyProperty1); }
            set { SetValue(MyProperty1, value); }
        }
        public ImgButton()
        {
            image.Width = 16;
            image.Height = 16;
            Style style = new Style();
            style.TargetType = typeof(Image);
            Trigger trigger = new Trigger();
            trigger.Property = Button.IsEnabledProperty;
            trigger.Value = false;
            Setter setter = new Setter();
            setter.Property = Image.OpacityProperty;
            setter.Value = (double)0.5;
            trigger.Setters.Add(setter);
            style.Triggers.Add(trigger);
            image.Style = style;
            stack.Orientation = Orientation.Horizontal;
            stack.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
            RightSide = false;
            this.Content = stack;
        }
        private static void ChangedPic(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((ImgButton)d).image.Source = new BitmapImage(new Uri((string)e.NewValue, UriKind.Relative));
        }
        private static void Changed1(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((ImgButton)d).block1.Text = (string)e.NewValue;
        }
        private static void Changed2(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((ImgButton)d).block2.Text = (string)e.NewValue;
        }
        private static void ChangedMargin(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((ImgButton)d).stack.Margin = (Thickness)e.NewValue;
        }
        private static void ChangedWidth(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((ImgButton)d).stack.Width = (double)e.NewValue;
        }
        private static void ChangedHeight(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((ImgButton)d).stack.Height = (double)e.NewValue;
        }
    }
}
