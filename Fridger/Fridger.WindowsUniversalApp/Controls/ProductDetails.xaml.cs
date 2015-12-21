using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Windows.Input;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace Fridger.WindowsUniversalApp.Controls
{
    public sealed partial class ProductDetails : UserControl
    {
        public ProductDetails()
        {
            this.InitializeComponent();
        }
        public ICommand Swipe
        {
            get { return (ICommand)GetValue(SwipeProperty); }
            set { SetValue(SwipeProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Swipe.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SwipeProperty =
            DependencyProperty.Register("Swipe", typeof(ICommand), typeof(ProductDetails), new PropertyMetadata(null, new PropertyChangedCallback(HandleSwipeChanged)));

        private static void HandleSwipeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as ProductDetails;
            var command = e.NewValue as ICommand;
            control.ManipulationDelta += (sender, args) =>
            {
                command.Execute(args);
            };
        }



        public string ProductId
        {
            get { return (string)GetValue(ProductIdProperty); }
            set { SetValue(ProductIdProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ProductId.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ProductIdProperty =
            DependencyProperty.Register("ProductId", typeof(string), typeof(ProductDetails), new PropertyMetadata(null, new PropertyChangedCallback(HandleProductIdChanged)));

        private static void HandleProductIdChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as ProductDetails;
            var newValue = e.NewValue.ToString();
            control.tbProductId.Text = newValue;
        }

        public string ProductName
        {
            get { return (string)GetValue(ProductNameProperty); }
            set { SetValue(ProductNameProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ProductName.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ProductNameProperty =
            DependencyProperty.Register("ProductName", typeof(string), typeof(ProductDetails), new PropertyMetadata(null, new PropertyChangedCallback(HandleProductNameChanged)));


        public string Source
        {
            get { return (string)GetValue(SourceProperty); }
            set { SetValue(SourceProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Source.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SourceProperty =
            DependencyProperty.Register("Source", typeof(string), typeof(ProductDetails), new PropertyMetadata(null, new PropertyChangedCallback(HandleSourceChanged)));
        
        private static void HandleProductNameChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as ProductDetails;
            var newValue = e.NewValue.ToString();
            control.tbProductName.Text = newValue;
        }
               
        private static void HandleSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as ProductDetails;            
            var newValue = e.NewValue.ToString();
            var imageSource = new BitmapImage(new Uri(newValue));
            control.imgProductImage.Source = imageSource;
        }       
    }
}
