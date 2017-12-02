Shader "Custom/DrawingShader" {

	Properties{
		_MainTex("Albedo (RGB)", 2D) = "white" {}
		_BackTex("Albedo (RGB)", 2D) = "black" {}
	}
	SubShader{
		Tags{ "RenderType" = "Opaque" }
		Cull Off

		CGPROGRAM
		#pragma surface surf Lambert

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

		sampler2D _MainTex;
		sampler2D _BackTex;
		float4 _BackTex_ST;

		struct Input {
			float2 uv_MainTex;
			float2 uv_BackTex;
			float3 screenPos;
		};

		UNITY_INSTANCING_CBUFFER_START(Props)
			// put more per-instance properties here
		UNITY_INSTANCING_CBUFFER_END

		void surf(Input IN, inout SurfaceOutput o) {
			// Albedo comes from a texture tinted by color
			float2 uv = IN.screenPos.xy;
			fixed4 c = tex2D(_MainTex, IN.uv_MainTex);
			fixed4 cB = tex2D(_BackTex, uv * _BackTex_ST.xy + _BackTex_ST.zw);
			if (c.a > c.r) {
				c.r = (255 - ((256 * (255 - (cB.r * 255))) / ((c.r * 255) + 1))) / 255;
				c.g = (255 - ((256 * (255 - (cB.g * 255))) / ((c.g * 255) + 1))) / 255;
				c.b = (255 - ((256 * (255 - (cB.b * 255))) / ((c.b * 255) + 1))) / 255;
			}
			else{
				clip(-1.0);
			}
			o.Albedo = c.rgb;
				
			o.Alpha = c.a;
		}
		ENDCG
	}
	FallBack "Diffuse"
}
