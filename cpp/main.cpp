#include <iostream>
#include "ray.h"
#include "stb_image_write.h"
#include "hitable_list.h"
#include "sphere.h"
#include "moving_sphere.h"
#include "camera.h"
#include "limits"
#include "random.h"
#include "material.h"


vec3 color(const ray &r, hitable *world, int depth) {

	hit_record rec;
	if (world->hit(r, 0.001f, std::numeric_limits<float>::max(), rec)) {
		ray scattered;
		vec3 attenuation;
		if (depth < 50 && rec.mat_ptr->scatter(r, rec, attenuation, scattered)) {
			return attenuation * color(scattered, world, depth + 1);
		}
		else {
			return vec3(0.0f, 0.0f, 0.0f);
		}
	}

	vec3 unit_direction = unit_vector(r.direction());
	float t = 0.5f * (unit_direction.y() + 1.0f);
	return (1.0f - t) * vec3(1.0f, 1.0f, 1.0f) + t * vec3(0.5f, 0.7f, 1.0f);
}


hitable *random_scene() {
	int n = 500;
	hitable **list = new hitable*[n + 1];
	list[0] = new sphere(vec3(0, -1000, 0), 1000, new lambertian(vec3(0.5, 0.5, 0.5)));
	int i = 1;
	for (int a = -11; a < 11; a++) {
		for (int b = -11; b < 11; b++) {
			float choose_mat = rand01();
			vec3 center(a + 0.9f*rand01(), 0.2f, b + 0.9f*rand01());
			if ((center - vec3(4.0f, 0.2f, 0.0f)).length() > 0.9f) {
				if (choose_mat < 0.8f) {  // diffuse
					list[i++] = new moving_sphere(
						center, center + vec3(0.0f, 0.5f * rand01(), 0.0f), 0.0f, 1.0f,
						0.2f,
						new lambertian(vec3(rand01()*rand01(),
							rand01()*rand01(),
							rand01()*rand01()))
					);
				}
				else if (choose_mat < 0.95f) { // metal
					list[i++] = new sphere(
						center, 0.2f,
						new metal(vec3(0.5f*(1 + rand01()),
							0.5f*(1 + rand01()),
							0.5f*(1 + rand01())),
							0.5f*rand01())
					);
				}
				else {  // glass
					list[i++] = new sphere(center, 0.2f, new dielectric(1.5f));
				}
			}
		}
	}

	list[i++] = new sphere(vec3(0.0f, 1.0f, 0.0f), 1.0f, new dielectric(1.5f));
	list[i++] = new sphere(vec3(-4.0f, 1.0f, 0.0f), 1.0f, new lambertian(vec3(0.4f, 0.2f, 0.1f)));
	list[i++] = new sphere(vec3(4.0f, 1.0f, 0.0f), 1.0f, new metal(vec3(0.7f, 0.6f, 0.5f), 0.0f));

	return new hitable_list(list, i);
}

int main(int argc, char **argv) {

	const int nx = 800;
	const int ny = 600;
	const int ns = 100;

	char *imgdata = new char[nx *ny *3];

	vec3 lookfrom(13, 2, 3);
	vec3 lookat(0, 0, 0);
	vec3 up(0, 1, 0);
	float fovy = 20;
	float dist_to_focus = 10.0;
	float aperture = 0.1f;
	float aspect = float(nx) / float(ny);
	float mintime = 0.0f;
	float maxtime = 0.3f;

	hitable *world = random_scene();
	camera cam(lookfrom, lookat, up, fovy, aspect, aperture, dist_to_focus, mintime, maxtime);

	for (int j = ny - 1; j >= 0; --j) {
		for (int i = 0; i < nx; ++i) {

			vec3 col(0.0f, 0.0f, 0.0f);
			for (int s = 0; s < ns; ++s) {
				float u = (i + rand01()) / float(nx);
				float v = (j + rand01()) / float(ny);

				ray r = cam.get_ray(u, v);
				col += color(r, world, 0);
			}

			col /= float(ns);
			col = vec3(sqrt(col[0]), sqrt(col[1]), sqrt(col[2]));
			int ir = int(255.99f * col[0]);
			int ig = int(255.99f * col[1]);
			int ib = int(255.99f * col[2]);


			imgdata[((ny-1-j) * nx + i) * 3 + 0] = ir;
			imgdata[((ny - 1 -j) * nx + i) * 3 + 1] = ig;
			imgdata[((ny - 1 -j) * nx + i) * 3 + 2] = ib;
		}
		printf("%d\n", j);
	}

	stbi_write_png("../out.png", nx, ny, 3, imgdata, nx*3);

	return 0;
}