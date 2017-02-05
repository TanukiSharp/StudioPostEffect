bool Switch;

texture Input1;
sampler InputSampler1 = sampler_state
{
	Texture = <Input1>;
};

texture Input2;
sampler InputSampler2 = sampler_state
{
	Texture = <Input2>;
};


float4 SwitchFunc(float2 uv : TEXCOORD) : COLOR
{
	if (Switch)
	{
		return (tex2D(InputSampler1, uv));
	}
	else
	{
		return (tex2D(InputSampler2, uv));
	}
}

technique Switching
{
	pass Switching
	{
		VertexShader = null;
		PixelShader = compile ps_3_0 SwitchFunc();
	}
}
