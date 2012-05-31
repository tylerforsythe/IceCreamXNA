#if XNATOUCH
using XnaTouch.Framework;
using XnaTouch.Framework.Audio;
using XnaTouch.Framework.Content;
using XnaTouch.Framework.GamerServices;
using XnaTouch.Framework.Graphics;
using XnaTouch.Framework.Input;
using XnaTouch.Framework.Media;
using XnaTouch.Framework.Net;
using XnaTouch.Framework.Storage;
#else
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;

using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;


#endif

using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Text;
using IceCream.Drawing;

namespace IceCream.Drawing
{
    public enum DrawingBlendingType
    {
        BackgroundAdditive,
        BackgroundSubtractive,
        Alpha,
        Additive,
        Subtractive,
        Refraction,
        #if WINDOWS 
        [BrowsableAttribute(false)] 
        #endif
        EnumSize,
    }

    public enum DrawingLayerType
    {
        Alpha,
        Additive,
        Subtractive,
    }

    class DrawingLayerComponent
    {
        private DrawingLayerType _type;
        private List<DrawRequest> _drawRequestCollection;
        private bool _useRefraction;
        public bool UseRefraction
        {
            get { return _useRefraction; }
            set { _useRefraction = value; }
        }

        public void Draw(DrawRequest drawRequest)
        {
            _drawRequestCollection.Add(drawRequest);
        }

        public void EndDrawing()
        {
            if (_drawRequestCollection.Count > 0)
            {
                _drawRequestCollection.Clear();
            }
        }

