using System;

namespace exunit
{


    abstract class Material
    {
        static protected Random rand = new Random();

       

        public abstract bool scatter(Ray ray, HitRecord rec, ref Vec3 attenuation, ref Ray scattered);

    }

    class Lambertian : Material
    {
        public Lambertian(Vec3 a)
        {
            albedo = a;
        }

        public override bool scatter(Ray ray, HitRecord rec, ref Vec3 attenuation, ref Ray scattered)
        {
            Vec3 target = rec.p + rec.normal + Util.random_in_unit_sphere();
            scattered = new Ray(rec.p, target - rec.p);
            attenuation = albedo;
            return true;
        }

        public Vec3 albedo { get; }

    }

    class Metal : Material
    {

        public Metal(Vec3 a, float fuzz)
        {
            albedo = a;
            fuzziness = Math.Min(1.0f, fuzz);
        }

        public override bool scatter(Ray ray, HitRecord rec, ref Vec3 attenuation, ref Ray scattered)
        {
            Vec3 reflected = Vec3.reflect(Vec3.unit_vector(ray.direction()), rec.normal);
            scattered = new Ray(rec.p, reflected + fuzziness * Util.random_in_unit_sphere());
            attenuation = albedo;
            return Vec3.dot(scattered.direction(), rec.normal) > 0;
        }

        public Vec3 albedo { get; }
        public float fuzziness { get; }

    }

    class Dielectric : Material
    {
        public Dielectric(float ri)
        {
            RefractiveIndex = ri;
        }

        private bool refract(Vec3 v, Vec3 n, float ni_over_nt, ref Vec3 refracted)
        {
            Vec3 uv = Vec3.unit_vector(v);
            float dt = Vec3.dot(uv, n);
            float discriminent = 1.0f - ni_over_nt * ni_over_nt * (1.0f - dt * dt);
            if (discriminent > 0)
            {
                refracted = ni_over_nt * (uv - n * dt) - n * (float)Math.Sqrt(discriminent);
                return true;
            }
            return false;
        }

        private float schlick(float cosine , float refidx)
        {
            float r0 = (1 - refidx) / (1 + refidx);
            r0 = r0 * r0;
            return r0 + (1 - r0) * (float)Math.Pow(1 - cosine, 5);
        }

        public override bool scatter(Ray ray, HitRecord rec, ref Vec3 attenuation, ref Ray scattered)
        {
            
            Vec3 reflected = Vec3.reflect(ray.direction(), rec.normal);
            Vec3 outward_normal;
            float ni_over_nt;
            float cosine;
            attenuation.x = 1.0f;
            attenuation.y = 1.0f;
            attenuation.z = 1.0f;

            if (Vec3.dot(ray.direction(), rec.normal) > 0)
            {
                outward_normal = -rec.normal;
                ni_over_nt = RefractiveIndex;
                cosine = RefractiveIndex * Vec3.dot(ray.direction(), rec.normal) / ray.direction().length();
            }
            else
            {
                outward_normal = rec.normal;
                ni_over_nt = 1.0f / RefractiveIndex;
                cosine = -(Vec3.dot(ray.direction(), rec.normal) / ray.direction().length());
            }

            float reflect_prob;
            Vec3 refracted = new Vec3();
            if(refract(ray.direction(), outward_normal, ni_over_nt, ref refracted))
            {
                reflect_prob = schlick(cosine, RefractiveIndex);
            }
            else
            {
                scattered = new Ray(rec.p, reflected);
                reflect_prob = 1.0f;
            }

            if(rand.NextDouble() < reflect_prob)
            {
                scattered = new Ray(rec.p, reflected);
            }
            else
            {
                scattered = new Ray(rec.p, refracted);
            }
            return true;
           
        }

        public float RefractiveIndex { get; }

    }

}
