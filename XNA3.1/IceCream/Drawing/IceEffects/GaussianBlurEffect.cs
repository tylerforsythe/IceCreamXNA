#if !XNATOUCH
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;

using System;
using System.Collections.Generic;
using System.Text;
using IceCream.Drawing;

namespace IceCream.Drawing.IceEffects
{
    public class GaussianBlurEffect : IceEffect
    {
        #region Fields
        
        private EffectParameter parameter_weights;
        private EffectParameter parameter_offsets;        
        private float _blurAmount = 4f;

        #endregion

        #region Properties

        public float BlurAmount
        {
            get { return _blurAmount; }
            set { _blurAmount = value; }
        }

        #endregion

        #region Constructor

        public GaussianBlurEffect()
            : base(AssetScope.Embedded, "GaussianBlurEffect")
        {
            this.ParametersProperties = new LinearProperty[1];
            this.ParametersProperties[0] = new LinearProperty(1f, "Blur Radius", 0, 10);
        }

        #endregion

        #region Methods

        public override void Draw(SpriteBatch spriteBatch)
        {            
            // resolve the current buffer to a texture
            DrawingManager.RenderTargetManager.ResolveToTexture(RenderTargetInstance.BackBuffer);
            // Pass 1: draw the buffer into rendertarget A,
            // using a shader to apply a horizontal gaussian blur filter.
            SetBlurEffectParameters(1.0f / (float)DrawingManager.ViewPortSize.X, 0);
            DrawingManager.RenderTargetManager.SwitchTo(RenderTargetInstance.A);
            DrawFullScreenQuad(spriteBatch,
                DrawingManager.RenderTargetManager.GetTexture(RenderTargetInstance.BackBuffer), Effects[0]);           
            // Pass 2: draw from rendertarget A back into the backbuffer,
            // using a shader to apply a vertical gaussian blur filter.
            SetBlurEffectParameters(0, 1.0f / (float)DrawingManager.ViewPortSize.Y);            
            DrawingManager.RenderTargetManager.SwitchTo(RenderTargetInstance.BackBuffer);
            DrawingManager.RenderTargetManager.ResolveToTexture(RenderTargetInstance.A);
            DrawFullScreenQuad(spriteBatch, DrawingManager.RenderTargetManager.GetTexture(RenderTargetInstance.A),
                Effects[0]);
        }

        public override void SetParameters(IceEffectParameters parameters)
        {
            if (parameters.Parameter1 <= 0f)
            {
                // prevent 0 or less radius
                parameters.Parameter1 = 0.0001f;
            }
            _blurAmount = parameters.Parameter1;
        }


        public override void LoadParameters()
        {            
            parameter_weights = Effects[0].Parameters["SampleWeights"];
            parameter_offsets = Effects[0].Parameters["SampleOffsets"];
        }

        /// <summary>
        /// Computes sample weightings and texture coordinate offsets
        /// for one pass of a separable gaussian blur filter.
        /// </summary>
        private void SetBlurEffectParameters(float dx, float dy)
        {
            // Look up how many samples our gaussian blur effect supports.
            int sampleCount = parameter_weights.Elements.Count;

            // Create temporary arrays for computing our filter settings.
            float[] sampleWeights = new float[sampleCount];
            Vector2[] sampleOffsets = new Vector2[sampleCount];

            // The first sample always has a zero offset.
            sampleWeights[0] = ComputeGaussian(0);
            sampleOffsets[0] = new Vector2(0);

            // Maintain a sum of all the weighting values.
            float totalWeights = sampleWeights[0];

            // Add pairs of additional sample taps, positioned
            // along a line in both directions from the center.
            for (int i = 0; i < sampleCount / 2; i++)
            {
                // Store weights for the positive and negative taps.
                float weight = ComputeGaussian(i + 1);

                sampleWeights[i * 2 + 1] = weight;
                sampleWeights[i * 2 + 2] = weight;

                totalWeights += weight * 2;

                // To get the maximum amount of blurring from a limited number of
                // pixel shader samples, we take advantage of the bilinear filtering
                // hardware inside the texture fetch unit. If we position our texture
                // coordinates exactly halfway between two texels, the filtering unit
                // will average them for us, giving two samples for the price of one.
                // This allows us to step in units of two texels per sample, rather
                // than just one at a time. The 1.5 offset kicks things off by
                // positioning us nicely in between two texels.
                float sampleOffset = i * 2 + 1.5f;

                Vector2 delta = new Vector2(dx, dy) * sampleOffset;

                // Store texture coordinate offsets for the positive and negative taps.
                sampleOffsets[i * 2 + 1] = delta;
                sampleOffsets[i * 2 + 2] = -delta;
            }

            // Normalize the list of sample weightings, so they will always sum to one.
            for (int i = 0; i < sampleWeights.Length; i++)
            {
                sampleWeights[i] /= totalWeights;
            }

            // Tell the effect about our new filter settings.
            parameter_weights.SetValue(sampleWeights);
            parameter_offsets.SetValue(sampleOffsets);
        }


        /// <summary>
        /// Evaluates a single point on the gaussian falloff curve.
        /// Used for setting up the blur filter weightings.
        /// </summary>
        private float ComputeGaussian(float n)
        {
            float theta = _blurAmount;
            return (float)((1.0 / Math.Sqrt(2 * Math.PI * theta)) *
                           Math.Exp(-(n * n) / (2 * theta * theta)));
        }


        #endregion
    }
}
#endif
