float SHIFT = 0.005f;


texture SourceTexture;
sampler SourceTextureSampler = sampler_state
{
	Texture = <SourceTexture>;
};


float4 EmbossFunc(float2 tex : TEXCOORD) : COLOR
{
	float4 texcolor = tex2D(SourceTextureSampler, tex);

	//return (float4(1.0f, 0.0f, 0.0f, 0.0f));

	texcolor -= tex2D(SourceTextureSampler, tex.xy - SHIFT) * 2.7f;
	texcolor += tex2D(SourceTextureSampler, tex.xy + SHIFT) * 2.7f;
	texcolor.rgb = (texcolor.r + texcolor.g + texcolor.b) / 3.0f;

	return (texcolor);
}


technique Emboss
{
	pass Emboss
	{
		VertexShader = null;
		PixelShader = compile ps_3_0 EmbossFunc();
	}
}
