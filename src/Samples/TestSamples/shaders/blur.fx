float2 Factor = 1.0f;

float2 PIXEL_SIZE;


texture SourceTexture;
sampler SourceTextureSampler = sampler_state
{
	Texture = <SourceTexture>;
};



float4 StrongBlurHFunc(float2 tex : TEXCOORD) : COLOR
{
	float hajime = -7.0f;
	float4 sum = 0.0f;

	for (float i = 0.0f; hajime <= 7.0f; i += 1.0f)
	{
		sum += tex2D(SourceTextureSampler, tex + (float2(hajime, 0.0f) * Factor.x * PIXEL_SIZE.x));
		hajime += 1.0f;
	}
	sum /= 15.0f;

	return (sum);
}


float4 StrongBlurVFunc(float2 tex : TEXCOORD) : COLOR
{
	float hajime = -7.0f;
	float4 sum = 0.0f;

	for (float i = 0.0f; hajime <= 7.0f; i += 1.0f)
	{
		sum += tex2D(SourceTextureSampler, tex + (float2(0.0f, hajime) * Factor.y * PIXEL_SIZE.y));
		hajime += 1.0f;
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
