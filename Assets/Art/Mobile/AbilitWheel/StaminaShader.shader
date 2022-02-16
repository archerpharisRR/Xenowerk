Shader "Unlit/StaminaShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
         _ChargingColor("ChargingColor", color) = (1,1,1,1)
         _ChargedColor("ChargedColor", color) = (1,1,1,1)
        _Progress("Progress", range(0,1)) = 0.5
        _ProgressMaskTex("Texture", 2D) = "white"{}
    }
    SubShader
    {
        Tags {  "Queue" = "Transparent" "IgnoreProjector" = "True" "RenderType" = "Transparent" }
        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha
        Cull back
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float2 uvOrig : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            sampler2D _ProgressMaskTex;
            float4 _MainTex_ST;
            float _Progress;
            float4 _ChargingColor;
            float4 _ChargedColor;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.uvOrig = v.uv;
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                // sample the texture
                fixed4 col = tex2D(_MainTex, i.uv);
                fixed4 progressMaskTex = tex2D(_ProgressMaskTex, i.uv);
                // apply fog
                UNITY_APPLY_FOG(i.fogCoord, col);

                float2 coord = i.uvOrig - float2(1, 0.5);

                float2 dir = normalize(coord); 
                float2 downDir = float2(0, -1); 
                float angle = degrees(acos(dot(dir, downDir))); 
                float rangeMin = (180 - 45) / 2; 
                float rangeMax = (180 - 45) / 2 + 45; 
                float progressMask = progressMaskTex.x;
                if (angle > rangeMin && angle < rangeMax)  
                {
                    float normalizedRange = (angle - rangeMin)/45; 
                    if (_Progress < normalizedRange) 
                    {
                        progressMask = 0; 
                    }
                }

                if (progressMask > 0) 
                {


                    float4 progressCol = _ChargingColor;
                    if (_Progress == 1)
                    {
                        progressCol = _ChargedColor;
                    }
                    col = progressCol;
                }
  

                return col;
            }
            ENDCG
        }
    }
}
