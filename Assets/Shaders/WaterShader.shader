Shader "Custom/WaterShader"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _Glossiness ("Smoothness", Range(0,1)) = 0.5
        _Metallic ("Metallic", Range(0,1)) = 0.0

        _Amplitude ("Amplitude", Range(0,1)) = 0.5
        _Wavelength ("Wavelength", Range(1,20)) = 5
        _Speed ("Speed", Range(0.1,10)) = 1
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" }
        LOD 200

        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf Standard alpha vertex:vert

        // Use shader model 3.0 target, to get nicer looking lighting
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
        float _Amplitude, _Wavelength, _Speed;

        // Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
        // See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
        // #pragma instancing_options assumeuniformscaling
        UNITY_INSTANCING_BUFFER_START(Props)
            // put more per-instance properties here
        UNITY_INSTANCING_BUFFER_END(Props)

        // procedure applied to each vertex
        void vert (inout appdata_full vertexData)
        {
            float3 v = vertexData.vertex.xyz;

            //wave number
            float k = 2 * UNITY_PI / _Wavelength;
            float x = k * (v.x - _Speed * _Time.y);
            v.x += _Amplitude * cos(x);
            v.y = _Amplitude * sin(x);

            //tangent = (dx, dy, dz)
            //float3 tangent = float3(1, k * _Amplitude * cos(x), 0);

            //normal = (-dy, dx, 0)
            //float3 normal = normalize(float3(-tangent.y, tangent.x, 0));

            vertexData.vertex.xyz = v;
            //vertexData.normal = normal;

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