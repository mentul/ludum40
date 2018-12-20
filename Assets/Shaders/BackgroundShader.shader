// Upgrade NOTE: upgraded instancing buffer 'Props' to new syntax.

Shader "Custom/BackgroundShader" {

	Properties{
		_PlayerPosition("PlayerPosition", Vector) = (0,0,0,0)
		_PlayerSpeed("PlayerSpeed", Float) = 0.0
		_Color("Color", Color) = (1,1,1,1)
		_MainTex("Albedo (RGB)", 2D) = "white" {}
		_Glossiness("Smoothness", Range(0,1)) = 0.5
		_Metallic("Metallic", Range(0,1)) = 0.0
	}
	SubShader{
		Tags{ "RenderType" = "Opaque" }
		LOD 200
		Cull Off

		CGPROGRAM
		#pragma surface surf Standard fullforwardshadows

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

		sampler2D _MainTex;
		float4 _MainTex_ST;
		float4 _PlayerPosition;
		float _PlayerSpeed;

		struct Input {
			float2 uv_MainTex;
			float3 screenPos;
		};

		half _Glossiness;
		half _Metallic;
		fixed4 _Color;

		UNITY_INSTANCING_BUFFER_START(Props)
		// put more per-instance properties here
		UNITY_INSTANCING_BUFFER_END(Props)

		void surf(Input IN, inout SurfaceOutputStandard o) {
			// Albedo comes from a texture tinted by color
			float2 uv = IN.screenPos.xy;

			float2 offset = (_PlayerPosition.xy / (_PlayerPosition.zw)) * (_PlayerSpeed * 0.9) * _MainTex_ST.x;

			fixed4 c = tex2D(_MainTex, uv * _MainTex_ST.xy + offset) * _Color;

			o.Albedo = c.rgb;
			o.Metallic = _Metallic;
			o.Smoothness = _Glossiness;
			o.Alpha = c.a;
		}
		ENDCG
	}
	FallBack "Diffuse"
}
