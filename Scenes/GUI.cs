using System;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System.Drawing;
using System.Drawing.Imaging;

namespace OpenGL_Game.Scenes
{
    static class GUI
    {

        private static Bitmap textBMP; //The image being drawn
        static Bitmap TextBMP
        {
            get { return textBMP; }
            set { }
        }//Stop outside scripts from changing GUI

        private static int textTexture;//The location of the texture on the graphics card
        private static Graphics textGFX;//Used to adjust the textBMP bitmap

        private static int m_width, m_height;
        public static Vector2 guiPosition = Vector2.Zero;

        public static Color clearColour = Color.CornflowerBlue;

        //Called by SceneManager onLoad, and when screen size is changed
        public static void SetUpGUI(int width, int height)
        {
            m_width = width;
            m_height = height;

            // Create Bitmap
            textBMP = new Bitmap(width, height, System.Drawing.Imaging.PixelFormat.Format32bppArgb); // match window size
            //Setup the graphics
            textGFX = Graphics.FromImage(textBMP);
            textGFX.Clear(clearColour);

            //Load the texture into the Graphics Card
            if (textTexture > 0)
            {
                GL.DeleteTexture(textTexture);
                textTexture = 0;
            }
            textTexture = GL.GenTexture();
            GL.Enable(EnableCap.Texture2D);
            GL.BindTexture(TextureTarget.Texture2D, textTexture);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)All.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)All.Linear);
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, textBMP.Width, textBMP.Height, 0, OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, IntPtr.Zero);
            GL.BindTexture(TextureTarget.Texture2D, 0);
            GL.Disable(EnableCap.Texture2D);
        }

        public static void Image(string pFileName, float pWidth, float pHeight)
        {
            var img = System.Drawing.Image.FromFile(pFileName);
            var resizedImg = new Bitmap(img, new Size((int)pWidth, (int)pHeight));
            textGFX.DrawImage(resizedImg, new Point(0, 0));
        }

        public static void Image(string pFileName, int pPositionX, int pPositionY)
        {
            var img = System.Drawing.Image.FromFile(pFileName);
            textGFX.DrawImage(img, new Point(pPositionX, pPositionY));
        }

        public static void Label(Rectangle rect, string text)
        {
            Label(rect, text, 20, StringAlignment.Near);
        }
        public static void Label(Rectangle rect, string text, StringAlignment sa)
        {
            Label(rect, text, 20, sa);
        }
        public static void Label(Rectangle rect, string text, int fontSize)
        {
            Label(rect, text, fontSize, StringAlignment.Near);
        }

        public static void Label(Rectangle rect, string text, int fontSize, StringAlignment sa)
        {
            Label(rect, text, fontSize, sa, Color.White);
        }

        public static void Label(Rectangle rect, string text, int fontSize, StringAlignment sa, Color color)
        {
            var stringFormat = new StringFormat();
            stringFormat.Alignment = sa;
            stringFormat.LineAlignment = sa;

            var brush = new SolidBrush(color);

            textGFX.DrawString(text, new Font("Arial", fontSize), brush, rect, stringFormat);
        }

        public static void Render()
        {
            // Enable the texture
            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactor.One, BlendingFactor.OneMinusSrcAlpha);

            GL.Enable(EnableCap.Texture2D);
            GL.Hint(HintTarget.PerspectiveCorrectionHint, HintMode.Nicest);

            GL.BindTexture(TextureTarget.Texture2D, textTexture);

            var data = textBMP.LockBits(new Rectangle(0, 0, textBMP.Width, textBMP.Height), ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, (int)textBMP.Width, (int)textBMP.Height, 0, OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, data.Scan0);
            textBMP.UnlockBits(data);

            GL.Begin(PrimitiveType.Quads);
            GL.TexCoord2(0f, 1f); GL.Vertex2(guiPosition.X, guiPosition.Y);
            GL.TexCoord2(1f, 1f); GL.Vertex2(guiPosition.X + m_width, guiPosition.Y);
            GL.TexCoord2(1f, 0f); GL.Vertex2(guiPosition.X + m_width, guiPosition.Y + m_height);
            GL.TexCoord2(0f, 0f); GL.Vertex2(guiPosition.X, guiPosition.Y + m_height);
            GL.End();

            GL.BindTexture(TextureTarget.Texture2D, 0);
            GL.Disable(EnableCap.Texture2D);

            textGFX.Clear(clearColour);
        }
    }
}
