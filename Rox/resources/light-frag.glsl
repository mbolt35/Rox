#version 330 core

struct Material {
    vec3 Color;
    sampler2D Texture;
};

struct Light {
    vec3 Position;
  
    vec3 Ambient;
    vec3 Diffuse;
    vec3 Specular;
	
    float Constant;
    float Linear;
    float Quadratic;
};

uniform Material material;
uniform Light light;

in vec3 FragPosition;
in vec3 FragNormal;
in vec2 FragUV;

out vec4 FragColor;

float attenuation(in Light l, in float distance) {
    return 1.0f / (l.Constant + l.Linear * distance + l.Quadratic * (distance * distance));
}

void main(void)
{
    vec3 lightDelta = light.Position - FragPosition;

    vec3 n = normalize(FragNormal);
    vec3 l = normalize(lightDelta);

    float cosTheta = clamp(dot(n, l), 0.0f, 1.0f);
    float lightAttenuation = attenuation(light, length(lightDelta));

    vec3 ambient = light.Ambient * lightAttenuation;
    vec3 diffuse = light.Diffuse * lightAttenuation * cosTheta;
    vec3 specular = light.Specular * lightAttenuation;
    
    vec3 fragColor = (ambient + diffuse) * texture(material.Texture, FragUV).rgb;
    //vec3 fragColor = (ambient + diffuse) * material.Color;
    FragColor = vec4(fragColor, 1);
}