Shader "Custom/MyWhiteMask" {
    Properties {
        _MainTex ("Texture", 2D) = "white" {}
        [Toggle] _UseWhite ("Show White", Float) = 0
    }
    
    SubShader {
        Tags { 
            "RenderType"="Transparent"
            "Queue"="Transparent"
        }
        
        Blend SrcAlpha OneMinusSrcAlpha
        Cull Off
        Pass {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                fixed4 color : COLOR;
            };

            struct v2f {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                fixed4 color : COLOR;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float _UseWhite;

            v2f vert (appdata v) {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.color = v.color; // �̳�Sprite Renderer����ɫ
                return o;
            }

            fixed4 frag (v2f i) : SV_Target {
                fixed4 col = tex2D(_MainTex, i.uv) * i.color;
                return lerp(col, fixed4(1,1,1,col.a), _UseWhite);
            }
            ENDCG
        }
    }
}