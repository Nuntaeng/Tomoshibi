Shader "Mask/AlphaMask" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		[PerRendererData] _MainTex("mainTex", 2D) = "white" {}
		
	}
	SubShader {

	Tags{
		"RenderType" = "Transparent"
		"Queue" = "Transparent"
		"CanUseSpriteAtlas" = "True"

	}
	Blend SrcColor One
	BlendOp Add


		Pass{

		CGPROGRAM
#pragma vertex surf
#pragma fragment frag
#include "UnityCG.cginc"


		sampler2D _MainTex;

	struct Input {
		float4 vertex:POSITION;
		float4 color : COLOR;
		float4 texcoord:TEXCOORD0;
	};

	struct v2f {
		float4 pos:SV_POSITION;
		float4 color : COLOR;
		float2 uv1:TEXCOORD0;
	};

	float4 _MainTex_ST;
	float4 _Color;

	v2f surf(Input v) {
		v2f o;
		o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
		o.color = v.color;
		o.uv1 = TRANSFORM_TEX(v.texcoord, _MainTex);

		return o;
	}

	fixed4 frag(v2f i) : COLOR{
		fixed4 base = tex2D(_MainTex, i.uv1);
		fixed4 col = _Color * base * i.color;
		col.rgb *= i.color.a;


		return col;
	}

		ENDCG
		}
	}
		FallBack Off
}
