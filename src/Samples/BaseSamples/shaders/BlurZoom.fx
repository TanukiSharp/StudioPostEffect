float2 Center = { 0.5f, 0.5f };
float2 ScaleFactor = { -0.01f, -0.01f };

static const int NUM_SAMPLES = 12;

texture SourceTexture;
sampler SourceTextureSampler = sampler_state
{
	Texture = <SourceTexture>;
};


float4 BlurZoomFunc(float2 tex : TEXCOORD) : COLOR
{
	float2 dUV = tex - Center.xy; // uv - uvcenter
	float2 dSUV = dUV * ScaleFactor.xy;

	float4 BlurColor = tex2D(SourceTextureSampler, tex);
	for (int i = 1; i < NUM_SAMPLES; i += 1)
	{
		float4 Tap = tex2D(SourceTextureSampler, tex + dSUV * i);
		BlurColor.rgb += Tap.rgb;
	}
	BlurColor.rgb = BlurColor.rgb / NUM_SAMPLES;

	return (BlurColor);
}


technique BlurZoom
{
	pass BlurZoom
	{
		VertexShader = null;
		PixelShader = compile ps_3_0 BlurZoomFunc();
	}
}
