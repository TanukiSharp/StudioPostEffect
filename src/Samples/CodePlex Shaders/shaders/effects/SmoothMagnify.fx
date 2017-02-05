//--------------------------------------------------------------------------------------
// 
// WPF ShaderEffect HLSL -- SmoothMagnifyEffect
//
//--------------------------------------------------------------------------------------

//-----------------------------------------------------------------------------------------
// Shader constant register mappings (scalars - float, double, Point, Color, Point3D, etc.)
//-----------------------------------------------------------------------------------------

float2 Center;
float InnerRadius;
float OuterRadius;
float Magnification;

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

float4 SmoothMagnifyFunc(float2 uv : TEXCOORD) : COLOR
{
	float2 center_to_pixel = uv - Center; // vector from center to pixel

	float distance = length(center_to_pixel);

	float4 color;

	float2 sample_point;

	if (distance < OuterRadius)
	{
		if (distance < InnerRadius)
		{
			sample_point = Center + (center_to_pixel / Magnification);
		}
		else
		{
			float radius_diff = OuterRadius - InnerRadius;
			float ratio = (distance - InnerRadius) / radius_diff; // 0 == inner radius, 1 == outer_radius
			ratio = ratio * 3.14159f; //  -pi/2 .. pi/2
			float adjusted_ratio = cos(ratio);  // -1 .. 1
			adjusted_ratio = adjusted_ratio + 1.0f;   // 0 .. 2
			adjusted_ratio = adjusted_ratio / 2.0f;   // 0 .. 1

			sample_point = ((Center + (center_to_pixel / Magnification)) * adjusted_ratio) + (uv * (1.0f - adjusted_ratio));
		}
	}
	else
	{
		sample_point = uv;
	}

	return (tex2D(SourceTextureSampler, sample_point));
}


technique SmoothMagnify
{
	pass SmoothMagnify
	{
		VertexShader = null;
		PixelShader = compile ps_3_0 SmoothMagnifyFunc();
	}
}
