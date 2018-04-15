// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'
// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Unlit/voronoiShader"
{
	Properties{
		_VoroTex("Texture", 2D) = "white" {}
	}
		SubShader{
		Tags{ "Queue" = "Transparent" }
		Blend SrcAlpha OneMinusSrcAlpha // Alpha blend

		Pass{
		CGPROGRAM
#pragma vertex vert             
#pragma fragment frag

		struct vertInput {
		float4 pos : POSITION;
	};

	struct vertOutput {
		float4 pos : POSITION;
		fixed3 worldPos : TEXCOORD1;
	};

	vertOutput vert(vertInput input) {
		vertOutput o;
		o.pos = UnityObjectToClipPos(input.pos);
		o.worldPos = mul(unity_ObjectToWorld, input.pos).xyz;
		return o;
	}

	uniform int _Points_Length = 0;
	uniform float3 _Points[100]; // (x, y, z) = position
	uniform fixed3 _Colors[100];

	sampler2D _VoroTex;

	fixed4 frag(vertOutput output) : COLOR
	{
	half minDist = 10000; // (Infinity)
	int minI = 0;
	for (int i = 0; i < _Points_Length; i++)
	{
		half dist = distance(output.worldPos.xy, _Points[i].xy);
		if (dist < minDist)
		{
			minDist = dist;
			minI = i;
		}
	}
		return fixed4(_Colors[minI], 1);
	}
		ENDCG
	}
	}
}