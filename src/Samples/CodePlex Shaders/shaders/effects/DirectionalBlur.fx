//--------------------------------------------------------------------------------------
// 
// WPF ShaderEffect HLSL -- DirectionalBlurEffect
//
//--------------------------------------------------------------------------------------

//-----------------------------------------------------------------------------------------
// Shader constant register mappings (scalars - float, double, Point, Color, Point3D, etc.)
//-----------------------------------------------------------------------------------------

float Angle;
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

float4 DirectionalBlurFunc(float2 uv : TEXCOORD) : COLOR
{
	float4 c = 0.0f;
	float rad = Angle * 0.0174533f;
	float xOffset = cos(rad);
	float yOffset = sin(rad);

	for (int i = 0; i < 16; i++)
	{
		uv.x = uv.x - BlurAmount * xOffset;
		uv.y = uv.y - BlurAmount * yOffset;
		c += tex2D(SourceTextureSampler, uv);
	}
	c /= 16;

	return c;
}


technique DirectionalBlur
{
	pass DirectionalBlur
	{
		VertexShader = null;
		PixelShader = compile ps_3_0 DirectionalBlurFunc();
	}
}
