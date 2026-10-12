Shader "Dead Sector/Shield"
{
    Properties
    {
        [HDR] _Color("Color", Color) = (0.35, 0.8, 1, 1)
        _Intensity("Intensity", Float) = 1.6
        _HexScale("Hex Scale", Float) = 7
        _HexStrength("Hex Strength", Range(0, 1)) = 0.35
        _RimPower("Rim Power", Float) = 2.5
        _RippleSpeed("Ripple Speed", Float) = 3
        _RippleWidth("Ripple Width", Float) = 0.18
        _RippleDuration("Ripple Duration", Float) = 0.6
        _Flash("Flash", Range(0, 1)) = 0
    }

    SubShader
    {
        Tags { "RenderType" = "Transparent" "Queue" = "Transparent" "RenderPipeline" = "UniversalPipeline" "IgnoreProjector" = "True" }

        Pass
        {
            Name "Shield"
            Tags { "LightMode" = "SRPDefaultUnlit" }

            Blend SrcAlpha One
            ZWrite Off
            ZTest LEqual
            Cull Off

            HLSLPROGRAM
            #pragma vertex Vert
            #pragma fragment Frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            CBUFFER_START(UnityPerMaterial)
                half4 _Color;
                float _Intensity;
                float _HexScale;
                half _HexStrength;
                float _RimPower;
                float _RippleSpeed;
                float _RippleWidth;
                float _RippleDuration;
                half _Flash;
            CBUFFER_END

            float4 _Hits[4];

            struct Attributes
            {
                float4 positionOS : POSITION;
                float3 normalOS : NORMAL;
            };

            struct Varyings
            {
                float4 positionCS : SV_POSITION;
                float3 positionWS : TEXCOORD0;
                float3 positionOS : TEXCOORD1;
                float3 normalWS : TEXCOORD2;
                float3 normalOS : TEXCOORD3;
            };

            Varyings Vert(Attributes input)
            {
                Varyings output;
                output.positionWS = TransformObjectToWorld(input.positionOS.xyz);
                output.positionCS = TransformWorldToHClip(output.positionWS);
                output.positionOS = input.positionOS.xyz;
                output.normalWS = TransformObjectToWorldNormal(input.normalOS);
                output.normalOS = input.normalOS;
                return output;
            }

            float HexEdge(float2 position)
            {
                const float2 size = float2(1.0, 1.7320508);
                float4 centers = floor(float4(position, position - float2(0.5, 1.0)) / size.xyxy) + 0.5;
                float4 offsets = float4(position - centers.xy * size, position - (centers.zw + 0.5) * size);
                float2 local = dot(offsets.xy, offsets.xy) < dot(offsets.zw, offsets.zw) ? offsets.xy : offsets.zw;
                float2 absolute = abs(local);
                float distanceToCenter = max(dot(absolute, size * 0.5), absolute.x);
                return smoothstep(0.42, 0.5, distanceToCenter);
            }

            float HexPattern(float3 position, float3 normal)
            {
                float3 weights = pow(abs(normal), 4.0);
                weights /= weights.x + weights.y + weights.z;
                float3 scaled = position * _HexScale;

                return HexEdge(scaled.yz) * weights.x + HexEdge(scaled.xz) * weights.y + HexEdge(scaled.xy) * weights.z;
            }

            float Ripples(float3 positionWS)
            {
                float total = 0.0;

                for (int i = 0; i < 4; i++)
                {
                    float age = _Time.y - _Hits[i].w;

                    if (age < 0.0 || age > _RippleDuration) continue;

                    float distanceToHit = distance(positionWS, _Hits[i].xyz);
                    float ring = exp(-pow((distanceToHit - age * _RippleSpeed) / _RippleWidth, 2.0));
                    float spot = exp(-distanceToHit * 6.0) * (1.0 - age / _RippleDuration);
                    total += (ring + spot) * (1.0 - age / _RippleDuration);
                }

                return total;
            }

            half4 Frag(Varyings input) : SV_Target
            {
                float3 normal = normalize(input.normalWS);
                float3 viewDirection = normalize(GetWorldSpaceViewDir(input.positionWS));
                float rim = pow(1.0 - saturate(abs(dot(normal, viewDirection))), _RimPower);

                float hex = HexPattern(input.positionOS, normalize(input.normalOS));
                float ripples = Ripples(input.positionWS);

                float intensity = 0.05 + rim * 0.9 + hex * _HexStrength * (0.35 + rim) + ripples * (0.7 + hex) + _Flash * 0.35;
                intensity *= _Intensity;

                return half4(_Color.rgb * intensity, saturate(intensity) * _Color.a);
            }
            ENDHLSL
        }
    }
}
