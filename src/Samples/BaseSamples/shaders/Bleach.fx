float OVERLAY_ADJUSTMENT = 0.5f;
float DARK_ADJUSTMENT = 0.0f;
float BRIGHT_ADJUSTMENT = 1.0f;


texture SourceTexture;
sampler SrcSamp = sampler_state
{
	Texture = <SourceTexture>;
};

float OverlayFunc(float color, float gray)
{
	float result = 0.0f;

	if (color >= OVERLAY_ADJUSTMENT)
	{
		float value = (1.0f - color) / OVERLAY_ADJUSTMENT;
		float min = color - (1.0f - color);

		result = (gray * value) + min;
	}
	else
	{
		float value = color / OVERLAY_ADJUSTMENT;
		result = gray * value;
	}

	return (result);
}

float4 BleachFunc(float2 tex : TEXCOORD) : COLOR
{
	float4 color = tex2D(SrcSamp, tex);
	float gray = (color.x * 0.299f + color.y * 0.587f + color.z * 0.114f);

	gray = (gray * (BRIGHT_ADJUSTMENT - DARK_ADJUSTMENT)) + DARK_ADJUSTMENT;

	color.a = color.a;
	color.r = OverlayFunc(color.r, gray);
	color.g = OverlayFunc(color.g, gray);
	color.b = OverlayFunc(color.b, gray);

    return (color);
}

technique BleachBypass
{
	pass BleachBypass
	{
		VertexShader = null;
		PixelShader = compile ps_3_0 BleachFunc();
	}
}
