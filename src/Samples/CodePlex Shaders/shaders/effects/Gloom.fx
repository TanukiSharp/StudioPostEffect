//--------------------------------------------------------------------------------------
// 
// WPF ShaderEffect HLSL -- GloomEffect
//
//--------------------------------------------------------------------------------------

//-----------------------------------------------------------------------------------------
// Shader constant register mappings (scalars - float, double, Point, Color, Point3D, etc.)
//-----------------------------------------------------------------------------------------

float GloomIntensity;
float BaseIntensity;
float GloomSaturation;
float BaseSaturation;

//--------------------------------------------------------------------------------------
// Sampler Inputs (Brushes, including ImplicitInput)
//--------------------------------------------------------------------------------------

texture SourceTexture;
sampler SourceTextureSampler = sampler_state
{
	Texture = <SourceTexture>;
};


//--------------------------------------------------------------------------------------
// Pixel Shader
//--------------------------------------------------------------------------------------

float4 AdjustSaturation(float4 color, float saturation)
{
    float grey = dot(color, float3(0.3f, 0.59f, 0.11f));
    return lerp(grey, color, saturation);
}

float4 GloomFunc(float2 uv : TEXCOORD) : COLOR
{
	float GloomThreshold = 0.25f;

	float4 base = 1.0f - tex2D(SourceTextureSampler, uv);
	float4 gloom = saturate((base - GloomThreshold) / (1.0f - GloomThreshold));

	// Adjust color saturation and intensity.
	gloom = AdjustSaturation(gloom, GloomSaturation) * GloomIntensity;
	base = AdjustSaturation(base, BaseSaturation) * BaseIntensity;

	// Darken down the base image in areas where there is a lot of bloom,
	// to prevent things looking excessively burned-out.
	base *= (1.0f - saturate(gloom));

	// Combine the two images.
	return 1.0f - (base + gloom);
}


technique Gloom
{
	pass Gloom
	{
		VertexShader = null;
		PixelShader = compile ps_3_0 GloomFunc();
	}
}
