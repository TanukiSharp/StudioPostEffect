//--------------------------------------------------------------------------------------
// 
// WPF ShaderEffect HLSL -- ZoomBlurEffect
//
//--------------------------------------------------------------------------------------

//-----------------------------------------------------------------------------------------
// Shader constant register mappings (scalars - float, double, Point, Color, Point3D, etc.)
//-----------------------------------------------------------------------------------------

float Center;
float BlurAmount;

//--------------------------------------------------------------------------------------
// Sampler Inputs (Brushes, including ImplicitInput)
//--------------------------------------------------------------------------------------

texture SourceTexture;
sampler SourceTextureSampler = sampler_state
{
	Texture = <SourceTexture>;
};

//--------------------------------------------------------------------------------------
// Pixel Shader
//--------------------------------------------------------------------------------------

float4 ZoomBlurFunc(float2 uv : TEXCOORD) : COLOR
{
	float4 c = 0.0f;
	uv -= Center;

	for (int i = 0; i < 15; i++)
	{
		float scale = 1.0f + BlurAmount * (i / 14.0f);
		c += tex2D(SourceTextureSampler, uv * scale + Center);
	}

	c /= 15.0f;
	return (c);
}


technique ZoomBlur
{
	pass ZoomBlur
	{
		VertexShader = null;
		PixelShader = compile ps_3_0 ZoomBlurFunc();
	}
}
