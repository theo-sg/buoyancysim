Shader "Custom/WaterShader"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _Glossiness ("Smoothness", Range(0,1)) = 0.5
        _Metallic ("Metallic", Range(0,1)) = 0.0

        _Steepness ("Steepness", Range(0,1)) = 0.5
        _Wavelength ("Wavelength", Range(1,20)) = 5
        _Direction ("Direction (2D)", Vector) = (1, 0, 0, 0)
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" }
        LOD 200

        CGPROGRAM
        #pragma surface surf Standard alpha vertex:vert
        #pragma target 3.0

        sampler2D _MainTex;

        struct Input
        {
            float2 uv_MainTex;
        };

        half _Glossiness;
        half _Metallic;
        fixed4 _Color;

        //water parameters
        float _Steepness, _Wavelength;
        float2 _Direction;

        UNITY_INSTANCING_BUFFER_START(Props)
        UNITY_INSTANCING_BUFFER_END(Props)

        // procedure applied to each vertex
        void vert (inout appdata_full vertexData)
        {
            float3 v = vertexData.vertex.xyz;

            //wave number
            float k = 2 * UNITY_PI / _Wavelength;
            float c = sqrt(9.8 / k);
            float2 d = normalize(_Direction);
            float j = k * (dot(d, v.xz) - c * _Time.y);
            float a = _Steepness / k;
            v.x += d.x * (a * cos(j));
            v.y = a * sin(j);
            v.z += d.y * (a * cos(j));

            //tangent = (dx, dy, dz)
            float3 tangent = normalize(float3(1 - _Steepness * sin(j), _Steepness * cos(j), 0));

            //normal = (-dy, dx, 0)
            float3 normal = float3(-tangent.y, tangent.x, 0);

            vertexData.vertex.xyz = v;
            vertexData.normal = normal;

        }

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            // Albedo comes from a texture tinted by color
            fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
            o.Albedo = c.rgb;
            // Metallic and smoothness come from slider variables
            o.Metallic = _Metallic;
            o.Smoothness = _Glossiness;
            o.Alpha = c.a;
        }
        ENDCG
    }
}
