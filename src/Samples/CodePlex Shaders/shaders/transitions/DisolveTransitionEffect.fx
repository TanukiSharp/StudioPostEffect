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
texture InputNoise;
sampler InputNoiseSampler = sampler_state
{
	Texture = <InputNoise>;
};


float4 Disolve(float2 uv)
{
	float noise = tex2D(InputNoiseSampler, frac(uv + RANDOMSEED)).x;
	if (noise > Progress)
	{
		return (tex2D(Input1Sampler, uv));
	}
	else
	{
		return (tex2D(Input2Sampler, uv));
	}
}

//--------------------------------------------------------------------------------------
// Pixel Shader
//--------------------------------------------------------------------------------------
float4 main(float2 uv : TEXCOORD) : COLOR
{
	return (Disolve(uv));
}


technique DisolveTransition
{
	pass DisolveTransition
	{
		VertexShader = null;
		PixelShader = compile ps_3_0 main();
	}
}
