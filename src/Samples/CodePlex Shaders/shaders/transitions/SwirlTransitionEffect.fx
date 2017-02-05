float Progress;
float2 TwistAmount;

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

float4 Swirl(float2 uv)
{
	float2 center = float2(0.5f, 0.5f);
	float2 toUV = uv - center;
	float distanceFromCenter = length(toUV);
	float2 normToUV = toUV / distanceFromCenter;
	float angle = atan2(normToUV.y, normToUV.x);

	angle += distanceFromCenter * distanceFromCenter * TwistAmount * Progress;
	float2 newUV;
	sincos(angle, newUV.y, newUV.x);
	newUV *= distanceFromCenter;
	newUV += center;

	float4 c1 = SampleWithBorder(0.0f, Input1Sampler, newUV);
	float4 c2 = tex2D(Input2Sampler, uv);

	return (lerp(c1, c2, Progress));
}

//--------------------------------------------------------------------------------------
// Pixel Shader
//--------------------------------------------------------------------------------------
float4 main(float2 uv : TEXCOORD) : COLOR
{
	return (Swirl(uv));
}


technique SwirlTransition
{
	pass SwirlTransition
	{
		VertexShader = null;
		PixelShader = compile ps_3_0 main();
	}
}
