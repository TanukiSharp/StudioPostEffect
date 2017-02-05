//--------------------------------------------------------------------------------------
// 
// WPF ShaderEffect HLSL -- EmbossedEffect
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

float4 EmbossedFunc(float2 uv : TEXCOORD) : COLOR
{
	float4 outC = { 0.5f, 0.5f, 0.5f, 1.0f };

	outC -= tex2D(SourceTextureSampler, uv - Width) * Amount;
	outC += tex2D(SourceTextureSampler, uv + Width) * Amount;
	outC.rgb = (outC.r + outC.g + outC.b) / 3.0f;

	return outC;
}


technique Embossed
{
	pass Embossed
	{
		VertexShader = null;
		PixelShader = compile ps_3_0 EmbossedFunc();
	}
}
