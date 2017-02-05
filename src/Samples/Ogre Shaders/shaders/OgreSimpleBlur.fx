texture Input;
sampler InputSampler = sampler_state
{
	Texture = <Input>;
};

static const float2 Samples[12] =
{
	-0.326212, -0.405805,
	-0.840144, -0.073580,
	-0.695914, 0.457137,
	-0.203345, 0.620716,
	0.962340, -0.194983,
	0.473434, -0.480026,
	0.519456, 0.767022,
	0.185461, -0.893124,
	0.507431, 0.064425,
	0.896420, 0.412458,
	-0.321940, -0.932615,
	-0.791559, -0.597705,
};


float4 SimpleBlurHFunc(float2 uv : TEXCOORD) : COLOR
{
	float4 sum = tex2D(InputSampler, uv);
	for (int i = 0; i < 12; i++)
	{
		sum += tex2D(InputSampler, uv + 0.025f * Samples[i]);
	}
	return (sum / 13);
}

float4 SimpleBlurVFunc(float2 uv : TEXCOORD) : COLOR
{
	float4 sum = tex2D(InputSampler, uv);
	for (int i = 0; i < 12; i++)
	{
		sum += tex2D(InputSampler, uv + 0.025f * Samples[i]);
	}
	return (sum / 13);
}


technique OgreSimpleBlur
{
	pass H
	{
		VertexShader = null;
		PixelShader = compile ps_3_0 SimpleBlurHFunc();
	}

	pass V
	{
		VertexShader = null;
		PixelShader = compile ps_3_0 SimpleBlurVFunc();
	}
}