        /// <summary>
        /// Main rendering method. Renders all the requests of the current blending type
        /// </summary>
        /// <param name="device">The GraphicsDevice to render on</param>
        /// <param name="transformMatrix">The camera transform matrix</param>
        /// <returns>True if any drawing was performed on that layer, false otherwise</returns>
        public bool RenderLayerComponent(GraphicsDevice graphicsDevice, Matrix transformMatrix)
        {
            // only draw if there is pending requests
            bool drawNeeded = (_drawRequestCollection.Count > 0);
            if (drawNeeded)
            {
                // if the layer is a refraction one, we need to stop the spritebatch no matter what
                if (_useRefraction == true)
                {
					#if !REACH
                    if (DrawingManager.DrawingLayerTypeInUse != null)
                    {
                        // stop the current batch
                        DrawingManager.SpriteBatch.End();
                    }
                    /*
                    // resolve the current buffer to a texture
                    DrawingManager.RenderTargetManager.ResolveToTexture(RenderTargetInstance.BackBuffer);
                    // switch the temporary render target to draw the normal maps
                    DrawingManager.RenderTargetManager.SwitchTo(RenderTargetInstance.A);
                    graphicsDevice.Clear(new Color(0.5f, 0.5f, 0.0f, 0.0f));
                    // restart the spritebatch using Additive mode
                    DrawingManager.SpriteBatch.Begin(SpriteBlendMode.AlphaBlend,
                        SpriteSortMode.Immediate, SaveStateMode.None, transformMatrix); 
                     */ 
                    #endif
                }
                // smart check to see if we need to end the current spritebatch
                else
                {
                    // Check if the spritebatch needs to be ended
                    if (DrawingManager.DrawingLayerTypeInUse != null && DrawingManager.LastTransformMatrix.HasValue
                        && (_type != DrawingManager.DrawingLayerTypeInUse.Value || DrawingManager.LastTransformMatrix.Value != transformMatrix))
                    {
                        DrawingManager.DrawingLayerTypeInUse = null;
                        DrawingManager.SpriteBatch.End();
                    }
                    // If we need to begin the effect, use .Begin() first
                    if (DrawingManager.DrawingLayerTypeInUse == null)
                    {
                        DrawingManager.LastTransformMatrix = transformMatrix;
                        // Alpha types blending uses the AlphaBlend mode
                        if (_type == DrawingLayerType.Alpha)
                        {
							#if REACH
							DrawingManager.SpriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, null, 
								null, null, null, transformMatrix);
							#else
                            DrawingManager.SpriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearWrap, 
								null, null, null, transformMatrix);
							#endif
                        }
                        // Additives and Subtractives types use Additive mode as the base
                        else
                        {
							#if REACH
							DrawingManager.SpriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive, null, 
								null, null, null, transformMatrix);
							#else
                            DrawingManager.SpriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive, SamplerState.LinearWrap, 
								null, null, null, transformMatrix);  
							#endif
							if (_type == DrawingLayerType.Subtractive)
                            {
								#if !REACH
                                graphicsDevice.BlendState.ColorBlendFunction = BlendFunction.ReverseSubtract;
                                #endif
							}
                        }
                    }
                }
                // Use the current type as the one in use
                DrawingManager.DrawingLayerTypeInUse = _type;                
                // Render all the requests
                for (int i = 0; i < _drawRequestCollection.Count; i++)
                {
                    DrawRequest drawRequest = _drawRequestCollection[i];
                    if (drawRequest.isPivotRelative == true)
                    {
                        if (drawRequest.isFont == false)
                        {
                            // if the pivot is relative (instead of absolute pixel values)
                            // multiply it by the texture or rect's size
                            if (drawRequest.sourceRectangle.HasValue == true)
                            {
                                drawRequest.pivot =
                                    new Vector2(drawRequest.pivot.X * drawRequest.sourceRectangle.Value.Width,
                                        drawRequest.pivot.Y * drawRequest.sourceRectangle.Value.Height);
                            }
                            else
                            {
                                drawRequest.pivot =
                                    new Vector2(drawRequest.pivot.X * drawRequest.texture.Width,
                                        drawRequest.pivot.Y * drawRequest.texture.Height);
                            }
                        }
                        else
                        {
                            drawRequest.pivot =
                                    new Vector2(drawRequest.pivot.X * drawRequest.textSize.X,
                                        drawRequest.pivot.Y * drawRequest.textSize.Y);
                        }
                    }
                    Vector2 drawPosition = drawRequest.position;
                    Vector2 drawScale = drawRequest.scaleRatio;                    
                    if (drawRequest.texture != null)
                    {
                        DrawingManager.SpriteBatch.Draw(
                            drawRequest.texture, 
                            drawPosition, 
                            drawRequest.sourceRectangle,
                            drawRequest.tint, 
                            drawRequest.rotation,
                            drawRequest.pivot, 
                            drawScale,
                            (drawRequest.hFlip ? SpriteEffects.FlipHorizontally : SpriteEffects.None)
                            | (drawRequest.vFlip ? SpriteEffects.FlipVertically : SpriteEffects.None), 
                            0);
                    }
                    else if (drawRequest.font != null)
                    {
                        DrawingManager.SpriteBatch.DrawString(drawRequest.font, drawRequest.text, drawPosition,
                            drawRequest.tint, drawRequest.rotation, drawRequest.pivot, drawScale,
                            (drawRequest.hFlip ? SpriteEffects.FlipHorizontally : SpriteEffects.None)
                            | (drawRequest.vFlip ? SpriteEffects.FlipVertically : SpriteEffects.None), 0);
                    }
                }                            
                if (_useRefraction == true)
                {
					#if !REACH
                    DrawingManager.SpriteBatch.End();
                    DrawingManager.RenderTargetManager.SwitchTo(RenderTargetInstance.BackBuffer);
                    graphicsDevice.Clear(new Color(1f, 1, 1, 0));
                    /*
                    DrawingManager.RenderTargetManager.ResolveToTexture(RenderTargetInstance.A);
                    graphicsDevice.Textures[1] = DrawingManager.RenderTargetManager.GetTexture(RenderTargetInstance.A);                    
                    //DrawingManager.RenderTargetManager.GetTexture(RenderTargetInstance.A).Save("E:/buffer.png", ImageFileFormat.Png);                    
                    DrawingManager.SpriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Opaque);
                
                    DrawingManager.RefractionLayerEffect.CurrentTechnique.Passes[0].Apply();
                    // only render the viewport part
                    DrawingManager.SpriteBatch.Draw(DrawingManager.RenderTargetManager.
                        GetTexture(RenderTargetInstance.BackBuffer), new Rectangle(0, 0, DrawingManager.ViewPortSize.X, DrawingManager.ViewPortSize.Y), 
                        new Rectangle(0, 0, DrawingManager.ViewPortSize.X, DrawingManager.ViewPortSize.Y),
                         Color.White);                    
                    DrawingManager.SpriteBatch.End();
                    // force the next layer to restart a spritebatch
                    DrawingManager.DrawingLayerTypeInUse = null;
                     * */
                    #endif
                }                

            }
            return drawNeeded;
        }

        #region Constructor

        public DrawingLayerComponent(DrawingLayerType drawingLayerType)
        {
            _type = drawingLayerType;
            _drawRequestCollection = new List<DrawRequest>();
            _useRefraction = false;
        }

        #endregion

    }
}
