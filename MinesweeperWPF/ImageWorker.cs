using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace MinesweeperWPF
{
    class ImageWorker
    {
        public static ImageSource GenerateImage(string path)
        {
            BitmapImage bitmapImage = new BitmapImage();
            bitmapImage.BeginInit();
            bitmapImage.UriSource = new Uri(@path, UriKind.Relative);
            bitmapImage.EndInit();

            return bitmapImage;
        }
    }
}
