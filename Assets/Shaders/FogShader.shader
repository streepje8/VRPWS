Shader "Hidden/FogShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        [HDR]_FOGCOLOR("FogColor", Color) = (1,1,1,1)
        _DepthMultiplier("Depth Multiplier", Float) = 1
        _Amount("Amount", Range(0,1)) = 1
        _StartDistance("Start Distance", Range(0,10)) = 0
        _Treshhold("Threshhold", Range(0,1)) = 0.95
    }
    SubShader
    {

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            sampler2D _MainTex;
            sampler2D _CameraDepthTexture;
            float4 _FOGCOLOR;
            float _DepthMultiplier;
            float _Amount;
            float _StartDistance;
            float _Treshhold;

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);
                float depth = tex2D(_CameraDepthTexture, i.uv).r;
                if (depth == 0) {
                    return col;
                }
                depth *= _DepthMultiplier * (depth + _StartDistance);
                depth *= 1 - _Amount;
                if (depth > 1) {
                    depth = 1;
                }
                float fogpercent = depth;
                if (fogpercent > _Treshhold) {
                    fogpercent = 1;
                }
                col = (((1 - fogpercent) * _FOGCOLOR) + (fogpercent * col)) / 2;
                //col = (((1 - fogpercent) * unity_AmbientSky) + (fogpercent * col)) / 2;
                return col;
            }
            ENDCG
        }
    }
}
