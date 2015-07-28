// Compiled shader for PC, Mac & Linux Standalone, uncompressed size: 13.5KB

// Skipping shader variants that would not be included into build of current scene.

Shader "UI/Lit/Transparent" {
Properties {
 _Color ("Main Color", Color) = (1,1,1,1)
 _Specular ("Specular Color", Color) = (0,0,0,0)
 _MainTex ("Diffuse (RGB), Alpha (A)", 2D) = "white" { }
 _StencilComp ("Stencil Comparison", Float) = 8
 _Stencil ("Stencil ID", Float) = 0
 _StencilOp ("Stencil Operation", Float) = 0
 _StencilWriteMask ("Stencil Write Mask", Float) = 255
 _StencilReadMask ("Stencil Read Mask", Float) = 255
 _ColorMask ("Color Mask", Float) = 15
}
SubShader { 
 LOD 400
 Tags { "QUEUE"="Transparent" "IGNOREPROJECTOR"="true" "RenderType"="Transparent" "PreviewType"="Plane" }


 // Stats for Vertex shader:
 //      opengl : 9 math, 2 texture, 1 branch
 Pass {
  Name "FORWARD"
  Tags { "LIGHTMODE"="ForwardBase" "QUEUE"="Transparent" "IGNOREPROJECTOR"="true" "RenderType"="Transparent" "PreviewType"="Plane" }
  ZTest [unity_GUIZTestMode]
  ZWrite Off
  Cull Off
  Stencil {
   Ref [_Stencil]
   ReadMask [_StencilReadMask]
   WriteMask [_StencilWriteMask]
   Comp [_StencilComp]
   Pass [_StencilOp]
  }
  Blend SrcAlpha OneMinusSrcAlpha
  AlphaTest Greater 0
  ColorMask RGB
  Offset -1, -1
  GpuProgramID 58823
Program "vp" {
SubProgram "opengl " {
// Stats: 9 math, 2 textures, 1 branches
Keywords { "DIRECTIONAL" "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" "DYNAMICLIGHTMAP_OFF" }
"!!GLSL#version 120

#ifdef VERTEX

uniform mat4 _Object2World;
uniform mat4 _World2Object;
uniform vec4 _MainTex_ST;
varying vec2 xlv_TEXCOORD0;
varying vec3 xlv_TEXCOORD1;
varying vec3 xlv_TEXCOORD2;
varying vec4 xlv_COLOR0;
void main ()
{
  vec4 v_1;
  v_1.x = _World2Object[0].x;
  v_1.y = _World2Object[1].x;
  v_1.z = _World2Object[2].x;
  v_1.w = _World2Object[3].x;
  vec4 v_2;
  v_2.x = _World2Object[0].y;
  v_2.y = _World2Object[1].y;
  v_2.z = _World2Object[2].y;
  v_2.w = _World2Object[3].y;
  vec4 v_3;
  v_3.x = _World2Object[0].z;
  v_3.y = _World2Object[1].z;
  v_3.z = _World2Object[2].z;
  v_3.w = _World2Object[3].z;
  gl_Position = (gl_ModelViewProjectionMatrix * gl_Vertex);
  xlv_TEXCOORD0 = ((gl_MultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
  xlv_TEXCOORD1 = normalize(((
    (v_1.xyz * gl_Normal.x)
   + 
    (v_2.xyz * gl_Normal.y)
  ) + (v_3.xyz * gl_Normal.z)));
  xlv_TEXCOORD2 = (_Object2World * gl_Vertex).xyz;
  xlv_COLOR0 = gl_Color;
}


#endif
#ifdef FRAGMENT
uniform vec4 _WorldSpaceLightPos0;
uniform vec4 _LightColor0;
uniform sampler2D _MainTex;
uniform vec4 _Color;
varying vec2 xlv_TEXCOORD0;
varying vec3 xlv_TEXCOORD1;
varying vec4 xlv_COLOR0;
void main ()
{
  vec4 tmpvar_1;
  tmpvar_1 = ((texture2D (_MainTex, xlv_TEXCOORD0) * _Color) * xlv_COLOR0);
  vec4 c_2;
  c_2.xyz = ((tmpvar_1.xyz * max (0.0, 
    dot (normalize(xlv_TEXCOORD1), _WorldSpaceLightPos0.xyz)
  )) * _LightColor0.xyz);
  c_2.xyz = c_2.xyz;
  c_2.w = tmpvar_1.w;
  float x_3;
  x_3 = (tmpvar_1.w - 0.01);
  if ((x_3 < 0.0)) {
    discard;
  };
  gl_FragData[0] = c_2;
}


#endif
"
}
}
Program "fp" {
SubProgram "opengl " {
Keywords { "DIRECTIONAL" "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" "DYNAMICLIGHTMAP_OFF" }
"!!GLSL"
}
}
 }


 // Stats for Vertex shader:
 //      opengl : 15 avg math (9..22), 3 avg texture (2..4), 1 branch
 Pass {
  Name "FORWARD"
  Tags { "LIGHTMODE"="ForwardAdd" "QUEUE"="Transparent" "IGNOREPROJECTOR"="true" "RenderType"="Transparent" "PreviewType"="Plane" }
  ZTest [unity_GUIZTestMode]
  ZWrite Off
  Cull Off
  Stencil {
   Ref [_Stencil]
   ReadMask [_StencilReadMask]
   WriteMask [_StencilWriteMask]
   Comp [_StencilComp]
   Pass [_StencilOp]
  }
  Blend SrcAlpha One
  AlphaTest Greater 0
  ColorMask RGB
  Offset -1, -1
  GpuProgramID 77307
Program "vp" {
SubProgram "opengl " {
// Stats: 16 math, 3 textures, 1 branches
Keywords { "POINT" }
"!!GLSL#version 120

#ifdef VERTEX

uniform mat4 _Object2World;
uniform mat4 _World2Object;
uniform vec4 _MainTex_ST;
varying vec2 xlv_TEXCOORD0;
varying vec3 xlv_TEXCOORD1;
varying vec3 xlv_TEXCOORD2;
varying vec4 xlv_COLOR0;
void main ()
{
  vec4 v_1;
  v_1.x = _World2Object[0].x;
  v_1.y = _World2Object[1].x;
  v_1.z = _World2Object[2].x;
  v_1.w = _World2Object[3].x;
  vec4 v_2;
  v_2.x = _World2Object[0].y;
  v_2.y = _World2Object[1].y;
  v_2.z = _World2Object[2].y;
  v_2.w = _World2Object[3].y;
  vec4 v_3;
  v_3.x = _World2Object[0].z;
  v_3.y = _World2Object[1].z;
  v_3.z = _World2Object[2].z;
  v_3.w = _World2Object[3].z;
  gl_Position = (gl_ModelViewProjectionMatrix * gl_Vertex);
  xlv_TEXCOORD0 = ((gl_MultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
  xlv_TEXCOORD1 = normalize(((
    (v_1.xyz * gl_Normal.x)
   + 
    (v_2.xyz * gl_Normal.y)
  ) + (v_3.xyz * gl_Normal.z)));
  xlv_TEXCOORD2 = (_Object2World * gl_Vertex).xyz;
  xlv_COLOR0 = gl_Color;
}


#endif
#ifdef FRAGMENT
uniform vec4 _WorldSpaceLightPos0;
uniform vec4 _LightColor0;
uniform sampler2D _LightTexture0;
uniform mat4 _LightMatrix0;
uniform sampler2D _MainTex;
uniform vec4 _Color;
varying vec2 xlv_TEXCOORD0;
varying vec3 xlv_TEXCOORD1;
varying vec3 xlv_TEXCOORD2;
varying vec4 xlv_COLOR0;
void main ()
{
  vec4 tmpvar_1;
  tmpvar_1 = ((texture2D (_MainTex, xlv_TEXCOORD0) * _Color) * xlv_COLOR0);
  vec4 tmpvar_2;
  tmpvar_2.w = 1.0;
  tmpvar_2.xyz = xlv_TEXCOORD2;
  vec3 tmpvar_3;
  tmpvar_3 = (_LightMatrix0 * tmpvar_2).xyz;
  vec4 c_4;
  c_4.xyz = ((tmpvar_1.xyz * max (0.0, 
    dot (normalize(xlv_TEXCOORD1), normalize(normalize((_WorldSpaceLightPos0.xyz - xlv_TEXCOORD2))))
  )) * _LightColor0.xyz);
  c_4.xyz = (c_4.xyz * texture2D (_LightTexture0, vec2(dot (tmpvar_3, tmpvar_3))).w);
  c_4.w = tmpvar_1.w;
  float x_5;
  x_5 = (tmpvar_1.w - 0.01);
  if ((x_5 < 0.0)) {
    discard;
  };
  gl_FragData[0] = c_4;
}


#endif
"
}
SubProgram "opengl " {
// Stats: 9 math, 2 textures, 1 branches
Keywords { "DIRECTIONAL" }
"!!GLSL#version 120

#ifdef VERTEX

uniform mat4 _Object2World;
uniform mat4 _World2Object;
uniform vec4 _MainTex_ST;
varying vec2 xlv_TEXCOORD0;
varying vec3 xlv_TEXCOORD1;
varying vec3 xlv_TEXCOORD2;
varying vec4 xlv_COLOR0;
void main ()
{
  vec4 v_1;
  v_1.x = _World2Object[0].x;
  v_1.y = _World2Object[1].x;
  v_1.z = _World2Object[2].x;
  v_1.w = _World2Object[3].x;
  vec4 v_2;
  v_2.x = _World2Object[0].y;
  v_2.y = _World2Object[1].y;
  v_2.z = _World2Object[2].y;
  v_2.w = _World2Object[3].y;
  vec4 v_3;
  v_3.x = _World2Object[0].z;
  v_3.y = _World2Object[1].z;
  v_3.z = _World2Object[2].z;
  v_3.w = _World2Object[3].z;
  gl_Position = (gl_ModelViewProjectionMatrix * gl_Vertex);
  xlv_TEXCOORD0 = ((gl_MultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
  xlv_TEXCOORD1 = normalize(((
    (v_1.xyz * gl_Normal.x)
   + 
    (v_2.xyz * gl_Normal.y)
  ) + (v_3.xyz * gl_Normal.z)));
  xlv_TEXCOORD2 = (_Object2World * gl_Vertex).xyz;
  xlv_COLOR0 = gl_Color;
}


#endif
#ifdef FRAGMENT
uniform vec4 _WorldSpaceLightPos0;
uniform vec4 _LightColor0;
uniform sampler2D _MainTex;
uniform vec4 _Color;
varying vec2 xlv_TEXCOORD0;
varying vec3 xlv_TEXCOORD1;
varying vec4 xlv_COLOR0;
void main ()
{
  vec4 tmpvar_1;
  tmpvar_1 = ((texture2D (_MainTex, xlv_TEXCOORD0) * _Color) * xlv_COLOR0);
  vec4 c_2;
  c_2.xyz = ((tmpvar_1.xyz * max (0.0, 
    dot (normalize(xlv_TEXCOORD1), _WorldSpaceLightPos0.xyz)
  )) * _LightColor0.xyz);
  c_2.xyz = c_2.xyz;
  c_2.w = tmpvar_1.w;
  float x_3;
  x_3 = (tmpvar_1.w - 0.01);
  if ((x_3 < 0.0)) {
    discard;
  };
  gl_FragData[0] = c_2;
}


#endif
"
}
SubProgram "opengl " {
// Stats: 22 math, 4 textures, 1 branches
Keywords { "SPOT" }
"!!GLSL#version 120

#ifdef VERTEX

uniform mat4 _Object2World;
uniform mat4 _World2Object;
uniform vec4 _MainTex_ST;
varying vec2 xlv_TEXCOORD0;
varying vec3 xlv_TEXCOORD1;
varying vec3 xlv_TEXCOORD2;
varying vec4 xlv_COLOR0;
void main ()
{
  vec4 v_1;
  v_1.x = _World2Object[0].x;
  v_1.y = _World2Object[1].x;
  v_1.z = _World2Object[2].x;
  v_1.w = _World2Object[3].x;
  vec4 v_2;
  v_2.x = _World2Object[0].y;
  v_2.y = _World2Object[1].y;
  v_2.z = _World2Object[2].y;
  v_2.w = _World2Object[3].y;
  vec4 v_3;
  v_3.x = _World2Object[0].z;
  v_3.y = _World2Object[1].z;
  v_3.z = _World2Object[2].z;
  v_3.w = _World2Object[3].z;
  gl_Position = (gl_ModelViewProjectionMatrix * gl_Vertex);
  xlv_TEXCOORD0 = ((gl_MultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
  xlv_TEXCOORD1 = normalize(((
    (v_1.xyz * gl_Normal.x)
   + 
    (v_2.xyz * gl_Normal.y)
  ) + (v_3.xyz * gl_Normal.z)));
  xlv_TEXCOORD2 = (_Object2World * gl_Vertex).xyz;
  xlv_COLOR0 = gl_Color;
}


#endif
#ifdef FRAGMENT
uniform vec4 _WorldSpaceLightPos0;
uniform vec4 _LightColor0;
uniform sampler2D _LightTexture0;
uniform mat4 _LightMatrix0;
uniform sampler2D _LightTextureB0;
uniform sampler2D _MainTex;
uniform vec4 _Color;
varying vec2 xlv_TEXCOORD0;
varying vec3 xlv_TEXCOORD1;
varying vec3 xlv_TEXCOORD2;
varying vec4 xlv_COLOR0;
void main ()
{
  vec4 tmpvar_1;
  tmpvar_1 = ((texture2D (_MainTex, xlv_TEXCOORD0) * _Color) * xlv_COLOR0);
  vec4 tmpvar_2;
  tmpvar_2.w = 1.0;
  tmpvar_2.xyz = xlv_TEXCOORD2;
  vec4 tmpvar_3;
  tmpvar_3 = (_LightMatrix0 * tmpvar_2);
  vec4 c_4;
  c_4.xyz = ((tmpvar_1.xyz * max (0.0, 
    dot (normalize(xlv_TEXCOORD1), normalize(normalize((_WorldSpaceLightPos0.xyz - xlv_TEXCOORD2))))
  )) * _LightColor0.xyz);
  c_4.xyz = (c_4.xyz * ((
    float((tmpvar_3.z > 0.0))
   * texture2D (_LightTexture0, 
    ((tmpvar_3.xy / tmpvar_3.w) + 0.5)
  ).w) * texture2D (_LightTextureB0, vec2(dot (tmpvar_3.xyz, tmpvar_3.xyz))).w));
  c_4.w = tmpvar_1.w;
  float x_5;
  x_5 = (tmpvar_1.w - 0.01);
  if ((x_5 < 0.0)) {
    discard;
  };
  gl_FragData[0] = c_4;
}


#endif
"
}
SubProgram "opengl " {
// Stats: 17 math, 4 textures, 1 branches
Keywords { "POINT_COOKIE" }
"!!GLSL#version 120

#ifdef VERTEX

uniform mat4 _Object2World;
uniform mat4 _World2Object;
uniform vec4 _MainTex_ST;
varying vec2 xlv_TEXCOORD0;
varying vec3 xlv_TEXCOORD1;
varying vec3 xlv_TEXCOORD2;
varying vec4 xlv_COLOR0;
void main ()
{
  vec4 v_1;
  v_1.x = _World2Object[0].x;
  v_1.y = _World2Object[1].x;
  v_1.z = _World2Object[2].x;
  v_1.w = _World2Object[3].x;
  vec4 v_2;
  v_2.x = _World2Object[0].y;
  v_2.y = _World2Object[1].y;
  v_2.z = _World2Object[2].y;
  v_2.w = _World2Object[3].y;
  vec4 v_3;
  v_3.x = _World2Object[0].z;
  v_3.y = _World2Object[1].z;
  v_3.z = _World2Object[2].z;
  v_3.w = _World2Object[3].z;
  gl_Position = (gl_ModelViewProjectionMatrix * gl_Vertex);
  xlv_TEXCOORD0 = ((gl_MultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
  xlv_TEXCOORD1 = normalize(((
    (v_1.xyz * gl_Normal.x)
   + 
    (v_2.xyz * gl_Normal.y)
  ) + (v_3.xyz * gl_Normal.z)));
  xlv_TEXCOORD2 = (_Object2World * gl_Vertex).xyz;
  xlv_COLOR0 = gl_Color;
}


#endif
#ifdef FRAGMENT
uniform vec4 _WorldSpaceLightPos0;
uniform vec4 _LightColor0;
uniform samplerCube _LightTexture0;
uniform mat4 _LightMatrix0;
uniform sampler2D _LightTextureB0;
uniform sampler2D _MainTex;
uniform vec4 _Color;
varying vec2 xlv_TEXCOORD0;
varying vec3 xlv_TEXCOORD1;
varying vec3 xlv_TEXCOORD2;
varying vec4 xlv_COLOR0;
void main ()
{
  vec4 tmpvar_1;
  tmpvar_1 = ((texture2D (_MainTex, xlv_TEXCOORD0) * _Color) * xlv_COLOR0);
  vec4 tmpvar_2;
  tmpvar_2.w = 1.0;
  tmpvar_2.xyz = xlv_TEXCOORD2;
  vec3 tmpvar_3;
  tmpvar_3 = (_LightMatrix0 * tmpvar_2).xyz;
  vec4 c_4;
  c_4.xyz = ((tmpvar_1.xyz * max (0.0, 
    dot (normalize(xlv_TEXCOORD1), normalize(normalize((_WorldSpaceLightPos0.xyz - xlv_TEXCOORD2))))
  )) * _LightColor0.xyz);
  c_4.xyz = (c_4.xyz * (texture2D (_LightTextureB0, vec2(dot (tmpvar_3, tmpvar_3))).w * textureCube (_LightTexture0, tmpvar_3).w));
  c_4.w = tmpvar_1.w;
  float x_5;
  x_5 = (tmpvar_1.w - 0.01);
  if ((x_5 < 0.0)) {
    discard;
  };
  gl_FragData[0] = c_4;
}


#endif
"
}
SubProgram "opengl " {
// Stats: 12 math, 3 textures, 1 branches
Keywords { "DIRECTIONAL_COOKIE" }
"!!GLSL#version 120

#ifdef VERTEX

uniform mat4 _Object2World;
uniform mat4 _World2Object;
uniform vec4 _MainTex_ST;
varying vec2 xlv_TEXCOORD0;
varying vec3 xlv_TEXCOORD1;
varying vec3 xlv_TEXCOORD2;
varying vec4 xlv_COLOR0;
void main ()
{
  vec4 v_1;
  v_1.x = _World2Object[0].x;
  v_1.y = _World2Object[1].x;
  v_1.z = _World2Object[2].x;
  v_1.w = _World2Object[3].x;
  vec4 v_2;
  v_2.x = _World2Object[0].y;
  v_2.y = _World2Object[1].y;
  v_2.z = _World2Object[2].y;
  v_2.w = _World2Object[3].y;
  vec4 v_3;
  v_3.x = _World2Object[0].z;
  v_3.y = _World2Object[1].z;
  v_3.z = _World2Object[2].z;
  v_3.w = _World2Object[3].z;
  gl_Position = (gl_ModelViewProjectionMatrix * gl_Vertex);
  xlv_TEXCOORD0 = ((gl_MultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
  xlv_TEXCOORD1 = normalize(((
    (v_1.xyz * gl_Normal.x)
   + 
    (v_2.xyz * gl_Normal.y)
  ) + (v_3.xyz * gl_Normal.z)));
  xlv_TEXCOORD2 = (_Object2World * gl_Vertex).xyz;
  xlv_COLOR0 = gl_Color;
}


#endif
#ifdef FRAGMENT
uniform vec4 _WorldSpaceLightPos0;
uniform vec4 _LightColor0;
uniform sampler2D _LightTexture0;
uniform mat4 _LightMatrix0;
uniform sampler2D _MainTex;
uniform vec4 _Color;
varying vec2 xlv_TEXCOORD0;
varying vec3 xlv_TEXCOORD1;
varying vec3 xlv_TEXCOORD2;
varying vec4 xlv_COLOR0;
void main ()
{
  vec4 tmpvar_1;
  tmpvar_1 = ((texture2D (_MainTex, xlv_TEXCOORD0) * _Color) * xlv_COLOR0);
  vec4 tmpvar_2;
  tmpvar_2.w = 1.0;
  tmpvar_2.xyz = xlv_TEXCOORD2;
  vec4 c_3;
  c_3.xyz = ((tmpvar_1.xyz * max (0.0, 
    dot (normalize(xlv_TEXCOORD1), _WorldSpaceLightPos0.xyz)
  )) * _LightColor0.xyz);
  c_3.xyz = (c_3.xyz * texture2D (_LightTexture0, (_LightMatrix0 * tmpvar_2).xy).w);
  c_3.w = tmpvar_1.w;
  float x_4;
  x_4 = (tmpvar_1.w - 0.01);
  if ((x_4 < 0.0)) {
    discard;
  };
  gl_FragData[0] = c_3;
}


#endif
"
}
}
Program "fp" {
SubProgram "opengl " {
Keywords { "POINT" }
"!!GLSL"
}
SubProgram "opengl " {
Keywords { "DIRECTIONAL" }
"!!GLSL"
}
SubProgram "opengl " {
Keywords { "SPOT" }
"!!GLSL"
}
SubProgram "opengl " {
Keywords { "POINT_COOKIE" }
"!!GLSL"
}
SubProgram "opengl " {
Keywords { "DIRECTIONAL_COOKIE" }
"!!GLSL"
}
}
 }
}
}