using System;

namespace exunit
{
    class Sphere : Hitable
    {
        public Sphere() { }
        public Sphere(Vec3 cen, float r, Material mat)
        {
            center = cen;
            radius = r;
            material = mat;
        }

        public bool hit(Ray r, float t_min, float t_max, ref HitRecord rec)
        {
            Vec3 oc = r.origin() - center;
            float a = Vec3.dot(r.direction(), r.direction());
            float b = Vec3.dot(oc, r.direction());
            float c = Vec3.dot(oc, oc) - radius * radius;
            float discriminant = b * b - a * c;

            if (discriminant > 0)
            {
                float temp = (-b - (float)Math.Sqrt(b * b - a * c)) / a;
                if (temp < t_max && temp > t_min)
                {
                    rec.t = temp;
                    rec.p = r.point_at_parameter(temp);
                    rec.normal = Vec3.unit_vector((rec.p - center) / radius);
                    rec.material = material;
                    return true;
                }

                temp = (-b + (float)Math.Sqrt(b * b - a * c)) / a;
                if (temp < t_max && temp > t_min)
                {
                    rec.t = temp;
                    rec.p = r.point_at_parameter(temp);
                    rec.normal = (rec.p - center) / radius;
                    rec.material = material;
                    return true;
                }
            }

            return false;

        }

        public Vec3 center { get; }
        public float radius { get; }

        public Material material { get; }
    }
}
