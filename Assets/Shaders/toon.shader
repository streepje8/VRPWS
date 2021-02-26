//STREEP'S 200+ LINE TOON SHADER
//I HAVE PUT 6+ HOURS IN TO THIS, HOPE IT WAS WORTH IT
Shader "Streep/toon"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        [Toggle]_BUMPBOOL("Use normal map", Float) = 0
        [Toggle]_DISPLACEBOOL("Use Displacement map", Float) = 0
        [Toggle]_AOBOOL("Use AO map", Float) = 0
        [Toggle]_SPECBOOL("Use SPEC map", Float) = 0
        _BUMPSTRENGTH("Bump Strength", Range(0,1)) = 0.25
        [HDR]_Color ("Color", Color) = (1,1,1,1)
        _MinimumLight("Minimum light amount",Range(0,1)) = 0
        _Specular("Spec", Float) = 0.95
        [HDR]_OutlineColor("Outline Color", Color) = (0, 0, 0, 1)
        _OutlineThickness("Outline Thickness", Range(0,10)) = 3
        _BumpMap("Normal map", 2D) = "white" {}
        _DispTex("Displacement Texture", 2D) = "white" {}
        _DispAmount("Displacement Amount", Range(0,3)) = 0.5
        _AOTex("AO Texture", 2D) = "white" {}
        _AOAmount("AO Amount", Range(0,3)) = 0.5
        _SpecularTex("Specular Texture", 2D) = "white" {}
        _SpecularAmount("Specular texture Amount", Range(0,3)) = 0.5
    }
    SubShader
    {
        Tags { 
            "RenderType"="Opaque" 
            "LightMode" = "ForwardBase"
	        "PassFlags" = "OnlyDirectional"
            "Queue" = "Geometry"
        }
        LOD 100

     
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_fog
            #pragma multi_compile_fwdbase

            #include "UnityCG.cginc"
            #include "Lighting.cginc"
            #include "AutoLight.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float3 normal : NORMAL;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 pos : POSITION2;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
                float3 worldNormal : NORMAL;
                LIGHTING_COORDS(3, 4)
            };

            sampler2D _MainTex;
            sampler2D _BumpMap;
            float2 _BumpMapp_TexelSize;
            float4 _MainTex_ST;
            float4 _Color;
            float _Specular;
            bool _BUMPBOOL;
            float _BUMPSTRENGTH;
            bool _DISPLACEBOOL;
            bool _AOBOOL;
            bool _SPECBOOL;
            sampler2D _DispTex;
            sampler2D _AOTex;
            sampler2D _SpecularTex;
            float _DispAmount;
            float _AOAmount;
            float _SpecularAmount;
            float _MinimumLight;

            v2f vert(appdata v)
            {
                v2f o;
                if (_DISPLACEBOOL) {
                    v.vertex.xyz += v.normal * tex2Dlod(_DispTex, float4(v.uv, 0, 0)).r * _DispAmount;
                }
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                o.worldNormal = UnityObjectToWorldNormal(v.normal);
                TRANSFER_VERTEX_TO_FRAGMENT(o);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 sample = tex2D(_MainTex, i.uv);
                float3 normal = normalize(i.worldNormal);
                float NdotL = dot(_WorldSpaceLightPos0, normal);
                float lighting = 0;
                if(NdotL < 0) {
                    lighting = 0.25;
				}
                if(NdotL > 0 && NdotL < _Specular) {
                    lighting = 0.70; 
				}
                if(NdotL > _Specular) {
                    lighting = 1;
                    if (_SPECBOOL) {
                        lighting *= -(1 - tex2D(_SpecularTex, i.uv) * _SpecularAmount);
                    }
				}
                if (_BUMPBOOL == 1) {
                    float2 delta = float2(_BumpMapp_TexelSize.x, 0);
                    float4 normalm = tex2D(_BumpMap, i.uv);
                    normal = normal + normalm;
                    normal = normalize(normal);
                    NdotL = dot(_WorldSpaceLightPos0, normal);
                    lighting = ((lighting * (1 - _BUMPSTRENGTH)) + (NdotL * _BUMPSTRENGTH)) / 2;
                    lighting += 0.3;
                }
                float shadow = LIGHT_ATTENUATION(i);
                if (shadow != 0) {
                    shadow = 1;
                }
                if (shadow == 0) {
                    shadow = 0.05;
                }
                float AO = 1;
                if (_AOBOOL) {
                    AO -= (1 - tex2D(_AOTex, i.uv)) * _AOAmount;
                }
                float lightvalue = (lighting * _LightColor0 + unity_AmbientSky) * shadow * AO;
                lightvalue = clamp(lightvalue, _MinimumLight, 1);
                float4 col = _Color * sample * lightvalue;
                UNITY_APPLY_FOG(i.fogCoord, col);
                return col;
            }
            ENDCG
        }
        


        Pass{
            Cull front

            CGPROGRAM

            #include "UnityCG.cginc"

            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_fog
            #pragma multi_compile_fwdbase

            fixed4 _OutlineColor;
            float _OutlineThickness;

            struct appdata {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
            };

            struct v2f {
                float4 position : SV_POSITION;
                UNITY_FOG_COORDS(1)
            };

            v2f vert(appdata v) {
                v2f o;
                float4 clipPosition = UnityObjectToClipPos(v.vertex);
                float3 clipNormal = mul((float3x3) UNITY_MATRIX_VP, mul((float3x3) UNITY_MATRIX_M, v.normal));
                float2 offset = normalize(clipNormal.xy) / _ScreenParams.xy * _OutlineThickness * clipPosition.w * 2;
                clipPosition.xy += offset;

                o.position = clipPosition;
                UNITY_TRANSFER_FOG(o, o.position);
                return o;
            }

            fixed4 frag(v2f i) : SV_TARGET {
            float4 col = _OutlineColor;
            UNITY_APPLY_FOG(i.fogCoord, col);
            return col;
            }

            ENDCG
        }
    }
    FallBack "Diffuse"
}