Shader "Altitude/ToonVariedAltitude"
{
    Properties
    {
        _Color("Color", Color) = (1,1,1,1)
        _MainTex("Albedo (RGB)", 2D) = "white" {}
        _RampTex("Ramp Texture", 2D) = "white"{}
        _Glossiness("Smoothness", Range(0,1)) = 0.5
        _Metallic("Metallic", Range(0,1)) = 0.0

        _oTopSurfaceColor("Top Surface Color", Color) = (1, 1, 1, 1)
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
            CGPROGRAM
            // Physically based Standard lighting model, and enable shadows on all light types
            #pragma surface surf ToonRamp fullforwardshadows

            // Use shader model 3.0 target, to get nicer looking lighting
            #pragma target 3.0

            sampler2D _MainTex;
            float4 _Color;
            sampler2D _RampTex;

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


            float4 LightingToonRamp(SurfaceOutput s, fixed3 lightDir, fixed atten)
            {
                float diff = dot(s.Normal, lightDir);
                float h = diff * 0.5 + 0.5;
                float2 rh = h;
                float3 ramp = tex2D(_RampTex, rh).rgb;

                fixed4 c;
                c.rgb = s.Albedo * _LightColor0.rgb * (ramp);
                c.rgb = c.rgb * atten + c.rgb;
                c.rgb = c.rgb / 2;
                c.a = s.Alpha;
                return c;
            }
            struct Input
            {
                float2 uv_MainTex;
                fixed3 worldPos : TEXCOORD1;
            };

            half _Glossiness;
            half _Metallic;

            UNITY_INSTANCING_BUFFER_START(Props)
            UNITY_INSTANCING_BUFFER_END(Props)

            void surf(Input IN, inout SurfaceOutput o)
            {
                fixed4 oColor = _oBottomWaterColor;

                float fTopWater = smoothstep(_fBottomWaterHeight, _fBorderHeight, IN.worldPos.y);
                oColor.rgb = lerp(_oBottomWaterColor.rgb, _oTopWaterColor.rgb, fTopWater);
                float fAboveWater = step(_fBorderHeight, IN.worldPos.y);
                oColor.rgb = lerp(oColor.rgb, _oBorderSurfaceColor.rgb, fAboveWater);
                float fBottomSurface = smoothstep(_fBorderHeight, 0, IN.worldPos.y);
                oColor.rgb = lerp(oColor.rgb, _oBottomSurfaceColor.rgb, fBottomSurface);
                float fMiddle = smoothstep(0, _fMiddleHeight, IN.worldPos.y);
                oColor.rgb = lerp(oColor.rgb, _oMiddleSurfaceColor.rgb, fMiddle);
                float fTop = smoothstep(_fMiddleHeight, _fTopHeight, IN.worldPos.y);
                oColor.rgb = lerp(oColor.rgb, _oTopSurfaceColor.rgb, fTop);

                o.Albedo = oColor.rgb* tex2D(_MainTex, IN.uv_MainTex);
            }
            ENDCG
        }
        FallBack "Diffuse"
}
