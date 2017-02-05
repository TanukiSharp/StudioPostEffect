float Progress;

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
texture InputTrig;
sampler InputTrigSampler = sampler_state
{
	Texture = <InputTrig>;
};


float4 CircularBlur(float2 uv)
{
	float2 center = float2(0.5f, 0.5f);
	float2 toUV = uv - center;
	float distanceFromCenter = length(toUV);
	float2 normToUV = toUV / distanceFromCenter;
	float angle = tex2D(InputTrigSampler, (normToUV + 1) * 0.5f).z;

	float4 c1 = 0.0f;
	float s = Progress * 0.005f;
	int count = 7;

	for (int i = 0; i < count; i++)
	{
		float newAngle = angle - i * s;
		float2 newUV = (tex2D(InputTrigSampler, frac(newAngle - 0.5f)).xy * 2.0f - 1.0f) * distanceFromCenter + center;
		c1 += tex2D(Input1Sampler, newUV);
	}

	c1 /= count;
	float4 c2 = tex2D(Input2Sampler, uv);

	return (lerp(c1, c2, Progress));
}

//--------------------------------------------------------------------------------------
// Pixel Shader
//--------------------------------------------------------------------------------------
float4 main(float2 uv : TEXCOORD) : COLOR
{
	return (CircularBlur(uv));
}


technique CircularBlurTransition
{
	pass CircularBlurTransition
	{
		VertexShader = null;
		PixelShader = compile ps_3_0 main();
	}
}
