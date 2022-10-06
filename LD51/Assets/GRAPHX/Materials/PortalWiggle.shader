Shader "Unlit/PortalWiggle"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Color ("Color", Color) = (1,1,1,1)
        _Glossiness ("Smoothness", Range(0,1)) = 0.5
        _Metallic ("Metallic", Range(0,1)) = 0.0
        _Amplitude ("Amplitude", Float) = 1
        _Wavelength ("Wavelength", Float) = 10
        _Speed("Speed", Float) = 10
        [HDR] _EmissionColor("EmissiveColor", Color) = (0,0,0)
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200
            CGPROGRAM
            // Physically based Standard lighting model, and enable shadows on all light types
            #pragma surface surf Standard fullforwardshadows vertex:vert

            #pragma target 3.0

            sampler2D _MainTex;

            struct Input
            {
                float2 uv_MainTex;
            };

            // shader local properties
            half _Glossiness;
            half _Metallic;
            fixed4 _Color;
            float _Amplitude;
            float _Wavelength;
            float _Speed;
            fixed4 _EmissionColor;
            fixed4 _currWiggle;

            UNITY_INSTANCING_BUFFER_START(Props)
            // put more per-instance properties here
            UNITY_INSTANCING_BUFFER_END(Props)

            void vert(inout appdata_full vertexData) 
            {
                // Vertex position
                float3 vp = vertexData.vertex.xyz;
                float k = 2*UNITY_PI / _Wavelength;
                vp.z += _Amplitude * sin( k * (vp.y- _Speed*_Time.z) );
                _currWiggle = _Amplitude * sin( k * (vp.y- _Speed*_Time.z) );
                vertexData.vertex.xyz = vp.xyz;
            }

            void surf (Input IN, inout SurfaceOutputStandard o)
            {
                // Albedo comes from a texture tinted by color
                fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
                o.Albedo = c.rgb / _currWiggle;
                o.Emission = c.rgb * tex2D(_MainTex, IN.uv_MainTex).a * _EmissionColor * _currWiggle;
                o.Metallic = _Metallic;
                o.Smoothness = _Glossiness;
                o.Alpha = c.a;
            }
            ENDCG 
        }
}
