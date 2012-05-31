sampler SourceTexture : register(s0);
sampler TextureNormalMap : register(s1);
float displacementFactor = 1.0f;
float blurFactor = 1.0f;

float2 BlurDisplacementCoords[12] =
{
    { 1.0f,  0.0f}, { 0.5f,  0.8660f}, {-0.5f,  0.8660f}, {-1.0f,  0.0f},
    {-0.5f, -0.8660f}, { 0.5f, -0.8660f}, { 1.5f,  0.8660f}, { 0.0f,  1.7320f},
    {-1.5f,  0.8660f}, {-1.5f, -0.8660f}, { 0.0f, -1.7320f}, { 1.5f, -0.8660f},
};

float4 main(float4 color : COLOR0, float2 Tex : TEXCOORD0) : COLOR0
{   	
	float4 Color;		
	float4 NormalColor;
	NormalColor = tex2D(TextureNormalMap, Tex.xy); 
	// use the alpha to control the displacement intensity
	NormalColor.r -= 0.5f;
	NormalColor.g -= 0.5f;
	// red color is horizontal displacement
	Tex.x += NormalColor.r * displacementFactor * NormalColor.a * 0.5f; 
	// green color is vertical displacement
    Tex.y += NormalColor.g * displacementFactor * NormalColor.a * 0.5f;
    // blue color is blur amount
    if (NormalColor.b > 0.0f)
    {
		Color = 0.0f;
		NormalColor.b *= 0.005f * blurFactor;	
		// add the colors of 12 different surrounding pixels together
		// to create a blur effect
		for( int i = 0; i < 12; i++)
		{
			Color += tex2D(SourceTexture, Tex + (NormalColor.b * BlurDisplacementCoords[i]));
		}	 
		// divide the total results by 12 to get the result color
		Color /= 12.0f;		
    }
    else
    {
		Color = tex2D(SourceTexture, Tex.xy);
    }	
    return Color;
}


technique Refraction
{
    pass Pass1
    {
        PixelShader = compile ps_2_0 main();
    }
}