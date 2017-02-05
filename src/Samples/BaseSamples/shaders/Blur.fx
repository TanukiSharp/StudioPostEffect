float2 Factor = { 2.0f, 2.0f };

float2 PIXEL_SIZE;
float2 RENDER_TARGET_SIZE;


texture SourceTexture;
sampler SourceTextureSampler = sampler_state
{
	Texture = <SourceTexture>;
};


float4 StrongBlurHFunc(float2 tex : TEXCOORD) : COLOR
{
	float4 sum = 0.0f;

	for (int i = -7; i <= 7; i++)
	{
		sum += tex2D(SourceTextureSampler, tex + (float2(i, 0.0f) * Factor.x * PIXEL_SIZE.x));
	}
	sum /= 15.0f;

	return (sum);
}

float4 StrongBlurVFunc(float2 tex : TEXCOORD) : COLOR
{
	float4 sum = 0.0f;

	for (int i = -7; i <= 7; i++)
	{
		sum += tex2D(SourceTextureSampler, tex + (float2(0.0f, i) * Factor.y * PIXEL_SIZE.y));
	}
	sum /= 15.0f;

	return (sum);
}



technique Blur
{
	pass Horizontal
	{
		VertexShader = null;
		PixelShader = compile ps_3_0 StrongBlurHFunc();
	}

	pass Vertical
	{
		VertexShader = null;
		PixelShader = compile ps_3_0 StrongBlurVFunc();
	}
}
