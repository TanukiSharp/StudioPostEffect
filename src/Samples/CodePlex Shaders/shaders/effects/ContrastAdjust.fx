//--------------------------------------------------------------------------------------
// 
// WPF ShaderEffect HLSL -- ContrastAdjustEffect
//
//--------------------------------------------------------------------------------------

//-----------------------------------------------------------------------------------------
// Shader constant register mappings (scalars - float, double, Point, Color, Point3D, etc.)
//-----------------------------------------------------------------------------------------

float Brightness;
float Contrast;

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

float4 ContrastAdjustFunc(float2 uv : TEXCOORD) : COLOR
{
	float4 pixelColor = tex2D(SourceTextureSampler, uv);

	//contrast
	pixelColor.rgb = ((pixelColor.rgb - 0.5f) * max(Contrast, 0)) + 0.5f;

	//brightness
	pixelColor.rgb = pixelColor.rgb + Brightness;

	// return final pixel color
	return pixelColor;
}


technique ContrastAdjust
{
	pass ContrastAdjust
	{
		VertexShader = null;
		PixelShader = compile ps_3_0 ContrastAdjustFunc();
	}
}
