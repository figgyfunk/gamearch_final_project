// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'
// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Unlit/voronoiShader"
{
	Properties{
		_MainTex("Texture", 2D) = "white" {}
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
		float2 worldPos : TEXCOORD0;
	};

	struct vertOutput {
		float4 pos : POSITION;
		float2 worldPos : TEXCOORD0;
	};

	vertOutput vert(vertInput input) {
		vertOutput o;
		o.pos = UnityObjectToClipPos(input.pos);
		o.worldPos = input.worldPos;
		return o;
	}

	uniform int _Points_Length;
	uniform float _Dist;
	uniform float4 _Points[1000]; // (x, y, z) = position
	uniform float4 _Colors[1000];
	uniform float4 _Gap_Color; 
	uniform float _Gap;

	sampler2D _MainTex;

	float4 frag(vertOutput output) : COLOR{
	half minDist = 10000; // (Infinity)
	half minDist2 = 10000;
	int minI = 0;
	for (int i = 0; i < _Points_Length; i++)
	{
		half dist = distance(output.pos.xy, _Points[i].xy);

		if (dist < minDist)
		{
			minDist2 = minDist;
			minDist = dist;
			
			minI = i;
		}
	}
	
	if(abs(minDist - minDist2) <= _Gap)
	{
		return _Gap_Color; 
	}
	else
	{
		return float4(_Colors[minI]);
	}
	//return (float4(1., 0., 0., 1.));
	}
		ENDCG
	}
	}
}