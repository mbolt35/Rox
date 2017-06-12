#version 330 core

uniform mat4 ProjectionMatrix;
uniform mat4 ModelMatrix;
uniform mat4 ViewMatrix;

in vec3 in_position;
in vec3 in_normal;
in vec2 in_uv;

out vec3 FragPos;
out vec3 FragNormal;
out vec2 FragUV;

void main(void)
{
    vec4 pos = ModelMatrix * vec4(in_position, 1);
    vec4 norm = ModelMatrix * vec4(in_normal, 1);

    FragPos = pos.xyz;
    FragNormal = norm.xyz;
    FragUV = in_uv;
    
    gl_Position = ProjectionMatrix * ViewMatrix * pos;
}