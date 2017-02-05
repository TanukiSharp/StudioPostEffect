float2 PIXEL_SIZE;
float4 BorderLineColor;

texture SourceTexture;
sampler SourceTextureSampler = sampler_state
{
	Texture = <SourceTexture>;
};


static const int GX[3][3] =
{
    { -1, +0, +1 },
    { -2, +0, +2 },
    { -1, +0, +1 },
};

static const int GY[3][3] =
{
    { +1, +2, +1 },
    { +0, +0, +0 },
    { -1, -2, -1 },
};


float ComputeGrayscaleFunc(float4 color)
{
	return (color.x * 0.3f + color.y * 0.59f + color.z * 0.11f);
}

float4 EdgedetectGrayscaleFunc(float2 tex : TEXCOORD) : COLOR
{
	float sumX = 0.0f;
	float sumY = 0.0f;

	for (int i = -1; i <= 1; i++)
	{
		for (int j = -1; j <= 1; j++)
		{
			float2 ntex = float2(i * PIXEL_SIZE.x, j * PIXEL_SIZE.y);

			float val = ComputeGrayscaleFunc(tex2D(SourceTextureSampler, tex + ntex));
			sumX += val * GX[i + 1][j + 1];
			sumY += val * GY[i + 1][j + 1];
		}
	}

	return (1.0f - (saturate(abs(sumX) + abs(sumY)) * (1.0f - BorderLineColor)));
}


technique EdgeDetect
{
	pass EdgeDetect
	{
		VertexShader = null;
		PixelShader = compile ps_3_0 EdgedetectGrayscaleFunc();
	}
}
