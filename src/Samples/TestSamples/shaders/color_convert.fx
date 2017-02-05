int COLORCONV_BOUNDS;
int COLORCONV_SEQUENCE;
float4 COLORCONV_OFFSET;
float4 COLORCONV_AMPLI;


texture SrcMap;
sampler SrcSamp = sampler_state
{
	Texture = <SrcMap>;
};



float RollingModuloColorFunc(float color)
{
	if (color > 1.0f)
		return (color % 1.0f);

	if (color < 0.0f)
		return (1.0f + (color % 1.0f)); // == 1 - abs(color % 1)

	return (color);
}

float4 ColorConvertFunc(float2 tex : TEXCOORD) : COLOR
{
	float4 color = tex2D(SrcSamp, tex);

	//if (COLORCONV_SEQUENCE == 0) // offset first, ampli then
	//{
		color += COLORCONV_OFFSET;
		color *= COLORCONV_AMPLI;
	//}
	//else if (COLORCONV_SEQUENCE == 1) // ampli first, offset then
	//{
	//	color *= COLORCONV_AMPLI;
	//	color += COLORCONV_OFFSET;
	//}
	// other kind of sequence

	//if (COLORCONV_BOUNDS == 0) // saturate
	//{
		color = saturate(color);
	//}
	//else if (COLORCONV_BOUNDS == 1) // modulo
	//{
	//	color.x = RollingModuloColorFunc(color.x);
	//	color.y = RollingModuloColorFunc(color.y);
	//	color.z = RollingModuloColorFunc(color.z);
	//	color.w = RollingModuloColorFunc(color.w);
	//}
	// other kind of bound management

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
