Shader "UI/Procedural Vignette"
{
    Properties
    {
        [PerRendererData] _MainTex ("Texture", 2D) = "white" {}
        _Color ("Vignette Color", Color) = (0, 0, 0, 1)
        _Intensity ("Intensity", Range(0, 1)) = 0.6
        _InnerRadius ("Inner Radius", Range(0, 1)) = 0.25
        _OuterRadius ("Outer Radius", Range(0, 1.5)) = 0.75
        _Softness ("Softness", Range(0.001, 1)) = 0.4

        _StencilComp ("Stencil Comparison", Float) = 8
        _Stencil ("Stencil ID", Float) = 0
        _StencilOp ("Stencil Operation", Float) = 0
        _StencilWriteMask ("Stencil Write Mask", Float) = 255
        _StencilReadMask ("Stencil Read Mask", Float) = 255
        _ColorMask ("Color Mask", Float) = 15
    }

    SubShader
    {
        Tags
        {
            "Queue"="Transparent"
            "IgnoreProjector"="True"
            "RenderType"="Transparent"
            "PreviewType"="Plane"
            "CanUseSpriteAtlas"="True"
        }

        Stencil
        {
            Ref [_Stencil]
            Comp [_StencilComp]
            Pass [_StencilOp]
            ReadMask [_StencilReadMask]
            WriteMask [_StencilWriteMask]
        }

        Cull Off
        Lighting Off
        ZWrite Off
        ZTest [unity_GUIZTestMode]
        Blend SrcAlpha OneMinusSrcAlpha
        ColorMask [_ColorMask]

        Pass
        {
            Name "Default"

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_local _ UNITY_UI_CLIP_RECT

            #include "UnityCG.cginc"
            #include "UnityUI.cginc"

            struct appdata_t
            {
                float4 vertex : POSITION;
                float4 color : COLOR;
                float2 texcoord : TEXCOORD0;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                fixed4 color : COLOR;
                float2 uv : TEXCOORD0;
                float4 worldPosition : TEXCOORD1;
                UNITY_VERTEX_OUTPUT_STEREO
            };

            fixed4 _Color;
            float _Intensity;
            float _InnerRadius;
            float _OuterRadius;
            float _Softness;
            float4 _ClipRect;
            sampler2D _MainTex;
            fixed4 _TextureSampleAdd;

            v2f vert(appdata_t v)
            {
                v2f OUT;
                UNITY_SETUP_INSTANCE_ID(v);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(OUT);

                OUT.worldPosition = v.vertex;
                OUT.vertex = UnityObjectToClipPos(OUT.worldPosition);
                OUT.uv = v.texcoord;
                OUT.color = v.color;

                return OUT;
            }

            fixed4 frag(v2f IN) : SV_Target
            {
                float2 centeredUV = IN.uv - 0.5;
                float distanceFromCenter = length(centeredUV);

                float inner = _InnerRadius;
                float outer = lerp(_InnerRadius, _OuterRadius, _Softness);

                float vignette = smoothstep(inner, outer, distanceFromCenter);
                vignette *= _Intensity;

                fixed4 color = _Color;
                color.a *= vignette;
                color *= IN.color;

                #ifdef UNITY_UI_CLIP_RECT
                color.a *= UnityGet2DClipping(IN.worldPosition.xy, _ClipRect);
                #endif

                return color;
            }

            ENDCG
        }
    }
}