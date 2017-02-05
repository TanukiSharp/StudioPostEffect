float Factor;

float2 PIXEL_SIZE;


texture Input;
sampler InputSampler = sampler_state
{
	Texture = <Input>;
};


static const int OffsetNumberGaussian = 13;

static const float BlurWeightsGaussian[OffsetNumberGaussian] = 
{
	1.0f / 4096.0f,
	12.0f / 4096.0f,
	66.0f / 4096.0f,
	220.0f / 4096.0f,
	495.0f / 4096.0f,
	792.0f / 4096.0f,
	924.0f / 4096.0f,
	792.0f / 4096.0f,
	495.0f / 4096.0f,
	220.0f / 4096.0f,
	66.0f / 4096.0f,
	12.0f / 4096.0f,
	1.0f / 4096.0f,
};


float4 GaussianBlurHFunc(float2 tex : TEXCOORD) : COLOR
{
	float4 sum = 0.0f;
	float i;

	int k = 0;
	for (i = -6.0f; i <= 6.0f; i += 1.0f)
	{
		sum += tex2D(InputSampler, tex + (float2(i, 0.0f) * Factor * PIXEL_SIZE.x)) * BlurWeightsGaussian[k++];
	}
	k = 0;
	for (i = -6.0f; i <= 6.0f; i += 1.0f)
	{
		sum += tex2D(InputSampler, tex + (float2(i, 0.0f) * Factor * PIXEL_SIZE.x)) * BlurWeightsGaussian[k++];
	}
	sum /= 2.0f;
	sum.a = 1.0f;

	return (sum);
}


float4 GaussianBlurVFunc(float2 tex : TEXCOORD) : COLOR
{
	float4 sum = 0.0f;
	float i;

	int k = 0;
	for (i = -6.0f; i <= 6.0f; i += 1.0f)
	{
		float2 trick = float2(0.0000000001f, i);
		sum += tex2D(InputSampler, tex + (trick * Factor * PIXEL_SIZE.y)) * BlurWeightsGaussian[k++];
	}
	k = 0;
	for (i = -6.0f; i <= 6.0f; i += 1.0f)
	{
		float2 trick = float2(0.0000000001f, i);
		sum += tex2D(InputSampler, tex + (trick * Factor * PIXEL_SIZE.y)) * BlurWeightsGaussian[k++];
	}
	sum /= 2.0f;
	sum.a = 1.0f;

	return (sum);
}



technique GaussianBlur
{
	pass H
	{
		VertexShader = null;
		PixelShader = compile ps_3_0 GaussianBlurHFunc();
	}

	pass V
	{
		VertexShader = null;
		PixelShader = compile ps_3_0 GaussianBlurVFunc();
	}
}
