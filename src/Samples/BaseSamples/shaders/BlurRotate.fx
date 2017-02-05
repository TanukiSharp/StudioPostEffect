float2 Center;
float2 Offset;

texture Input;
sampler InputSampler = sampler_state
{
	Texture = <Input>;
};


float4 BlurRotateFunc(in float2 uv : TEXCOORD) : COLOR
{
	float2 duv = uv - Center.xy;

	float4 color = tex2D(InputSampler, uv);
	for (int i = 1; i < 5; i += 1)
	{
		duv += float2(duv.y, -1.0f * duv.x) * Offset.xy;
		color += tex2D(InputSampler, Center.xy + duv);
	}
	return (color / 5);
}


technique BlurRotate
{
	pass BlurRotate
	{
		VertexShader = null;
		PixelShader = compile ps_3_0 BlurRotateFunc();
	}
}
