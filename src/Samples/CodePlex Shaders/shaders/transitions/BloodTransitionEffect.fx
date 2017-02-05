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
	Texture = <Input1>;
};


float4 Blood(float2 uv)
{
	float offset = min(Progress + Progress * tex2D(InputCloudSampler, float2(uv.x, RANDOMSEED)).r, 1.0f);
	uv.y -= offset;

	if (uv.y > 0.0f)
	{
		return (tex2D(Input1Sampler, uv));
	}
	else
	{
		return (tex2D(Input2Sampler, frac(uv)));
	}
}

//--------------------------------------------------------------------------------------
// Pixel Shader
//--------------------------------------------------------------------------------------
float4 main(float2 uv : TEXCOORD) : COLOR
{
	return (Blood(uv));
}


technique BloodTransition
{
	pass BloodTransition
	{
		VertexShader = null;
		PixelShader = compile ps_3_0 main();
	}
}
