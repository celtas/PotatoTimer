Shader "Custom/Highlight Text"
{
    Properties{
        _HighlightColor		("Highlight Color", Color) = (1,1,1,1)
        _Stencil		("Stencil ID", Float) = 10
    }
    SubShader {
        Tags { "RenderType"="Opacue" "Queue"="Transparent+40"}

        Stencil {
            Ref [_Stencil] // リファレンス値
            Comp Equal
        }

        Pass {
            ztest Always
            Blend SrcAlpha OneMinusSrcAlpha
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            
            half4 _HighlightColor;
            
            struct appdata {
                float4 vertex : POSITION;
            };
            struct v2f {
                float4 pos : SV_POSITION;
            };
            v2f vert(appdata v) {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                return o;
            }
            half4 frag(v2f i) : SV_Target {
                return _HighlightColor;
            }
            ENDCG
        }
    } 
}