//--------------------------------------------------------------------------------------
// 
// WPF ShaderEffect HLSL -- ToneMappingEffect
//
//--------------------------------------------------------------------------------------

//-----------------------------------------------------------------------------------------
// Shader constant register mappings (scalars - float, double, Point, Color, Point3D, etc.)
//-----------------------------------------------------------------------------------------

float Exposure;
float Defog;
float Gamma;
float4 FogColor;
float VignetteRadius;
float2 VignetteCenter;
float BlueShift;

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

float4 ToneMappingFunc(float2 uv : TEXCOORD) : COLOR
{
	float4 c = tex2D(SourceTextureSampler, uv);
	c.rgb = max(0.0f, c.rgb - Defog * FogColor.rgb);
	c.rgb *= pow(2.0f, Exposure);
	c.rgb = pow(c.rgb, Gamma);

	float2 tc = uv - VignetteCenter;
	float v = 1.0f - dot(tc, tc);
	c.rgb += pow(v, 4) * VignetteRadius;

	float3 d = c.rgb * float3(1.05f, 0.97f, 1.27f);
	c.rgb = lerp(c.rgb, d, BlueShift);

	return (c);
}


technique ToneMapping
{
	pass ToneMapping
	{
		VertexShader = null;
		PixelShader = compile ps_3_0 ToneMappingFunc();
	}
}
