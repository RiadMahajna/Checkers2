using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
//using System.Net.PeerToPeer.Collaboration;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Checkers2.Models
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Start : Window
    {
        private const int pvp = 1;
        private const int pc = 2;
        private const int web = 3;
        private int choise = 0;

        public Start()
        {
            InitializeComponent();


        }

        public Start(double l, double t, double w, double h, WindowState windowState)
        {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(
      delegate (object sender, RoutedEventArgs args)
      {
          Left = l;
          Top = t;
          Width = w;
          Height = h;
          WindowState = windowState;
      });
        }
        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                DragMove();
        }
        private void btnMinimize_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void pvpb_Click(object sender, RoutedEventArgs e)
        {
            double l = this.Left;
            double t = this.Top;
            double w = this.Width;
            double h = this.Height;
            var newForm = new Pvp(l, t, w, h, this.WindowState); //create your new form.

            newForm.Show(); //show the new form.

            this.Close(); //


        }

        private void pvc_Click(object sender, RoutedEventArgs e)
        {
            pvcHard.Visibility = Visibility.Visible;
            pvcEasy.Visibility = Visibility.Visible;
            pvcMed.Visibility = Visibility.Visible;
            pvc.BorderThickness = new Thickness(3);
            pvc.BorderBrush = new SolidColorBrush(Colors.Red);

        }

        private void pvw_Click(object sender, RoutedEventArgs e)
        {
            double l = this.Left;
            double t = this.Top;
            double w = this.Width;
            double h = this.Height;
            var newForm2 = new Pvp( l, t, w, h, this.WindowState, true,true); //create your new form.
            newForm2.Show();

            //show the new form.
            this.Close(); //
        }


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            double l = this.Left;
            double t = this.Top;
            double w = this.Width;
            double h = this.Height;
            var newForm2 = new Pvp( l, t, w, h, this.WindowState,true,false, "localhost"); //create your new form.
            newForm2.Show();

            //show the new form.
            this.Close(); //

        }

        private void pvcEasy_Click(object sender, RoutedEventArgs e)
        {
            double l = this.Left;
            double t = this.Top;
            double w = this.Width;
            double h = this.Height;
            var newForm1 = new Pvp(l, t, w, h, this.WindowState, false, false, null , false , true); //create your new form.
            newForm1.Show(); //show the new form.
            this.Close(); //
        }

        private void pvcMed_Click(object sender, RoutedEventArgs e)
        {
            double l = this.Left;
            double t = this.Top;
            double w = this.Width;
            double h = this.Height;
            var newForm1 = new Pvp(l, t, w, h, this.WindowState, false, false, null, false, true); //create your new form.
            newForm1.Show(); //show the new form.
            this.Close(); //
        }

        private void pvcHard_Click(object sender, RoutedEventArgs e)
        {
            double l = this.Left;
            double t = this.Top;
            double w = this.Width;
            double h = this.Height;
            var newForm1 = new Pvp(l, t, w, h, this.WindowState, false, false, null, false, true); //create your new form.
            newForm1.Show(); //show the new form.
            this.Close(); //
        }
    }
}