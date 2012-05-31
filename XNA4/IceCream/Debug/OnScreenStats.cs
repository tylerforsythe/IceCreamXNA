using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IceCream.Drawing;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using IceCream.SceneItems;

namespace IceCream.Debug
{
    public static class OnScreenStats
    {
        static List<string> Stats = new List<string>();
        static Vector2 BaseLocation = new Vector2(0, 0);
        static SpriteFont mFont;

        public static void Init() {
            mFont = SceneManager.GetEmbeddedFont("DefaultFont").Font;
        }

        public static void AddStat(string stat) {
            Stats.Add(stat);
        }

        public static void Draw() {
            Vector2 drawPosition = BaseLocation;

            DrawingManager.SpriteBatch.Begin();
            foreach (string stat in Stats) {
                //DrawingManager.SpriteBatch.DrawString(mFont, stat, drawPosition, Color.Black);
                DrawingManager.SpriteBatch.DrawString(mFont, stat, drawPosition, Color.Black, 0.0f, Vector2.Zero, 1.0f, SpriteEffects.None, 0);
                drawPosition.Y += mFont.LineSpacing;
            }
            DrawingManager.SpriteBatch.End();

            Stats.Clear();
        }
    }
}
