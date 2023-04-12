Shader "Custom/CheckerboardBlending3Materials" {
    Properties {
        _MainTex ("Main Texture (Red)", 2D) = "white" {}
        _MainTex2 ("Second Texture (Orange)", 2D) = "white" {}
        _MainTex3 ("Third Texture (Green)", 2D) = "white" {}
        _Blend ("Blend Factor", Range(0, 1)) = 0
    }

    SubShader {
        Tags {"Queue"="Transparent" "RenderType"="Transparent"}
        LOD 100

        CGPROGRAM
        #pragma surface surf Lambert

        sampler2D _MainTex;
        sampler2D _MainTex2;
        sampler2D _MainTex3;
        float _Blend;

        struct Input {
            float2 uv_MainTex;
        };

        void surf (Input IN, inout SurfaceOutput o) {
            // Sample the three textures using the input UV coordinates
            float4 col1 = tex2D(_MainTex, IN.uv_MainTex);
            float4 col2 = tex2D(_MainTex2, IN.uv_MainTex);
            float4 col3 = tex2D(_MainTex3, IN.uv_MainTex);

            // Blend between the three textures based on the blend factor
            float4 finalColor;
            if (_Blend <= 0.5) {
                float blend = _Blend * 2;
                finalColor = lerp(col1, col2, blend);
            } else {
                float blend = (_Blend - 0.5) * 2;
                finalColor = lerp(col2, col3, blend);
            }

            o.Albedo = finalColor.rgb;
            o.Alpha = finalColor.a;
        }
        ENDCG
    }
    FallBack "Diffuse"
}