#version 330 core

uniform vec3 Color;
out vec4 FragColor;

void main(void) {
    FragColor = vec4(Color, 1);
}