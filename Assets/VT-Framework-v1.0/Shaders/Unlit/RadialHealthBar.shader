// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "RadialHealthBar"
{
	Properties
	{
		[HideInInspector] _AlphaCutoff("Alpha Cutoff ", Range(0, 1)) = 0.5
		[HideInInspector] _EmissionColor("Emission Color", Color) = (1,1,1,1)
		[ASEBegin]_MainTex("Main Texture", 2D) = "white" {}
		_FillAmount("Fill Amount", Range( 0 , 1)) = 1
		_FillColor("Fill Color", Color) = (0,1,0.07320976,1)
		_Radius("Radius", Range( 0 , 0.5)) = 0.361
		_LineWidth("Line Width", Range( 0 , 0.5)) = 0.03999162
		_Segments("Segments", Range( 1 , 32)) = 90
		_RotationAngle("Rotation Angle", Range( 0 , 360)) = 90
		_OutlineColor("Outline Color", Color) = (0,1,0.07320976,1)
		_OutlineWidth("Outline Width", Range( 0 , 0.5)) = 0.03999162
		_AASmooth("AA Smooth", Range( 0 , 0.05)) = 0.01
		[ASEEnd]_SegmentDistance("Segment Distance", Range( 0.01 , 0.5)) = 0.01
		[HideInInspector] _texcoord( "", 2D ) = "white" {}

		//_TessPhongStrength( "Tess Phong Strength", Range( 0, 1 ) ) = 0.5
		//_TessValue( "Tess Max Tessellation", Range( 1, 32 ) ) = 16
		//_TessMin( "Tess Min Distance", Float ) = 10
		//_TessMax( "Tess Max Distance", Float ) = 25
		//_TessEdgeLength ( "Tess Edge length", Range( 2, 50 ) ) = 16
		//_TessMaxDisp( "Tess Max Displacement", Float ) = 25
	}

	SubShader
	{
		LOD 0

		
		Tags { "RenderPipeline"="UniversalPipeline" "RenderType"="Transparent" "Queue"="Transparent" }
		
		Cull Back
		AlphaToMask Off
		HLSLINCLUDE
		#pragma target 2.0

		float4 FixedTess( float tessValue )
		{
			return tessValue;
		}
		
		float CalcDistanceTessFactor (float4 vertex, float minDist, float maxDist, float tess, float4x4 o2w, float3 cameraPos )
		{
			float3 wpos = mul(o2w,vertex).xyz;
			float dist = distance (wpos, cameraPos);
			float f = clamp(1.0 - (dist - minDist) / (maxDist - minDist), 0.01, 1.0) * tess;
			return f;
		}

		float4 CalcTriEdgeTessFactors (float3 triVertexFactors)
		{
			float4 tess;
			tess.x = 0.5 * (triVertexFactors.y + triVertexFactors.z);
			tess.y = 0.5 * (triVertexFactors.x + triVertexFactors.z);
			tess.z = 0.5 * (triVertexFactors.x + triVertexFactors.y);
			tess.w = (triVertexFactors.x + triVertexFactors.y + triVertexFactors.z) / 3.0f;
			return tess;
		}

		float CalcEdgeTessFactor (float3 wpos0, float3 wpos1, float edgeLen, float3 cameraPos, float4 scParams )
		{
			float dist = distance (0.5 * (wpos0+wpos1), cameraPos);
			float len = distance(wpos0, wpos1);
			float f = max(len * scParams.y / (edgeLen * dist), 1.0);
			return f;
		}

		float DistanceFromPlane (float3 pos, float4 plane)
		{
			float d = dot (float4(pos,1.0f), plane);
			return d;
		}

		bool WorldViewFrustumCull (float3 wpos0, float3 wpos1, float3 wpos2, float cullEps, float4 planes[6] )
		{
			float4 planeTest;
			planeTest.x = (( DistanceFromPlane(wpos0, planes[0]) > -cullEps) ? 1.0f : 0.0f ) +
						  (( DistanceFromPlane(wpos1, planes[0]) > -cullEps) ? 1.0f : 0.0f ) +
						  (( DistanceFromPlane(wpos2, planes[0]) > -cullEps) ? 1.0f : 0.0f );
			planeTest.y = (( DistanceFromPlane(wpos0, planes[1]) > -cullEps) ? 1.0f : 0.0f ) +
						  (( DistanceFromPlane(wpos1, planes[1]) > -cullEps) ? 1.0f : 0.0f ) +
						  (( DistanceFromPlane(wpos2, planes[1]) > -cullEps) ? 1.0f : 0.0f );
			planeTest.z = (( DistanceFromPlane(wpos0, planes[2]) > -cullEps) ? 1.0f : 0.0f ) +
						  (( DistanceFromPlane(wpos1, planes[2]) > -cullEps) ? 1.0f : 0.0f ) +
						  (( DistanceFromPlane(wpos2, planes[2]) > -cullEps) ? 1.0f : 0.0f );
			planeTest.w = (( DistanceFromPlane(wpos0, planes[3]) > -cullEps) ? 1.0f : 0.0f ) +
						  (( DistanceFromPlane(wpos1, planes[3]) > -cullEps) ? 1.0f : 0.0f ) +
						  (( DistanceFromPlane(wpos2, planes[3]) > -cullEps) ? 1.0f : 0.0f );
			return !all (planeTest);
		}

		float4 DistanceBasedTess( float4 v0, float4 v1, float4 v2, float tess, float minDist, float maxDist, float4x4 o2w, float3 cameraPos )
		{
			float3 f;
			f.x = CalcDistanceTessFactor (v0,minDist,maxDist,tess,o2w,cameraPos);
			f.y = CalcDistanceTessFactor (v1,minDist,maxDist,tess,o2w,cameraPos);
			f.z = CalcDistanceTessFactor (v2,minDist,maxDist,tess,o2w,cameraPos);

			return CalcTriEdgeTessFactors (f);
		}

		float4 EdgeLengthBasedTess( float4 v0, float4 v1, float4 v2, float edgeLength, float4x4 o2w, float3 cameraPos, float4 scParams )
		{
			float3 pos0 = mul(o2w,v0).xyz;
			float3 pos1 = mul(o2w,v1).xyz;
			float3 pos2 = mul(o2w,v2).xyz;
			float4 tess;
			tess.x = CalcEdgeTessFactor (pos1, pos2, edgeLength, cameraPos, scParams);
			tess.y = CalcEdgeTessFactor (pos2, pos0, edgeLength, cameraPos, scParams);
			tess.z = CalcEdgeTessFactor (pos0, pos1, edgeLength, cameraPos, scParams);
			tess.w = (tess.x + tess.y + tess.z) / 3.0f;
			return tess;
		}

		float4 EdgeLengthBasedTessCull( float4 v0, float4 v1, float4 v2, float edgeLength, float maxDisplacement, float4x4 o2w, float3 cameraPos, float4 scParams, float4 planes[6] )
		{
			float3 pos0 = mul(o2w,v0).xyz;
			float3 pos1 = mul(o2w,v1).xyz;
			float3 pos2 = mul(o2w,v2).xyz;
			float4 tess;

			if (WorldViewFrustumCull(pos0, pos1, pos2, maxDisplacement, planes))
			{
				tess = 0.0f;
			}
			else
			{
				tess.x = CalcEdgeTessFactor (pos1, pos2, edgeLength, cameraPos, scParams);
				tess.y = CalcEdgeTessFactor (pos2, pos0, edgeLength, cameraPos, scParams);
				tess.z = CalcEdgeTessFactor (pos0, pos1, edgeLength, cameraPos, scParams);
				tess.w = (tess.x + tess.y + tess.z) / 3.0f;
			}
			return tess;
		}
		ENDHLSL

		
		Pass
		{
			
			Name "Forward"
			Tags { "LightMode"="UniversalForward" }
			
			Blend SrcAlpha OneMinusSrcAlpha, One OneMinusSrcAlpha
			ZWrite Off
			ZTest LEqual
			Offset 0 , 0
			ColorMask RGBA
			

			HLSLPROGRAM
			#pragma multi_compile_instancing
			#define ASE_SRP_VERSION 80200

			#pragma prefer_hlslcc gles
			#pragma exclude_renderers d3d11_9x

			#pragma vertex vert
			#pragma fragment frag

			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/UnityInstancing.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"

			#if ASE_SRP_VERSION <= 70108
			#define REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR
			#endif

			

			struct VertexInput
			{
				float4 vertex : POSITION;
				float3 ase_normal : NORMAL;
				float4 ase_texcoord : TEXCOORD0;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct VertexOutput
			{
				float4 clipPos : SV_POSITION;
				#if defined(ASE_NEEDS_FRAG_WORLD_POSITION)
				float3 worldPos : TEXCOORD0;
				#endif
				#if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR) && defined(ASE_NEEDS_FRAG_SHADOWCOORDS)
				float4 shadowCoord : TEXCOORD1;
				#endif
				#ifdef ASE_FOG
				float fogFactor : TEXCOORD2;
				#endif
				float4 ase_texcoord3 : TEXCOORD3;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};

			CBUFFER_START(UnityPerMaterial)
			float4 _OutlineColor;
			float4 _FillColor;
			float4 _MainTex_ST;
			float _Radius;
			float _OutlineWidth;
			float _AASmooth;
			float _LineWidth;
			float _RotationAngle;
			float _FillAmount;
			float _Segments;
			float _SegmentDistance;
			#ifdef TESSELLATION_ON
				float _TessPhongStrength;
				float _TessValue;
				float _TessMin;
				float _TessMax;
				float _TessEdgeLength;
				float _TessMaxDisp;
			#endif
			CBUFFER_END
			sampler2D _MainTex;


						
			VertexOutput VertexFunction ( VertexInput v  )
			{
				VertexOutput o = (VertexOutput)0;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_TRANSFER_INSTANCE_ID(v, o);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);

				o.ase_texcoord3.xy = v.ase_texcoord.xy;
				
				//setting value to unused interpolator channels and avoid initialization warnings
				o.ase_texcoord3.zw = 0;
				#ifdef ASE_ABSOLUTE_VERTEX_POS
					float3 defaultVertexValue = v.vertex.xyz;
				#else
					float3 defaultVertexValue = float3(0, 0, 0);
				#endif
				float3 vertexValue = defaultVertexValue;
				#ifdef ASE_ABSOLUTE_VERTEX_POS
					v.vertex.xyz = vertexValue;
				#else
					v.vertex.xyz += vertexValue;
				#endif
				v.ase_normal = v.ase_normal;

				float3 positionWS = TransformObjectToWorld( v.vertex.xyz );
				float4 positionCS = TransformWorldToHClip( positionWS );

				#if defined(ASE_NEEDS_FRAG_WORLD_POSITION)
				o.worldPos = positionWS;
				#endif
				#if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR) && defined(ASE_NEEDS_FRAG_SHADOWCOORDS)
				VertexPositionInputs vertexInput = (VertexPositionInputs)0;
				vertexInput.positionWS = positionWS;
				vertexInput.positionCS = positionCS;
				o.shadowCoord = GetShadowCoord( vertexInput );
				#endif
				#ifdef ASE_FOG
				o.fogFactor = ComputeFogFactor( positionCS.z );
				#endif
				o.clipPos = positionCS;
				return o;
			}

			#if defined(TESSELLATION_ON)
			struct VertexControl
			{
				float4 vertex : INTERNALTESSPOS;
				float3 ase_normal : NORMAL;
				float4 ase_texcoord : TEXCOORD0;

				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct TessellationFactors
			{
				float edge[3] : SV_TessFactor;
				float inside : SV_InsideTessFactor;
			};

			VertexControl vert ( VertexInput v )
			{
				VertexControl o;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_TRANSFER_INSTANCE_ID(v, o);
				o.vertex = v.vertex;
				o.ase_normal = v.ase_normal;
				o.ase_texcoord = v.ase_texcoord;
				return o;
			}

			TessellationFactors TessellationFunction (InputPatch<VertexControl,3> v)
			{
				TessellationFactors o;
				float4 tf = 1;
				float tessValue = _TessValue; float tessMin = _TessMin; float tessMax = _TessMax;
				float edgeLength = _TessEdgeLength; float tessMaxDisp = _TessMaxDisp;
				#if defined(ASE_FIXED_TESSELLATION)
				tf = FixedTess( tessValue );
				#elif defined(ASE_DISTANCE_TESSELLATION)
				tf = DistanceBasedTess(v[0].vertex, v[1].vertex, v[2].vertex, tessValue, tessMin, tessMax, GetObjectToWorldMatrix(), _WorldSpaceCameraPos );
				#elif defined(ASE_LENGTH_TESSELLATION)
				tf = EdgeLengthBasedTess(v[0].vertex, v[1].vertex, v[2].vertex, edgeLength, GetObjectToWorldMatrix(), _WorldSpaceCameraPos, _ScreenParams );
				#elif defined(ASE_LENGTH_CULL_TESSELLATION)
				tf = EdgeLengthBasedTessCull(v[0].vertex, v[1].vertex, v[2].vertex, edgeLength, tessMaxDisp, GetObjectToWorldMatrix(), _WorldSpaceCameraPos, _ScreenParams, unity_CameraWorldClipPlanes );
				#endif
				o.edge[0] = tf.x; o.edge[1] = tf.y; o.edge[2] = tf.z; o.inside = tf.w;
				return o;
			}

			[domain("tri")]
			[partitioning("fractional_odd")]
			[outputtopology("triangle_cw")]
			[patchconstantfunc("TessellationFunction")]
			[outputcontrolpoints(3)]
			VertexControl HullFunction(InputPatch<VertexControl, 3> patch, uint id : SV_OutputControlPointID)
			{
			   return patch[id];
			}

			[domain("tri")]
			VertexOutput DomainFunction(TessellationFactors factors, OutputPatch<VertexControl, 3> patch, float3 bary : SV_DomainLocation)
			{
				VertexInput o = (VertexInput) 0;
				o.vertex = patch[0].vertex * bary.x + patch[1].vertex * bary.y + patch[2].vertex * bary.z;
				o.ase_normal = patch[0].ase_normal * bary.x + patch[1].ase_normal * bary.y + patch[2].ase_normal * bary.z;
				o.ase_texcoord = patch[0].ase_texcoord * bary.x + patch[1].ase_texcoord * bary.y + patch[2].ase_texcoord * bary.z;
				#if defined(ASE_PHONG_TESSELLATION)
				float3 pp[3];
				for (int i = 0; i < 3; ++i)
					pp[i] = o.vertex.xyz - patch[i].ase_normal * (dot(o.vertex.xyz, patch[i].ase_normal) - dot(patch[i].vertex.xyz, patch[i].ase_normal));
				float phongStrength = _TessPhongStrength;
				o.vertex.xyz = phongStrength * (pp[0]*bary.x + pp[1]*bary.y + pp[2]*bary.z) + (1.0f-phongStrength) * o.vertex.xyz;
				#endif
				UNITY_TRANSFER_INSTANCE_ID(patch[0], o);
				return VertexFunction(o);
			}
			#else
			VertexOutput vert ( VertexInput v )
			{
				return VertexFunction( v );
			}
			#endif

			half4 frag ( VertexOutput IN  ) : SV_Target
			{
				UNITY_SETUP_INSTANCE_ID( IN );
				UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX( IN );

				#if defined(ASE_NEEDS_FRAG_WORLD_POSITION)
				float3 WorldPosition = IN.worldPos;
				#endif
				float4 ShadowCoords = float4( 0, 0, 0, 0 );

				#if defined(ASE_NEEDS_FRAG_SHADOWCOORDS)
					#if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR)
						ShadowCoords = IN.shadowCoord;
					#elif defined(MAIN_LIGHT_CALCULATE_SHADOWS)
						ShadowCoords = TransformWorldToShadowCoord( WorldPosition );
					#endif
				#endif
				float2 texCoord1_g34 = IN.ase_texcoord3.xy * float2( 1,1 ) + float2( -0.5,-0.5 );
				float clampResult8_g34 = clamp( _Radius , 0.0 , 0.5 );
				float clampResult23_g33 = clamp( _OutlineWidth , 0.0 , 0.5 );
				float clampResult21_g33 = clamp( _AASmooth , 0.0 , 0.05 );
				float clampResult19_g33 = clamp( ( 1.0 - ( ( abs( ( length( texCoord1_g34 ) - clampResult8_g34 ) ) - clampResult23_g33 ) / clampResult21_g33 ) ) , 0.0 , 1.0 );
				float2 texCoord1_g32 = IN.ase_texcoord3.xy * float2( 1,1 ) + float2( -0.5,-0.5 );
				float clampResult8_g32 = clamp( _Radius , 0.0 , 0.5 );
				float clampResult23_g31 = clamp( _LineWidth , 0.0 , 0.5 );
				float clampResult21_g31 = clamp( _AASmooth , 0.0 , 0.05 );
				float clampResult19_g31 = clamp( ( 1.0 - ( ( abs( ( length( texCoord1_g32 ) - clampResult8_g32 ) ) - clampResult23_g31 ) / clampResult21_g31 ) ) , 0.0 , 1.0 );
				float2 texCoord1_g29 = IN.ase_texcoord3.xy * float2( 1,1 ) + float2( -0.5,-0.5 );
				float clampResult15_g29 = clamp( _RotationAngle , 0.0 , 360.0 );
				float cos5_g29 = cos( (0.0 + (clampResult15_g29 - 0.0) * (6.282 - 0.0) / (360.0 - 0.0)) );
				float sin5_g29 = sin( (0.0 + (clampResult15_g29 - 0.0) * (6.282 - 0.0) / (360.0 - 0.0)) );
				float2 rotator5_g29 = mul( texCoord1_g29 - float2( 0,0 ) , float2x2( cos5_g29 , -sin5_g29 , sin5_g29 , cos5_g29 )) + float2( 0,0 );
				float2 break6_g29 = rotator5_g29;
				float clampResult16_g29 = clamp( _FillAmount , 0.0 , 1.0 );
				float lerpResult12_g29 = lerp( 0.0 , 6.282 , clampResult16_g29);
				float clampResult11_g29 = clamp( ( ( ( atan2( break6_g29.x , break6_g29.y ) + 3.141 ) - lerpResult12_g29 ) / 0.0005 ) , 0.0 , 1.0 );
				float2 texCoord32_g30 = IN.ase_texcoord3.xy * float2( 1,1 ) + float2( -0.5,-0.5 );
				float clampResult52_g30 = clamp( _RotationAngle , 0.0 , 360.0 );
				float cos34_g30 = cos( (0.0 + (clampResult52_g30 - 0.0) * (6.282 - 0.0) / (360.0 - 0.0)) );
				float sin34_g30 = sin( (0.0 + (clampResult52_g30 - 0.0) * (6.282 - 0.0) / (360.0 - 0.0)) );
				float2 rotator34_g30 = mul( texCoord32_g30 - float2( 0,0 ) , float2x2( cos34_g30 , -sin34_g30 , sin34_g30 , cos34_g30 )) + float2( 0,0 );
				float2 break35_g30 = rotator34_g30;
				float clampResult41_g30 = clamp( _Segments , 1.0 , 32.0 );
				float temp_output_5_0_g30 = ( 6.282 / round( clampResult41_g30 ) );
				float temp_output_6_0_g30 = ( temp_output_5_0_g30 / 2.0 );
				float clampResult51_g30 = clamp( _SegmentDistance , 0.01 , 0.5 );
				float clampResult47_g30 = clamp( _AASmooth , 0.0 , 0.05 );
				float clampResult26_g30 = clamp( ( 1.0 - ( ( ( abs( sin( ( ( ( ( atan2( break35_g30.x , break35_g30.y ) + 3.141 ) + temp_output_6_0_g30 ) % temp_output_5_0_g30 ) - temp_output_6_0_g30 ) ) ) * length( texCoord32_g30 ) ) - clampResult51_g30 ) / clampResult47_g30 ) ) , 0.0 , 1.0 );
				float clampResult48 = clamp( ( ( clampResult19_g31 - clampResult11_g29 ) - ( clampResult26_g30 * ( round( _Segments ) > 1.0 ? 1.0 : 0.0 ) ) ) , 0.0 , 1.0 );
				float clampResult197 = clamp( ( clampResult19_g33 - clampResult48 ) , 0.0 , 1.0 );
				float temp_output_246_0 = ( _OutlineColor.a * clampResult197 );
				float temp_output_247_0 = ( _FillColor.a * clampResult48 );
				float2 uv_MainTex = IN.ase_texcoord3.xy * _MainTex_ST.xy + _MainTex_ST.zw;
				float4 tex2DNode89 = tex2D( _MainTex, uv_MainTex );
				
				float3 BakedAlbedo = 0;
				float3 BakedEmission = 0;
				float3 Color = ( ( ( _OutlineColor * temp_output_246_0 ) + ( _FillColor * temp_output_247_0 ) ) * tex2DNode89 ).rgb;
				float Alpha = ( tex2DNode89.a * ( temp_output_246_0 + temp_output_247_0 ) );
				float AlphaClipThreshold = 0.5;
				float AlphaClipThresholdShadow = 0.5;

				#ifdef _ALPHATEST_ON
					clip( Alpha - AlphaClipThreshold );
				#endif

				#ifdef LOD_FADE_CROSSFADE
					LODDitheringTransition( IN.clipPos.xyz, unity_LODFade.x );
				#endif

				#ifdef ASE_FOG
					Color = MixFog( Color, IN.fogFactor );
				#endif

				return half4( Color, Alpha );
			}

			ENDHLSL
		}

		
		Pass
		{
			
			Name "ShadowCaster"
			Tags { "LightMode"="ShadowCaster" }

			ZWrite On
			ZTest LEqual
			AlphaToMask Off

			HLSLPROGRAM
			#pragma multi_compile_instancing
			#define ASE_SRP_VERSION 80200

			#pragma prefer_hlslcc gles
			#pragma exclude_renderers d3d11_9x

			#pragma vertex vert
			#pragma fragment frag

			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"

			

			struct VertexInput
			{
				float4 vertex : POSITION;
				float3 ase_normal : NORMAL;
				float4 ase_texcoord : TEXCOORD0;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct VertexOutput
			{
				float4 clipPos : SV_POSITION;
				#if defined(ASE_NEEDS_FRAG_WORLD_POSITION)
				float3 worldPos : TEXCOORD0;
				#endif
				#if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR) && defined(ASE_NEEDS_FRAG_SHADOWCOORDS)
				float4 shadowCoord : TEXCOORD1;
				#endif
				float4 ase_texcoord2 : TEXCOORD2;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};

			CBUFFER_START(UnityPerMaterial)
			float4 _OutlineColor;
			float4 _FillColor;
			float4 _MainTex_ST;
			float _Radius;
			float _OutlineWidth;
			float _AASmooth;
			float _LineWidth;
			float _RotationAngle;
			float _FillAmount;
			float _Segments;
			float _SegmentDistance;
			#ifdef TESSELLATION_ON
				float _TessPhongStrength;
				float _TessValue;
				float _TessMin;
				float _TessMax;
				float _TessEdgeLength;
				float _TessMaxDisp;
			#endif
			CBUFFER_END
			sampler2D _MainTex;


			
			float3 _LightDirection;

			VertexOutput VertexFunction( VertexInput v )
			{
				VertexOutput o;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_TRANSFER_INSTANCE_ID(v, o);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO( o );

				o.ase_texcoord2.xy = v.ase_texcoord.xy;
				
				//setting value to unused interpolator channels and avoid initialization warnings
				o.ase_texcoord2.zw = 0;
				#ifdef ASE_ABSOLUTE_VERTEX_POS
					float3 defaultVertexValue = v.vertex.xyz;
				#else
					float3 defaultVertexValue = float3(0, 0, 0);
				#endif
				float3 vertexValue = defaultVertexValue;
				#ifdef ASE_ABSOLUTE_VERTEX_POS
					v.vertex.xyz = vertexValue;
				#else
					v.vertex.xyz += vertexValue;
				#endif

				v.ase_normal = v.ase_normal;

				float3 positionWS = TransformObjectToWorld( v.vertex.xyz );

				#if defined(ASE_NEEDS_FRAG_WORLD_POSITION)
				o.worldPos = positionWS;
				#endif

				float3 normalWS = TransformObjectToWorldDir( v.ase_normal );

				float4 clipPos = TransformWorldToHClip( ApplyShadowBias( positionWS, normalWS, _LightDirection ) );

				#if UNITY_REVERSED_Z
					clipPos.z = min(clipPos.z, clipPos.w * UNITY_NEAR_CLIP_VALUE);
				#else
					clipPos.z = max(clipPos.z, clipPos.w * UNITY_NEAR_CLIP_VALUE);
				#endif

				#if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR) && defined(ASE_NEEDS_FRAG_SHADOWCOORDS)
					VertexPositionInputs vertexInput = (VertexPositionInputs)0;
					vertexInput.positionWS = positionWS;
					vertexInput.positionCS = clipPos;
					o.shadowCoord = GetShadowCoord( vertexInput );
				#endif
				o.clipPos = clipPos;

				return o;
			}
			
			#if defined(TESSELLATION_ON)
			struct VertexControl
			{
				float4 vertex : INTERNALTESSPOS;
				float3 ase_normal : NORMAL;
				float4 ase_texcoord : TEXCOORD0;

				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct TessellationFactors
			{
				float edge[3] : SV_TessFactor;
				float inside : SV_InsideTessFactor;
			};

			VertexControl vert ( VertexInput v )
			{
				VertexControl o;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_TRANSFER_INSTANCE_ID(v, o);
				o.vertex = v.vertex;
				o.ase_normal = v.ase_normal;
				o.ase_texcoord = v.ase_texcoord;
				return o;
			}

			TessellationFactors TessellationFunction (InputPatch<VertexControl,3> v)
			{
				TessellationFactors o;
				float4 tf = 1;
				float tessValue = _TessValue; float tessMin = _TessMin; float tessMax = _TessMax;
				float edgeLength = _TessEdgeLength; float tessMaxDisp = _TessMaxDisp;
				#if defined(ASE_FIXED_TESSELLATION)
				tf = FixedTess( tessValue );
				#elif defined(ASE_DISTANCE_TESSELLATION)
				tf = DistanceBasedTess(v[0].vertex, v[1].vertex, v[2].vertex, tessValue, tessMin, tessMax, GetObjectToWorldMatrix(), _WorldSpaceCameraPos );
				#elif defined(ASE_LENGTH_TESSELLATION)
				tf = EdgeLengthBasedTess(v[0].vertex, v[1].vertex, v[2].vertex, edgeLength, GetObjectToWorldMatrix(), _WorldSpaceCameraPos, _ScreenParams );
				#elif defined(ASE_LENGTH_CULL_TESSELLATION)
				tf = EdgeLengthBasedTessCull(v[0].vertex, v[1].vertex, v[2].vertex, edgeLength, tessMaxDisp, GetObjectToWorldMatrix(), _WorldSpaceCameraPos, _ScreenParams, unity_CameraWorldClipPlanes );
				#endif
				o.edge[0] = tf.x; o.edge[1] = tf.y; o.edge[2] = tf.z; o.inside = tf.w;
				return o;
			}

			[domain("tri")]
			[partitioning("fractional_odd")]
			[outputtopology("triangle_cw")]
			[patchconstantfunc("TessellationFunction")]
			[outputcontrolpoints(3)]
			VertexControl HullFunction(InputPatch<VertexControl, 3> patch, uint id : SV_OutputControlPointID)
			{
			   return patch[id];
			}

			[domain("tri")]
			VertexOutput DomainFunction(TessellationFactors factors, OutputPatch<VertexControl, 3> patch, float3 bary : SV_DomainLocation)
			{
				VertexInput o = (VertexInput) 0;
				o.vertex = patch[0].vertex * bary.x + patch[1].vertex * bary.y + patch[2].vertex * bary.z;
				o.ase_normal = patch[0].ase_normal * bary.x + patch[1].ase_normal * bary.y + patch[2].ase_normal * bary.z;
				o.ase_texcoord = patch[0].ase_texcoord * bary.x + patch[1].ase_texcoord * bary.y + patch[2].ase_texcoord * bary.z;
				#if defined(ASE_PHONG_TESSELLATION)
				float3 pp[3];
				for (int i = 0; i < 3; ++i)
					pp[i] = o.vertex.xyz - patch[i].ase_normal * (dot(o.vertex.xyz, patch[i].ase_normal) - dot(patch[i].vertex.xyz, patch[i].ase_normal));
				float phongStrength = _TessPhongStrength;
				o.vertex.xyz = phongStrength * (pp[0]*bary.x + pp[1]*bary.y + pp[2]*bary.z) + (1.0f-phongStrength) * o.vertex.xyz;
				#endif
				UNITY_TRANSFER_INSTANCE_ID(patch[0], o);
				return VertexFunction(o);
			}
			#else
			VertexOutput vert ( VertexInput v )
			{
				return VertexFunction( v );
			}
			#endif

			half4 frag(VertexOutput IN  ) : SV_TARGET
			{
				UNITY_SETUP_INSTANCE_ID( IN );
				UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX( IN );

				#if defined(ASE_NEEDS_FRAG_WORLD_POSITION)
				float3 WorldPosition = IN.worldPos;
				#endif
				float4 ShadowCoords = float4( 0, 0, 0, 0 );

				#if defined(ASE_NEEDS_FRAG_SHADOWCOORDS)
					#if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR)
						ShadowCoords = IN.shadowCoord;
					#elif defined(MAIN_LIGHT_CALCULATE_SHADOWS)
						ShadowCoords = TransformWorldToShadowCoord( WorldPosition );
					#endif
				#endif

				float2 uv_MainTex = IN.ase_texcoord2.xy * _MainTex_ST.xy + _MainTex_ST.zw;
				float4 tex2DNode89 = tex2D( _MainTex, uv_MainTex );
				float2 texCoord1_g34 = IN.ase_texcoord2.xy * float2( 1,1 ) + float2( -0.5,-0.5 );
				float clampResult8_g34 = clamp( _Radius , 0.0 , 0.5 );
				float clampResult23_g33 = clamp( _OutlineWidth , 0.0 , 0.5 );
				float clampResult21_g33 = clamp( _AASmooth , 0.0 , 0.05 );
				float clampResult19_g33 = clamp( ( 1.0 - ( ( abs( ( length( texCoord1_g34 ) - clampResult8_g34 ) ) - clampResult23_g33 ) / clampResult21_g33 ) ) , 0.0 , 1.0 );
				float2 texCoord1_g32 = IN.ase_texcoord2.xy * float2( 1,1 ) + float2( -0.5,-0.5 );
				float clampResult8_g32 = clamp( _Radius , 0.0 , 0.5 );
				float clampResult23_g31 = clamp( _LineWidth , 0.0 , 0.5 );
				float clampResult21_g31 = clamp( _AASmooth , 0.0 , 0.05 );
				float clampResult19_g31 = clamp( ( 1.0 - ( ( abs( ( length( texCoord1_g32 ) - clampResult8_g32 ) ) - clampResult23_g31 ) / clampResult21_g31 ) ) , 0.0 , 1.0 );
				float2 texCoord1_g29 = IN.ase_texcoord2.xy * float2( 1,1 ) + float2( -0.5,-0.5 );
				float clampResult15_g29 = clamp( _RotationAngle , 0.0 , 360.0 );
				float cos5_g29 = cos( (0.0 + (clampResult15_g29 - 0.0) * (6.282 - 0.0) / (360.0 - 0.0)) );
				float sin5_g29 = sin( (0.0 + (clampResult15_g29 - 0.0) * (6.282 - 0.0) / (360.0 - 0.0)) );
				float2 rotator5_g29 = mul( texCoord1_g29 - float2( 0,0 ) , float2x2( cos5_g29 , -sin5_g29 , sin5_g29 , cos5_g29 )) + float2( 0,0 );
				float2 break6_g29 = rotator5_g29;
				float clampResult16_g29 = clamp( _FillAmount , 0.0 , 1.0 );
				float lerpResult12_g29 = lerp( 0.0 , 6.282 , clampResult16_g29);
				float clampResult11_g29 = clamp( ( ( ( atan2( break6_g29.x , break6_g29.y ) + 3.141 ) - lerpResult12_g29 ) / 0.0005 ) , 0.0 , 1.0 );
				float2 texCoord32_g30 = IN.ase_texcoord2.xy * float2( 1,1 ) + float2( -0.5,-0.5 );
				float clampResult52_g30 = clamp( _RotationAngle , 0.0 , 360.0 );
				float cos34_g30 = cos( (0.0 + (clampResult52_g30 - 0.0) * (6.282 - 0.0) / (360.0 - 0.0)) );
				float sin34_g30 = sin( (0.0 + (clampResult52_g30 - 0.0) * (6.282 - 0.0) / (360.0 - 0.0)) );
				float2 rotator34_g30 = mul( texCoord32_g30 - float2( 0,0 ) , float2x2( cos34_g30 , -sin34_g30 , sin34_g30 , cos34_g30 )) + float2( 0,0 );
				float2 break35_g30 = rotator34_g30;
				float clampResult41_g30 = clamp( _Segments , 1.0 , 32.0 );
				float temp_output_5_0_g30 = ( 6.282 / round( clampResult41_g30 ) );
				float temp_output_6_0_g30 = ( temp_output_5_0_g30 / 2.0 );
				float clampResult51_g30 = clamp( _SegmentDistance , 0.01 , 0.5 );
				float clampResult47_g30 = clamp( _AASmooth , 0.0 , 0.05 );
				float clampResult26_g30 = clamp( ( 1.0 - ( ( ( abs( sin( ( ( ( ( atan2( break35_g30.x , break35_g30.y ) + 3.141 ) + temp_output_6_0_g30 ) % temp_output_5_0_g30 ) - temp_output_6_0_g30 ) ) ) * length( texCoord32_g30 ) ) - clampResult51_g30 ) / clampResult47_g30 ) ) , 0.0 , 1.0 );
				float clampResult48 = clamp( ( ( clampResult19_g31 - clampResult11_g29 ) - ( clampResult26_g30 * ( round( _Segments ) > 1.0 ? 1.0 : 0.0 ) ) ) , 0.0 , 1.0 );
				float clampResult197 = clamp( ( clampResult19_g33 - clampResult48 ) , 0.0 , 1.0 );
				float temp_output_246_0 = ( _OutlineColor.a * clampResult197 );
				float temp_output_247_0 = ( _FillColor.a * clampResult48 );
				
				float Alpha = ( tex2DNode89.a * ( temp_output_246_0 + temp_output_247_0 ) );
				float AlphaClipThreshold = 0.5;
				float AlphaClipThresholdShadow = 0.5;

				#ifdef _ALPHATEST_ON
					#ifdef _ALPHATEST_SHADOW_ON
						clip(Alpha - AlphaClipThresholdShadow);
					#else
						clip(Alpha - AlphaClipThreshold);
					#endif
				#endif

				#ifdef LOD_FADE_CROSSFADE
					LODDitheringTransition( IN.clipPos.xyz, unity_LODFade.x );
				#endif
				return 0;
			}

			ENDHLSL
		}

		
		Pass
		{
			
			Name "DepthOnly"
			Tags { "LightMode"="DepthOnly" }

			ZWrite On
			ColorMask 0
			AlphaToMask Off

			HLSLPROGRAM
			#pragma multi_compile_instancing
			#define ASE_SRP_VERSION 80200

			#pragma prefer_hlslcc gles
			#pragma exclude_renderers d3d11_9x

			#pragma vertex vert
			#pragma fragment frag

			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"

			

			struct VertexInput
			{
				float4 vertex : POSITION;
				float3 ase_normal : NORMAL;
				float4 ase_texcoord : TEXCOORD0;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct VertexOutput
			{
				float4 clipPos : SV_POSITION;
				#if defined(ASE_NEEDS_FRAG_WORLD_POSITION)
				float3 worldPos : TEXCOORD0;
				#endif
				#if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR) && defined(ASE_NEEDS_FRAG_SHADOWCOORDS)
				float4 shadowCoord : TEXCOORD1;
				#endif
				float4 ase_texcoord2 : TEXCOORD2;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};

			CBUFFER_START(UnityPerMaterial)
			float4 _OutlineColor;
			float4 _FillColor;
			float4 _MainTex_ST;
			float _Radius;
			float _OutlineWidth;
			float _AASmooth;
			float _LineWidth;
			float _RotationAngle;
			float _FillAmount;
			float _Segments;
			float _SegmentDistance;
			#ifdef TESSELLATION_ON
				float _TessPhongStrength;
				float _TessValue;
				float _TessMin;
				float _TessMax;
				float _TessEdgeLength;
				float _TessMaxDisp;
			#endif
			CBUFFER_END
			sampler2D _MainTex;


			
			VertexOutput VertexFunction( VertexInput v  )
			{
				VertexOutput o = (VertexOutput)0;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_TRANSFER_INSTANCE_ID(v, o);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);

				o.ase_texcoord2.xy = v.ase_texcoord.xy;
				
				//setting value to unused interpolator channels and avoid initialization warnings
				o.ase_texcoord2.zw = 0;
				#ifdef ASE_ABSOLUTE_VERTEX_POS
					float3 defaultVertexValue = v.vertex.xyz;
				#else
					float3 defaultVertexValue = float3(0, 0, 0);
				#endif
				float3 vertexValue = defaultVertexValue;
				#ifdef ASE_ABSOLUTE_VERTEX_POS
					v.vertex.xyz = vertexValue;
				#else
					v.vertex.xyz += vertexValue;
				#endif

				v.ase_normal = v.ase_normal;

				float3 positionWS = TransformObjectToWorld( v.vertex.xyz );

				#if defined(ASE_NEEDS_FRAG_WORLD_POSITION)
				o.worldPos = positionWS;
				#endif

				o.clipPos = TransformWorldToHClip( positionWS );
				#if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR) && defined(ASE_NEEDS_FRAG_SHADOWCOORDS)
					VertexPositionInputs vertexInput = (VertexPositionInputs)0;
					vertexInput.positionWS = positionWS;
					vertexInput.positionCS = clipPos;
					o.shadowCoord = GetShadowCoord( vertexInput );
				#endif
				return o;
			}

			#if defined(TESSELLATION_ON)
			struct VertexControl
			{
				float4 vertex : INTERNALTESSPOS;
				float3 ase_normal : NORMAL;
				float4 ase_texcoord : TEXCOORD0;

				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct TessellationFactors
			{
				float edge[3] : SV_TessFactor;
				float inside : SV_InsideTessFactor;
			};

			VertexControl vert ( VertexInput v )
			{
				VertexControl o;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_TRANSFER_INSTANCE_ID(v, o);
				o.vertex = v.vertex;
				o.ase_normal = v.ase_normal;
				o.ase_texcoord = v.ase_texcoord;
				return o;
			}

			TessellationFactors TessellationFunction (InputPatch<VertexControl,3> v)
			{
				TessellationFactors o;
				float4 tf = 1;
				float tessValue = _TessValue; float tessMin = _TessMin; float tessMax = _TessMax;
				float edgeLength = _TessEdgeLength; float tessMaxDisp = _TessMaxDisp;
				#if defined(ASE_FIXED_TESSELLATION)
				tf = FixedTess( tessValue );
				#elif defined(ASE_DISTANCE_TESSELLATION)
				tf = DistanceBasedTess(v[0].vertex, v[1].vertex, v[2].vertex, tessValue, tessMin, tessMax, GetObjectToWorldMatrix(), _WorldSpaceCameraPos );
				#elif defined(ASE_LENGTH_TESSELLATION)
				tf = EdgeLengthBasedTess(v[0].vertex, v[1].vertex, v[2].vertex, edgeLength, GetObjectToWorldMatrix(), _WorldSpaceCameraPos, _ScreenParams );
				#elif defined(ASE_LENGTH_CULL_TESSELLATION)
				tf = EdgeLengthBasedTessCull(v[0].vertex, v[1].vertex, v[2].vertex, edgeLength, tessMaxDisp, GetObjectToWorldMatrix(), _WorldSpaceCameraPos, _ScreenParams, unity_CameraWorldClipPlanes );
				#endif
				o.edge[0] = tf.x; o.edge[1] = tf.y; o.edge[2] = tf.z; o.inside = tf.w;
				return o;
			}

			[domain("tri")]
			[partitioning("fractional_odd")]
			[outputtopology("triangle_cw")]
			[patchconstantfunc("TessellationFunction")]
			[outputcontrolpoints(3)]
			VertexControl HullFunction(InputPatch<VertexControl, 3> patch, uint id : SV_OutputControlPointID)
			{
			   return patch[id];
			}

			[domain("tri")]
			VertexOutput DomainFunction(TessellationFactors factors, OutputPatch<VertexControl, 3> patch, float3 bary : SV_DomainLocation)
			{
				VertexInput o = (VertexInput) 0;
				o.vertex = patch[0].vertex * bary.x + patch[1].vertex * bary.y + patch[2].vertex * bary.z;
				o.ase_normal = patch[0].ase_normal * bary.x + patch[1].ase_normal * bary.y + patch[2].ase_normal * bary.z;
				o.ase_texcoord = patch[0].ase_texcoord * bary.x + patch[1].ase_texcoord * bary.y + patch[2].ase_texcoord * bary.z;
				#if defined(ASE_PHONG_TESSELLATION)
				float3 pp[3];
				for (int i = 0; i < 3; ++i)
					pp[i] = o.vertex.xyz - patch[i].ase_normal * (dot(o.vertex.xyz, patch[i].ase_normal) - dot(patch[i].vertex.xyz, patch[i].ase_normal));
				float phongStrength = _TessPhongStrength;
				o.vertex.xyz = phongStrength * (pp[0]*bary.x + pp[1]*bary.y + pp[2]*bary.z) + (1.0f-phongStrength) * o.vertex.xyz;
				#endif
				UNITY_TRANSFER_INSTANCE_ID(patch[0], o);
				return VertexFunction(o);
			}
			#else
			VertexOutput vert ( VertexInput v )
			{
				return VertexFunction( v );
			}
			#endif

			half4 frag(VertexOutput IN  ) : SV_TARGET
			{
				UNITY_SETUP_INSTANCE_ID(IN);
				UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX( IN );

				#if defined(ASE_NEEDS_FRAG_WORLD_POSITION)
				float3 WorldPosition = IN.worldPos;
				#endif
				float4 ShadowCoords = float4( 0, 0, 0, 0 );

				#if defined(ASE_NEEDS_FRAG_SHADOWCOORDS)
					#if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR)
						ShadowCoords = IN.shadowCoord;
					#elif defined(MAIN_LIGHT_CALCULATE_SHADOWS)
						ShadowCoords = TransformWorldToShadowCoord( WorldPosition );
					#endif
				#endif

				float2 uv_MainTex = IN.ase_texcoord2.xy * _MainTex_ST.xy + _MainTex_ST.zw;
				float4 tex2DNode89 = tex2D( _MainTex, uv_MainTex );
				float2 texCoord1_g34 = IN.ase_texcoord2.xy * float2( 1,1 ) + float2( -0.5,-0.5 );
				float clampResult8_g34 = clamp( _Radius , 0.0 , 0.5 );
				float clampResult23_g33 = clamp( _OutlineWidth , 0.0 , 0.5 );
				float clampResult21_g33 = clamp( _AASmooth , 0.0 , 0.05 );
				float clampResult19_g33 = clamp( ( 1.0 - ( ( abs( ( length( texCoord1_g34 ) - clampResult8_g34 ) ) - clampResult23_g33 ) / clampResult21_g33 ) ) , 0.0 , 1.0 );
				float2 texCoord1_g32 = IN.ase_texcoord2.xy * float2( 1,1 ) + float2( -0.5,-0.5 );
				float clampResult8_g32 = clamp( _Radius , 0.0 , 0.5 );
				float clampResult23_g31 = clamp( _LineWidth , 0.0 , 0.5 );
				float clampResult21_g31 = clamp( _AASmooth , 0.0 , 0.05 );
				float clampResult19_g31 = clamp( ( 1.0 - ( ( abs( ( length( texCoord1_g32 ) - clampResult8_g32 ) ) - clampResult23_g31 ) / clampResult21_g31 ) ) , 0.0 , 1.0 );
				float2 texCoord1_g29 = IN.ase_texcoord2.xy * float2( 1,1 ) + float2( -0.5,-0.5 );
				float clampResult15_g29 = clamp( _RotationAngle , 0.0 , 360.0 );
				float cos5_g29 = cos( (0.0 + (clampResult15_g29 - 0.0) * (6.282 - 0.0) / (360.0 - 0.0)) );
				float sin5_g29 = sin( (0.0 + (clampResult15_g29 - 0.0) * (6.282 - 0.0) / (360.0 - 0.0)) );
				float2 rotator5_g29 = mul( texCoord1_g29 - float2( 0,0 ) , float2x2( cos5_g29 , -sin5_g29 , sin5_g29 , cos5_g29 )) + float2( 0,0 );
				float2 break6_g29 = rotator5_g29;
				float clampResult16_g29 = clamp( _FillAmount , 0.0 , 1.0 );
				float lerpResult12_g29 = lerp( 0.0 , 6.282 , clampResult16_g29);
				float clampResult11_g29 = clamp( ( ( ( atan2( break6_g29.x , break6_g29.y ) + 3.141 ) - lerpResult12_g29 ) / 0.0005 ) , 0.0 , 1.0 );
				float2 texCoord32_g30 = IN.ase_texcoord2.xy * float2( 1,1 ) + float2( -0.5,-0.5 );
				float clampResult52_g30 = clamp( _RotationAngle , 0.0 , 360.0 );
				float cos34_g30 = cos( (0.0 + (clampResult52_g30 - 0.0) * (6.282 - 0.0) / (360.0 - 0.0)) );
				float sin34_g30 = sin( (0.0 + (clampResult52_g30 - 0.0) * (6.282 - 0.0) / (360.0 - 0.0)) );
				float2 rotator34_g30 = mul( texCoord32_g30 - float2( 0,0 ) , float2x2( cos34_g30 , -sin34_g30 , sin34_g30 , cos34_g30 )) + float2( 0,0 );
				float2 break35_g30 = rotator34_g30;
				float clampResult41_g30 = clamp( _Segments , 1.0 , 32.0 );
				float temp_output_5_0_g30 = ( 6.282 / round( clampResult41_g30 ) );
				float temp_output_6_0_g30 = ( temp_output_5_0_g30 / 2.0 );
				float clampResult51_g30 = clamp( _SegmentDistance , 0.01 , 0.5 );
				float clampResult47_g30 = clamp( _AASmooth , 0.0 , 0.05 );
				float clampResult26_g30 = clamp( ( 1.0 - ( ( ( abs( sin( ( ( ( ( atan2( break35_g30.x , break35_g30.y ) + 3.141 ) + temp_output_6_0_g30 ) % temp_output_5_0_g30 ) - temp_output_6_0_g30 ) ) ) * length( texCoord32_g30 ) ) - clampResult51_g30 ) / clampResult47_g30 ) ) , 0.0 , 1.0 );
				float clampResult48 = clamp( ( ( clampResult19_g31 - clampResult11_g29 ) - ( clampResult26_g30 * ( round( _Segments ) > 1.0 ? 1.0 : 0.0 ) ) ) , 0.0 , 1.0 );
				float clampResult197 = clamp( ( clampResult19_g33 - clampResult48 ) , 0.0 , 1.0 );
				float temp_output_246_0 = ( _OutlineColor.a * clampResult197 );
				float temp_output_247_0 = ( _FillColor.a * clampResult48 );
				
				float Alpha = ( tex2DNode89.a * ( temp_output_246_0 + temp_output_247_0 ) );
				float AlphaClipThreshold = 0.5;

				#ifdef _ALPHATEST_ON
					clip(Alpha - AlphaClipThreshold);
				#endif

				#ifdef LOD_FADE_CROSSFADE
					LODDitheringTransition( IN.clipPos.xyz, unity_LODFade.x );
				#endif
				return 0;
			}
			ENDHLSL
		}

	
	}
	CustomEditor "UnityEditor.ShaderGraph.PBRMasterGUI"
	Fallback "Hidden/InternalErrorShader"
	
}
/*ASEBEGIN
Version=18800
1920;0;1920;1059;448.9401;625.2397;1.660885;True;True
Node;AmplifyShaderEditor.RangedFloatNode;50;31.90824,189.8686;Inherit;False;Property;_RotationAngle;Rotation Angle;6;0;Create;True;0;0;0;False;0;False;90;0;0;360;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;227;30.35268,80.85717;Inherit;False;Property;_AASmooth;AA Smooth;9;0;Create;True;0;0;0;False;0;False;0.01;0.01;0;0.05;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;225;34.24494,430.6404;Inherit;False;Property;_Segments;Segments;5;0;Create;True;0;0;0;False;0;False;90;6.49414;1;32;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;34;36.58583,305.8516;Inherit;False;Property;_FillAmount;Fill Amount;1;0;Create;True;0;0;0;False;0;False;1;0.6117647;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.WireNode;285;433.3616,359.3756;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RoundOpNode;305;372.0755,682.778;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.WireNode;300;386.2692,338.1831;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;9;31.88144,-156.5391;Inherit;False;Property;_Radius;Radius;3;0;Create;True;0;0;0;False;0;False;0.361;0.3314706;0;0.5;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;289;33.16238,516.0942;Inherit;False;Property;_SegmentDistance;Segment Distance;10;0;Create;True;0;0;0;False;0;False;0.01;0.03152942;0.01;0.5;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;13;28.3201,-15.75276;Inherit;False;Property;_LineWidth;Line Width;4;0;Create;True;0;0;0;False;0;False;0.03999162;0.073;0;0.5;0;1;FLOAT;0
Node;AmplifyShaderEditor.FunctionNode;303;494.0964,449.9597;Inherit;True;Segmentize;-1;;30;98fbd0030d201b74286f3ac0edde817d;0;4;40;FLOAT;1;False;50;FLOAT;0.01;False;42;FLOAT;0;False;46;FLOAT;0.01;False;1;FLOAT;0
Node;AmplifyShaderEditor.Compare;291;495.4358,682.1021;Inherit;False;2;4;0;FLOAT;0;False;1;FLOAT;1;False;2;FLOAT;1;False;3;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.FunctionNode;297;493.2236,223.594;Inherit;True;RadialFill;-1;;29;d869075b4676d6c4688aca39753ec382;0;2;13;FLOAT;0;False;14;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.FunctionNode;306;501.1059,-24.39884;Inherit;True;Donut;-1;;31;1290b21a108018e47aef93a3f451992e;0;3;2;FLOAT;0;False;22;FLOAT;0;False;20;FLOAT;0.01;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;81;862.8035,624.5269;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;47;893.5479,119.73;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;79;1098.52,356.3256;Inherit;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;173;31.5409,-281.0544;Inherit;False;Property;_OutlineWidth;Outline Width;8;0;Create;True;0;0;0;False;0;False;0.03999162;0.13482;0;0.5;0;1;FLOAT;0
Node;AmplifyShaderEditor.FunctionNode;307;502.9187,-296.9621;Inherit;True;Donut;-1;;33;1290b21a108018e47aef93a3f451992e;0;3;2;FLOAT;0;False;22;FLOAT;0;False;20;FLOAT;0.01;False;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;48;1335.975,358.3016;Inherit;True;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;191;1704.264,112.9199;Inherit;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.WireNode;301;1708.939,617.1597;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;32;1728.501,418.9828;Inherit;False;Property;_FillColor;Fill Color;2;0;Create;True;0;0;0;False;0;False;0,1,0.07320976,1;0.3019608,0.8745099,0.7803922,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;247;1962.828,543.7557;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;197;1915.618,110.8316;Inherit;True;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;180;1897.145,-122.1061;Inherit;False;Property;_OutlineColor;Outline Color;7;0;Create;True;0;0;0;False;0;False;0,1,0.07320976,1;0,0,0,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;246;2147.442,23.61515;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.WireNode;302;2175.762,652.3633;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;264;2498.299,609.5096;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;89;2685.481,370.9066;Inherit;True;Property;_MainTex;Main Texture;0;0;Create;False;0;0;0;False;0;False;-1;d2fa16b4e8e25ab448ddb02430ef315c;d2fa16b4e8e25ab448ddb02430ef315c;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;266;3000.872,587.9261;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;90;2990.587,250.7421;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;181;2402.342,-113.6443;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;195;2743.531,127.097;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.WireNode;265;3269.496,506.1634;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;31;2201.468,423.5955;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;2;0,0;Float;False;False;-1;2;UnityEditor.ShaderGraph.PBRMasterGUI;0;1;New Amplify Shader;2992e84f91cbeb14eab234972e07ea9d;True;ShadowCaster;0;2;ShadowCaster;0;False;False;False;False;False;False;False;False;True;0;False;-1;True;0;False;-1;False;False;False;False;False;False;False;False;True;3;RenderPipeline=UniversalPipeline;RenderType=Opaque=RenderType;Queue=Geometry=Queue=0;True;0;0;False;False;False;False;False;False;False;False;True;0;False;-1;False;False;False;False;False;False;True;1;False;-1;True;3;False;-1;False;True;1;LightMode=ShadowCaster;False;0;Hidden/InternalErrorShader;0;0;Standard;0;False;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;3;0,0;Float;False;False;-1;2;UnityEditor.ShaderGraph.PBRMasterGUI;0;1;New Amplify Shader;2992e84f91cbeb14eab234972e07ea9d;True;DepthOnly;0;3;DepthOnly;0;False;False;False;False;False;False;False;False;True;0;False;-1;True;0;False;-1;False;False;False;False;False;False;False;False;True;3;RenderPipeline=UniversalPipeline;RenderType=Opaque=RenderType;Queue=Geometry=Queue=0;True;0;0;False;False;False;False;False;False;False;False;True;0;False;-1;False;True;False;False;False;False;0;False;-1;False;False;False;False;True;1;False;-1;False;False;True;1;LightMode=DepthOnly;False;0;Hidden/InternalErrorShader;0;0;Standard;0;False;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;0;0,0;Float;False;False;-1;2;UnityEditor.ShaderGraph.PBRMasterGUI;0;1;New Amplify Shader;2992e84f91cbeb14eab234972e07ea9d;True;ExtraPrePass;0;0;ExtraPrePass;5;False;False;False;False;False;False;False;False;True;0;False;-1;True;0;False;-1;False;False;False;False;False;False;False;False;True;3;RenderPipeline=UniversalPipeline;RenderType=Opaque=RenderType;Queue=Geometry=Queue=0;True;0;0;True;1;1;False;-1;0;False;-1;0;1;False;-1;0;False;-1;False;False;False;False;False;False;False;False;True;0;False;-1;True;True;True;True;True;0;False;-1;False;False;False;True;False;255;False;-1;255;False;-1;255;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;True;1;False;-1;True;3;False;-1;True;True;0;False;-1;0;False;-1;True;0;False;0;Hidden/InternalErrorShader;0;0;Standard;0;False;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;1;3366.822,250.544;Float;False;True;-1;2;UnityEditor.ShaderGraph.PBRMasterGUI;0;3;RadialHealthBar;2992e84f91cbeb14eab234972e07ea9d;True;Forward;0;1;Forward;8;False;False;False;False;False;False;False;False;True;0;False;-1;True;0;False;-1;False;False;False;False;False;False;False;False;True;3;RenderPipeline=UniversalPipeline;RenderType=Transparent=RenderType;Queue=Transparent=Queue=0;True;0;0;True;1;5;False;-1;10;False;-1;1;1;False;-1;10;False;-1;False;False;False;False;False;False;False;False;False;True;True;True;True;True;0;False;-1;False;False;False;True;False;255;False;-1;255;False;-1;255;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;True;2;False;-1;True;3;False;-1;True;True;0;False;-1;0;False;-1;True;1;LightMode=UniversalForward;False;0;Hidden/InternalErrorShader;0;0;Standard;22;Surface;1;  Blend;0;Two Sided;1;Cast Shadows;1;  Use Shadow Threshold;0;Receive Shadows;1;GPU Instancing;1;LOD CrossFade;0;Built-in Fog;0;DOTS Instancing;0;Meta Pass;0;Extra Pre Pass;0;Tessellation;0;  Phong;0;  Strength;0.5,False,-1;  Type;0;  Tess;16,False,-1;  Min;10,False,-1;  Max;25,False,-1;  Edge Length;16,False,-1;  Max Displacement;25,False,-1;Vertex Position,InvertActionOnDeselection;1;0;5;False;True;True;True;False;False;;False;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;4;0,0;Float;False;False;-1;2;UnityEditor.ShaderGraph.PBRMasterGUI;0;1;New Amplify Shader;2992e84f91cbeb14eab234972e07ea9d;True;Meta;0;4;Meta;0;False;False;False;False;False;False;False;False;True;0;False;-1;True;0;False;-1;False;False;False;False;False;False;False;False;True;3;RenderPipeline=UniversalPipeline;RenderType=Opaque=RenderType;Queue=Geometry=Queue=0;True;0;0;False;False;False;False;False;False;False;False;False;True;2;False;-1;False;False;False;False;False;False;False;False;True;1;LightMode=Meta;False;0;Hidden/InternalErrorShader;0;0;Standard;0;False;0
WireConnection;285;0;50;0
WireConnection;305;0;225;0
WireConnection;300;0;227;0
WireConnection;303;40;225;0
WireConnection;303;50;289;0
WireConnection;303;42;285;0
WireConnection;303;46;300;0
WireConnection;291;0;305;0
WireConnection;297;13;50;0
WireConnection;297;14;34;0
WireConnection;306;2;9;0
WireConnection;306;22;13;0
WireConnection;306;20;227;0
WireConnection;81;0;303;0
WireConnection;81;1;291;0
WireConnection;47;0;306;0
WireConnection;47;1;297;0
WireConnection;79;0;47;0
WireConnection;79;1;81;0
WireConnection;307;2;9;0
WireConnection;307;22;173;0
WireConnection;307;20;227;0
WireConnection;48;0;79;0
WireConnection;191;0;307;0
WireConnection;191;1;48;0
WireConnection;301;0;48;0
WireConnection;247;0;32;4
WireConnection;247;1;301;0
WireConnection;197;0;191;0
WireConnection;246;0;180;4
WireConnection;246;1;197;0
WireConnection;302;0;247;0
WireConnection;264;0;246;0
WireConnection;264;1;302;0
WireConnection;266;0;89;4
WireConnection;266;1;264;0
WireConnection;90;0;195;0
WireConnection;90;1;89;0
WireConnection;181;0;180;0
WireConnection;181;1;246;0
WireConnection;195;0;181;0
WireConnection;195;1;31;0
WireConnection;265;0;266;0
WireConnection;31;0;32;0
WireConnection;31;1;247;0
WireConnection;1;2;90;0
WireConnection;1;3;265;0
ASEEND*/
//CHKSM=7E219BEAD05BDA379E5D31B1B98BF5A8900D8DF9