// global variable (uniform)
//--------------------------

float4x4	SCREEN_MAT;
float		STRENGTH;
float		SHIFT;
float		ANGLE;

//--------------------------




// texture samplers
//--------------------------

texture SrcMap;
sampler SrcSamp = sampler_state
{
	Texture = <SrcMap>;
};

texture LastMap;
sampler LastSamp = sampler_state
{
	Texture = <LastMap>;
};

//--------------------------





// program input/output structures
//--------------------------

struct VS_INPUT
{
	float4 pos		: POSITION;
	float2 tex		: TEXCOORD0;
};

struct VS_OUTPUT
{
	float4 pos		: POSITION;
	float2 tex		: TEXCOORD0;
	float2 tex_merge	: TEXCOORD1;
};

//--------------------------









// programs
//--------------------------

float2 change_tex_to_axis(float2 tex)
{
	return ((tex * 2.0f) - 1.0f);
}

float2 change_axis_to_tex(float2 axis)
{
	return ((axis + 1.0f) / 2.0f);
}

float2 rotate(float angle, float2 coord)
{
	float cosLength, sinLength;
	sincos(angle, sinLength, cosLength);

	float rotx = cosLength * coord.x - sinLength * coord.y;
	float roty = sinLength * coord.x + cosLength * coord.y;

	return (float2(rotx, roty));
}


VS_OUTPUT vs_passthrough(VS_INPUT vtx_in)
{
	VS_OUTPUT vtx_out;

	vtx_out.pos = mul(vtx_in.pos, SCREEN_MAT);
	vtx_out.tex = vtx_in.tex;
	vtx_out.tex_merge = vtx_in.tex;

	return (vtx_out);
}

VS_OUTPUT vs_zoom(VS_INPUT vtx_in)
{
	VS_OUTPUT vtx_out;

	float2 tex = vtx_in.tex;
	float2 axis = change_tex_to_axis(tex);
	axis *= 1.0f + (SHIFT / 100.0f);
	tex = change_axis_to_tex(axis);

	vtx_out.pos = mul(vtx_in.pos, SCREEN_MAT);
	vtx_out.tex = vtx_in.tex;
	vtx_out.tex_merge = tex;
	
	return (vtx_out);
}

VS_OUTPUT vs_rotation(VS_INPUT vtx_in)
{
	VS_OUTPUT vtx_out;

	float2 axis = change_tex_to_axis(vtx_in.tex);
	axis = rotate(ANGLE / 10.0f, axis);
	axis *= 1.0f + (SHIFT / 100.0f);
	float2 tex = change_axis_to_tex(axis);

	vtx_out.pos = mul(vtx_in.pos, SCREEN_MAT);
	vtx_out.tex = vtx_in.tex;
	vtx_out.tex_merge = tex;

	return (vtx_out);
}



float4 merge(VS_OUTPUT frg_in) : COLOR
{
	float4 last = tex2D(LastSamp, frg_in.tex_merge);
	float4 cur = tex2D(SrcSamp, frg_in.tex);

	//float k = saturate(STRENGTH) - 0.03f;
	float k = STRENGTH;
	return (last * k + cur * (1.0f - k));
}

//--------------------------





// techniques
//--------------------------

technique MotionBlurZoom
{
	pass MotionBlurZoom
	{
		VertexShader = compile vs_2_0 vs_zoom();
		PixelShader = compile ps_2_0 merge();
	}
}

technique MotionBlurRotation
{
	pass MotionBlurRotation
	{
		VertexShader = compile vs_2_0 vs_rotation();
		PixelShader = compile ps_2_0 merge();
	}
}

technique MotionBlurNormal
{
	pass MotionBlurNormal
	{
		VertexShader = compile vs_2_0 vs_passthrough();
		PixelShader = compile ps_2_0 merge();
	}
}

//--------------------------
