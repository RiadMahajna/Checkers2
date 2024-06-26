using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using System.Windows.Media;


namespace Checkers2.Classes
{
    public class Setups
    {
        public const int DIMENSION = 8;
        public static string UriString = "..\\..\\Images\\";
        public static ImageBrush black_p = new ImageBrush(new BitmapImage(new Uri(UriString + "bp.png", UriKind.Relative)));
        public static ImageBrush black_k = new ImageBrush(new BitmapImage(new Uri(UriString + "bk.png", UriKind.Relative)));
        public static ImageBrush white_p = new ImageBrush(new BitmapImage(new Uri(UriString + "wp.png", UriKind.Relative)));
        public static ImageBrush white_k = new ImageBrush(new BitmapImage(new Uri(UriString + "wk.png", UriKind.Relative)));
        public static bool whiteTurn = true;
        public static bool blackTurn = false;
        public static bool whitePlayer = true;
        public static bool blackPlayer = false;
        public static int onlineMessageLength = 9;
    }
}
