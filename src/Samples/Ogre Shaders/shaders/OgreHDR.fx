texture Input;
sampler InputSampler = sampler_state
{
	Texture = <Input>;
};

float4 HDRFunc(float2 uv : TEXCOORD) : COLOR
{
	float4 color = tex2D(InputSampler, uv);
	return (color * 1.7f);
}

technique OgreHDR
{
	pass OgreHDR
	{
		VertexShader = null;
		PixelShader = compile ps_3_0 HDRFunc();
	}
}
