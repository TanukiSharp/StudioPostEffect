float FaderCoef;

texture Input1;
sampler Input1Sampler = sampler_state
{
	Texture = <Input1>;
};

texture Input2;
sampler Input2Sampler = sampler_state
{
	Texture = <Input2>;
};

texture InputFader;
sampler InputFaderSampler = sampler_state
{
	Texture = <InputFader>;
};

float4 ImageFadingFunc(float2 uv : TEXCOORD) : COLOR
{
	float4 color1 = tex2D(Input1Sampler, uv);
	float4 color2 = tex2D(Input2Sampler, uv);
	float4 fader = tex2D(InputFaderSampler, uv);

	float coef = saturate(FaderCoef + fader.x);

	float4 result = (color1 * (1.0f - coef)) + (color2 * coef);

	return (result);
}

technique ImageFading
{
	pass ImageFading
	{
		VertexShader = null;
		PixelShader = compile ps_3_0 ImageFadingFunc();
	}
}
