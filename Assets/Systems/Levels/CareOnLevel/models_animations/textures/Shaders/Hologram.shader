  Shader "Example/Rim" {
    Properties {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Texture", 2D) = "white" {}
        _RimColor ("Rim Color", Color) = (1,1,1,1)
        _RimStrenght ("Rim Strenght", Range(0.5,8.0)) = 3.0
        _HoloSize ("Hologramwave Size", float) = 0
        _HoloSpeed ("Hologramwave Speed", float) = 0
        _HoloStrenght ("Hologramwave Strenght", float) = 0
    }
    SubShader {
      Tags { "RenderType" = "Opaque" }
      CGPROGRAM
      #pragma surface surf Lambert

      struct Input {
          float2 uv_MainTex;
          float3 viewDir;
          float3 worldPos;
      };

      sampler2D _MainTex;
      float4 _RimColor;
      float4 _Color;
      float _RimStrenght;
      float _HoloSize;
      float _HoloSpeed;
      float _HoloStrenght;

      void surf (Input IN, inout SurfaceOutput o)
      {

          o.Albedo = tex2D (_MainTex, IN.uv_MainTex).rgb * _Color;
          half rim = 1.0 - saturate(dot (normalize(IN.viewDir), o.Normal));
          float holoAnimation = IN.worldPos.y + (_HoloSpeed * _Time.y);
          o.Emission = _RimColor.rgb * pow (rim, _RimStrenght) * ((frac((holoAnimation*_HoloSize)) - _HoloStrenght));
      }
      ENDCG
    } 
    Fallback "Diffuse"
  }