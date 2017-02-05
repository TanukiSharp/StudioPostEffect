//--------------------------------------------------------------------------------------
// 
// WPF ShaderEffect HLSL -- MagnifyEffect
//
//--------------------------------------------------------------------------------------

//-----------------------------------------------------------------------------------------
// Shader constant register mappings (scalars - float, double, Point, Color, Point3D, etc.)
//-----------------------------------------------------------------------------------------

float2 Radius;
float2 Center;
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

float4 MagnifyFunc(float2 uv : TEXCOORD) : COLOR
{
	float2 ray = uv - Center;
	float2 rt = ray / Radius;

	// Outside of radii, we jus show the regular image.  Radii is ellipse radii, so width x height radius 
	float lengthRt = length(rt);

	float2 texuv;
	if (lengthRt > 1.0f)
	{
		texuv = uv;
	}
	else
	{
		texuv = Center + Amount * ray;
	}

	return (tex2D(SourceTextureSampler, texuv));
}

technique Magnify
{
	pass Magnify
	{
		VertexShader = null;
		PixelShader = compile ps_3_0 MagnifyFunc();
	}
}
