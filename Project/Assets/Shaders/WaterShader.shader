Shader "Tecnocampus/WaterShader"
{
	Properties
	{	
	    [Header(Textures)][Space(5)]
	    _MainTex("Main Texture", 2D) = "defaulttexture" {}
	    _WaterDepthTex("Water Depth Texture", 2D) = "defaulttexture" {}
	    _WaterHeightTex("Water Height Texture", 2D) = "defaulttexture" {}
	    _FoamTex("Foam Texture", 2D) = "defaulttexture" {}
	    _NoiseTex("Noise Texture", 2D) = "defaulttexture" {}

	    [Header(Colors)] [Space(5)]
	    _DeepWaterColor("Deep Water Color", Color) = (0,0,0.5,1)
	    _WaterColor("Water Color", Color) = (0,0,1,1)
	    
        [Header(Wave Settings)][Space(5)]
        _WaveSpeed("Wave Speed", float) = 1.0
        _WaveMaxHeight("Max Wave Height", float) = 0.02
        _WaveDir("Wave Direction 0", Vector) = (0,0,0,0)
        
	    [Header(Water 1)][Space(5)]
        _WaterSpeed1("Water Speed", float) = 1.0
        _WaterDir1("Water Direction", Vector) = (0,0,0,0)
        
        [Header(Water 2)][Space(5)]
        _WaterSpeed2("Water Speed", float) = 1.0
        _WaterDir2("Water Direction", Vector) = (0,0,0,0)
          
        [Header(Foam)][Space(5)]
        _FoamDistance("Foam Distance", Range(0.0, 1.0)) = 1.0
        _FoamSpeed("Foam Speed", float) = 2.0
        _FoamMul("Foam Multiplier", float) = 1.0
        _FoamDirection("Foam Direction", Vector) = (0,0,0,0)
        
        [Header(Other)][Space(5)]
        _NoiseSpeed("Noise Speed", float) = 1.0
        _NoiseDir("Noise Direction", Vector) = (0,0,0,0)
        
	}
	Subshader
	{
		//Tags{ "Queue" = "Transparent" "RenderType" = "Opaque" "IgnoreProjector" = "True"  }
		Tags{ "RenderType" = "Transparent" "IgnoreProjector" = "False"  }
		LOD 100

        Blend One Zero
		Pass
		{
			//Blend SrcAlpha OneMinusSrcAlpha
			ZWrite Off

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 UV : TEXCOORD0;
			};
			struct v2f
			{
				float4 vertex : SV_POSITION;
				float4 UV : TEXCOORD0;
                float4 UVFoam : TEXCOORD1;
				float WaveHeight : TEXCOORD2;
			};
			
            uniform sampler2D _MainTex;
            uniform float4 _MainTex_ST;
            
            uniform sampler2D _WaterDepthTex;
            
	        uniform sampler2D _WaterHeightTex;
	        
	        uniform sampler2D _FoamTex;
	        uniform float4 _FoamTex_ST;
	        
	        uniform sampler2D _NoiseTex;
	        uniform float4 _NoiseTex_ST;
	        
	        uniform float4 _DeepWaterColor;
	        uniform float4 _WaterColor;
            
            uniform float _WaveMaxHeight;
            uniform float _WaveSpeed;
            uniform float4 _WaveDir;
            
            uniform float _WaterSpeed1;
            uniform float4 _WaterDir1;
            
            uniform float _WaterSpeed2;
            uniform float4 _WaterDir2;
              
            uniform float _FoamDistance;
            uniform float _FoamSpeed;
            uniform float _FoamMul;
            uniform float4 _FoamDirection;
            
            uniform float4 _NoiseDir;
            uniform float _NoiseSpeed;

			v2f vert(appdata v)
			{
				v2f o;
				
				float2 l_WaterUV = v.UV + _WaveDir * _Time.y * _WaveSpeed;
				float l_WaterHeight = tex2Dlod(_WaterHeightTex, float4(l_WaterUV,0,0)).x;
				
				o.WaveHeight = l_WaterHeight;
				
				o.vertex = mul(unity_ObjectToWorld, float4(v.vertex.xyz, 1.0));
				o.vertex.y += l_WaterHeight * _WaveMaxHeight;
				o.vertex = mul(UNITY_MATRIX_V, o.vertex);
				o.vertex = mul(UNITY_MATRIX_P, o.vertex);
				
				//o.UV = TRANSFORM_TEX(v.UV, _MainTex);
				o.UV.xy = v.UV.xy * _MainTex_ST.xy;
				o.UV.zw = v.UV;
				
				o.UVFoam.xy = v.UV * _FoamTex_ST.xy;
				o.UVFoam.zw = v.UV * _NoiseTex_ST.xy;
				
				return o;
			}

			half4 frag(v2f i) : SV_Target
			{
			    float l_WaterDepth = tex2D(_WaterDepthTex, i.UV.zw).x;
			    float4 l_WaterFinalColor = _WaterColor * l_WaterDepth 
			    + (1.0 - l_WaterDepth) * _DeepWaterColor;
			    
			    float2 l_UV2 = i.UV.xy;
			    
			    // Water 1 Movement
			    float3 l_DirectionWater1 = _WaterDir1.xyz * _Time.y * _WaterSpeed1;
			    i.UV.xy += l_DirectionWater1;
			    // Water 2 Movement
			    float3 l_DirectionWater2 = _WaterDir2.xyz * _Time.y * _WaterSpeed2;
			    l_UV2 += l_DirectionWater2;
			    
			    // Foam
			    float3 l_FoamMovement = _FoamDirection.xyz * _Time.y * _FoamSpeed;
			    float3 l_NoiseMovement = _NoiseDir.xyz * _Time.y * _NoiseSpeed;
			    i.UVFoam.xy += l_FoamMovement;
			    i.UVFoam.zw += l_NoiseMovement;
			    
			    float4 l_FoamColorVar = float4(0,0,0,0);
			    //if(l_WaterDepth > _FoamDistance)
			    //{
			        float4 l_FoamColor = tex2D(_FoamTex, i.UVFoam.xy);
			        float4 l_NoiseColor = tex2D(_NoiseTex, i.UVFoam.zw);
			        
			        l_FoamColorVar = saturate(l_FoamColor - l_NoiseColor) * _FoamMul;
			        //l_FoamColorVar.xyz *= lerp(1, _FoamDistance, l_WaterDepth);
			        //float4 l_LerpFoam = l_FoamColorVar * l_WaterDepth;
			        float l_FoamPCT = 0.0;
			        
			        if(l_WaterDepth > _FoamDistance)
			            l_FoamPCT = saturate((l_WaterDepth - _FoamDistance ) / (1.0 - _FoamDistance));
			        else
			            l_FoamPCT = saturate((i.WaveHeight - _FoamDistance ) / saturate(1.0 - _FoamDistance));
			        
			        l_FoamColorVar *= l_FoamPCT; 
			        //float l_FoamPCT = saturate((i.WaveHeight - _FoamDistance) / (1 - _FoamDistance));
			        
			        //l_FoamColorVar *= saturate(l_WaterDepth - (_FoamDistance));
			    //}
			    //float4 l_WaterFinalColor = _WaterColor * l_WaterDepth + (1.0 - l_WaterDepth) * _DeepWaterColor;
			    
			    //lerp 1 to FoamDistance. t = l_WaterDepth;
				float4 l_Water1Color = tex2D(_MainTex, i.UV.xy);
				float4 l_Water2Color = tex2D(_MainTex, l_UV2);
				
				l_WaterFinalColor *= (l_Water1Color + l_Water2Color) * 0.5;
				
				return l_WaterFinalColor + l_FoamColorVar; 
			}
			ENDCG
		}
	}
	Fallback "Diffuse"
}
