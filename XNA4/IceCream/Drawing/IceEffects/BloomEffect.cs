#if !REACH
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;

using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;



using System;
using System.Collections.Generic;
using System.Text;
using IceCream.Drawing;

namespace IceCream.Drawing.IceEffects
{
    public class BloomEffect : IceEffect
    {
        #region Fields

        private EffectParameter parameter_BloomIntensity;
        private EffectParameter parameter_BaseIntensity;
        private EffectParameter parameter_BloomSaturation;
        private EffectParameter parameter_BaseSaturation;
        private EffectParameter parameter_BloomThreshold;
        private EffectParameter parameter_weights;
        private EffectParameter parameter_offsets;
        private float _bloomIntensity = 1f;
        private float _baseIntensity = 1;
        private float _bloomSaturation = 1;
        private float _baseSaturation = 1;
        private float _bloomThreshold = 0.25f;
        private float _blurRadius = 4f;

        #endregion

        #region Constructor

        public BloomEffect()
            : base(AssetScope.Embedded, "BloomEffect")
        {
            this.ParametersProperties = new LinearProperty[6];
            this.ParametersProperties[0] = new LinearProperty(0.35f, "Bloom Threshold", 0, 1);
            this.ParametersProperties[1] = new LinearProperty(1.25f, "Bloom Intensity", 0, 10);
            this.ParametersProperties[2] = new LinearProperty(1f, "Base Intensity", 0, 10);
            this.ParametersProperties[3] = new LinearProperty(1f, "Bloom Saturation", 0, 10);
            this.ParametersProperties[4] = new LinearProperty(1f, "Base Saturation", 0, 10);
            this.ParametersProperties[5] = new LinearProperty(4f, "Blur Radius", 0, 10);            
        }

        #endregion

        #region Methods

        public override void Draw(SpriteBatch spriteBatch)
        {           
            // Pass 1: draw the scene (rt A) into rendertarget B, using a
            // shader that extracts only the brightest parts of the image.
            parameter_BloomThreshold.SetValue(_bloomThreshold);           
            // resolve the current buffer to a texture                    
            DrawingManager.RenderTargetManager.SwitchTo(RenderTargetInstance.EffectA);
            GraphicsDevice.Clear(Color.Transparent);
            DrawFullScreenQuad(spriteBatch,
                DrawingManager.RenderTargetManager.GetRenderTarget2D(DrawingManager.CurrentRenderTarget), Effects[0]);
            // Pass 2: draw from rendertarget B into rendertarget C,
            // using a shader to apply a horizontal gaussian blur filter.
            SetBlurEffectParameters(1.0f / (float)DrawingManager.ViewPortSize.X, 0);
            DrawingManager.RenderTargetManager.SwitchTo(RenderTargetInstance.EffectB);
            GraphicsDevice.Clear(Color.Transparent);
            DrawFullScreenQuad(spriteBatch, DrawingManager.RenderTargetManager.GetRenderTarget2D(RenderTargetInstance.EffectA),
                Effects[1]);            
            // Pass 3: draw from rendertarget C back into rendertarget B,
            // using a shader to apply a vertical gaussian blur filter.
            SetBlurEffectParameters(0, 1.0f / (float)DrawingManager.ViewPortSize.Y);
            DrawingManager.RenderTargetManager.SwitchTo(RenderTargetInstance.EffectA);
            GraphicsDevice.Clear(Color.Transparent);
            DrawFullScreenQuad(spriteBatch, DrawingManager.RenderTargetManager.GetRenderTarget2D(RenderTargetInstance.EffectB), 
                Effects[1]);
            parameter_BaseIntensity.SetValue(_baseIntensity);
            parameter_BaseSaturation.SetValue(_baseSaturation);
            parameter_BloomIntensity.SetValue(_bloomIntensity);
            parameter_BloomSaturation.SetValue(_bloomSaturation);
            // Draw A over C
            DrawingManager.RenderTargetManager.SwitchTo(RenderTargetInstance.EffectB);
            GraphicsDevice.Clear(Color.Transparent);
            DrawFullScreenQuad(spriteBatch,
                DrawingManager.RenderTargetManager.GetRenderTarget2D(DrawingManager.CurrentRenderTarget), null);
            DrawingManager.RenderTargetManager.SwitchTo(DrawingManager.CurrentRenderTarget);
            GraphicsDevice.Clear(Color.Transparent);
            GraphicsDevice.Textures[1] =
                DrawingManager.RenderTargetManager.GetRenderTarget2D(RenderTargetInstance.EffectB);
            DrawFullScreenQuad(spriteBatch, DrawingManager.RenderTargetManager.GetRenderTarget2D(RenderTargetInstance.EffectA),
                Effects[2]);
        }

        public override void SetParameters(IceEffectParameters parameters)
        {
            _bloomThreshold = parameters.Parameter1;
            _bloomIntensity = parameters.Parameter2;
            _baseIntensity = parameters.Parameter3;
            _bloomSaturation = parameters.Parameter4;
            _baseSaturation = parameters.Parameter5;
            if (parameters.Parameter6 <= 0f)
            {
                // prevent 0 or less radius
                parameters.Parameter6 = 0.0001f;
            }
            _blurRadius = parameters.Parameter6;
        }
        

        public override void LoadParameters()
        {
            parameter_BloomIntensity = Effects[2].Parameters["BloomIntensity"];
            parameter_BaseIntensity = Effects[2].Parameters["BaseIntensity"];
            parameter_BloomSaturation = Effects[2].Parameters["BloomSaturation"];
            parameter_BaseSaturation = Effects[2].Parameters["BaseSaturation"];
            parameter_BloomThreshold = Effects[0].Parameters["BloomThreshold"];
            parameter_weights = Effects[1].Parameters["SampleWeights"];
            parameter_offsets = Effects[1].Parameters["SampleOffsets"];
            // Look up how many samples our gaussian blur effect supports.
            sampleCount = parameter_weights.Elements.Count;
            // Create temporary arrays for computing our filter settings.
            sampleWeights = new float[sampleCount];
            sampleOffsets = new Vector2[sampleCount];
        }
        int sampleCount;
        Vector2[] sampleOffsets;
        float[] sampleWeights;
        /// <summary>
        /// Computes sample weightings and texture coordinate offsets
        /// for one pass of a separable gaussian blur filter.
        /// </summary>
        private void SetBlurEffectParameters(float dx, float dy)
        {
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
            float theta = _blurRadius;
            return (float)((1.0 / Math.Sqrt(2 * Math.PI * theta)) *
                           Math.Exp(-(n * n) / (2 * theta * theta)));
        }


        #endregion

        public float Base
        {
            get { return _baseIntensity; }
            set { _baseIntensity = value; }
        }
        public float BaseSat
        {
            get { return _baseSaturation; }
            set { _baseSaturation = value; }
        }
        public float Bloom
        {
            get { return _bloomIntensity; }
            set { _bloomIntensity = value; }
        }
        public float BloomSat
        {
            get { return _bloomSaturation; }
            set { _bloomSaturation = value; }
        }
        public float Threshold
        {
            get { return _bloomThreshold; }
            set { _bloomThreshold = value; }
        }
        public float Blur
        {
            get { return _blurRadius; }
            set { _blurRadius = value; }
        }

    }
}
#endif