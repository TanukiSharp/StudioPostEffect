//--------------------------------------------------------------------------------------
// 
// WPF ShaderEffect HLSL -- SwirlEffect
//
//--------------------------------------------------------------------------------------

//-----------------------------------------------------------------------------------------
// Shader constant register mappings (scalars - float, double, Point, Color, Point3D, etc.)
//-----------------------------------------------------------------------------------------

float2 Center;
float SpiralStrength;
float2 AngleFrequency;

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

float4 SwirlFunc(float2 uv : TEXCOORD) : COLOR
{
	float2 dir = uv - Center;
	float len = length(dir);
	float angle = atan2(dir.y, dir.x);

	float newAng = angle + SpiralStrength * len;
	float xAmt = cos(AngleFrequency.x * newAng) * len;
	float yAmt = sin(AngleFrequency.y * newAng) * len;

	float2 newCoord = Center + float2(xAmt, yAmt);

	return (tex2D(SourceTextureSampler, newCoord));
}


technique Swirl
{
	pass Swirl
	{
		VertexShader = null;
		PixelShader = compile ps_3_0 SwirlFunc();
	}
}
