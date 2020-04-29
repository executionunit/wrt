using System;

namespace exunit
{
    class Util
    {

        static private Random rand = new Random();

        static public Vec3 random_in_unit_sphere()
        {
            Vec3 p;
            do
            {
                p = 2.0f * new Vec3((float)rand.NextDouble(), (float)rand.NextDouble(), (float)rand.NextDouble()) - new Vec3(1, 1, 1);
            } while (p.squared_length() >= 1.0);
            return p;
        }

        static public Vec3 random_in_unit_disk()
        {
            Vec3 p;
            do
            {
                p = 2.0f * new Vec3((float)rand.NextDouble(), (float)rand.NextDouble(), 0) - new Vec3(1, 1, 0);
            } while (Vec3.dot(p,p) >= 1.0);
            return p;
        }
    }
}
