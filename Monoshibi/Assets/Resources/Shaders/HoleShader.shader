Shader "Mask/HoleShader" {
	Properties{
		_Size("HoleSize", float) = 0.1
		_Blur("BlurPower", float) = 0.1
		_Pos("Position", Vector) = (0.5, 0.5, 0.5, 1.0)
		_Color("Color", Color) = (0.0, 0.0, 0.0, 1.0)
		_MainTex("Texture", 2D) = "white" {}
	}
		SubShader{

			Tags{
				"RenderType" = "Transparent"
				"Queue" = "Transparent"
			}
			Blend SrcAlpha OneMinusSrcAlpha


		Pass{
			CGPROGRAM
			#pragma vertex surf
			#pragma fragment frag
			#include "UnityCG.cginc"


			sampler2D _MainTex;

			struct Input {
				float4 vertex:POSITION;
				float4 texcoord:TEXCOORD0;
			};

			struct v2f {
				float4 pos:SV_POSITION;
				float2 uv1:TEXCOORD0;
			};

			float4 _MainTex_ST;
			float _Size;
			float _Blur;
			fixed4 _Pos;
			float4 _Color;

			v2f surf(appdata_base v) {
				v2f o;
				o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
				o.uv1 = TRANSFORM_TEX(v.texcoord, _MainTex);
				return o;
			}

			fixed4 frag(v2f i) : COLOR{
				fixed4 base = tex2D(_MainTex, i.uv1);
				float2 uvPos = i.uv1;
				

				float dist = distance(uvPos, float2(_Pos.x, _Pos.y)) / 0.1f;
				if (dist < _Size) {
					clip(-1.0);
				}
				else if (dist < _Size + _Blur) {
					base.a = (dist - _Size) * 10;


				}

				return base * _Color;
			}

			ENDCG

		}
	}
}
