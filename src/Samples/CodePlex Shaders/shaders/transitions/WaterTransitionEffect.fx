float Progress;
float RANDOMSEED;

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
texture InputCloud;
sampler InputCloudSampler = sampler_state
{
	Texture = <InputCloud>;
};


float4 Water(float2 uv)
{
	float2 offset = tex2D(InputCloudSampler, float2(uv.x / 10.0f, frac(uv.y / 10.0f + min(0.9f, RANDOMSEED)))).xy * 2.0f - 1.0f;
	float4 c1 = tex2D(Input1Sampler, frac(uv + offset * Progress));
	float4 c2 = tex2D(Input2Sampler, uv);

	if (c1.a <= 0.0f)
		return (c2);
	else
		return (lerp(c1, c2, Progress));
}

//--------------------------------------------------------------------------------------
// Pixel Shader
//--------------------------------------------------------------------------------------
float4 main(float2 uv : TEXCOORD) : COLOR
{
	return (Water(uv));
}


technique WaterTransition
{
	pass WaterTransition
	{
		VertexShader = null;
		PixelShader = compile ps_3_0 main();
	}
}
