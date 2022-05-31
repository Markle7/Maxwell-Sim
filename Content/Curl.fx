#if OPENGL
	#define SV_POSITION POSITION
	#define VS_SHADERMODEL vs_3_0
	#define PS_SHADERMODEL ps_3_0
#else
	#define VS_SHADERMODEL vs_4_0_level_9_1
	#define PS_SHADERMODEL ps_4_0_level_9_1
#endif

Texture2D SpriteTexture;

sampler2D SpriteTextureSampler = sampler_state
{
	Texture = <SpriteTexture>;
	Filter = Point;
};

struct VertexShaderOutput
{
	float4 Position : SV_POSITION;
	float4 Color : COLOR0;
	float2 TextureCoordinates : TEXCOORD0;
};

float4 MainPS(VertexShaderOutput input) : COLOR
{
	float2 offsetX = float2(0.002,0);
	float2 offsetY = float2(0,0.002);
	float4 center = tex2D(SpriteTextureSampler,input.TextureCoordinates);
	float4 right = tex2D(SpriteTextureSampler, input.TextureCoordinates+offsetX);
	float4 down = tex2D(SpriteTextureSampler, input.TextureCoordinates+offsetY);

	float curl = 5*((right.g - center.g) - (down.r - center.r));

	return float4(curl,curl,curl,0);


}

technique SpriteDrawing
{
	pass P0
	{
		PixelShader = compile PS_SHADERMODEL MainPS();
	}
};