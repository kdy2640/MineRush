Shader "UI/Radial Glow"
{
    Properties
    {
        [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
        _Color ("Tint", Color) = (1,1,1,1)

        _Intensity ("Intensity", Float) = 3
        _RayCount ("Ray Count", Float) = 32

        _MinRayWidth ("Min Ray Width", Range(0.01, 0.49)) = 0.06
        _MaxRayWidth ("Max Ray Width", Range(0.01, 0.49)) = 0.18
        _RayEdgeSoftness ("Ray Edge Softness", Range(0.001, 0.2)) = 0.02
        _RayOffsetStrength ("Ray Offset Strength", Range(0, 0.49)) = 0.3

        _VariationPower ("Variation Power", Float) = 0.8
        _Seed ("Seed", Float) = 0

        _InnerRadius ("Inner Radius", Float) = 0.05
        _OuterRadius ("Outer Radius", Float) = 0.65
        _CoreSize ("Core Size", Float) = 0.14
        _CoreSoftness ("Core Softness", Range(0.1, 1)) = 0.7
        _RotateSpeed ("Rotate Speed", Float) = 0.15

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
        Blend SrcAlpha One
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

            sampler2D _MainTex;
            fixed4 _Color;
            fixed4 _TextureSampleAdd;
            float4 _ClipRect;

            float _Intensity;
            float _RayCount;

            float _MinRayWidth;
            float _MaxRayWidth;
            float _RayEdgeSoftness;
            float _RayOffsetStrength;

            float _VariationPower;
            float _Seed;

            float _InnerRadius;
            float _OuterRadius;
            float _CoreSize;
            float _CoreSoftness;
            float _RotateSpeed;

            static const float TWO_PI = 6.2831853;

            float hash(float n)
            {
                return frac(sin(n) * 43758.5453123);
            }

            float WrapIndex(float index, float count)
            {
                return index - floor(index / count) * count;
            }

            float GetRayIndex(float angle01, float count)
            {
                float p = frac(angle01) * count;
                float index = floor(p + 0.5);
                return WrapIndex(index, count);
            }

            float GetRayWidth(float rayIndex, float salt)
            {
                float widthRandomA = hash(rayIndex * 17.173 + salt + _Seed * 13.13);
                float widthRandomB = hash(rayIndex * 41.731 + salt * 2.37 + _Seed * 37.71);
                float widthRandom = saturate(widthRandomA * 0.7 + widthRandomB * 0.3);

                return lerp(_MinRayWidth, _MaxRayWidth, widthRandom);
            }

            float GetRayBrightness(float rayIndex, float salt)
            {
                float brightnessRandomA = hash(rayIndex * 23.371 + salt + _Seed * 19.19);
                float brightnessRandomB = hash(rayIndex * 11.917 + salt * 3.11 + _Seed * 53.53);
                float brightness = saturate(brightnessRandomA * 0.65 + brightnessRandomB * 0.35);

                brightness = pow(brightness, _VariationPower);
                return lerp(0.5, 1.0, brightness);
            }

            float GetRayOffset(float rayIndex, float salt)
            {
                float offsetRandom = hash(rayIndex * 29.913 + salt + _Seed * 91.17);
                return (offsetRandom - 0.5) * _RayOffsetStrength;
            }

            float MakeRay(float angle01, float count, float widthScale, float softnessScale, float salt)
            {
                count = max(1.0, floor(count + 0.5));

                float wrappedAngle = frac(angle01);
                float p = wrappedAngle * count;

                float rayIndex = GetRayIndex(wrappedAngle, count);
                float offset = GetRayOffset(rayIndex, salt);

                float cell = frac(p + offset);
                float distToRay = min(cell, 1.0 - cell);

                float width = GetRayWidth(rayIndex, salt) * widthScale;
                float halfWidth = width * 0.5;

                float ray = 1.0 - smoothstep(
                    halfWidth,
                    halfWidth + _RayEdgeSoftness * softnessScale,
                    distToRay
                );

                float brightness = GetRayBrightness(rayIndex, salt + 9.17);

                return ray * brightness;
            }

            v2f vert(appdata_t v)
            {
                v2f OUT;

                UNITY_SETUP_INSTANCE_ID(v);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(OUT);

                OUT.worldPosition = v.vertex;
                OUT.vertex = UnityObjectToClipPos(OUT.worldPosition);
                OUT.uv = v.texcoord;
                OUT.color = v.color * _Color;

                return OUT;
            }

            fixed4 frag(v2f IN) : SV_Target
            {
                float2 centeredUV = IN.uv - 0.5;
                float radius = length(centeredUV);

                float angle = atan2(centeredUV.y, centeredUV.x);
                float angle01 = frac(angle / TWO_PI + 1.0);
                float rotatedAngle = frac(angle01 + _Time.y * _RotateSpeed);

                float baseCount = max(1.0, floor(_RayCount + 0.5));
                float countB = max(1.0, floor(baseCount * 1.7 + 0.5));
                float countC = max(1.0, floor(baseCount * 0.55 + 0.5));

                float rayA = MakeRay(rotatedAngle, baseCount, 1.0, 1.0, 13.1);
                float rayB = MakeRay(rotatedAngle + 0.037, countB, 0.65, 0.8, 71.7);
                float rayC = MakeRay(rotatedAngle - 0.021, countC, 1.35, 1.2, 151.3);

                float ray = saturate(rayA * 0.75 + rayB * 0.4 + rayC * 0.3);

                float radialFade = 1.0 - smoothstep(_InnerRadius, _OuterRadius, radius);

                float coreBase = saturate(1.0 - radius / max(_CoreSize, 0.0001));
                float corePower = lerp(3.0, 0.15, _CoreSoftness);
                float core = pow(coreBase, corePower);

                float softHalo = 1.0 - smoothstep(0.0, _OuterRadius, radius);
                softHalo = pow(saturate(softHalo), 2.2) * 0.18;

                float mask = saturate(ray * radialFade + core * 1.35 + softHalo);

                fixed4 color = IN.color;
                color.rgb *= mask * _Intensity;
                color.a *= mask;

                #ifdef UNITY_UI_CLIP_RECT
                color.a *= UnityGet2DClipping(IN.worldPosition.xy, _ClipRect);
                #endif

                return color;
            }
            ENDCG
        }
    }
}