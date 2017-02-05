float3 ConstColor;
float Scale;
float Interpolater0;
float Interpolater1;


texture Input;
sampler InputSampler = sampler_state
{
	Texture = <Input>;
};

float4 IllustrationFunc(float2 uv : TEXCOORD) : COLOR
{
	float3 color = tex2D(InputSampler, uv).rgb;

	float3 stage0 = dot(color, ConstColor) * Scale;
	float3 stage1 = lerp(stage0, 1.0f, Interpolater0);
	float3 stage2 = lerp(stage1, color, Interpolater1);
	stage2 *= stage2;

	return (float4(stage2, 1.0f));
}

technique Illustration
{
	pass Illustration
	{
		VertexShader = null;
		PixelShader = compile ps_3_0 IllustrationFunc();
	}
}
