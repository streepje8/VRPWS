Shader "Streep/toonNoOutline"
{
    Properties
    {
        _MainTex("Texture", 2D) = "white" {}
        [Toggle]_BUMPBOOL("Use normal map", Float) = 0
        _BUMPSTRENGTH("Bump Strength", Range(0,1)) = 0.25
        _BumpMap("Normal map", 2D) = "white" {}
        [HDR]_Color("Color", Color) = (1,1,1,1)
        _Specular("Spec", Float) = 0.95
        _ShadowMax("Shadow maximum", Range(0,1)) = 0.5
    }
        SubShader
        {
            Tags {
                "RenderType" = "Opaque"
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
                    UNITY_FOG_COORDS(1)
                    LIGHTING_COORDS(3, 4)
                    float4 vertex : SV_POSITION;
                    float4 pos : POSITION2;
                    float3 worldNormal : NORMAL;
                };

                sampler2D _MainTex;
                sampler2D _BumpMap;
                float2 _BumpMapp_TexelSize;
                float4 _MainTex_ST;
                float4 _Color;
                float _Specular;
                bool _BUMPBOOL;
                float _BUMPSTRENGTH;
                float _ShadowMax;

                v2f vert(appdata v)
                {
                    v2f o;
                    o.vertex = UnityObjectToClipPos(v.vertex);
                    o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                    UNITY_TRANSFER_FOG(o,o.vertex);
                    o.worldNormal = UnityObjectToWorldNormal(v.normal);
                    o.pos = UnityObjectToClipPos(v.vertex);
                    TRANSFER_VERTEX_TO_FRAGMENT(o);
                    return o;
                }

                fixed4 frag(v2f i) : SV_Target
                {
                    fixed4 sample = tex2D(_MainTex, i.uv);
                    float3 normal = normalize(i.worldNormal);
                    float NdotL = dot(_WorldSpaceLightPos0, normal);
                    float lighting = 0;
                    if (NdotL < 0) {
                        lighting = 0.25;
                    }
                    if (NdotL > 0 && NdotL < _Specular) {
                        lighting = 0.70;
                    }
                    if (NdotL > _Specular) {
                        lighting = 1;
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
                    if (shadow <= _ShadowMax) {
                        shadow = _ShadowMax;
                    }
                    float4 col = _Color * sample * (lighting * _LightColor0 + unity_AmbientSky) * shadow;
                    UNITY_APPLY_FOG(i.fogCoord, col);
                    return col;
                }
                ENDCG
            }
        }
        FALLBACK "Diffuse"
}
