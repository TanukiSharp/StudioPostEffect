//--------------------------------------------------------------------------------------
//
// WPF ShaderEffect HLSL -- BandedSwirlEffect
// Swirl shader with bands going in different directions
//--------------------------------------------------------------------------------------

float2 Center;
float SpiralStrength;
float DistanceThreshold;



//--------------------------------------------------------------------------------------
// Sampler Inputs (Brushes, including ImplicitInput)
//--------------------------------------------------------------------------------------

texture SourceTexture;
sampler SourceTextureSampler = sampler_state
{
	Texture = <SourceTexture>;
};



//--------------------------------------------------------------------------------------
// Pixel Shader
//--------------------------------------------------------------------------------------

float4 BandedSwirlFunc(float2 uv : TEXCOORD) : COLOR
{
	float2 dir = uv - Center;
	float l = length(dir);

	dir = dir / l;
	float angle = atan2(dir.y, dir.x);

	float remainder = frac(l / DistanceThreshold);

	float preTransitionWidth = 0.25f;
	float fac;

	if (remainder < 0.25f)
	{
		fac = 1.0f;
	}
	else if (remainder < 0.5f)
	{
		// transition zone - go smoothly from previous zone to next.
		fac = 1.0f - 8.0f * (remainder - preTransitionWidth);
	}
	else if (remainder < 0.75f)
	{
		fac = -1.0f;
	}
	else
	{
		// transition zone - go smoothly from previous zone to next.
		fac = -(1.0f - 8.0f * (remainder - 0.75f));
	}

	float newAng = angle + fac * SpiralStrength * l;

	float xAmt = cos(newAng) * l;
	float yAmt = sin(newAng) * l;

	float2 newCoord = Center + float2(xAmt, yAmt);

	return tex2D(SourceTextureSampler, newCoord);
}

technique BandedSwirl
{
	pass BandedSwirl
	{
		VertexShader = null;
		PixelShader = compile ps_3_0 BandedSwirlFunc();
	}
}
