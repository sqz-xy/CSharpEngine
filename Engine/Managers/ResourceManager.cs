using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using OpenTK.Graphics.OpenGL;
using OpenGL_Game.OBJLoader;
using OpenTK.Audio.OpenAL;

namespace OpenGL_Game.Managers
{
    public static class ResourceManager
    {
        static Dictionary<string, Geometry> geometryDictionary = new Dictionary<string, Geometry>();
        static Dictionary<string, int> textureDictionary = new Dictionary<string, int>();
        private static Dictionary<string, int> shaderDictionary = new Dictionary<string, int>();
        
        static Dictionary<string, int> audioDictionary = new Dictionary<string, int>();
        
        // I added this
        //static List<int> audioSources = new List<int>();

        public static void RemoveAllAssets()
        {
            foreach (var geometry in geometryDictionary)
            {
                geometry.Value.RemoveGeometry();
            }
            geometryDictionary.Clear();
            
            foreach (var texture in textureDictionary)
            {
                GL.DeleteTexture(texture.Value);
            }
            textureDictionary.Clear();
            
            foreach (var shader in shaderDictionary)
            {
                GL.DeleteShader(shader.Value);
            }
            shaderDictionary.Clear();
        }

        public static Geometry LoadGeometry(string filename)
        {
            Geometry geometry;
            geometryDictionary.TryGetValue(filename, out geometry);
            if (geometry == null)
            {
                geometry = new Geometry();
                geometry.LoadObject(filename);
                geometryDictionary.Add(filename, geometry);
            }

            return geometry;
        }

        public static int LoadShader(string pFileName, ShaderType pType)
        {
            int shader;
            shaderDictionary.TryGetValue(pFileName, out shader);

            if (shader == 0)
            {
                shader = GL.CreateShader(pType);
                using (var sr = new StreamReader(pFileName))
                {
                    GL.ShaderSource(shader, sr.ReadToEnd());
                }
                GL.CompileShader(shader);
                Console.WriteLine(GL.GetProgramInfoLog(shader));
                shaderDictionary.Add(pFileName, shader);
            }

            return shader;
        }

        public static int LoadTexture(string filename)
        {
            if (String.IsNullOrEmpty(filename))
                throw new ArgumentException(filename);

            int texture;
            textureDictionary.TryGetValue(filename, out texture);
            if (texture == 0)
            {
                texture = GL.GenTexture();
                textureDictionary.Add(filename, texture);
                GL.BindTexture(TextureTarget.Texture2D, texture);

                // We will not upload mipmaps, so disable mipmapping (otherwise the texture will not appear).
                // We can use GL.GenerateMipmaps() or GL.Ext.GenerateMipmaps() to create
                // mipmaps automatically. In that case, use TextureMinFilter.LinearMipmapLinear to enable them.
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);

                var bmp = new Bitmap(filename);
                var bmp_data = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

                GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, bmp_data.Width, bmp_data.Height, 0,
                    OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, bmp_data.Scan0);

                bmp.UnlockBits(bmp_data);
            }
 
            return texture;
        }
        
        public static int LoadAudio(string filename)
        {
            // Mine
            audioDictionary.TryGetValue(filename, out var audioBuffer);

            if (audioBuffer == 0)
            {
                // reserve a Handle for the audio file
                audioBuffer = AL.GenBuffer();

                // Load a .wav file from disk.
                int channels, bits_per_sample, sample_rate;
                var sound_data = LoadWave(
                    File.Open(filename, FileMode.Open),
                    out channels,
                    out bits_per_sample,
                    out sample_rate);
                var sound_format =
                    channels == 1 && bits_per_sample == 8 ? ALFormat.Mono8 :
                    channels == 1 && bits_per_sample == 16 ? ALFormat.Mono16 :
                    channels == 2 && bits_per_sample == 8 ? ALFormat.Stereo8 :
                    channels == 2 && bits_per_sample == 16 ? ALFormat.Stereo16 :
                    (ALFormat)0; // unknown
                AL.BufferData(audioBuffer, sound_format, sound_data, sound_data.Length, sample_rate);
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
        private static byte[] LoadWave(Stream stream, out int channels, out int bits, out int rate)
        {
            if (stream == null)
                throw new ArgumentNullException("stream");

            using (var reader = new BinaryReader(stream))
            {
                // RIFF header
                var signature = new string(reader.ReadChars(4));
                if (signature != "RIFF")
                    throw new NotSupportedException("Specified stream is not a wave file.");

                var riff_chunck_size = reader.ReadInt32();

                var format = new string(reader.ReadChars(4));
                if (format != "WAVE")
                    throw new NotSupportedException("Specified stream is not a wave file.");

                // WAVE header
                var format_signature = new string(reader.ReadChars(4));
                if (format_signature != "fmt ")
                    throw new NotSupportedException("Specified wave file is not supported.");

                var format_chunk_size = reader.ReadInt32();
                int audio_format = reader.ReadInt16();
                int num_channels = reader.ReadInt16();
                var sample_rate = reader.ReadInt32();
                var byte_rate = reader.ReadInt32();
                int block_align = reader.ReadInt16();
                int bits_per_sample = reader.ReadInt16();

                var data_signature = new string(reader.ReadChars(4));
                if (data_signature != "data")
                    throw new NotSupportedException("Specified wave file is not supported.");

                var data_chunk_size = reader.ReadInt32();

                channels = num_channels;
                bits = bits_per_sample;
                rate = sample_rate;

                return reader.ReadBytes((int)reader.BaseStream.Length);
            }
        }
    }
}
