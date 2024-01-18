shader_type spatial;
render_mode unshaded;

#define NUM_STEPS 512

uniform sampler3D data_tex;

varying vec3 vertexLocal;
varying vec3 camDirLocal;

vec4 getTF1DColour(float density)
{
	vec3 col = density < 0.5 ? vec3(0.5, 0.4, 0.3) * (density * 2.0) : vec3(1.0, 1.0, 1.0) * (density);
	float alpha = smoothstep(0.0, 1.0, density) * 0.1;
	return vec4(col, alpha);
}

vec4 raymarch_dvr(vec3 rayStart, vec3 rayDir)
{
    const float stepSize = 1.732 /*greatest distance in box*/ / float(NUM_STEPS);
	vec4 col = vec4(0.0, 0.0, 0.0, 0.0);
	for (int iStep = 0; iStep < NUM_STEPS; iStep++)
    {
		float t = float(iStep) * stepSize;
        vec3 currPos = rayStart + rayDir * t;
 
		currPos += vec3(0.5, 0.5, 0.5);
		if (currPos.x < 0.0 || currPos.x >= 1.0 || currPos.y < 0.0 || currPos.y > 1.0 || currPos.z < 0.0 || currPos.z > 1.0)
            continue;
		float density = texture(data_tex, currPos).x;
        vec4 src = getTF1DColour(density);
		src.rgb *= src.a;
		col = (1.0f - col.a) * src + col;
    }
	return col;
}

vec4 raymarch_mip(vec3 rayStart, vec3 rayDir)
{
    const float stepSize = 1.732 /*greatest distance in box*/ / float(NUM_STEPS);
	vec4 outColour = vec4(1.0, 1.0, 1.0, 0.0);
	float maxDensity = 0.0;
	for (int iStep = 0; iStep < NUM_STEPS; iStep++)
    {
		float t = float(iStep) * stepSize;
        vec3 currPos = rayStart + rayDir * t;
 
		currPos += vec3(0.5, 0.5, 0.5);
		if (currPos.x < 0.0 || currPos.x >= 1.0 || currPos.y < 0.0 || currPos.y > 1.0 || currPos.z < 0.0 || currPos.z > 1.0)
            continue;
		float density = texture(data_tex, currPos).x;
        maxDensity = max(density, maxDensity);
    }
	outColour.a = maxDensity;
	return outColour;
}

void vertex() {
	vertexLocal = VERTEX;
	vec4 camLocal = inverse(MODEL_MATRIX) * vec4(CAMERA_POSITION_WORLD, 1.0);
	camDirLocal = normalize(VERTEX - vec3(camLocal.x, camLocal.y, camLocal.z));
}

void fragment() {
	vec3 rayStart = vertexLocal;
    vec3 rayDir = camDirLocal;
	vec4 outColour = raymarch_dvr(rayStart, rayDir);
	ALBEDO = outColour.rgb;
	ALPHA = outColour.a;
}
