Shader "Hidden/BrokenGlass"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Mask ("Mask", 2D) = "white" {}
        _Degats("Degats", Float) = 0
    }
    SubShader
    {
        // No culling or depth
        Cull Off ZWrite Off ZTest Always

        Pass
        {
            CGPROGRAM
            #pragma vertex vert_img
            #pragma fragment frag

            #include "UnityCG.cginc"

            sampler2D _MainTex;
            sampler2D _Mask;

            float _Degats;

            fixed4 frag (v2f_img i) : Color
            {
                fixed4 col = tex2D(_MainTex, i.uv);
                if (length(i.uv-0.5) < (1-_Degats)) {
                    // no display
                    return col;
                }
                fixed4 mask = tex2D(_Mask, i.uv);
                if (mask.x > 0.5) {
                    return mask;
                }
                //return float4(i.uv,0,1);
                return float4(col.xyz,1);
            }
            ENDCG
        }
    }
}
