Shader "Custom/Cell" {
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
			float innerRadius = _Radius * 0.9;
                       
            if(dist > _Radius) {
				o.Alpha = 0;
            } else if (dist > innerRadius) {
				float w = (dist - innerRadius) / (_Radius - innerRadius);
				o.Alpha = smoothstep(1, 0, w);
			} else {
				o.Alpha = 1;
			}
            half coef = (_Radius + 0.5 - dist*2.5);
			half3 h = _Color.rgb * 0.2;
			half3 c = lerp(_Color.rgb, h, coef);			
            o.Albedo = lerp(c, half3(0, 0, 0), coef * _GrayVal);
		}
		ENDCG
	} 
	FallBack "Diffuse"
}
