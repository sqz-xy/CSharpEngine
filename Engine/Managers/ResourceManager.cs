using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using OpenGL_Game.Engine.OBJLoader;
using OpenTK.Audio.OpenAL;
using OpenTK.Graphics.OpenGL;

namespace OpenGL_Game.Engine.Managers
{
    public static class ResourceManager
    {
        static Dictionary<string, Geometry> _geometryDictionary = new Dictionary<string, Geometry>();
        static Dictionary<string, int> _textureDictionary = new Dictionary<string, int>();
        private static Dictionary<string, int> _shaderDictionary = new Dictionary<string, int>();
        
        static Dictionary<string, int> _audioDictionary = new Dictionary<string, int>();
        
        // I added this
        //static List<int> audioSources = new List<int>();

        public static void RemoveAllAssets()
        {
            foreach (var geometry in _geometryDictionary)
            {
                geometry.Value.RemoveGeometry();
            }
            _geometryDictionary.Clear();
            
            foreach (var texture in _textureDictionary)
            {
                GL.DeleteTexture(texture.Value);
            }
            _textureDictionary.Clear();
            
            foreach (var shader in _shaderDictionary)
            {
                GL.DeleteShader(shader.Value);
            }
            _shaderDictionary.Clear();
        }

        /// <summary>
        /// Loads geometry
        /// </summary>
        /// <param name="pFilename">Geometry file name</param>
        /// <returns>A geometry object</returns>
        public static Geometry LoadGeometry(string pFilename)
        {
            Geometry geometry;
            _geometryDictionary.TryGetValue(pFilename, out geometry);
            if (geometry == null)
            {
                geometry = new Geometry();
                geometry.LoadObject(pFilename);
                _geometryDictionary.Add(pFilename, geometry);
            }

            return geometry;
        }

        /// <summary>
        /// Loads shaders
        /// </summary>
        /// <param name="pFileName">Shader file name</param>
        /// <param name="pType">Type of shader</param>
        /// <returns>A shader program id</returns>
        public static int LoadShader(string pFileName, ShaderType pType)
        {
            int shader;
            _shaderDictionary.TryGetValue(pFileName, out shader);

            if (shader == 0)
            {
                shader = GL.CreateShader(pType);
                using (var sr = new StreamReader(pFileName))
                {
                    GL.ShaderSource(shader, sr.ReadToEnd());
                }
                GL.CompileShader(shader);
                Console.WriteLine(GL.GetProgramInfoLog(shader));
                _shaderDictionary.Add(pFileName, shader);
            }

            return shader;
        }

        /// <summary>
        /// Loads textures
        /// </summary>
        /// <param name="pFilename">Texture file name</param>
        /// <returns>A texture buffer index</returns>
        /// <exception cref="ArgumentException">Empty filename</exception>
        public static int LoadTexture(string pFilename)
        {
            if (String.IsNullOrEmpty(pFilename))
                throw new ArgumentException(pFilename);

            int texture;
            _textureDictionary.TryGetValue(pFilename, out texture);
            if (texture == 0)
            {
                texture = GL.GenTexture();
                _textureDictionary.Add(pFilename, texture);
                GL.BindTexture(TextureTarget.Texture2D, texture);

                // We will not upload mipmaps, so disable mipmapping (otherwise the texture will not appear).
                // We can use GL.GenerateMipmaps() or GL.Ext.GenerateMipmaps() to create
                // mipmaps automatically. In that case, use TextureMinFilter.LinearMipmapLinear to enable them.
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);

                var bmp = new Bitmap(pFilename);
                var bmpData = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

                GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, bmpData.Width, bmpData.Height, 0,
                    OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, bmpData.Scan0);

                bmp.UnlockBits(bmpData);
            }
 
            return texture;
        }
        
        /// <summary>
        /// Loads audio
        /// </summary>
        /// <param name="pFilename">Name of the audio file</param>
        /// <returns>An audio buffer index</returns>
        public static int LoadAudio(string pFilename)
        {
            // Mine
            _audioDictionary.TryGetValue(pFilename, out var audioBuffer);

            if (audioBuffer == 0)
            {
                // reserve a Handle for the audio file
                audioBuffer = AL.GenBuffer();

                // Load a .wav file from disk.
                int channels, bitsPerSample, sampleRate;
                var soundData = LoadWave(
                    File.Open(pFilename, FileMode.Open),
                    out channels,
                    out bitsPerSample,
                    out sampleRate);
                var soundFormat =
                    channels == 1 && bitsPerSample == 8 ? ALFormat.Mono8 :
                    channels == 1 && bitsPerSample == 16 ? ALFormat.Mono16 :
                    channels == 2 && bitsPerSample == 8 ? ALFormat.Stereo8 :
                    channels == 2 && bitsPerSample == 16 ? ALFormat.Stereo16 :
                    (ALFormat)0; // unknown
                AL.BufferData(audioBuffer, soundFormat, soundData, soundData.Length, sampleRate);
                if (AL.GetError() != ALError.NoError)
                {
                    Console.WriteLine("Error");
                }
            }
            return audioBuffer;
        }

        /// <summary>
        /// Load a WAV file.
        /// </summary>
        private static byte[] LoadWave(Stream pStream, out int pChannels, out int pBits, out int pRate)
        {
            if (pStream == null)
                throw new ArgumentNullException("pStream");

            using (var reader = new BinaryReader(pStream))
            {
                // RIFF header
                var signature = new string(reader.ReadChars(4));
                if (signature != "RIFF")
                    throw new NotSupportedException("Specified stream is not a wave file.");

                var riffChunckSize = reader.ReadInt32();

                var format = new string(reader.ReadChars(4));
                if (format != "WAVE")
                    throw new NotSupportedException("Specified stream is not a wave file.");

                // WAVE header
                var formatSignature = new string(reader.ReadChars(4));
                if (formatSignature != "fmt ")
                    throw new NotSupportedException("Specified wave file is not supported.");

                var formatChunkSize = reader.ReadInt32();
                int audioFormat = reader.ReadInt16();
                int numChannels = reader.ReadInt16();
                var sampleRate = reader.ReadInt32();
                var byteRate = reader.ReadInt32();
                int blockAlign = reader.ReadInt16();
                int bitsPerSample = reader.ReadInt16();

                var dataSignature = new string(reader.ReadChars(4));
                if (dataSignature != "data")
                    throw new NotSupportedException("Specified wave file is not supported.");

                var dataChunkSize = reader.ReadInt32();

                pChannels = numChannels;
                pBits = bitsPerSample;
                pRate = sampleRate;

                return reader.ReadBytes((int)reader.BaseStream.Length);
            }
        }
    }
}
