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


float4 PixelateOut(float2 uv)
{
	float pixels = max(4.0f, 100.0f * (1.0f - Progress));
	float2 newUV = round(uv * pixels) / pixels;
	float4 c1 = tex2D(Input1Sampler, newUV);
	float4 c2 = tex2D(Input2Sampler, uv);

	if (Progress > 0.8f)
	{
		float new_progress = (Progress - 0.8f) * 5.0f;
		return (lerp(c1, c2, new_progress));
	}
	else
	{
		return (c1);
	}
}

//--------------------------------------------------------------------------------------
// Pixel Shader
//--------------------------------------------------------------------------------------
float4 main(float2 uv : TEXCOORD) : COLOR
{
	return (PixelateOut(uv));
}


technique PixelateOutTransition
{
	pass PixelateOutTransition
	{
		VertexShader = null;
		PixelShader = compile ps_3_0 main();
	}
}
