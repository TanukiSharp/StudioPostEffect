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

float4 Wave(float2 uv)
{
	float mag = 0.1f;
	float phase = 14.0f;
	float freq = 20.0f;

	float2 newUV = uv + float2(mag * Progress * sin(freq * uv.y + phase * Progress), 0.0f);

	float4 c1 = SampleWithBorder(0.0f, Input1Sampler, newUV);
	float4 c2 = tex2D(Input2Sampler, uv);

	return (lerp(c1, c2, Progress));
}

float4 StandingWave(float2 uv)
{
	float pi = 3.141592f;
	float mag = 0.01f;
	float freq = 8.0f * pi;
	float freq2 = 6.0f * pi;

	float2 newUV = uv + mag * sin(Progress*freq2) * float2(cos(freq * uv.x), sin(freq * uv.y));

	float4 c1 = tex2D(Input1Sampler, frac(newUV));
	float4 c2 = tex2D(Input2Sampler, uv);

	return (lerp(c1, c2, Progress));
}

float4 MotionBlur(float2 uv)
{
	float4 c1 = 0.0f;
	int count = 26;
	float2 direction = float2(0.05f, 0.05f);
	float2 offset = Progress * direction;
	float2 startUV = uv - offset * 0.5f;
	float2 delta = offset / (count - 1.0f);

	for (int i = 0; i < count; i++)
	{
		c1 += tex2D(Input1Sampler, startUV + delta * i);
	}

	c1 /= count;
	return (c1);
}

//--------------------------------------------------------------------------------------
// Pixel Shader
//--------------------------------------------------------------------------------------
float4 main(float2 uv : TEXCOORD) : COLOR
{
	return (Wave(uv));
}


technique WaveTransition
{
	pass WaveTransition
	{
		VertexShader = null;
		PixelShader = compile ps_3_0 main();
	}
}
