// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/Platform"
{
	Properties{
		_Color("Color (RGBA)", Color) = (1, 1, 1, 1)
		_Intensity("Intensity", Float) = 1
		_HeightOffset("Height Offset", Float) = 0
	}

		SubShader{
		Tags{ "Queue" = "Transparent" "IgnoreProjector" = "True" "RenderType" = "Transparent" }
		ZWrite On
		Cull Back
		LOD 100

		Pass{
		CGPROGRAM

#pragma vertex vert alpha
#pragma fragment frag alpha

#include "UnityCG.cginc"

		struct appdata_t {
		float4 vertex : POSITION;
		float3 normal : NORMAL;
		float4 color  : COLOR;
	};

	struct v2f {
		float4 vertex : SV_POSITION;
		fixed4 color : COLOR;
	};

	float4 _Color;
	float _Intensity;
	float  _HeightOffset;
	v2f vert(appdata_t v)
	{
		v2f o;
		o.vertex = UnityObjectToClipPos(v.vertex);
		float4 pos = v.vertex;
		pos.y -= _HeightOffset;
		o.color.xyz = (((dot(-v.normal, normalize(pos)) + 1.0) * 0.5)) * _Intensity * _Color + v.color.rgb;
		o.color.w = _Color.a;
		return o;
	}

	fixed4 frag(v2f i) : SV_Target
	{
		return i.color;
	}

		ENDCG
	}
	}
}