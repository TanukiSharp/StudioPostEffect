//--------------------------------------------------------------------------------------
// 
// WPF ShaderEffect HLSL -- PixelateEffect
//
//--------------------------------------------------------------------------------------

//-----------------------------------------------------------------------------------------
// Shader constant register mappings (scalars - float, double, Point, Color, Point3D, etc.)
//-----------------------------------------------------------------------------------------

float HorizontalPixelCounts;
float VerticalPixelCounts;

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

float4 PixelateFunc(float2 uv : TEXCOORD) : COLOR
{
	float2 brickCounts = { HorizontalPixelCounts, VerticalPixelCounts };
	float2 brickSize = 1.0f / brickCounts;

	// Offset every other row of bricks
	float2 offsetuv = uv;
	bool oddRow = floor(offsetuv.y / brickSize.y) % 2.0f >= 1.0f;
	if (oddRow)
	{
		offsetuv.x += brickSize.x / 2.0f;
	}

	float2 brickNum = floor(offsetuv / brickSize);
	float2 centerOfBrick = brickNum * brickSize + brickSize / 2.0f;
	float4 color = tex2D(SourceTextureSampler, centerOfBrick);

	return (color);
}


technique Pixelate
{
	pass Pixelate
	{
		VertexShader = null;
		PixelShader = compile ps_3_0 PixelateFunc();
	}
}
