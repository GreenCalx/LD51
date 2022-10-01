Shader "Unlit/RadarShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _PlayerPosition("CenterPosition", Vector) = (0,0,0,0)
        _RadarSpeed("Speed", Float) = 1
        _RadarFalloffSpeed("Falloff Speed", Float) = 1
        _CurrentTime("Time", Float) = 1
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
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

            float4 _PlayerPosition;
            float _RadarSpeed;
            float _RadarFalloffSpeed;

            float _CurrentTime;

            sampler2D _CameraDepthTexture;

            float4x4 _UNITY_MATRIX_I_V;

            float3 DepthToWorld(float2 uv, float depth) {
                // float4 cPos = float4(uv*2-1, depth, 1.0);
                // float4 vPos = mul(_UNITY_MATRIX_I_P, cPos);
                // vPos /= vPos.w;
                // float4 wPos = mul(_UNITY_MATRIX_I_V, vPos);
                // return wPos;
                // float4 hpositionWS = mul(_UNITY_MATRIX_I_VP, cPos);
                // return hpositionWS.xyz/hpositionWS.w;

                 const float2 p11_22 = float2(unity_CameraProjection._11, unity_CameraProjection._22);
                 const float2 p13_31 = float2(unity_CameraProjection._13, unity_CameraProjection._23);
                 const float isOrtho = unity_OrthoParams.w;
                 const float near = _ProjectionParams.y;
                 const float far = _ProjectionParams.z;

                 #if defined(UNITY_REVERSED_Z)
                     depth = 1 - depth;
                 #endif
                 float vz = near * far / lerp(far, near, depth);

                 float3 vpos = float3((uv * 2 - 1 - p13_31) / p11_22 * vz, -vz);
                 return mul(_UNITY_MATRIX_I_V, float4(vpos, 1));
             }
            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 center = _PlayerPosition;
                float radius = _CurrentTime * _RadarSpeed;
                float falloffRadius = _CurrentTime * _RadarFalloffSpeed;

                float D = SAMPLE_DEPTH_TEXTURE(_CameraDepthTexture, i.uv);
                float3 WSPosition = DepthToWorld(i.uv, D);

                float depth = 1 - Linear01Depth(D);

                float dist = distance(WSPosition.xyz, center.xyz);
                if (dist < radius && dist > falloffRadius) {
                    float range = clamp(0.0001, radius, radius - falloffRadius);
                    float alpha = saturate((dist - falloffRadius) / range);
                    float grayscale = depth * alpha;
                    return float4(grayscale,grayscale,grayscale,1);
                }

                return float4(0,0,0,0);
            }
            ENDCG
        }
    }
}
