Shader "Custom/SimpleAlphaDirectionalWithFade"
{
    Properties
    {
        _BaseColor("Base Color", Color) = (1,1,1,1)
        _Alpha("Global Alpha", Range(0,1)) = 1.0

        _FadeDirection("Fade Direction", Vector) = (0,1,0,0)
        _FadeInStart("Fade In Start", Float) = 0.0
        _FadeInEnd("Fade In End", Float) = 0.2
        _FadeOutStart("Fade Out Start", Float) = 0.8
        _FadeOutEnd("Fade Out End", Float) = 1.0
    }

    SubShader
    {
        Tags
        {
            "RenderType"="Transparent"
            "Queue"="Transparent"
        }

        Pass
        {
            Name "Forward"
            Tags { "LightMode"="UniversalForward" }

            Cull Off
            ZWrite Off
            Blend SrcAlpha OneMinusSrcAlpha

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            float4 _BaseColor;
            float _Alpha;
            float3 _FadeDirection;
            float _FadeInStart;
            float _FadeInEnd;
            float _FadeOutStart;
            float _FadeOutEnd;

            struct Attributes
            {
                float4 positionOS : POSITION;
            };

            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
                float fade : TEXCOORD0;
            };

            Varyings vert(Attributes IN)
            {
                Varyings OUT;
                float3 pos = IN.positionOS.xyz;

                // Proyección de la posición sobre el vector de fade
                float fadeValue = saturate(dot(normalize(_FadeDirection), pos));

                // Aplicamos fade in
                float fadeIn = smoothstep(_FadeInStart, _FadeInEnd, fadeValue);

                // Aplicamos fade out
                float fadeOut = 1.0 - smoothstep(_FadeOutStart, _FadeOutEnd, fadeValue);

                // Combinamos fade in + fade out
                OUT.fade = fadeIn * fadeOut;

                OUT.positionHCS = TransformObjectToHClip(pos);
                return OUT;
            }

            float4 frag(Varyings IN) : SV_TARGET
            {
                float alpha = _Alpha * IN.fade;
                return float4(_BaseColor.rgb, alpha);
            }
            ENDHLSL
        }
    }
}
