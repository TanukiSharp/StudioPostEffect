int Mode;
float ZoneSize = 0.0f;

texture Effect;
sampler EffectSampler = sampler_state
{
	Texture = <Effect>;
};

texture NormalRender;
sampler NormalRenderSampler = sampler_state
{
	Texture = <NormalRender>;
};


float4 Interpolate(float4 aa, float4 bb, float value, float diff)
{
	float th1 = diff - ZoneSize;
	float th2 = diff + ZoneSize;
	float coef = 1.0f / (th2 - th1);
	if (value < th1)		{ return (bb); }
	else if (value >= th2)	{ return (aa); }
	else { return (lerp(bb, aa, ((value - th1) * coef))); }
}

float4 ComparerFunc(float2 uv : TEXCOORD) : COLOR
{
	//float res = 0.0f;

	float4 aa = tex2D(EffectSampler, uv);
	float4 bb = tex2D(NormalRenderSampler, uv);

	if (Mode <= 0)
	{
		return (Interpolate(aa, bb, uv.x, 0.5f));
	}
	else if (Mode == 1)
	{
		return (Interpolate(bb, aa, uv.x, 0.5f));
	}
	else if (Mode == 2)
	{
		return (Interpolate(aa, bb, uv.y, 0.5f));
	}
	else if (Mode == 3)
	{
		return (Interpolate(bb, aa, uv.y, 0.5f));
	}
	else if (Mode == 4)
	{
		return (Interpolate(aa, bb, uv.x + uv.y, 1.0f));
	}
	else if (Mode == 5)
	{
		return (Interpolate(bb, aa, uv.x + uv.y, 1.0f));
	}
	else if (Mode == 6)
	{
		return (Interpolate(aa, bb, uv.x - uv.y, 0.0f));
	}
	else // if (Mode >= 7)
	{
		return (Interpolate(bb, aa, uv.x - uv.y, 0.0f));
	}
}

technique Comparer
{
	pass Comparer
	{
		VertexShader = null;
		PixelShader = compile ps_3_0 ComparerFunc();
	}
}
