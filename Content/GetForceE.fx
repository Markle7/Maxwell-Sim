#if OPENGL
	#define SV_POSITION POSITION
	#define VS_SHADERMODEL vs_3_0
	#define PS_SHADERMODEL ps_3_0
#else
#define VS_SHADERMODEL vs_5_0
#define PS_SHADERMODEL ps_5_0
#endif

Texture2D OwnFieldE;

sampler2D OwnFieldESampler = sampler_state
{
	Texture = <OwnFieldE>;
	Filter = Point;
};

Texture2D OwnFieldM;

sampler2D OwnFieldMSampler = sampler_state
{
	Texture = <OwnFieldM>;
	Filter = Point;
};

Texture2D TotalFieldE;

sampler2D TotalFieldESampler = sampler_state
{
	Texture = <TotalFieldE>;
	Filter = Point;
};

Texture2D TotalFieldM;

sampler2D TotalFieldMSampler = sampler_state
{
	Texture = <TotalFieldM>;
	Filter = Point;
};

float3 position[128];
float3 velocity;
float charge;
struct VertexShaderOutput
{
	float4 Position : SV_POSITION;
	float4 Color : COLOR0;
	float2 TextureCoordinates : TEXCOORD0;
};

float4 Particle(VertexShaderOutput input) : COLOR
{
	float4 ownE = tex2D(OwnFieldESampler, position[0].xy);
	float4 ownM = tex2D(OwnFieldMSampler, position[0].xy);
	float4 totalE = tex2D(TotalFieldESampler, position[0].xy);
	float4 totalM = tex2D(TotalFieldMSampler, position[0].xy);

	float3 force = (totalE.xyz - ownE.xyz) + cross(velocity, totalM.xyz - ownM.xyz);
	force *= charge;

	return float4(force,0);
}

float4 Magnet(VertexShaderOutput input) : COLOR
{
	float3 moment = float3(cos(velocity.x),-sin(velocity.x),0);
	float3 total = float3(0, 0, 0);
	for (int i = 0; i < 128; ++i)
	{
		float4 ownM = tex2D(OwnFieldMSampler, position[i].xy);
		float4 totalM = tex2D(TotalFieldMSampler, position[i].xy);

		float3 force = cross(moment, totalM.xyz - ownM.xyz);
		force *= charge;
		total += force;
	}

	return float4(total, 0);
}

technique Particle
{
	pass P0
	{
		PixelShader = compile PS_SHADERMODEL Particle();
	}
};
technique Magnet
{
	pass P0
	{
		PixelShader = compile PS_SHADERMODEL Magnet();
	}
};