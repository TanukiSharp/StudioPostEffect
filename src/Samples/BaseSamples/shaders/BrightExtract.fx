//--------------------------------------------------------------------------------------
// 
// WPF ShaderEffect HLSL -- BrightExtractEffect
//
//--------------------------------------------------------------------------------------

//-----------------------------------------------------------------------------------------
// Shader constant register mappings (scalars - float, double, Point, Color, Point3D, etc.)
//-----------------------------------------------------------------------------------------

float Threshold;

//--------------------------------------------------------------------------------------
// Sampler Inputs (Brushes, including ImplicitInput)
//--------------------------------------------------------------------------------------

texture SourceTexure;
sampler SourceTextureSampler = sampler_state
{
	Texture = <SourceTexure>;
};


//--------------------------------------------------------------------------------------
// Pixel Shader
//--------------------------------------------------------------------------------------

float4 BrightExtractFunc(float2 uv : TEXCOORD) : COLOR
{
    // Look up the original image color.
    float4 c = tex2D(SourceTextureSampler, uv);

    // Adjust it to keep only values brighter than the specified threshold.
    return saturate((c - Threshold) / (1.0f - Threshold));
}


technique BrightExtract
{
	pass BrightExtract
	{
		VertexShader = null;
		PixelShader = compile ps_3_0 BrightExtractFunc();
	}
}
