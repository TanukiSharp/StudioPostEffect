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


float4 SampleWithBorder(float4 border, sampler2D tex, float2 uv)
{
	if (any(saturate(uv) - uv))
	{
		return (border);
	}
	else
	{
		return (tex2D(tex, uv));
	}
}

float4 Shrink(float2 uv)
{
	float speed = 200.0f;
	float2 center = float2(0.5f, 0.5f);
	float2 toUV = uv - center;
	float distanceFromCenter = length(toUV);
	float2 normToUV = toUV / distanceFromCenter;

	float2 newUV = center + normToUV * (distanceFromCenter * (Progress * speed + 1.0f));

	float4 c1 = SampleWithBorder(0.0f, Input1Sampler, newUV);

	if (c1.a <= 0.0f)
	{
		return (tex2D(Input2Sampler, uv));
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
	return (Shrink(uv));
}


technique ShrinkTransition
{
	pass ShrinkTransition
	{
		VertexShader = null;
		PixelShader = compile ps_3_0 main();
	}
}
