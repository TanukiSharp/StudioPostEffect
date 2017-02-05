//--------------------------------------------------------------------------------------
// 
// WPF ShaderEffect HLSL -- ToonEffect
//
//--------------------------------------------------------------------------------------

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

float4 ToonShadingFunc(float2 uv : TEXCOORD) : COLOR
{
	float4 color = tex2D(SourceTextureSampler, uv);

	color *= 3.0f;
	color = floor(color);
	color /= 3.0f;

	return (color);
}


technique ToonShading
{
	pass ToonShading
	{
		VertexShader = null;
		PixelShader = compile ps_3_0 ToonShadingFunc();
	}
}
