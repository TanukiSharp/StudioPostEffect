//--------------------------------------------------------------------------------------
// 
// WPF ShaderEffect HLSL -- RippleEffect
//
//--------------------------------------------------------------------------------------

//-----------------------------------------------------------------------------------------
// Shader constant register mappings (scalars - float, double, Point, Color, Point3D, etc.)
//-----------------------------------------------------------------------------------------

float2 Center;
float Amplitude;
float Frequency;
float Phase;

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

float4 RippleFunc(float2 uv : TEXCOORD) : COLOR
{
	float2 dir = uv - Center;

	float2 toPixel = uv - Center; // vector from center to pixel
	float distance = length(toPixel);
	float2 direction = toPixel / distance;
	float angle = atan2(direction.y, direction.x);
	float2 wave;
	sincos(Frequency * distance + Phase, wave.x, wave.y);

	float falloff = saturate(1.0f - distance);
	falloff *= falloff;

	distance += Amplitude * wave.x * falloff;
	sincos(angle, direction.y, direction.x);
	float2 uv2 = Center + distance * direction;

	float lighting = saturate(wave.y * falloff) * 0.2f + 0.8f;

	float4 color = tex2D(SourceTextureSampler, uv2);
	color.rgb *= lighting;

	return (color);
}


technique Ripple
{
	pass Ripple
	{
		VertexShader = null;
		PixelShader = compile ps_3_0 RippleFunc();
	}
}
