using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;
using System.IO;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using GameClasses.Enums;

namespace DungeonResearcher.RenderingProcess
{
    public class Renderer
    {
        public Dictionary<Directions,string> HeroTexturePath { get; }

        public Dictionary<EnemyType, string> EnemiesTextures { get; }

        public Dictionary<Type, string> GameObjectsSpritesPath { get; }
        public Renderer(Dictionary<Directions, string> heroTexturePath, Dictionary<EnemyType,string> enemiesTextures,
            Dictionary<Type,string> gameObjectsSpritesPath)
        {
            HeroTexturePath = heroTexturePath;
            EnemiesTextures = enemiesTextures;
            GameObjectsSpritesPath = gameObjectsSpritesPath;
        }
        public int GetPlayerTexture(Directions direction)
        {
            Bitmap image = new Bitmap(HeroTexturePath[direction]);
            return LoadTexture(image);
        }

        public int GetMapObjectTexture(Type stuffType)
        {
            Bitmap image = new Bitmap(GameObjectsSpritesPath[stuffType]);
            return LoadTexture(image);
        }

        public int GetEnemyObject(EnemyType enemy)
        {
            Bitmap image = new Bitmap(EnemiesTextures[enemy]);
            return LoadTexture(image);
        }

        private int LoadTexture(Bitmap bitmap) {
            int tex;
            GL.Hint(HintTarget.PerspectiveCorrectionHint, HintMode.Nicest);

            GL.GenTextures(1, out tex);
            GL.BindTexture(TextureTarget.Texture2D, tex);

            BitmapData data = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height),
                ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, data.Width, data.Height, 0,
                OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, data.Scan0);
            bitmap.UnlockBits(data);


            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);

            return tex;
        }
    }
}
