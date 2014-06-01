Shader "Custom/metaball" {
	Properties {
		_Color ("Main Color", Color) = (1, 1, 1, 1)
		_MainTex ("Texture", 2d) = "white" {}
		_Radius ("Radius", Float) = 0.25
		_GrayVal ("Gray Value", Float) = 0

		cells ("Cells", Vector) = (0, 0, 0, 0)

	}
	SubShader {
		Tags { "Queue" = "Transparent" "RenderType" = "Opaque" }
		
		CGPROGRAM
		#pragma surface surf Lambert alpha

		fixed4 _Color;
		sampler2D _MainTex;
		float _Radius;
		float _GrayVal;

		half4 cells;

		struct Input {
			float2 uv_MainTex;
		};

		void surf (Input IN, inout SurfaceOutput o) {
			half2 uv = IN.uv_MainTex;
			half dist = distance(half2(0.5, 0.5), IN.uv_MainTex);
			
			if(dist > _Radius) {
				o.Alpha = 0;
			} else {
				o.Alpha = 1 - _GrayVal;
			}

			half coef = (_Radius + 0.5 - dist*2);
			half3 c = _Color.rgb * coef;
			half3 g = half3(1, 1, 1) * coef;
			o.Albedo = lerp(c, g, _GrayVal);
		}
		ENDCG
	} 
	FallBack "Diffuse"
}
