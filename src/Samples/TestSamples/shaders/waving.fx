float2 Parameters;


texture SourceTexture;
sampler SourceTextureSampler = sampler_state
{
	Texture = <SourceTexture>;
};


float4 WavingFunc(float2 tex : TEXCOORD) : COLOR
{
	float4 texcolor = tex2D(SourceTextureSampler, tex);

	tex.y = tex.y + (sin(tex.y * Parameters.x) * Parameters.y);
	texcolor = tex2D(SourceTextureSampler, tex.xy);
	return (texcolor);
}


technique Waving
{
	pass Waving
	{
		VertexShader = null;
		PixelShader = compile ps_3_0 WavingFunc();
	}
}
