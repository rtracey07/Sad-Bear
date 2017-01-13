Shader "Post/GrayScale" 
{
	Properties 
	{
		_MainTex ("Main", 2D) = "white" {}
		_RampTex ("Ramp", 2D) = "grayscaleRamp" {}
	}

	SubShader 
	{
		Pass 
		{
			//Write to everything.
			ZTest Always 
			Cull Off 
			ZWrite Off

            CGPROGRAM
            //Vertex function inherent in the cgincludes file.
            #pragma vertex vert_img
            #pragma fragment frag

            #include "UnityCG.cginc"

            //This is an image effect, so MainTex will be passed in by Blit.
            uniform sampler2D _MainTex;
            	
            //Gray ramp by default.	
            uniform sampler2D _RampTex;		

            fixed4 frag (v2f_img i) : COLOR
            {
            	//Sample the texture.
           		fixed4 original = tex2D(_MainTex, i.uv);

           		//Get pixel luminance and store in faked UV.
           		//V-value is arbitrary, since RampTex V is 1.
           		half2 grayscale = half2(Luminance(original.rgb), 0.5);

           		//Interpolate between coloured and grey, based off of alpha value.
           		//Our alpha mask pass will come into use here.
     			fixed4 output = lerp(tex2D(_RampTex, grayscale), original, original.a);

           		//Add the original alpha.
           		output.a = original.a;

           		return output;
            }
            ENDCG
		}
	}
	Fallback off
}