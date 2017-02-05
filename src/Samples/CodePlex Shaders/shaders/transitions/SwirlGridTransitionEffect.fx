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

float4 SwirlGrid(float2 uv)
{
	float cellcount = 10.0f;
	float cellsize = 1.0f / cellcount;

	float2 cell = floor(uv * cellcount);
	float2 oddeven = fmod(cell, 2.0f);
	float cellTwistAmount = TwistAmount;
	if (oddeven.x < 1.0f)
	{
		cellTwistAmount *= -1.0f;
	}
	if (oddeven.y < 1.0f)
	{
		cellTwistAmount *= -1.0f;
	}

	float2 newUV = frac(uv * cellcount);

	float2 center = float2(0.5f, 0.5f);
	float2 toUV = newUV - center;
	float distanceFromCenter = length(toUV);
	float2 normToUV = toUV / distanceFromCenter;
	float angle = atan2(normToUV.y, normToUV.x);

	angle += distanceFromCenter * distanceFromCenter * cellTwistAmount * Progress;
	float2 newUV2;
	sincos(angle, newUV2.y, newUV2.x);
	newUV2 *= distanceFromCenter;
	newUV2 += center;

	newUV2 *= cellsize;
	newUV2 += cell * cellsize;

	float4 c1 = tex2D(Input1Sampler, newUV2);
	float4 c2 = tex2D(Input2Sampler, uv);

	return (lerp(c1, c2, Progress));
}


//--------------------------------------------------------------------------------------
// Pixel Shader
//--------------------------------------------------------------------------------------
float4 main(float2 uv : TEXCOORD) : COLOR
{
	return (SwirlGrid(uv));
}


technique SwirlGridTransition
{
	pass SwirlGridTransition
	{
		VertexShader = null;
		PixelShader = compile ps_3_0 main();
	}
}
