float Progress;

texture Input;
sampler InputSampler = sampler_state
{
	Texture = <Input>;
};


float4 PixelateIn(float2 uv)
{
	float pixels = 10.0f + 1000.0f * Progress * Progress;
	float2 newUV = round(uv * pixels) / pixels;
	float4 c2 = tex2D(InputSampler, newUV);

	return (c2);
}


//--------------------------------------------------------------------------------------
// Pixel Shader
//--------------------------------------------------------------------------------------
float4 main(float2 uv : TEXCOORD) : COLOR
{
	return (PixelateIn(uv));
}


technique PixelateInTransition
{
	pass PixelateInTransition
	{
		VertexShader = null;
		PixelShader = compile ps_3_0 main();
	}
}
