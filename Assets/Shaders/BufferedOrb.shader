Shader "Project/BufferedOrb"
{
    Properties 
    {
    	_MainTex("Texture", 2D) = "white"{}
        _Color ("Color", Color) = (1,1,1,1)
        _IntersectColor ("IntersectColor", Color) = (1,1,1,1)
        _Rim("Rim Power", Range(0,3)) = 1
        _Speed("Speed", Range(0,1)) = 0
        _Width("Intersect Width", Float) = 0.1
    }

    Category 
    {
        Tags 
        {
        	"RenderType" = "Transparent" 
        	"Queue"="Transparent"
        }

        ZWrite Off 
        Cull Off

        SubShader 
        {

            Pass 
            {
            	//Additive pass to only back-facing vertices.
            	Cull Front
            	Blend One One

            	CGPROGRAM
            	#pragma vertex vert
            	#pragma fragment frag
            	#include "UnityCG.cginc"

				struct appdata
				{
					float4 vertex : POSITION;
					float2 uv : TEXCOORD0;
					float3 normal : NORMAL;
				};

				struct v2f
				{
					float2 screenuv : TEXCOORD1;
					float2 uv : TEXCOORD0;
					float3 viewDir : TEXCOORD2;
					float4 vertex : SV_POSITION;
					float depth : DEPTH;
					float3 normal : NORMAL;
				};

				float _Width;							//Width of the intersecting band.
				float _Rim;								//Fresnel rim intensity.
				float _Speed;							//Texture uv scroll speed.
				float4 _Color;							//Color value. RGB applied to fresenel. A used for mask.
				float4 _IntersectColor;					//Color value applied to our object intersection.
				sampler2D _CameraDepthNormalsTexture;	//Rendertexture from the camera, including the surface normals.

				sampler2D _MainTex;
				float4 _MainTex_ST;

				v2f vert(appdata v)
				{
					v2f o;
					o.uv = TRANSFORM_TEX(v.uv, _MainTex);
					o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);

					//UV coordinates in screen space, modulated to be [0-1].
					o.screenuv = ((o.vertex.xy / o.vertex.w) + 1)/2;

					// Depth of vertex, given by the z-pos in camera-space/farplane.
					o.depth = -mul(UNITY_MATRIX_MV, v.vertex).z *_ProjectionParams.w;	

					//Vertex normal in world-space.
                	o.normal = UnityObjectToWorldNormal(v.normal);

                	//Normalized direction from vertex to camera.
                	o.viewDir = normalize(UnityWorldSpaceViewDir(mul(unity_ObjectToWorld, v.vertex)));
                				
                	return o;
				}

				fixed4 frag(v2f i) : SV_TARGET
				{
					//Colorized texture value with UV-coordinates shifting over time.
					fixed4 c = tex2D(_MainTex, i.uv + _Time.y * _Speed) * _Color;

					//Fresnel effect.
					float rim = 1 - abs(dot(i.normal, i.viewDir)) * _Rim;

					//Decode a screen depth of pixel using the depth normals texture, rendered by camera.
					float screenDepth = DecodeFloatRG(tex2D(_CameraDepthNormalsTexture, i.screenuv).zw);

					//define the range, diff, by getting the difference of our two depth values. Use this to
					//create an intersection band on our geometry.
					float diff = screenDepth - i.depth;
					float intersect = 1 - smoothstep(0, _ProjectionParams.w * _Width, diff);

					//Apply rim to the texture only. Add a new intersection colour on top, allowing to
					//reduce alpha of _IntersectColor to get a color-bleed effect from intersecting objects.
					return c * rim + intersect * _IntersectColor;
				}
				ENDCG
            }

            Pass {
            	Name "Mask"
            	//Create an alpha mask for the entire object. This will be used by
            	//the screenspace effect for applying grayscale (or whatever other screenspace effect we choose).
                Colormask A                             
                Color [_Color]
            }

			Pass {
            	//Exactly the same as first pass, but only to front-facing vertices.
            	//By order of passes, this means any intersections in the 1st pass will
            	//be masked by the 2nd pass. But the 3rd will overwrite, allowing for colour
            	//on front-facing vertices.
            	Cull Back
            	Blend One One

            	CGPROGRAM
            	#pragma vertex vert
            	#pragma fragment frag
            	#include "UnityCG.cginc"

				struct appdata
				{
					float4 vertex : POSITION;
					float2 uv : TEXCOORD0;
					float3 normal : NORMAL;
				};

				struct v2f
				{
					float2 screenuv : TEXCOORD1;
					float2 uv : TEXCOORD0;
					float3 viewDir : TEXCOORD2;
					float4 vertex : SV_POSITION;
					float depth : DEPTH;
					float3 normal : NORMAL;
				};

				float _Width;							//Width of the intersecting band.
				float _Rim;								//Fresnel rim intensity.
				float _Speed;							//Texture uv scroll speed.
				float4 _Color;							//Color value. RGB applied to fresenel. A used for mask.
				float4 _IntersectColor;					//Color value applied to our object intersection.
				sampler2D _CameraDepthNormalsTexture;	//Rendertexture from the camera, including the surface normals.

				sampler2D _MainTex;
				float4 _MainTex_ST;

				v2f vert(appdata v)
				{
					v2f o;
					o.uv = TRANSFORM_TEX(v.uv, _MainTex);
					o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);

					//UV coordinates in screen space, modulated to be [0-1].
					o.screenuv = ((o.vertex.xy / o.vertex.w) + 1)/2;

					// Depth of vertex, given by the z-pos in camera-space/farplane.
					o.depth = -mul(UNITY_MATRIX_MV, v.vertex).z *_ProjectionParams.w;	

					//Vertex normal in world-space.
                	o.normal = UnityObjectToWorldNormal(v.normal);

                	//Normalized direction from vertex to camera.
                	o.viewDir = normalize(UnityWorldSpaceViewDir(mul(unity_ObjectToWorld, v.vertex)));
                				
                	return o;
				}

				fixed4 frag(v2f i) : SV_TARGET
				{
					//Colorized texture value with UV-coordinates shifting over time.
					fixed4 c = tex2D(_MainTex, i.uv + _Time.y * _Speed) * _Color;

					//Fresnel effect.
					float rim = 1 - abs(dot(i.normal, i.viewDir)) * _Rim;

					//Decode a screen depth of pixel using the depth normals texture, rendered by camera.
					float screenDepth = DecodeFloatRG(tex2D(_CameraDepthNormalsTexture, i.screenuv).zw);

					//define the range, diff, by getting the difference of our two depth values. Use this to
					//create an intersection band on our geometry.
					float diff = screenDepth - i.depth;
					float intersect = 1 - smoothstep(0, _ProjectionParams.w * _Width, diff);

					//Apply rim to the texture only. Add a new intersection colour on top, allowing to
					//reduce alpha of _IntersectColor to get a color-bleed effect from intersecting objects.
					return c * rim + intersect * _IntersectColor;
				}
				ENDCG
            }
        }
    } 
}