Shader "Custom/CloseCameraStippler" {
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _Glossiness ("Smoothness", Range(0,1)) = 0.5
        _Metallic ("Metallic", Range(0,1)) = 0.0
        _MinDistance ("Minimum Distance", float) = 6
        _MaxDistance ("Maximum Distance", float) = 7
    }
    SubShader {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" }
        LOD 200
        ZWrite Off
        ZTest LEqual
        Cull Back
 
//        Pass 
//        {
//            ZWrite On
//            ColorMask 0
//        }
        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf Standard fullforwardshadows alpha:fade
 
        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0
 
        sampler2D _MainTex;
 
        struct Input 
        {
            float2 uv_MainTex;
            float3 worldPos;
            float4 screenPos;
        };
 
        half _Glossiness;
        half _Metallic;
        float _MinDistance;
        float _MaxDistance;
        fixed4 _Color;
        
        half _Transparency;
 
        // Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
        // See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
        // #pragma instancing_options assumeuniformscaling
        UNITY_INSTANCING_BUFFER_START(Props)
            // put more per-instance properties here
        UNITY_INSTANCING_BUFFER_END(Props)
 
        void surf (Input IN, inout SurfaceOutputStandard o) {
            // Albedo comes from a texture tinted by color
            fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
            o.Albedo = c.rgb;
            o.Alpha = c.a;
            // Screen-door transparency: Discard pixel if below threshold.
            float4x4 thresholdMatrix =
            {  
                1.0 / 17.0,  9.0 / 17.0,  3.0 / 17.0, 11.0 / 17.0,
                13.0 / 17.0,  5.0 / 17.0, 15.0 / 17.0,  7.0 / 17.0,
                4.0 / 17.0, 12.0 / 17.0,  2.0 / 17.0, 10.0 / 17.0,
                16.0 / 17.0,  8.0 / 17.0, 14.0 / 17.0,  6.0 / 17.0
            };
            float4x4 _RowAccess = { 1,0,0,0, 0,1,0,0, 0,0,1,0, 0,0,0,1 };
            float2 pos = IN.screenPos.xy / IN.screenPos.w;
            pos *= _ScreenParams.xy; // pixel position
            // Metallic and smoothness come from slider variables
            o.Metallic = _Metallic;
            o.Smoothness = _Glossiness;
 
            // Fade the pixels as they get close to the camera (Start fading at _MaxDistance and become fully transparent at _MinDistance)
            float distanceFromCamera = distance(IN.worldPos, _WorldSpaceCameraPos);
            float fade = (distanceFromCamera - _MinDistance)/ _MaxDistance;
            
            // Fade the pixels with the above calculation and the threshold from the matrix.
            float value = (c.a *fade) - thresholdMatrix[fmod(pos.x, 4)] * _RowAccess[fmod(pos.y, 4)];
            clip(value);
         
            o.Alpha = saturate(value);
        }
        ENDCG
    }
    FallBack "Mobile/VertexLit"
}