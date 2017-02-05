float BRIGHTNESS_THRESHOLD = 0.35f;
static const float3 LUMINANCE_FACTOR = { 0.3f, 0.59f, 0.11f };


texture SourceTexture;
sampler SourceTextureSampler = sampler_state
{
	Texture = <SourceTexture>;
};


float4 BrightnessFunc(float2 tex : TEXCOORD) : COLOR
{
	float4 color = tex2D(SourceTextureSampler, tex);

	float luminance = dot(LUMINANCE_FACTOR, color.rgb);
	luminance = max(0.0f, luminance - BRIGHTNESS_THRESHOLD);
	color.rgb *= sign(luminance);
	color.a = 1.0f;

	return (color);
}

technique LightSource
{
	pass LightSource
	{
		VertexShader = null;
		PixelShader = compile ps_3_0 BrightnessFunc();
	}
}
