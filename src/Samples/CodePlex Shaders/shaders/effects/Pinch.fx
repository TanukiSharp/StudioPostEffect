//--------------------------------------------------------------------------------------
// 
// WPF ShaderEffect HLSL -- PinchEffect
//
//--------------------------------------------------------------------------------------

//-----------------------------------------------------------------------------------------
// Shader constant register mappings (scalars - float, double, Point, Color, Point3D, etc.)
//-----------------------------------------------------------------------------------------

float2 Center;
float Radius;
float Amount;

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

float4 PinchFunc(float2 uv : TEXCOORD) : COLOR
{
	float2 displace = Center - uv;
	float range = saturate(1.0f - (length(displace) / (abs(-sin(Radius * 8.0f) * Radius) + 0.00000001f)));
	return (tex2D(SourceTextureSampler, uv + displace * range * Amount));
}


technique Pinch
{
	pass Pinch
	{
		VertexShader = null;
		PixelShader = compile ps_3_0 PinchFunc();
	}
}
