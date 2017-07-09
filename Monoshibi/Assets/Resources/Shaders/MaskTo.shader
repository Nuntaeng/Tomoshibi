Shader "Mask/MaskTo"
{
	Properties
	{
		[PerRenderer] _MainTex("Texture", 2D) = "white" {}
		_Color("Color", Color) = (0, 0, 0, 0)
	}
		SubShader
		{
			Tags{ "RenderType" = "Transparent" "Queue" = "Transparent" }
			Blend SrcAlpha OneMinusSrcAlpha
		Pass
		{


			Stencil{
				Ref 0
				Comp Equal
			}
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float4 color : COLOR;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 color : COLOR;
				float4 vertex : SV_POSITION;
			};

			v2f vert (appdata v)
			{
				v2f o;
				o.color = v.color;
				o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
				return o;
			}
			
			sampler2D _MainTex;
			fixed4 _Color;

			fixed4 frag(v2f i) : SV_Target
			{
				return i.color;
			}
			ENDCG
		}

			Pass
			{


				Stencil{
					Ref 1
					Comp Equal
				}
				CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag

				#include "UnityCG.cginc"

				struct appdata
				{
					float4 vertex : POSITION;
					float4 color : COLOR;
					float2 uv : TEXCOORD0;
				};

				struct v2f
				{
					float2 uv : TEXCOORD0;
					float4 color : COLOR;
					float4 vertex : SV_POSITION;
				};

				v2f vert(appdata v)
				{
					v2f o;
					o.color = v.color;
					o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
					return o;
				}

				sampler2D _MainTex;
				fixed4 _Color;

				fixed4 frag(v2f i) : SV_Target
				{
					i.color.a /= 1.5f;
					return i.color;
				}
				ENDCG
			}

			Pass
			{


				Stencil{
					Ref 2
					Comp Equal
				}
				CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag

				#include "UnityCG.cginc"

				struct appdata
				{
					float4 vertex : POSITION;
					float4 color : COLOR;
					float2 uv : TEXCOORD0;
				};

				struct v2f
				{
					float2 uv : TEXCOORD0;
					float4 color : COLOR;
					float4 vertex : SV_POSITION;
				};

				v2f vert(appdata v)
				{
					v2f o;
					o.color = v.color;
					o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
					return o;
				}

				sampler2D _MainTex;
				fixed4 _Color;

				fixed4 frag(v2f i) : SV_Target
				{
					i.color.a /= 3.0f;
					return i.color;
				}
				ENDCG
			}
		}
}