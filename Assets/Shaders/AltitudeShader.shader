Shader "Altitude/Altitude"
{
	Properties
	{
		_oTopSurfaceColor ("Top Surface Color", Color) = (1, 1, 1, 1)
		_oMiddleSurfaceColor("Middle Surface Color", Color) = (1, 1, 1, 1)
		_oBottomSurfaceColor("Bottom Surface Color", Color) = (1, 1, 1, 1)
		_oBorderSurfaceColor("Border Surface Color", Color) = (1, 1, 1, 1)
		_oTopWaterColor("Top Water Color", Color) = (1, 1, 1, 1)
		_oBottomWaterColor("Bottom Water Color", Color) = (1, 1, 1, 1)


		_fTopHeight("Top Height", Float) = 51.2
		_fMiddleHeight("Middle Height", Float) = 51.2
		_fBorderHeight("Border Height", Float) = 51.2
		_fBottomWaterHeight("Bottom Water Height", Float) = 51.2
	}
	SubShader
	{
		LOD 100

		Pass
		{
			Tags {  "RenderType"="Transparent" "BW" = "TrueProbes" "LightMode" = "ForwardBase" }

	        Blend DstAlpha OneMinusDstAlpha

			ZWrite On

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

            #pragma multi_compile_fwdbase nolightmap nodirlightmap nodynlightmap novertexlight
            #include "UnityCG.cginc"
            #include "UnityLightingCommon.cginc"
            #include "Lighting.cginc"
            #include "AutoLight.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float4 normal : NORMAL;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 pos : SV_POSITION;
				fixed4 worldPos : TEXCOORD1;
				LIGHTING_COORDS(4,5)
			};

			fixed4 _oTopSurfaceColor;
			fixed4 _oMiddleSurfaceColor;
			fixed4 _oBottomSurfaceColor;
			fixed4 _oBorderSurfaceColor;
			fixed4 _oTopWaterColor;
			fixed4 _oBottomWaterColor;

			float _fTopHeight;
			float _fMiddleHeight;
			float _fBorderHeight;
			float _fBottomWaterHeight;

			v2f vert (appdata v)
			{
				v2f o;
				o.worldPos = mul(UNITY_MATRIX_M, v.vertex);
				o.pos = UnityObjectToClipPos(v.vertex);

				TRANSFER_VERTEX_TO_FRAGMENT(o);


				return o;
			}

			fixed4 frag(v2f i) : SV_Target
			{
				fixed4 oColor = _oBottomWaterColor;

				float fTopWater = smoothstep(_fBottomWaterHeight, _fBorderHeight, i.worldPos.y);
				oColor.rgb = lerp(_oBottomWaterColor.rgb, _oTopWaterColor.rgb, fTopWater);
				float fAboveWater = step(_fBorderHeight, i.worldPos.y);
				oColor.rgb = lerp(oColor.rgb, _oBorderSurfaceColor.rgb, fAboveWater);
				float fBottomSurface = smoothstep(_fBorderHeight, 0, i.worldPos.y);
				oColor.rgb = lerp(oColor.rgb, _oBottomSurfaceColor.rgb, fBottomSurface);
				float fMiddle = smoothstep(0, _fMiddleHeight, i.worldPos.y);
				oColor.rgb = lerp(oColor.rgb, _oMiddleSurfaceColor.rgb, fMiddle);
				float fTop = smoothstep(_fMiddleHeight, _fTopHeight, i.worldPos.y);
				oColor.rgb = lerp(oColor.rgb, _oTopSurfaceColor.rgb, fTop);

				return oColor;
			}
			ENDCG
		}
	}
	Fallback "Transparent/Cutout/VertexLit"
}
