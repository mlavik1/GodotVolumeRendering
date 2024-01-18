shader_type spatial;
render_mode unshaded;

uniform sampler3D data_tex;

varying vec3 vertexLocal;
varying vec3 camDirLocal;

void vertex() {
	vertexLocal = VERTEX;
	vec4 camLocal = inverse(MODEL_MATRIX) * vec4(CAMERA_POSITION_WORLD, 1.0);
	camDirLocal = normalize(VERTEX - vec3(camLocal.x, camLocal.y, camLocal.z));
	//camDirLocal = CAMERA_DIRECTION_WORLD;
}

void fragment() {
	#define NUM_STEPS 512
    const float stepSize = 1.732 /*greatest distance in box*/ / float(NUM_STEPS);
	
	vec3 outColour = vec3(1.0, 1.0, 1.0);
	vec3 rayStart = vertexLocal;
    vec3 rayDir = camDirLocal;
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
	ALBEDO = outColour;
	ALPHA = maxDensity;
}
