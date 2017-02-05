texture UpperLayer;
sampler UpperLayerSampler = sampler_state
{
	Texture = <UpperLayer>;
};

texture LowerLayer;
sampler LowerLayerSampler = sampler_state
{
	Texture = <LowerLayer>;
};



// Add
float4 AddFunc(float2 uv : TEXCOORD) : COLOR
{
	float4 lower = tex2D(LowerLayerSampler, uv);
	float4 upper = tex2D(UpperLayerSampler, uv);

	return (saturate(upper + lower));
}


// Subtract
float4 SubtractFunc(float2 uv : TEXCOORD) : COLOR
{
	float4 lower = tex2D(LowerLayerSampler, uv);
	float4 upper = tex2D(UpperLayerSampler, uv);

	return (saturate(lower - upper));
}


// Multiply
float4 MultiplyFunc(float2 uv : TEXCOORD) : COLOR
{
	float4 lower = tex2D(LowerLayerSampler, uv);
	float4 upper = tex2D(UpperLayerSampler, uv);

	return (saturate(upper * lower));
}


// Screen
float4 ScreenFunc(float2 uv : TEXCOORD) : COLOR
{
	float4 lower = tex2D(LowerLayerSampler, uv);
	float4 upper = tex2D(UpperLayerSampler, uv);

	return (1.0f - (1.0f - lower) * (1.0f - upper));
}


// Overlay per channel
float OverlayChannel(float upper, float lower)
{
	if (lower < 0.5f)
	{
		return (2.0f * lower * upper);
	}
	else
	{
		return (1.0f - 2.0f * (1.0f - lower) * (1.0f - upper));
	}
}

// Overlay
float4 OverlayFunc(float2 uv : TEXCOORD) : COLOR
{
	float4 lower = tex2D(LowerLayerSampler, uv);
	float4 upper = tex2D(UpperLayerSampler, uv);

	return (float4(
		OverlayChannel(upper.r, lower.r),
		OverlayChannel(upper.g, lower.g),
		OverlayChannel(upper.b, lower.b),
		1.0f));
}


// HardLight per channel
float HardLightChannel(float upper, float lower)
{
	if (upper < 0.5f)
	{
		return (2.0f * lower * upper);
	}
	else
	{
		return (1.0f - 2.0f * (1.0f - lower) * (1.0f - upper));
	}
}

// Hardlight
float4 HardLightFunc(float2 uv : TEXCOORD) : COLOR
{
	float4 lower = tex2D(LowerLayerSampler, uv);
	float4 upper = tex2D(UpperLayerSampler, uv);

	return (float4(
		HardLightChannel(upper.r, lower.r),
		HardLightChannel(upper.g, lower.g),
		HardLightChannel(upper.b, lower.b),
		1.0f));
}


// Softlight
float4 SoftLightFunc(float2 uv : TEXCOORD) : COLOR
{
	float4 lower = tex2D(LowerLayerSampler, uv);
	float4 upper = tex2D(UpperLayerSampler, uv);

	return ((1.0f - lower) * upper * lower + lower * (1.0f - (1.0f - lower) * (1.0f - upper)));
}


// VividLight per channel
float VividLightChannel(float upper, float lower)
{
	if (upper < 0.5f)
	{
		if (lower <= 1.0f - upper * 2.0f)
		{
			return (0.0f);
		}
		else
		{
			return (saturate((lower - 1.0f - upper * 2.0f) / upper * 2.0f));
		}
	}
	else
	{
		if (lower < 2.0f - upper * 2.0f)
		{
			return (saturate(lower / (2.0f - upper * 2.0f)));
		}
		else
		{
			return (1.0f);
		}
	}
}

// VividLight
float4 VividLightFunc(float2 uv : TEXCOORD) : COLOR
{
	float4 lower = tex2D(LowerLayerSampler, uv);
	float4 upper = tex2D(UpperLayerSampler, uv);

	return (float4(
		VividLightChannel(upper.r, lower.r),
		VividLightChannel(upper.g, lower.g),
		VividLightChannel(upper.b, lower.b),
		1.0f));
}


// LinearLight per channel
float LinearLightChannel(float upper, float lower)
{
	if (upper < 0.5f)
	{
		if (lower < 1.0f - upper * 2.0f)
		{
			return (0.0f);
		}
		else
		{
			return (upper * 2.0f + lower - 1.0f);
		}
	}
	else
	{
		if (lower < 2.0f - upper * 2.0f)
		{
			return (upper * 2.0f + lower - 1.0f);
		}
		else
		{
			return (1.0f);
		}
	}
}

// LinearLight
float4 LinearLightFunc(float2 uv : TEXCOORD) : COLOR
{
	float4 lower = tex2D(LowerLayerSampler, uv);
	float4 upper = tex2D(UpperLayerSampler, uv);

	return (float4(
		LinearLightChannel(upper.r, lower.r),
		LinearLightChannel(upper.g, lower.g),
		LinearLightChannel(upper.b, lower.b),
		1.0f));
}


// PinLight per channel
float PinLightChannel(float upper, float lower)
{
	if (upper < 0.5f)
	{
		float val = upper * 2.0f;
		if (val < lower)
		{
			return (val);
		}
		else
		{
			return (lower);
		}
	}
	else
	{
		float val = upper * 2.0f - 1.0f;
		if (val < lower)
		{
			return (lower);
		}
		else
		{
			return (val);
		}
	}
}

