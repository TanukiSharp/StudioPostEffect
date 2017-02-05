float2 Factor = 1.0f;

float2 PIXEL_SIZE;


texture SourceTexture;
sampler SourceTextureSampler = sampler_state
{
	Texture = <SourceTexture>;
};

texture PatternTexture;
sampler PatternTextureSampler = sampler_state
{
	Texture = <PatternTexture>;
};



float4 StrongBlurHFunc(float2 tex : TEXCOORD) : COLOR
{
	float hajime = -7.0f;
	float4 sum = 0.0f;

	float4 input = tex2D(SourceTextureSampler, tex);
	float4 pattern = tex2D(PatternTextureSampler, tex);

	for (int i = 0; hajime <= 7.0f; i++)
	{
		sum += tex2D(SourceTextureSampler, tex + (float2(hajime, 0.0f) * Factor.x * PIXEL_SIZE.x));
		hajime += 1.0f;
	}
	sum /= 15.0f;

	return ((input * pattern.x) + (sum * (1.0f - pattern.x)));
}


float4 StrongBlurVFunc(float2 tex : TEXCOORD) : COLOR
{
	float hajime = -7.0f;
	float4 sum = 0.0f;

	float4 input = tex2D(SourceTextureSampler, tex);
	float4 pattern = tex2D(PatternTextureSampler, tex);

	for (int i = 0; hajime <= 7.0f; i++)
	{
		sum += tex2D(SourceTextureSampler, tex + (float2(0.0f, hajime) * Factor.y * PIXEL_SIZE.y));
		hajime += 1.0f;
	}
	sum /= 15.0f;

	return ((input * pattern.x) + (sum * (1.0f - pattern.x)));
}




technique BlurPattern
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
