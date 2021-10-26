Shader "Custom/Unlit/WitchBrewShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _MainTexSpeed("Main Texture Speed", float) = 1.0
        _AlphaCutoff("Alpha Threshold", Range(0, 1)) = 0.5
		_Color("New Color", Color) = (1,1,1,1)
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" }
        LOD 100
        

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float _MainTexSpeed;
            
            float _Angle;
            float _AlphaCutoff;
			float4 _Color;
			
            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
              
                
                // Pivot
                float2 pivot = float2(0.5, 0.5);
                // Rotation Matrix
                _Angle += _Time.y * _MainTexSpeed;
                float cosAngle = cos(_Angle);
                float sinAngle = sin(_Angle);
                float2x2 rot = float2x2(cosAngle, -sinAngle, sinAngle, cosAngle);
 
                // Rotation consedering pivot
                float2 uv = v.uv.xy - pivot;
                o.uv = mul(rot, uv);
                o.uv += pivot;
                o.uv = TRANSFORM_TEX(o.uv, _MainTex);
                //o.uv += (_MainTexDir.xy * _Time.y * _MainTexSpeed);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                
                fixed4 col = tex2D(_MainTex, i.uv);
                if(col.a < _AlphaCutoff)
                    clip(-1);
                // apply fog
                UNITY_APPLY_FOG(i.fogCoord, col);
				col *= _Color;
                return col;
            }
            ENDCG
        }
    }
}
