texture SourceTexture;
sampler SourceTextureSampler = sampler_state
{
	Texture = <SourceTexture>;
};


float4 BlackAndWhiteFunc(float2 tex : TEXCOORD) : COLOR
{
	float4 texcolor = tex2D(SourceTextureSampler, tex);

	const float3 bw_const = {0.299f, 0.587f, 0.114f};
	float3 result = dot(texcolor.xyz, bw_const);
	return (float4(result, 1.0f));
}


technique BlackAndWhite
{
	pass BlackAndWhite
	{
		VertexShader = null;
		PixelShader = compile ps_3_0 BlackAndWhiteFunc();
	}
}
