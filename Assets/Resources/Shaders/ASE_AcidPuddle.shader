// Made with Amplify Shader Editor v1.9.1.8
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "ASE_AcidPuddle"
{
	Properties
	{
		_MainText("MainText", 2D) = "white" {}
		[HDR]_MainColor("Main Color", Color) = (0,0.9667376,1.339623,0)
		_MaskSpeed("MaskSpeed", Vector) = (0.5,0.2,0,0)
		_MainSpeed("MainSpeed", Vector) = (0,0,0,0)
		_NoiseScale("NoiseScale", Float) = 4
		_PuddleRimMask("PuddleRimMask", 2D) = "black" {}
		_Float2("Float 2", Range( 0 , 2)) = 0

	}
	
	SubShader
	{
		
		
		Tags { "RenderType"="Transparent" "RenderQueue"="3000" }
	LOD 100

		CGINCLUDE
		#pragma target 3.0
		ENDCG
		Blend One One, SrcAlpha OneMinusSrcAlpha
		AlphaToMask Off
		Cull Back
		ColorMask RGBA
		ZWrite On
		ZTest LEqual
		Offset 0 , 0
		
		
		
		Pass
		{
			Name "Unlit"

			CGPROGRAM

			

			#ifndef UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX
			//only defining to not throw compilation error over Unity 5.5
			#define UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input)
			#endif
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile_instancing
			#include "UnityCG.cginc"
			#include "UnityShaderVariables.cginc"


			struct appdata
			{
				float4 vertex : POSITION;
				float4 color : COLOR;
				float4 ase_texcoord : TEXCOORD0;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};
			
			struct v2f
			{
				float4 vertex : SV_POSITION;
				#ifdef ASE_NEEDS_FRAG_WORLD_POSITION
				float3 worldPos : TEXCOORD0;
				#endif
				float4 ase_texcoord1 : TEXCOORD1;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};

			uniform float _NoiseScale;
			uniform float2 _MaskSpeed;
			uniform float4 _MainColor;
			uniform sampler2D _MainText;
			uniform float2 _MainSpeed;
			uniform float4 _MainText_ST;
			uniform sampler2D _PuddleRimMask;
			uniform float _Float2;
					float2 voronoihash13( float2 p )
					{
						
						p = float2( dot( p, float2( 127.1, 311.7 ) ), dot( p, float2( 269.5, 183.3 ) ) );
						return frac( sin( p ) *43758.5453);
					}
			
					float voronoi13( float2 v, float time, inout float2 id, inout float2 mr, float smoothness, inout float2 smoothId )
					{
						float2 n = floor( v );
						float2 f = frac( v );
						float F1 = 8.0;
						float F2 = 8.0; float2 mg = 0;
						for ( int j = -1; j <= 1; j++ )
						{
							for ( int i = -1; i <= 1; i++ )
						 	{
						 		float2 g = float2( i, j );
						 		float2 o = voronoihash13( n + g );
								o = ( sin( time + o * 6.2831 ) * 0.5 + 0.5 ); float2 r = f - g - o;
								float d = 0.5 * dot( r, r );
						 		if( d<F1 ) {
						 			F2 = F1;
						 			F1 = d; mg = g; mr = r; id = o;
						 		} else if( d<F2 ) {
						 			F2 = d;
						
						 		}
						 	}
						}
						return F1;
					}
			

			
			v2f vert ( appdata v )
			{
				v2f o;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
				UNITY_TRANSFER_INSTANCE_ID(v, o);

				o.ase_texcoord1.xy = v.ase_texcoord.xy;
				
				//setting value to unused interpolator channels and avoid initialization warnings
				o.ase_texcoord1.zw = 0;
				float3 vertexValue = float3(0, 0, 0);
				#if ASE_ABSOLUTE_VERTEX_POS
				vertexValue = v.vertex.xyz;
				#endif
				vertexValue = vertexValue;
				#if ASE_ABSOLUTE_VERTEX_POS
				v.vertex.xyz = vertexValue;
				#else
				v.vertex.xyz += vertexValue;
				#endif
				o.vertex = UnityObjectToClipPos(v.vertex);

				#ifdef ASE_NEEDS_FRAG_WORLD_POSITION
				o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
				#endif
				return o;
			}
			
			fixed4 frag (v2f i ) : SV_Target
			{
				UNITY_SETUP_INSTANCE_ID(i);
				UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(i);
				fixed4 finalColor;
				#ifdef ASE_NEEDS_FRAG_WORLD_POSITION
				float3 WorldPosition = i.worldPos;
				#endif
				float time13 = 0.0;
				float2 voronoiSmoothId13 = 0;
				float2 texCoord7 = i.ase_texcoord1.xy * float2( 1,1 ) + float2( 0,0 );
				float2 panner8 = ( 1.0 * _Time.y * _MaskSpeed + texCoord7);
				float2 coords13 = panner8 * _NoiseScale;
				float2 id13 = 0;
				float2 uv13 = 0;
				float voroi13 = voronoi13( coords13, time13, id13, uv13, 0, voronoiSmoothId13 );
				float2 uv_MainText = i.ase_texcoord1.xy * _MainText_ST.xy + _MainText_ST.zw;
				float2 panner5 = ( 1.0 * _Time.y * _MainSpeed + uv_MainText);
				float2 texCoord33 = i.ase_texcoord1.xy * float2( 1,1 ) + float2( 0,0 );
				float2 temp_output_140_0 = ( ( texCoord33 - float2( 0.5,0.5 ) ) * _Float2 );
				float2 appendResult105 = (float2(_CosTime.w , -_SinTime.w));
				float dotResult109 = dot( temp_output_140_0 , appendResult105 );
				float2 appendResult107 = (float2(_SinTime.w , _CosTime.w));
				float dotResult110 = dot( temp_output_140_0 , appendResult107 );
				float2 appendResult40 = (float2(( dotResult109 + 0.5 ) , ( dotResult110 + 0.5 )));
				
				
				finalColor = ( voroi13 * _MainColor * tex2D( _MainText, panner5 ) * tex2D( _PuddleRimMask, appendResult40 ).a );
				return finalColor;
			}
			ENDCG
		}
	}
	CustomEditor "ASEMaterialInspector"
	
	Fallback Off
}
/*ASEBEGIN
Version=19108
Node;AmplifyShaderEditor.CommentaryNode;118;-1258.098,1139.001;Inherit;False;360.7012;508.2986;Add 0.5 back to UV;4;51;43;50;45;;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;117;-2215.506,1145.429;Inherit;False;701.705;625.1709;2x2 Rotation matrix multiplicatioin ;4;41;49;106;108;;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;108;-2158.142,1529.028;Inherit;False;165;139;2x2 rotation row 2;1;107;;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;106;-2181.112,1229.285;Inherit;False;153;148;2x2 rotation row 1;1;105;;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;49;-1720.798,1484.201;Inherit;False;160;141;;1;110;New V;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;41;-1728.167,1209.429;Inherit;False;153;139;;1;109;New U;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;36;-2758.927,769.3758;Inherit;False;457.5693;172.8898;to make its center (0,0) instead of (0.5, 0.5).;2;33;130;Subtract (0.5, 0.5) from uv coord ;1,1,1,1;0;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;4;-1708,-268;Inherit;False;0;1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.PannerNode;5;-1414,-257;Inherit;True;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.PannerNode;8;-1492.531,240.7668;Inherit;True;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.Vector2Node;6;-1827,-71;Inherit;False;Property;_MainSpeed;MainSpeed;3;0;Create;True;0;0;0;False;0;False;0,0;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.Vector2Node;9;-1930.232,437.167;Inherit;False;Property;_MaskSpeed;MaskSpeed;2;0;Create;True;0;0;0;False;0;False;0.5,0.2;0.5,0.2;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.RangedFloatNode;15;-1424.338,605.9202;Inherit;False;Property;_NoiseScale;NoiseScale;4;0;Create;True;0;0;0;False;0;False;4;4;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;0;-109.0001,-40.9;Float;False;True;-1;2;ASEMaterialInspector;100;5;ASE_AcidPuddle;0770190933193b94aaa3065e307002fa;True;Unlit;0;0;Unlit;2;True;True;4;1;False;;1;False;;2;5;False;;10;False;;True;0;False;;0;False;;False;False;False;False;False;False;False;False;False;True;0;False;;False;True;0;False;;False;True;True;True;True;True;0;False;;False;False;False;False;False;False;False;True;False;0;False;;255;False;;255;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;False;True;1;False;;True;3;False;;True;True;0;False;;0;False;;True;2;RenderType=Transparent=RenderType;RenderQueue=3000;True;2;False;0;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;0;;0;0;Standard;1;Vertex Position,InvertActionOnDeselection;1;0;0;1;True;False;;False;0
Node;AmplifyShaderEditor.ColorNode;11;-1000.977,406.5159;Inherit;False;Property;_MainColor;Main Color;1;1;[HDR];Create;True;0;0;0;False;0;False;0,0.9667376,1.339623,0;0,1.339623,0.5759163,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.VoronoiNode;13;-993.6512,112.0443;Inherit;True;0;0;1;0;1;False;1;False;False;False;4;0;FLOAT2;0,0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;0;False;3;FLOAT;0;FLOAT2;1;FLOAT2;2
Node;AmplifyShaderEditor.TextureCoordinatesNode;7;-1846.333,272.6667;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;1;-1025,-272;Inherit;True;Property;_MainText;MainText;0;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.DotProductOpNode;110;-1688.136,1527.629;Inherit;False;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;105;-2154.11,1270.285;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.DynamicAppendNode;107;-2133.14,1571.028;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.DotProductOpNode;109;-1708.137,1248.227;Inherit;False;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;51;-1228.199,1540.5;Inherit;False;Constant;_Float1;Float 0;6;0;Create;True;0;0;0;False;0;False;0.5;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;50;-1072.797,1436.301;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;45;-1066.097,1218.801;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;43;-1225.998,1316.3;Inherit;False;Constant;_Float0;Float 0;6;0;Create;True;0;0;0;False;0;False;0.5;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.CosTime;137;-2588.857,1195.679;Inherit;False;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.NegateNode;119;-2400.633,1494.048;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SinTimeNode;136;-2574.854,1498.776;Inherit;False;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.DynamicAppendNode;40;-854.1998,1218.799;Inherit;True;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;33;-2708.927,819.3758;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;140;-2169.175,825.0023;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;130;-2431.434,824.2128;Inherit;False;2;0;FLOAT2;0,0;False;1;FLOAT2;0.5,0.5;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;141;-2596.453,975.9323;Inherit;False;Property;_Float2;Float 2;6;0;Create;True;0;0;0;False;0;False;0;0.9358715;0;2;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;16;-481.4419,1037.219;Inherit;True;Property;_PuddleRimMask;PuddleRimMask;5;0;Create;True;0;0;0;False;0;False;-1;None;37581444a88812a44b5d71558441d797;True;0;False;black;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;10;-371.3,-40.87127;Inherit;True;4;4;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;3;FLOAT;0;False;1;COLOR;0
WireConnection;5;0;4;0
WireConnection;5;2;6;0
WireConnection;8;0;7;0
WireConnection;8;2;9;0
WireConnection;0;0;10;0
WireConnection;13;0;8;0
WireConnection;13;2;15;0
WireConnection;1;1;5;0
WireConnection;110;0;140;0
WireConnection;110;1;107;0
WireConnection;105;0;137;4
WireConnection;105;1;119;0
WireConnection;107;0;136;4
WireConnection;107;1;137;4
WireConnection;109;0;140;0
WireConnection;109;1;105;0
WireConnection;50;0;110;0
WireConnection;50;1;51;0
WireConnection;45;0;109;0
WireConnection;45;1;43;0
WireConnection;119;0;136;4
WireConnection;40;0;45;0
WireConnection;40;1;50;0
WireConnection;140;0;130;0
WireConnection;140;1;141;0
WireConnection;130;0;33;0
WireConnection;16;1;40;0
WireConnection;10;0;13;0
WireConnection;10;1;11;0
WireConnection;10;2;1;0
WireConnection;10;3;16;4
ASEEND*/
//CHKSM=C7D543347FDAE4513A721425E5F4762A1D48DCE3