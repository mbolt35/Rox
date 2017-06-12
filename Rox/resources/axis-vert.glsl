#version 330 core
        
uniform mat4 ProjectionMatrix;
uniform mat4 ModelMatrix;
uniform mat4 ViewMatrix;

in vec3 in_position;

void main(void) {
    gl_Position = ProjectionMatrix * ViewMatrix * ModelMatrix * vec4(in_position, 1.0f);
}