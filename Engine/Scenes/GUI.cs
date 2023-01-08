using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace OpenGL_Game.Engine.Scenes
{
    static class Gui
    {
        // List of textures for GUI layers
        private static List<int> _layerIDs;
        private static List<Graphics> _layers;
        private static List<Bitmap> _textures;

        private static int _mWidth, _mHeight;
        public static Vector2 GuiPosition = Vector2.Zero;

        public static Color ClearColour = Color.CornflowerBlue;

        //Called by SceneManager onLoad, and when screen size is changed
        public static void SetUpGui(int pWidth, int pHeight, int pLayerCount)
        {
            // Clear old GUI Data 
            if (_layerIDs != null)
                GL.DeleteTextures(_layerIDs.Count, _layerIDs.ToArray());
            
            // Initialise GUI Data storage
            _layerIDs = new List<int>();
            _layers = new List<Graphics>();
            _textures = new List<Bitmap>();

            _mWidth = pWidth;
            _mHeight = pHeight;
            
            for (int i = 0; i < pLayerCount; i++)
            {
                _textures.Add(new Bitmap(pWidth, pHeight, System.Drawing.Imaging.PixelFormat.Format32bppArgb)); // match window size); 
                _layers.Add(Graphics.FromImage(_textures[i]));
                _layers[i].Clear(ClearColour);
            }
            
            for (int i = 0; i < pLayerCount; i++)
            {
                int layerId = GL.GenTexture();
                GL.Enable(EnableCap.Texture2D);
                GL.BindTexture(TextureTarget.Texture2D, layerId);
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)All.Linear);
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)All.Linear);
                GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, _textures[i].Width, _textures[i].Height, 0, OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, IntPtr.Zero);
                GL.BindTexture(TextureTarget.Texture2D, layerId);
                GL.Disable(EnableCap.Texture2D);
                _layerIDs.Add(layerId);
            }
        }

        /// <summary>
        /// Puts an image on screen at 0, 0
        /// </summary>
        /// <param name="pFileName">name of the image file</param>
        /// <param name="pWidth">screen width</param>
        /// <param name="pHeight">screen height</param>
        /// <param name="pLayer">layer to place the image on</param>
        public static void Image(string pFileName, float pWidth, float pHeight, int pLayer)
        {
            var img = new Bitmap(System.Drawing.Image.FromFile(pFileName), new Size((int)pWidth, (int)pHeight));
            img.MakeTransparent();
            _layers[pLayer].DrawImage(img, new Point(0, 0));
        }

        /// <summary>
        /// Puts an image on screen at desired coordinates
        /// </summary>
        /// <param name="pFileName">name of the image file</param>
        /// <param name="pWidth">screen width</param>
        /// <param name="pHeight">screen height</param>
        /// <param name="pPositionY">Y position to place the image</param>
        /// <param name="pPositionX">X position to place the image</param>
        /// <param name="pLayer">layer to place the image on</param>
        public static void Image(string pFileName, float pWidth, float pHeight, int pPositionX, int pPositionY, int pLayer)
        {
            var img = new Bitmap(System.Drawing.Image.FromFile(pFileName), new Size((int)pWidth, (int)pHeight));
            img.MakeTransparent();
            _layers[pLayer].DrawImage(img, new Point(pPositionX, pPositionY));
        }

        /// <summary>
        /// Puts an image on screen at desired coordinates
        /// </summary>
        /// <param name="pFileName">name of the image file</param>
        /// <param name="pWidth">screen width</param>
        /// <param name="pHeight">screen height</param>
        /// <param name="pPositionY">Y position to place the image</param>
        /// <param name="pPositionX">X position to place the image</param>
        /// <param name="pLayer">layer to place the image on</param>
        /// <param name="pAngle">Angle of the image</param>
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
        
        public static void Label(Rectangle pRect, string pText, int pLayer)
        {
            Label(pRect, pText, 20, StringAlignment.Near, pLayer);
        }
        public static void Label(Rectangle pRect, string pText, StringAlignment pSa, int pLayer)
        {
            Label(pRect, pText, 20, pSa, pLayer);
        }
        public static void Label(Rectangle pRect, string pText, int pFontSize, int pLayer)
        {
            Label(pRect, pText, pFontSize, StringAlignment.Near, pLayer);
        }

        /// <summary>
        /// Places text on the screen at desired coordinates
        /// </summary>
        /// <param name="pRect">Bounding rectangle for text</param>
        /// <param name="pText">Text to add</param>
        /// <param name="pFontSize">Size of text</param>
        /// <param name="pSa">String alignment</param>
        /// <param name="pLayer">Layer to put the text on</param>
        public static void Label(Rectangle pRect, string pText, int pFontSize, StringAlignment pSa, int pLayer)
        {
            Label(pRect, pText, pFontSize, pSa, Color.White, pLayer);
        }

        public static void Label(Rectangle pRect, string pText, int pFontSize, StringAlignment pSa, Color pColor, int pLayer)
        {
            var stringFormat = new StringFormat();
            stringFormat.Alignment = pSa;
            stringFormat.LineAlignment = pSa;

            var brush = new SolidBrush(pColor);

            _layers[pLayer].DrawString(pText, new Font("Arial", pFontSize), brush, pRect, stringFormat);
        }

        /// <summary>
        /// Renders a gui layer
        /// </summary>
        /// <param name="pLayer">layer index to render</param>
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
            GL.TexCoord2(0f, 1f); GL.Vertex2(GuiPosition.X, GuiPosition.Y);
            GL.TexCoord2(1f, 1f); GL.Vertex2(GuiPosition.X + _mWidth, GuiPosition.Y);
            GL.TexCoord2(1f, 0f); GL.Vertex2(GuiPosition.X + _mWidth, GuiPosition.Y + _mHeight);
            GL.TexCoord2(0f, 0f); GL.Vertex2(GuiPosition.X, GuiPosition.Y + _mHeight);
            GL.End();

            GL.BindTexture(TextureTarget.Texture2D, _layerIDs[pLayer]);
            GL.Disable(EnableCap.Texture2D);

            _layers[pLayer].Clear(ClearColour);
            
        }
        
    }
}
