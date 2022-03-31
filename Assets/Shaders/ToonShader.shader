Shader "Altitude/Toon"
{
    Properties
    {
        _Color("Color", Color) = (1,1,1,1)
        _MainTex("Albedo (RGB)", 2D) = "white" {}
        _RampTex("Ramp Texture", 2D) = "white"{}
        _Glossiness("Smoothness", Range(0,1)) = 0.5
        _Metallic("Metallic", Range(0,1)) = 0.0

        _oTopWaterColor("Top Water Color", Color) = (1, 1, 1, 1)
        _oBottomWaterColor("Bottom Water Color", Color) = (1, 1, 1, 1)
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

        fixed4 _oTopWaterColor;
        fixed4 _oBottomWaterColor;

        float _fBorderHeight;
        float _fBottomWaterHeight;

        float4 LightingToonRamp(SurfaceOutput s, fixed3 lightDir, fixed atten)
        {
            float diff = dot(s.Normal, lightDir);
            float h = diff * 0.5 + 0.5;
            float2 rh = h;
            float3 ramp = tex2D(_RampTex, rh).rgb;

            float4 c;
            c.rgb = s.Albedo * _LightColor0.rgb * (ramp);
            c.rgb = c.rgb * atten + c.rgb;
            c.rgb = c.rgb / 2;
            c.a = s.Alpha;
            return c;
        }
        struct Input
        {
            float2 uv_MainTex;
            fixed4 worldPos : TEXCOORD1;
        };

        half _Glossiness;
        half _Metallic;

        UNITY_INSTANCING_BUFFER_START(Props)
        UNITY_INSTANCING_BUFFER_END(Props)

        void surf(Input IN, inout SurfaceOutput o)
        {
            fixed4 oColor = tex2D(_MainTex, IN.uv_MainTex) * _Color;

             o.Albedo = oColor.rgb;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
