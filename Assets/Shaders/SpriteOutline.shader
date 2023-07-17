Shader "Unlit/OutlineSprite"{
  Properties {
    _OutlineColor ("OutlineColor", Color) = (1, 1, 1, 1)
    _OutlineWidth ("OutlineWidth", Range(0, 10)) = 1
    _MainTex ("Texture", 2D) = "white" {}
  }

  SubShader {
    Tags {
      "RenderType"="Transparent"
      "Queue"="Transparent"
    }

    Blend SrcAlpha OneMinusSrcAlpha

    // 深度かきこみしない カリングしない=>Spriteなので必要ない
    ZWrite off
    Cull off

    Pass {
      CGPROGRAM

      #include "UnityCG.cginc"

      #pragma vertex vert
      #pragma fragment frag

      #define DIV_SQRT_2 0.70710678118
      static const float2 directions[8] = {float2(1, 0), float2(0, 1), float2(-1, 0), float2(0, -1),
          float2(DIV_SQRT_2, DIV_SQRT_2), float2(-DIV_SQRT_2, DIV_SQRT_2),
          float2(-DIV_SQRT_2, -DIV_SQRT_2), float2(DIV_SQRT_2, -DIV_SQRT_2)};


      sampler2D _MainTex;
      fixed4 _OutlineColor;
      float _OutlineWidth;

      struct appdata {
        float4 vertex : POSITION;
        float2 uv : TEXCOORD0;
      };

      struct v2f {
        float4 position : SV_POSITION;
        float2 uv : TEXCOORD0;
        float3 worldPos : TEXCOORD1;
      };

      v2f vert(appdata v){
        v2f o;
        o.position = UnityObjectToClipPos(v.vertex);
        o.worldPos = mul(unity_ObjectToWorld, v.vertex);
        o.uv = v.uv;
        return o;
      }

      float2 uvPerWorldUnit(float2 uv, float2 space) {
        // 周辺ピクセルとのuv差分を取得して絶対値をとる
        float2 uvPerPixelX = abs(ddx(uv)); 
        float2 uvPerPixelY = abs(ddy(uv));
        // space=>xy座標(canvasのため） 周辺ピクセルとのワールド座標差分の移動量を取る。
        float unitsPerPixelX = length(ddx(space));
        float unitsPerPixelY = length(ddy(space));
        // uvPerPixel / unitsPerPixel でワールド座標あたりのuvの変化量を取得する
        float2 uvPerUnitX = uvPerPixelX / unitsPerPixelX;
        float2 uvPerUnitY = uvPerPixelY / unitsPerPixelY;
        // x,y方向を考慮したuvの変化量を返す
        return (uvPerUnitX + uvPerUnitY);
      }

      fixed4 frag(v2f i) : SV_TARGET {
        fixed4 col = tex2D(_MainTex, i.uv);

        // ワールド座標の変化分に応じたuvの変化量を取得する
        float2 sampleDistance = uvPerWorldUnit(i.uv, i.worldPos.xy) * _OutlineWidth;

        float maxAlpha = 0;

        // for文をunrollする 8方向の
        [unroll]
        for(uint index = 0; index<8; index++) {
          // 8方向にuvをずらしてサンプリングする。その中で最大のアルファ値を取得する
          float2 sampleUV = i.uv + directions[index] * sampleDistance;
          // spriteの端っこだった場合(outlineを描くべきピクセルだった場合）Spriteの端っこ部分のアルファ値が
          // 最大になって代入される
          maxAlpha = max(maxAlpha, tex2D(_MainTex, sampleUV).a);
        }

        col.rgb = lerp(_OutlineColor.rgb, col.rgb, col.a);
        // maxAlphaが大きい=>Spriteの境界線=>Alpha値が大きくなりOutlineがひかれる。
        col.a = max(col.a, maxAlpha);

        return col;
      }
      ENDCG
    }
  }
}