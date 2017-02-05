//--------------------------------------------------------------------------------------
// 
// WPF ShaderEffect HLSL -- LightStreakEffect
//
//--------------------------------------------------------------------------------------

//-----------------------------------------------------------------------------------------
// Shader constant register mappings (scalars - float, double, Point, Color, Point3D, etc.)
//-----------------------------------------------------------------------------------------

float BrightThreshold;
float Scale;
float Attenuation;

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

float4 LightStreakFunc(float2 uv : TEXCOORD) : COLOR
{
	float2 Direction = { 1.0f, 1.0f };
	float2 PixelSize = { 0.0009766f, 0.0013021f };
	const static int numSamples = 2;
	int Iteration = 1;

	float4 pixelColor = tex2D(SourceTextureSampler, uv);
	float4 bright = saturate((pixelColor - BrightThreshold) / (1.0f - BrightThreshold));

	pixelColor += bright;

	float weightIter = pow(numSamples, Iteration);

	for (int sample = 0; sample < numSamples; sample++)
	{
		float weight = pow(Attenuation, weightIter * sample);
		float2 texCoord = uv + (Direction * weightIter * float2(sample, sample) * PixelSize);
		pixelColor.rgb += saturate(weight) * tex2D(SourceTextureSampler, texCoord);
	}

	return pixelColor * Scale;
}


technique LightStreak
{
	pass LightStreak
	{
		VertexShader = null;
		PixelShader = compile ps_3_0 LightStreakFunc();
	}
}
