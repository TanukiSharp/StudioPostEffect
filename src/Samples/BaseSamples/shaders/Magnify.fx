float MagnifyCoef = 0.95f;


texture SourceTexture;
sampler SourceTextureSampler = sampler_state
{
	Texture = <SourceTexture>;
};


float4 MagnifyFunc(float2 tex : TEXCOORD) : COLOR
{
	// convert texture space to world space (0 -> 1 ==> -1 -> 1)
	float2 n = (tex * 2.0f) - 1.0f;

	n = n * MagnifyCoef;

	// convert world space to texture space (-1 -> 1 ==> 0 -> 1)
	float2 ntex = (n + 1.0f) / 2.0f;

	return (tex2D(SourceTextureSampler, clamp(ntex, 0.0f, 1.0f)));
}


technique Magnify
{
	pass Magnify
	{
		VertexShader = null;
		PixelShader = compile ps_3_0 MagnifyFunc();
	}
}
