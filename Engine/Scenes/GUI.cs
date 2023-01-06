using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace OpenGL_Game.Engine.Scenes
{
    static class GUI
    {
        // List of textures for GUI layers
        private static List<int> _layerIDs;
        private static List<Graphics> _layers;
        private static List<Bitmap> _textures;

        private static int m_width, m_height;
        public static Vector2 guiPosition = Vector2.Zero;

        public static Color clearColour = Color.CornflowerBlue;

        //Called by SceneManager onLoad, and when screen size is changed
        public static void SetUpGUI(int width, int height, int pLayerCount)
        {
            // Clear old GUI Data 
            if (_layerIDs != null)
                GL.DeleteTextures(_layerIDs.Count, _layerIDs.ToArray());
            
            // Initialise GUI Data storage
            _layerIDs = new List<int>();
            _layers = new List<Graphics>();
            _textures = new List<Bitmap>();

            m_width = width;
            m_height = height;
            
            for (int i = 0; i < pLayerCount; i++)
            {
                _textures.Add(new Bitmap(width, height, System.Drawing.Imaging.PixelFormat.Format32bppArgb)); // match window size); 
                _layers.Add(Graphics.FromImage(_textures[i]));
                _layers[i].Clear(clearColour);
            }
            
            for (int i = 0; i < pLayerCount; i++)
            {
                int layerID = GL.GenTexture();
                GL.Enable(EnableCap.Texture2D);
                GL.BindTexture(TextureTarget.Texture2D, layerID);
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)All.Linear);
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)All.Linear);
                GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, _textures[i].Width, _textures[i].Height, 0, OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, IntPtr.Zero);
                GL.BindTexture(TextureTarget.Texture2D, layerID);
                GL.Disable(EnableCap.Texture2D);
                _layerIDs.Add(layerID);
            }
        }

        public static void Image(string pFileName, float pWidth, float pHeight, int pLayer)
        {
            var img = new Bitmap(System.Drawing.Image.FromFile(pFileName), new Size((int)pWidth, (int)pHeight));
            img.MakeTransparent();
            _layers[pLayer].DrawImage(img, new Point(0, 0));
        }

        public static void Image(string pFileName, float pWidth, float pHeight, int pPositionX, int pPositionY, int pLayer)
        {
            var img = new Bitmap(System.Drawing.Image.FromFile(pFileName), new Size((int)pWidth, (int)pHeight));
            img.MakeTransparent();
            _layers[pLayer].DrawImage(img, new Point(pPositionX, pPositionY));
        }
        
        public static void Image(string pFileName, float pWidth, float pHeight, int pPositionX, int pPositionY, int pLayer, int pAngle)
        {
            // resize for screen bounds
            var img = new Bitmap(System.Drawing.Image.FromFile(pFileName), new Size((int)pWidth, (int)pHeight));
            img.MakeTransparent();

            // Create a new bitmap which is larger than the image to be drawn
            var emptyImg = new Bitmap((int) ((int)pWidth + (pWidth / 2)), (int)((int)pHeight + (pWidth / 2)));

            // Centre the image so it rotates around the origin of itself correctly
            var graphics = Graphics.FromImage(emptyImg);
            graphics.DrawImageUnscaled(img, (img.Width / 4), (img.Height / 4), emptyImg.Width, emptyImg.Height);
            
            // Create a new bitmap for the rotated image 
            Bitmap rotatedImage = new Bitmap(emptyImg.Width, emptyImg.Height);

            // Make a graphics object from the empty bitmap
            graphics = Graphics.FromImage(rotatedImage);
            
            // Rotate it using the dimensions from the larger bitmap (Prevents the image being cutoff from the bounds of the original image)
            graphics.TranslateTransform((float)emptyImg.Width / 2, (float)emptyImg.Height / 2);
            graphics.RotateTransform(pAngle);
            graphics.TranslateTransform(-(float)emptyImg.Width / 2, -(float)emptyImg.Height / 2);
            graphics.DrawImage(emptyImg, new Point(0, 0));

            // Draw the rotated image
            _layers[pLayer].DrawImage(rotatedImage, new Point(pPositionX, pPositionY));
        }
        
        public static void Label(Rectangle rect, string text, int pLayer)
        {
            Label(rect, text, 20, StringAlignment.Near, pLayer);
        }
        public static void Label(Rectangle rect, string text, StringAlignment sa, int pLayer)
        {
            Label(rect, text, 20, sa, pLayer);
        }
        public static void Label(Rectangle rect, string text, int fontSize, int pLayer)
        {
            Label(rect, text, fontSize, StringAlignment.Near, pLayer);
        }

        public static void Label(Rectangle rect, string text, int fontSize, StringAlignment sa, int pLayer)
        {
            Label(rect, text, fontSize, sa, Color.White, pLayer);
        }

        public static void Label(Rectangle rect, string text, int fontSize, StringAlignment sa, Color color, int pLayer)
        {
            var stringFormat = new StringFormat();
            stringFormat.Alignment = sa;
            stringFormat.LineAlignment = sa;

            var brush = new SolidBrush(color);

            _layers[pLayer].DrawString(text, new Font("Arial", fontSize), brush, rect, stringFormat);
        }

        public static void RenderLayer(int pLayer)
        {
            // Enable the texture
            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactor.One, BlendingFactor.OneMinusSrcAlpha);

            GL.Enable(EnableCap.Texture2D);
            GL.Hint(HintTarget.PerspectiveCorrectionHint, HintMode.Nicest);
            GL.BindTexture(TextureTarget.Texture2D, _layerIDs[pLayer]);

            var data = _textures[pLayer].LockBits(new Rectangle(0, 0, _textures[pLayer].Width, _textures[pLayer].Height), ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, (int)_textures[pLayer].Width, (int)_textures[pLayer].Height, 0, OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, data.Scan0);
            _textures[pLayer].UnlockBits(data);

            GL.Begin(PrimitiveType.Quads);
            GL.TexCoord2(0f, 1f); GL.Vertex2(guiPosition.X, guiPosition.Y);
            GL.TexCoord2(1f, 1f); GL.Vertex2(guiPosition.X + m_width, guiPosition.Y);
            GL.TexCoord2(1f, 0f); GL.Vertex2(guiPosition.X + m_width, guiPosition.Y + m_height);
            GL.TexCoord2(0f, 0f); GL.Vertex2(guiPosition.X, guiPosition.Y + m_height);
            GL.End();

            GL.BindTexture(TextureTarget.Texture2D, _layerIDs[pLayer]);
            GL.Disable(EnableCap.Texture2D);

            _layers[pLayer].Clear(clearColour);
            
        }
        
    }
}
