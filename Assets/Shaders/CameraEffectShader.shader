Shader "Custom/CameraEffectShader" {

    Properties {
        _MainTex ("Texture", 2D) = "white" {}
        _Activated ("Activated", Float) = 0
        _Scroll ("Scroll", Vector) = (0, 0, 0, 0)
        _Conjunction ("Conjunction", Vector) = (0, 0, 0, 0)
        _BorderWidth ("Border Width", Range(0, 0.2)) = 0.01
        _BorderColor ("Border Color", Color) = (1, 1, 1, 1)
    }
    
    SubShader {
        // No culling or depth
        Cull Off ZWrite Off ZTest Always

        Pass {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            v2f vert(appdata v) {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            sampler2D _MainTex;
            float _Activated;
            float2 _Scroll;
            float2 _Conjunction;
            float _BorderWidth;
            float4 _BorderColor;

            float4 frag(v2f i) : SV_Target {
                float4 color = float4(1, 1, 1, 1);
                float2 uv = float2(0, 0);
                // color = float4(i.uv.x, i.uv.y, 0, 1);
                
                uv = i.uv + _Scroll;
                if (uv.x < 0) uv.x += 1;
                else if (uv.x > 1) uv.x -= 1;
                if (uv.y < 0) uv.y += 1;
                else if (uv.y > 1) uv.y -= 1;
                
                color = tex2D(_MainTex, uv);
                
                float2 dist = i.uv - _Conjunction;
                if (abs(dist.x) <= _BorderWidth || abs(dist.y) <= _BorderWidth) color = _BorderColor;
                
                color = lerp(tex2D(_MainTex, i.uv), color, _Activated);
                
                // color = float4(uv.x, uv.y, 0, 1);
                
                return color;
            }
            ENDCG
        }
    }
}
