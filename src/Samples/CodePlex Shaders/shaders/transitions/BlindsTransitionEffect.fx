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


float4 Blinds(float2 uv)
{
	if (frac(uv.y * 5.0f) < Progress)
	{
		return (tex2D(Input2Sampler, uv));
	}
	else
	{
		return (tex2D(Input1Sampler, uv));
	}
}

//--------------------------------------------------------------------------------------
// Pixel Shader
//--------------------------------------------------------------------------------------
float4 main(float2 uv : TEXCOORD) : COLOR
{
	return (Blinds(uv));
}


technique BlindsTransition
{
	pass BlindsTransition
	{
		VertexShader = null;
		PixelShader = compile ps_3_0 main();
	}
}
