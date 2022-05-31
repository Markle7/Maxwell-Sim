#if OPENGL
#define SV_POSITION POSITION
#define VS_SHADERMODEL vs_3_0
#define PS_SHADERMODEL ps_3_0
#else
#define VS_SHADERMODEL vs_5_0
#define PS_SHADERMODEL ps_5_0
#endif


uniform float4 constants;
uniform float4x4 rotation;
uniform float4x4 translation;
uniform float charge;
uniform float velocity;
uniform float angle;

uniform float3 positions[128];

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

float4 MainPS(VertexShaderOutput input) : COLOR
{
	float4 color = float4(0,0,0,0);

	float3 r = float3(input.TextureCoordinates.x, input.TextureCoordinates.y, 0);
	float c = constants.y;
	
	for (int k = 0; k < 128; ++k)
	{
		float3 position = mul(mul(float4(positions[k].xyz, 1), translation), rotation);

		float3 up = float3(0, 0, -1);
		float3 mid = float3(0.707106*sin(angle), -0.707106*cos(angle), -0.707106);
		float3 side = float3(sin(angle), -cos(angle), 0);

		float pS = 0.5*constants.z;
		float vS = velocity;

		float3 rrp1 = r - (position + pS*up);
		float comDiv = 1 / length(rrp1);
		float3 J = charge * (vS*side);
		float3 dJ = charge * (vS*side - vS * float3(mid.xy, -mid.z)) / (constants.x*constants.y);
		float3 resultField = cross(J * comDiv*comDiv*comDiv, rrp1) + cross(dJ*comDiv*comDiv, rrp1);
		color.xyz +=  resultField;

		rrp1 = r - (position + pS * side);
		comDiv = 1 / length(rrp1);
		J = charge * (-vS*up);
		dJ = charge * (-vS*up - vS * float3(-mid.xy, -mid.z)) / (constants.x*constants.y);
		resultField = cross(J * comDiv*comDiv*comDiv, rrp1) + cross(dJ*comDiv*comDiv, rrp1);
		color.xyz += resultField;

		rrp1 = r - (position - pS * up);
		comDiv = 1 / length(rrp1);
		J = charge * (-vS*side);
		dJ = charge * (-vS * side - vS * float3(-mid.xy, mid.z)) / (constants.x*constants.y);
		resultField = cross(J * comDiv*comDiv*comDiv, rrp1) + cross(dJ*comDiv*comDiv, rrp1);
		color.xyz += resultField;

		rrp1 = r - (position - pS * side);
		comDiv = 1 / length(rrp1);
		J = charge * (vS * up);
		dJ = charge * (vS * up - vS * mid) / (constants.x*constants.y);
		resultField = cross(J * comDiv*comDiv*comDiv, rrp1) + cross(dJ*comDiv*comDiv, rrp1);
		color.xyz += resultField;

	}
	

	return color;

}

technique mField
{
	pass P0
	{
		PixelShader = compile PS_SHADERMODEL MainPS();
	}
};