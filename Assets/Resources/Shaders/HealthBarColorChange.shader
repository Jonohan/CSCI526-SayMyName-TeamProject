Shader "Custom/HealthBarColorChange"
{
     Properties
    {
        _MainTex("Main Texture", 2D) = "white" {}
        _IntendedColor("Intended Health Bar Color", Color) = (1,1,1,1) // The color we want the health bar to be
        _CurrentColor("Current Health Bar Color", Color) = (0.65, 0.08, 0.73, 1) // The color that needs to be changed accordingly
        _Sensitivity("Sensitivity", Range(0,1)) = 0.25 // Color match sensitivity
    }
    SubShader
    {
        Tags { "RenderType"="Overlay" }
        
         // Used for UI elements
        Cull Off
        ZWrite Off
        ZTest Always
        Blend SrcAlpha OneMinusSrcAlpha
        
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            sampler2D _MainTex;
            fixed4 _IntendedColor;
            fixed4 _CurrentColor;
            float _Sensitivity;
            
            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };
            

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                fixed4 col = tex2D(_MainTex, i.uv);
                if( all( abs( col - _CurrentColor) < _Sensitivity) ) // Should be changed
                    return _IntendedColor;
                
                return col;
            }
            ENDCG
        }
    }
}
