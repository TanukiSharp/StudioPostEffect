bool Order;
float4 Offset;
float4 Ampli;


texture SrcMap;
sampler SrcSamp = sampler_state
{
	Texture = <SrcMap>;
};


float4 ColorConvertFunc(float2 tex : TEXCOORD) : COLOR
{
	float4 color = tex2D(SrcSamp, tex);

	if (Order) // offset first, ampli then
	{
		color += Offset;
		color *= Ampli;
	}
	else
	{
		color *= Ampli;
		color += Offset;
	}

	return (color);
}

technique ColorConvert
{
	pass ColorConvert
	{
		VertexShader = null;
		PixelShader = compile ps_3_0 ColorConvertFunc();
	}
}
