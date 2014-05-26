Shader "Custom/metaball" {
	Properties {
		_Color ("Main Color", Color) = (1, 1, 1, 1)
		_MainTex ("Texture", 2d) = "white" {}
		_Radius ("Radius", Float) = 0.25

		topLeft ("TopLeft", Float) = 0
		topMiddle ("TopMiddle", Float) = 0
		topRight ("TopRight", Float) = 0

		middleLeft ("MiddleLeft", Float) = 0
		middleRight ("MiddleRight", Float) = 0

		bottomLeft ("BottomLeft", Float) = 0
		bottomMiddle ("BottomMiddle", Float) = 0
		bottomRight ("BottomRight", Float) = 0

	}
	SubShader {
		Tags { "Queue" = "Transparent" "RenderType" = "Opaque" }
		LOD 200
		
		CGPROGRAM
		#pragma target 3.0
		#pragma surface surf Lambert alpha

		fixed4 _Color;
		sampler2D _MainTex;
		float _Radius;

		half topLeft;
		half topMiddle;
		half topRight;
		half middleLeft;
		half middleRight;
		half bottomLeft;
		half bottomMiddle;
		half bottomRight;

		struct Input {
			float2 uv_MainTex;
		};

		void surf (Input IN, inout SurfaceOutput o) {
			half2 uv = IN.uv_MainTex;
			half dist = distance(half2(0.5, 0.5), IN.uv_MainTex);
			
			if(dist > _Radius) {
				o.Alpha = 0;
			} else {
				o.Alpha = 1;
			}

			half l = 0.5 - _Radius;
			half r = 0.5 + _Radius;

			if(topMiddle == 1 && uv.x > l && uv.x < r && uv.y < 0.5) {
				o.Alpha = 1;
			}

			if(bottomMiddle == 1 && uv.x > l && uv.x < r && uv.y > 0.5) {
				o.Alpha = 1;
			}

			if(middleLeft == 1 && uv.y > l && uv.y < r && uv.x > 0.5) {
				o.Alpha = 1;
			}

			if(middleRight == 1 && uv.y > l && uv.y < r && uv.x < 0.5) {
				o.Alpha = 1;
			}

			o.Albedo = _Color;
		}
		ENDCG
	} 
	FallBack "Diffuse"
}
