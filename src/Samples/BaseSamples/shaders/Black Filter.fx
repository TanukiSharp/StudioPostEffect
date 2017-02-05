float Threshold;

texture Input;
sampler InputSampler = sampler_state
{
	Texture = <Input>;
};

static const float4 bw_const = {0.299f, 0.587f, 0.114f, 0.0f };

float4 BlackFilterFunc(float2 uv : TEXCOORD) : COLOR
{
	float4 color = tex2D(InputSampler, uv);
	float result = dot(color, bw_const);

	if (result <= Threshold)
	{
		return (0.0f);
	}
	else
	{
		return (1.0f);
	}
}

technique BlackFilter
{
	pass BlackFilter
	{
		VertexShader = null;
		PixelShader = compile ps_3_0 BlackFilterFunc();
	}
}
