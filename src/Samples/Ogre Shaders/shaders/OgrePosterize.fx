texture Input;
sampler InputSampler = sampler_state
{
	Texture = <Input>;
};


float4 PosterizeFunc(float2 uv : TEXCOORD) : COLOR
{
	float nColors = 8.0f;
	float gamma = 0.6f;

	float4 texCol = tex2D(InputSampler, uv);
	float3 tc = texCol.xyz;
	tc = pow(tc, gamma);
	tc = tc * nColors;
	tc = floor(tc);
	tc = tc / nColors;
	tc = pow(tc, 1.0f / gamma);
	return (float4(tc, texCol.w));
}


technique OgrePosterize
{
	pass OgrePosterize
	{
		VertexShader = null;
		PixelShader = compile ps_3_0 PosterizeFunc();
	}
}
