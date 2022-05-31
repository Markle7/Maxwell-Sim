#if OPENGL
#define SV_POSITION POSITION
#define VS_SHADERMODEL vs_3_0
#define PS_SHADERMODEL ps_3_0
#else
#define VS_SHADERMODEL vs_5_0
#define PS_SHADERMODEL ps_5_0
#endif

matrix WVP;

Texture2D rotationData;
SamplerState Sampler
{
	Texture = <rotationData>;
	Filter = Point;
};

struct VertexShaderInput
{
	float4 Position : POSITION0;
	float4 Color : COLOR0;
};

struct VertexShaderOutput
{
	float4 Position : SV_POSITION;
	float4 Color : COLOR0;
};

VertexShaderOutput MainVS(in VertexShaderInput input, float2 instanceT : POSITION1)
{
	VertexShaderOutput output = (VertexShaderOutput)0;


	float2 pos = instanceT * 0.004*0.5;
	float4 data = tex2Dlod(Sampler, float4(pos,0,0));

	float L = length(data.xyz);
	data.xyz = normalize(data.xyz) * 10 * atan(L);

	float allowed = length(input.Position.xyz) == 0 ? 0 : 1;

	float4 position = float4(allowed*data.xyz, 1);
	

	float4 space = mul(position, WVP);

	space.x += instanceT.x *0.004;
	space.y -= instanceT.y *0.004;
	
	output.Position = space;
	output.Color = input.Color;

	return output;
}

float4 MainPS(VertexShaderOutput input) : COLOR
{
	return float4(1,0,0,1);
}

technique Instancing
{
	pass P0
	{
		VertexShader = compile VS_SHADERMODEL MainVS();
		PixelShader = compile PS_SHADERMODEL MainPS();
	}
};