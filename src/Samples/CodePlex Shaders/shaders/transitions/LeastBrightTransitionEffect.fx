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


float4 LeastBright(float2 uv)
{
	int c = 4;
	int c2 = 3;
	float oc = (c - 1) / 2.0f;
	float oc2 = (c2 - 1) / 2.0f;
	float offset = 0.01f * Progress;

	float leastBright = 1.0f;
	float4 leastBrightColor;
	for (int y = 0; y < c; y++)
	{
		for (int x = 0; x < c2; x++)
		{
			float2 newUV = uv + (float2(x, y) - float2(oc2, oc)) * offset;
			float4 color = tex2D(Input1Sampler, newUV);
			float brightness = dot(color.rgb, float3(1.0f, 1.1f, 0.9f));
			if (brightness < leastBright)
			{
				leastBright = brightness;
				leastBrightColor = color;
			}
		}
	}

	float4 impl = tex2D(Input2Sampler, uv);

	return (lerp(leastBrightColor, impl, Progress));
}

//--------------------------------------------------------------------------------------
// Pixel Shader
//--------------------------------------------------------------------------------------
float4 main(float2 uv : TEXCOORD) : COLOR
{
	return (LeastBright(uv));
}


technique LeastBrightTransition
{
	pass LeastBrightTransition
	{
		VertexShader = null;
		PixelShader = compile ps_3_0 main();
	}
}
