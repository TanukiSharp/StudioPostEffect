texture Input;
sampler InputSampler = sampler_state
{
	Texture = <Input>;
};


float Distortion;
float Strength;


static const float _samples[10] =
{
	-0.08f,
	-0.05f,
	-0.03f,
	-0.02f,
	-0.01f,
	0.01f,
	0.02f,
	0.03f,
	0.05f,
	0.08f
};


float4 RadialBlurFunc(float2 uv : TEXCOORD) : COLOR
{
	//Vector from pixel to the center of the screen
	float2 dir = 0.5f - uv;

	//Distance from pixel to the center (distant pixels have stronger effect)
	//float dist = distance( float2( 0.5, 0.5 ), texCoord );
	float dist = sqrt(dir.x * dir.x + dir.y * dir.y);

	//Now that we have dist, we can normlize vector
	dir = normalize(dir);

	//Save the color to be used later
	float4 color = tex2D(InputSampler, uv);

	//Average the pixels going along the vector
	float4 sum = color;
	for (int i = 0; i < 10; i++)
	{
		sum += tex2D(InputSampler, uv + dir * _samples[i] * Distortion);
	}
	sum /= 11.0f;

	//Calculate amount of blur based on
	//distance and a strength parameter
	float t = saturate(dist * Strength); //We need 0 <= t <= 1

	//Blend the original color with the averaged pixels
	return (lerp(color, sum, t));
}


technique OgreRadialBlur
{
	pass OgreRadialBlur
	{
		VertexShader = null;
		PixelShader = compile ps_3_0 RadialBlurFunc();
	}
}
