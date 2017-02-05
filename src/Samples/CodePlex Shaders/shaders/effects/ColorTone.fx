//--------------------------------------------------------------------------------------
// 
// WPF ShaderEffect HLSL -- ColorToneEffect
//
//--------------------------------------------------------------------------------------

//-----------------------------------------------------------------------------------------
// Shader constant register mappings (scalars - float, double, Point, Color, Point3D, etc.)
//-----------------------------------------------------------------------------------------

float Desaturation;
float Toned;
float4 LightColor;
float4 DarkColor;

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

float4 ColorToneFunc(float2 uv : TEXCOORD) : COLOR
{
	float4 scnColor = LightColor * tex2D(SourceTextureSampler, uv);
	float gray = dot(float3(0.3, 0.59, 0.11), scnColor.rgb);

	float3 muted = lerp(scnColor.rgb, gray.xxx, Desaturation);
	float3 middle = lerp(DarkColor, LightColor, gray);

	scnColor.rgb = lerp(muted, middle, Toned);
	return scnColor;
}


technique ColorTone
{
	pass ColorTone
	{
		VertexShader = null;
		PixelShader = compile ps_3_0 ColorToneFunc();
	}
}
