//--------------------------------------------------------------------------------------
// 
// WPF ShaderEffect HLSL -- SharpenEffect
//
//--------------------------------------------------------------------------------------

//-----------------------------------------------------------------------------------------
// Shader constant register mappings (scalars - float, double, Point, Color, Point3D, etc.)
//-----------------------------------------------------------------------------------------

float Amount;
float Width;

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

float4 SharpenFunc(float2 uv : TEXCOORD) : COLOR
{
	float4 color = tex2D(SourceTextureSampler, uv);
	color.rgb += tex2D(SourceTextureSampler, uv - Width) * Amount;
	color.rgb -= tex2D(SourceTextureSampler, uv + Width) * Amount;
	return (color);
}


technique Sharpen
{
	pass Sharpen
	{
		VertexShader = null;
		PixelShader = compile ps_3_0 SharpenFunc();
	}
}
