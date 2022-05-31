#if OPENGL
#define SV_POSITION POSITION
#define VS_SHADERMODEL vs_3_0
#define PS_SHADERMODEL ps_3_0
#else
#define VS_SHADERMODEL vs_5_0
#define PS_SHADERMODEL ps_5_0
#endif


uniform float2 constants;

uniform float3 positions[32];
uniform float3 velocities[32];
uniform float3 oldvelocities[32];
uniform float charge;

Texture2D SpriteTexture;
SamplerState SpriteTextureSampler
{
	Texture = <SpriteTexture>;
};


struct VertexShaderOutput
{
	float4 Position : SV_POSITION;
	float4 Color : COLOR0;
	float2 TextureCoordinates : TEXCOORD0;
};

float4 eMainPS(VertexShaderOutput input) : COLOR
{
	float4 color = float4(0,0,0,0);

	float3 r = float3(input.TextureCoordinates.x, input.TextureCoordinates.y, 0);
	float c = constants.y;

	for (int k = 0; k < 16; ++k)
	{
		float3 position = positions[k];
		float3 velocity = velocities[k];
		float3 oldVelocity = oldvelocities[k];

		float3 rrp1 = r - position;
		float R = 1 / length(rrp1);

		float tr = length(rrp1) / c;
		float index = floor(tr / constants.x);
		float influence = (k == 2 * index) ? 1 : 0;

		float3 n = normalize(rrp1);

		float3 J = charge * velocity;
		float3 dJ = charge * (velocity - oldVelocity) / (constants.x*R)
			; // +2 * charge * velocity * dot(rrp1, velocity);

		float3 resultField = charge * n*R*R + (1 / c)*(dot(J, n)*n + cross(cross(J, n), n) *R*R)
			+ 1 / (c*c) * (cross(cross(dJ, n), n)*R);

		color.xyz += influence * resultField;
	}

	return color;

}

float4 mMainPS(VertexShaderOutput input) : COLOR
{
	float4 color = float4(0,0,0,0);

	float3 r = float3(input.TextureCoordinates.x, input.TextureCoordinates.y, 0);
	float c = constants.y;

	for (int k = 0; k < 16; ++k)
	{
		float3 position = positions[k];
		float3 velocity = velocities[k];
		float3 oldVelocity = oldvelocities[k];

		float3 rrp1 = r - position;
		float comDiv = 1 / length(rrp1);

		float tr = length(rrp1) / c;
		float index = floor(tr / constants.x);
		float influence = (k == 2 * index) ? 1 : 0;

		float3 J = charge * velocity;
		float3 dJ = charge * (velocity - oldVelocity) / (constants.x*comDiv)
			+ 2 * charge * velocity * dot(rrp1, velocity);

		float3 resultField = cross(J * comDiv*comDiv*comDiv, rrp1) + cross(dJ*comDiv * comDiv / c, rrp1);

		color.xyz += influence * resultField;
	}

	return color;

}


technique eField
{
	pass P0
	{
		PixelShader = compile PS_SHADERMODEL eMainPS();
	}
};

technique mField
{
	pass P0
	{
		PixelShader = compile PS_SHADERMODEL mMainPS();
	}
};