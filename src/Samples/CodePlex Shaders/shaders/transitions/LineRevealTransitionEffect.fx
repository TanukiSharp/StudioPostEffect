float Progress;
float2 LineOrigin;
float2 LineNormal;
float2 LineOffset;
float FuzzyAmount;

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


float4 LineReveal(float2 uv)
{
	float2 currentLineOrigin = lerp(LineOrigin, LineOffset, Progress);
	float2 normLineNormal = normalize(LineNormal);
	float4 c1 = tex2D(Input1Sampler, uv);
	float4 c2 = tex2D(Input2Sampler, uv);

	float distFromLine = dot(normLineNormal, uv - currentLineOrigin);
	float p = saturate((distFromLine + FuzzyAmount) / (2.0f * FuzzyAmount));
	return (lerp(c2, c1, p));
}

//--------------------------------------------------------------------------------------
// Pixel Shader
//--------------------------------------------------------------------------------------
float4 main(float2 uv : TEXCOORD) : COLOR
{
	return (LineReveal(uv));
}


technique LineRevealTransition
{
	pass LineRevealTransition
	{
		VertexShader = null;
		PixelShader = compile ps_3_0 main();
	}
}
