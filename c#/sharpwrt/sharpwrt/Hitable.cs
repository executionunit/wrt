using System;


namespace exunit
{
    struct HitRecord
    {
        public float t;
        public Vec3 p;
        public Vec3 normal;
        public Material material;
    }

    interface Hitable
    {
        bool hit(Ray r, float t_min, float t_max, ref HitRecord rec);
    }
}