// PinLight
float4 PinLightFunc(float2 uv : TEXCOORD) : COLOR
{
	float4 lower = tex2D(LowerLayerSampler, uv);
	float4 upper = tex2D(UpperLayerSampler, uv);

	return (float4(
		PinLightChannel(upper.r, lower.r),
		PinLightChannel(upper.g, lower.g),
		PinLightChannel(upper.b, lower.b),
		1.0f));
}


// ColorDodge
float4 ColorDodgeFunc(float2 uv : TEXCOORD) : COLOR
{
	float4 lower = tex2D(LowerLayerSampler, uv);
	float4 upper = tex2D(UpperLayerSampler, uv);

	return (saturate((256.0f * lower) / (256.0f - 255.0f * upper)));
}


// LinearDodge
float4 LinearDodgeFunc(float2 uv : TEXCOORD) : COLOR
{
	float4 lower = tex2D(LowerLayerSampler, uv);
	float4 upper = tex2D(UpperLayerSampler, uv);

	return (saturate(lower + upper));
}


// ColorBurn
float4 ColorBurnFunc(float2 uv : TEXCOORD) : COLOR
{
	float4 lower = tex2D(LowerLayerSampler, uv);
	float4 upper = tex2D(UpperLayerSampler, uv);

	return (1.0f - (1.0f - lower) / upper);
}


// LinearBurn
float4 LinearBurnFunc(float2 uv : TEXCOORD) : COLOR
{
	float4 lower = tex2D(LowerLayerSampler, uv);
	float4 upper = tex2D(UpperLayerSampler, uv);

	return (lower + upper - 1.0f);
}


// Darken
float4 DarkenFunc(float2 uv : TEXCOORD) : COLOR
{
	float4 lower = tex2D(LowerLayerSampler, uv);
	float4 upper = tex2D(UpperLayerSampler, uv);

	return (min(lower, upper));
}


// Lighten
float4 LightenFunc(float2 uv : TEXCOORD) : COLOR
{
	float4 lower = tex2D(LowerLayerSampler, uv);
	float4 upper = tex2D(UpperLayerSampler, uv);

	return (max(lower, upper));
}


// Difference
float4 DifferenceFunc(float2 uv : TEXCOORD) : COLOR
{
	float4 lower = tex2D(LowerLayerSampler, uv);
	float4 upper = tex2D(UpperLayerSampler, uv);

	return (abs(lower - upper));
}


// Exclusion
float4 ExclusionFunc(float2 uv : TEXCOORD) : COLOR
{
	float4 lower = tex2D(LowerLayerSampler, uv);
	float4 upper = tex2D(UpperLayerSampler, uv);

	return (abs(2.0f * (0.5f - lower) * (0.5f - upper) - 0.5f));
}



technique PhotoShopFilters
{
	pass Add
	{
		VertexShader = null;
		PixelShader = compile ps_3_0 AddFunc();
	}
	
	pass Subtract
	{
		VertexShader = null;
		PixelShader = compile ps_3_0 SubtractFunc();
	}

	pass Multiply
	{
		VertexShader = null;
		PixelShader = compile ps_3_0 MultiplyFunc();
	}

	pass Screen
	{
		VertexShader = null;
		PixelShader = compile ps_3_0 ScreenFunc();
	}

	pass Overlay
	{
		VertexShader = null;
		PixelShader = compile ps_3_0 OverlayFunc();
	}

	pass HardLight
	{
		VertexShader = null;
		PixelShader = compile ps_3_0 HardLightFunc();
	}

	pass SoftLight
	{
		VertexShader = null;
		PixelShader = compile ps_3_0 SoftLightFunc();
	}

	pass VividLight
	{
		VertexShader = null;
		PixelShader = compile ps_3_0 VividLightFunc();
	}

	pass LinearLight
	{
		VertexShader = null;
		PixelShader = compile ps_3_0 LinearLightFunc();
	}

	pass PinLight
	{
		VertexShader = null;
		PixelShader = compile ps_3_0 PinLightFunc();
	}

	pass ColorDodge
	{
		VertexShader = null;
		PixelShader = compile ps_3_0 ColorDodgeFunc();
	}

	pass LinearDodge
	{
		VertexShader = null;
		PixelShader = compile ps_3_0 LinearDodgeFunc();
	}

	pass ColorBurn
	{
		VertexShader = null;
		PixelShader = compile ps_3_0 ColorBurnFunc();
	}

	pass LinearBurn
	{
		VertexShader = null;
		PixelShader = compile ps_3_0 LinearBurnFunc();
	}

	pass Darken
	{
		VertexShader = null;
		PixelShader = compile ps_3_0 DarkenFunc();
	}

	pass Lighten
	{
		VertexShader = null;
		PixelShader = compile ps_3_0 LightenFunc();
	}

	pass Difference
	{
		VertexShader = null;
		PixelShader = compile ps_3_0 DifferenceFunc();
	}

	pass Exclusion
	{
		VertexShader = null;
		PixelShader = compile ps_3_0 ExclusionFunc();
	}
}
