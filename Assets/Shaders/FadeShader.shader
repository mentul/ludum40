Shader "Custom/FadeShader" {

	Properties{
		[Toggle] _InvertFade("InvertFade", Int) = 0
		_FadeAmount("FadeAmount", Range(0,1)) = 0
		_MainTex("MainTexture", 2D) = "white" {}
	}
	SubShader{
		Tags{ "Queue" = "Transparent" "RenderType" = "Transparent" }
		Cull Off

		CGPROGRAM
		#pragma surface surf Lambert alpha:fade

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

		sampler2D _MainTex;
		float _FadeAmount;
		float _InvertFade;

		struct Input {
			float2 uv_MainTex;
			float3 screenPos;
		};

		UNITY_INSTANCING_CBUFFER_START(Props)
			// put more per-instance properties here
		UNITY_INSTANCING_CBUFFER_END

		void surf(Input IN, inout SurfaceOutput o) {
			// Albedo comes from a texture tinted by color
			float2 uv = IN.screenPos.xy;
			fixed4 c = tex2D(_MainTex, IN.uv_MainTex);
			o.Albedo = c.rgb;

			float alphaFactor = (1-sqrt((uv.x - 0.5)*(uv.x - 0.5) + (uv.y - 0.5)*(uv.y - 0.5)));
			alphaFactor = alphaFactor * alphaFactor;
			if (_InvertFade >= 1) {
				alphaFactor = (_FadeAmount) / (1 - alphaFactor);
			}
			else {
				alphaFactor = (1 - _FadeAmount) / (1 - alphaFactor);
			}
			if (alphaFactor > 1) alphaFactor = 1;
			else if (alphaFactor < 0) alphaFactor = 0;
			c.a = c.a * alphaFactor;

			o.Alpha = c.a;
		}
		ENDCG
	}
	FallBack "Diffuse"
}
