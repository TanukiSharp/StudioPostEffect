texture Input;
sampler InputSampler = sampler_state
{
	Texture = <Input>;
};

texture NormalMap;
sampler NormalMapSampler = sampler_state
{
	Texture = <NormalMap>;
};


float4 GlassFunc(float2 uv : TEXCOORD) : COLOR
{
	float4 nm = 2.0f * (tex2D(NormalMapSampler, uv /** 2.5f*/) - 0.5f);

	return (tex2D(InputSampler, uv + nm.xy * 0.05f));
}


technique OgreGlass
{
	pass OgreGlass
	{
		VertexShader = null;
		PixelShader = compile ps_3_0 GlassFunc();
	}
}
