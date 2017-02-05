float MergeCoef = 0.2f;

texture FirstTexture;
sampler FirstTextureSampler = sampler_state
{
	Texture = <FirstTexture>;
};

texture SecondTexture;
sampler SecondTextureSampler = sampler_state
{
	Texture = <SecondTexture>;
};


float4 MergeCoefFunc(float2 tex : TEXCOORD) : COLOR
{
	float4 first = tex2D(FirstTextureSampler, tex);
	float4 second = tex2D(SecondTextureSampler, tex);

	//return (first * MergeCoef + second * (1.0f - MergeCoef));
	return (lerp(first, second, MergeCoef));
}

float4 MergeAddFunc(float2 tex : TEXCOORD) : COLOR
{
	float4 first = tex2D(FirstTextureSampler, tex);
	float4 second = tex2D(SecondTextureSampler, tex);

	return (first + second);
}

float4 MergeSubstractFunc(float2 tex : TEXCOORD) : COLOR
{
	float4 first = tex2D(FirstTextureSampler, tex);
	float4 second = tex2D(SecondTextureSampler, tex);

	return (first - second);
}

float4 FadeMergeFunc(float2 tex : TEXCOORD) : COLOR
{
	float4 first = tex2D(FirstTextureSampler, tex);
	float4 second = tex2D(SecondTextureSampler, tex);

	return (lerp(first, second, tex.x));
}


technique Merge
{
	pass CoefMerge
	{
		VertexShader = null;
		PixelShader = compile ps_3_0 MergeCoefFunc();
	}
	
	pass AddMerge
	{
		VertexShader = null;
		PixelShader = compile ps_3_0 MergeAddFunc();
	}

	pass SubstractMerge
	{
		VertexShader = null;
		PixelShader = compile ps_3_0 MergeSubstractFunc();
	}
	
	pass FadeMerge
	{
		VertexShader = null;
		PixelShader = compile ps_3_0 FadeMergeFunc();
	}
}
