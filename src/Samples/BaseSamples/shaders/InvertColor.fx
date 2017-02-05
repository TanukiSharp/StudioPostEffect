texture SourceTexture;
sampler SourceTextureSampler = sampler_state
{
	Texture = <SourceTexture>;
};


float4 InvertColorFunc(float2 tex : TEXCOORD) : COLOR
{
	float4 texcolor = 1.0f - tex2D(SourceTextureSampler, tex);
	return (float4(texcolor.rgb, 1.0f));
}


technique InvertColor
{
	pass InvertColor
	{
		VertexShader = null;
		PixelShader = compile ps_3_0 InvertColorFunc();
	}
}
