using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Drawing.Imaging;
using System.Windows.Forms;
using System.IO;


namespace Client
{
    public class DisplayScreener
    {
        private static ImageCodecInfo jpgEncoder;
        private static EncoderParameters encoderParameters;
        private static System.Drawing.Imaging.Encoder encoder;
        private static Bitmap BM2;
        

        public static byte[] ScreenDisplay()
        {              
            BM2 = CaptureScreen.CaptureDesktopWithCursor();
            byte[] byteScreen = GetCompessedArray(BM2);
            return byteScreen;
        }
        public static void InitParameters()
        {
            jpgEncoder = GetEncoder(ImageFormat.Jpeg);
            encoder = System.Drawing.Imaging.Encoder.Quality;
            encoderParameters = new EncoderParameters(1);
            encoderParameters.Param[0] = new EncoderParameter(encoder, 30L);
            BM2 = new Bitmap(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);
        }
        public static void SetScreenOnPictureBox(byte[] screen, ref PictureBox pictureBox)
        {
            MemoryStream memoryStream = new MemoryStream();
            memoryStream.Write(screen, 0, screen.Length);
            pictureBox.Image = Image.FromStream(memoryStream);
        }
        private static byte[] GetCompessedArray(Bitmap bmp1) 
        { 
            MemoryStream ms = new MemoryStream(); 
            bmp1.Save(ms, jpgEncoder, encoderParameters); 
            ms.Close(); 
            byte[] data = ms.ToArray(); 
            return data; 
        }
        private static ImageCodecInfo GetEncoder(ImageFormat format)
        {
            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageDecoders();
            foreach (ImageCodecInfo codec in codecs)
                if (codec.FormatID == format.Guid)
                    return codec;
            return null;
        }
    }
}
