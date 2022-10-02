Shader "Unlit/BloodParticlesDepth"
{

Properties {
    _MainTex ("Particle Texture", 2D) = "white" {}
}

Category {
    Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" }
    Blend SrcAlpha One
    Cull Off Lighting Off ZWrite Off Fog { Color (0,0,0,0) }
    
    BindChannels {
        Bind "Color", color
        Bind "Vertex", vertex
        Bind "TexCoord", texcoord
    }
    
    SubShader {
        Pass {
            SetTexture [_MainTex] {
                combine texture * primary
            }
        }
        Pass
                {
                    Tags { "LightMode" = "ShadowCaster" "RenderType" = "Opaque" }
                    ColorMask 0
                    AlphaToMask On
 
                    CGPROGRAM
                    #pragma vertex vert
                    #pragma fragment frag
 
                    #include "UnityCG.cginc"
 
                    sampler2D _MainTex;
                    fixed4 _TintColor;
                    float4 _MainTex_ST;
 
                    struct appdata_t {
                        float4 vertex : POSITION;
                        fixed4 color : COLOR;
                        float2 texcoord : TEXCOORD0;
                    };
 
                    struct v2f {
                        float4 vertex : SV_POSITION;
                        fixed4 color : COLOR;
                        float2 texcoord : TEXCOORD0;
                    };
 
                    v2f vert(appdata_t v)
                    {
                        v2f o;
                        o.vertex = UnityObjectToClipPos(v.vertex);
                        o.color = v.color;
                        o.texcoord = TRANSFORM_TEX(v.texcoord, _MainTex);
                        return o;
                    }
 
                    fixed4 frag(v2f i) : SV_Target
                    {
                        fixed4 col = 2.0f * i.color * _TintColor * tex2D(_MainTex, i.texcoord);
                        return col;
                    }
                    ENDCG
                }
    }
}
}
