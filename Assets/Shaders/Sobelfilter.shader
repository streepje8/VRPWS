Shader "Custom/SobelFilter" {

	Properties {
		_MainTex("Base (RGB)", 2D) = "white" {}
		_Treshhold("Tres", Float) = 0.01
		_DSTTRESH("Distance treshhold", Float) = 0.1
		_Scale("Scale", Float) = 0.01
	}

		SubShader {
			Tags { "RenderType" = "Opaque" }
			LOD 200

			CGINCLUDE

			#include "UnityCG.cginc"

			sampler2D _MainTex;
			float2 _MainTex_TexelSize;
			float _DeltaX;
			float _DeltaY;
			float _Treshhold;
			sampler2D _CameraDepthTexture;
			float _Scale;
			float _DSTTRESH;



			float sobel(sampler2D tex, float2 uv) {
				float halfScaleFloor = floor(_Scale * 0.5);
				float halfScaleCeil = ceil(_Scale * 0.5);

				float2 bottomLeftUV = uv - float2(_MainTex_TexelSize.x, _MainTex_TexelSize.y) * halfScaleFloor;
				float2 topRightUV = uv + float2(_MainTex_TexelSize.x, _MainTex_TexelSize.y) * halfScaleCeil;
				float2 bottomRightUV = uv + float2(_MainTex_TexelSize.x * halfScaleCeil, -_MainTex_TexelSize.y * halfScaleFloor);
				float2 topLeftUV = uv + float2(-_MainTex_TexelSize.x * halfScaleFloor, _MainTex_TexelSize.y * halfScaleCeil);

				float depth0 = tex2D(tex, bottomLeftUV);
				float depth1 = tex2D(tex, topRightUV);
				float depth2 = tex2D(tex, bottomRightUV);
				float depth3 = tex2D(tex, topLeftUV);

				float depthFiniteDifference0 = depth1 - depth0;
				float depthFiniteDifference1 = depth3 - depth2;

				float edgeDepth = sqrt(pow(depthFiniteDifference0, 2) + pow(depthFiniteDifference1, 2)) * 100;

				// Replace the abs(depthFiniteDifference0) * 100 line.
				return edgeDepth;
			}

			float4 frag(v2f_img IN) : COLOR{
				//float s = 0;
				float s = sobel(_CameraDepthTexture, IN.uv);
				//_MainTex
				fixed4 sample = tex2D(_MainTex, IN.uv);
				float depth = tex2D(_CameraDepthTexture, IN.uv);
				if (depth <= _DSTTRESH) {
					depth = 0.5f;
				}
				if (s > (_Treshhold * (1-depth))) {
					return fixed4(0, 0, 0, 0);
				}
				return sample;
			}

			ENDCG

			Pass {
				CGPROGRAM
				#pragma vertex vert_img
				#pragma fragment frag
				ENDCG
			}

		}
			FallBack "Diffuse"
}