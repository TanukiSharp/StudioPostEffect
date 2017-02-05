texture Input;
sampler InputSampler = sampler_state
{
	Texture = <Input>;
};

texture Pattern;
sampler PatternSampler = sampler_state
{
	Texture = <Pattern>;
};


float4 DitherFunc(float2 uv : TEXCOORD) : COLOR
{
	float c = dot(tex2D(InputSampler, uv), float4(0.3f, 0.11f, 0.59f, 0.0f));
	float n = dot(tex2D(PatternSampler, uv), float4(0.3f, 0.11f, 0.59f, 0.0f)) * 2.0f - 1.0f;
	
	c += n;
	
	if (c > 0.5f)
	{
		c = 0.0f;
	}
	else
	{
		c = 1.0f;
	}   
	return (float4(c, c, c, 1.0f));
}

technique OgreDither
{
	pass OgreDither
	{
		VertexShader = null;
		PixelShader = compile ps_3_0 DitherFunc();
	}
}
