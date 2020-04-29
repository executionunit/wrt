using System;
using System.Drawing;
using System.Drawing.Imaging;

using System.Collections.Generic;
using exunit;

namespace sharpwrt
{
    class Program
    {

        static Random rand = new Random();
        const int NUM_RAY_BOUNCES = 5;
        const int NUM_SAMPLES = 40;

        static private float rand01()
        {
            return (float)rand.NextDouble();
        }

        static private Hitable RandomScene()
        {

            List<Hitable> hitables = new List<Hitable>();
            
            hitables.Add(new Sphere(new Vec3(0, -1000, 0), 1000, new Lambertian(new Vec3(0.5f, 0.5f, 0.5f))));
            for (int a = -11; a < 11; a++)
            {
                for (int b = -11; b < 11; b++)
                {
                    float choose_mat = (float)rand.NextDouble();
                    Vec3 center = new Vec3(a +0.9f * rand01(), 0.2f, b + 0.9f * rand01());
                    if ((center - new Vec3(4.0f, 0.2f, 0.0f)).length() > 0.9f)
                    {
                        if (choose_mat < 0.8f)
                        {  // diffuse
                            hitables.Add(new Sphere(center, 0.2f,
                                new Lambertian(new Vec3(rand01() * rand01(),
                                    rand01() * rand01(),
                                    rand01() * rand01()))
                            ));
                        }
                        else if (choose_mat < 0.95f)
                        { // metal
                            hitables.Add(new Sphere(
                                center, 0.2f,
                                new Metal(new Vec3(0.5f * (1 + rand01()),
                                    0.5f * (1 + rand01()),
                                    0.5f * (1 + rand01())),
                                    0.5f * rand01())
                            ));
                        }
                        else
                        {  // glass
                            hitables.Add(new Sphere(center, 0.2f, new Dielectric(1.5f)));
                        }
                    }
                }
            }

            hitables.Add(new Sphere(new Vec3(0.0f, 1.0f, 0.0f), 1.0f, new Dielectric(1.5f)));
            hitables.Add(new Sphere(new Vec3(-4.0f, 1.0f, 0.0f), 1.0f, new Lambertian(new Vec3(0.4f, 0.2f, 0.1f))));
            hitables.Add(new Sphere(new Vec3(4.0f, 1.0f, 0.0f), 1.0f, new Metal(new Vec3(0.7f, 0.6f, 0.5f), 0.0f)));

            return new HitableList(ref hitables);
        }


        static Vec3 GetColor(Ray r, ref Hitable world, int depth=0)
        {
            HitRecord rec = new HitRecord();

            if (world.hit(r, 0.001f, float.MaxValue, ref rec))
            {
                Ray scattered = new Ray();
                Vec3 attenuation = new Vec3();
                if (depth < NUM_RAY_BOUNCES && rec.material.scatter(r, rec, ref attenuation, ref scattered))
                {
                    return attenuation * GetColor(scattered, ref world, depth + 1);
                }
                else
                {
                    return new Vec3(0, 0, 0);
                }
            }
            else
            {
                //missed everything, it's the sky.
                Vec3 unit_direction = Vec3.unit_vector(r.direction());
                float t = 0.5f * (unit_direction.y + 1.0f);
                return (1.0f - t) * new Vec3(1.0f, 1.0f, 1.0f) + t * new Vec3(0.5f, 0.7f, 1.0f);
            }
        }

        static void Main(string[] args)
        {
            int nx = 600;
            int ny = 400;
            int ns = NUM_SAMPLES;            

            Bitmap bitmap = new Bitmap(nx, ny);
            Graphics graphics = Graphics.FromImage(bitmap);
            const int pixelalpha = 255;

            //ArrayList hitables = new ArrayList();
            //hitables.Add(new Sphere(new Vec3(0, 0, -1), 0.5f, new Lambertian(new Vec3(0.8f, 0.3f, 0.3f))));
            //hitables.Add(new Sphere(new Vec3(0, -100.5f, -1), 100.0f, new Lambertian(new Vec3(0.8f, 0.8f, 0.0f))));
            //hitables.Add(new Sphere(new Vec3(1, 0, -1), 0.5f, new Metal(new Vec3(0.8f, 0.6f, 0.2f), 1.0f)));
            //hitables.Add(new Sphere(new Vec3(-1, 0, -1), 0.5f, new Dielectric(1.5f)));
            //hitables.Add(new Sphere(new Vec3(-1, 0, -1), -0.45f, new Dielectric(1.5f)));

            //Hitable world = new HitableList(ref hitables);
            Hitable world = RandomScene();

            Vec3 lookfrom = new Vec3(6, 2, 3);
            Vec3 lookat = new Vec3(0, 0, -1);
            Vec3 up = new Vec3(0, 1, 0);
            float apperature = 0.002f;
            float focusdist = (lookfrom - lookat).length();
            float aspect = (float)nx / (float)ny;
            float fovy = 50;

            Camera cam = new Camera(lookfrom, lookat, up, fovy, 
                aspect, apperature, focusdist);

            Random rand = new Random();

            for (int j = 0; j < ny; j++)
            {
                Console.WriteLine("Line {0:D}", j);
                for (int i = 0; i < nx; i++)
                {
                    Vec3 col = new Vec3(0, 0, 0);

                    for (int s = 0; s < ns; s++){
                        float u = (float)(i+rand.NextDouble()) / (float)nx;
                        float v = (float)(j+rand.NextDouble()) / (float)ny;

                        Ray ray = cam.get_ray(u, v);
                        col += GetColor(ray, ref world);

                    }
                    col /= ns;
                    col.x = (float)Math.Sqrt(col.x);
                    col.y = (float)Math.Sqrt(col.y);
                    col.z = (float)Math.Sqrt(col.z);

                    int r = (int)(col.getr() * 255);
                    int g = (int)(col.getg() * 255);
                    int b = (int)(col.getb() * 255);

                    //flip the y component so the PNG is the correct way
                    //up for the tutorial!
                    bitmap.SetPixel(i, ny-j-1, Color.FromArgb(pixelalpha, r, g, b));
                }
            }


            bitmap.Save(@"out.png", ImageFormat.Png);
        }
    }
}
