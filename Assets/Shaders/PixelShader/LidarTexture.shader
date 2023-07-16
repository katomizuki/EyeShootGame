Shader "Unlit/LidarTexture"
{
    Properties
    {
        _MainTex("Main Texture", 2D) = "white" { }
    }
    SubShader
    {
        Blend SrcAlpha OneMinusSrcAlpha
        
        // Textureをはるだけなので深度値いらない、カリングもいらない
        ZWrite off
        Cull off

        Pass
        {
            CGPROGRAM
            
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex: POSITION;
            };

            struct v2f
            {
                float4 position : SV_POSITION;
                float3 objPos : TEXCOORD0;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.position = UnityObjectToClipPos(v.vertex);
                o.objPos = v.vertex.xyz;
                return o;
            }

            sampler2D _MainTex;
	        float4 _MainTex_ST;

            fixed4 frag (v2f i) : SV_Target
            {
                // Lidar=>uv取れない。天井=>xz平面でobjPosをuv代わりにする。
                fixed4 col = tex2D(_MainTex, TRANSFORM_TEX(i.objPos.xz, _MainTex));
                return col;
            }

            ENDCG
        }
    }
}