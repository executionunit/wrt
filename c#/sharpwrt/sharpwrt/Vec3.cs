using System;

namespace exunit
{
    public class Vec3
    {
        public Vec3() { }

        public Vec3(float ix, float iy, float iz)
        {
            x = ix;
            y = iy;
            z = iz;
        }

        public Vec3(Vec3 b)
        {
            x = b.x;
            y = b.y;
            z = b.z;
        }

        public float x { get; set; }
        public float y { get; set; }
        public float z { get; set; }

        public float getr(){return x;}
        public float getg() { return y; }
        public float getb() { return z; }

        public float length()
        {
            return (float)Math.Sqrt(x * x + y * y + z * z);
        }

        public float squared_length()
        {
            return x * x + y * y + z * z;
        }

        static public Vec3 unit_vector(Vec3 b) => new Vec3(b / b.length());

        public void make_unit_vector()
        {
            float k = 1.0f / length();
            x *= k;
            y *= k;
            z *= k;
        }

        public static float dot(Vec3 a, Vec3 b) => (a.x * b.x + a.y * b.y+ a.z* b.z);

        public static Vec3 cross(Vec3 a, Vec3 b) => new Vec3(a.y * b.z - a.z * b.y, -(a.x * b.z - a.z * b.x), a.x * b.y - a.y * b.x);

        public static Vec3 operator +(Vec3 a, Vec3 b) => new Vec3(a.x + b.x, a.y + b.y, a.z + b.z);
        public static Vec3 operator -(Vec3 a, Vec3 b) => new Vec3(a.x - b.x, a.y - b.y, a.z - b.z);
        public static Vec3 operator -(Vec3 a) => new Vec3(-a.x, -a.y, -a.z);
        public static Vec3 operator*(Vec3 a, Vec3 b) => new Vec3(a.x * b.x, a.y * b.y, a.z * b.z);
        public static Vec3 operator *(Vec3 a, float f) => new Vec3(a.x * f, a.y * f, a.z * f);
        public static Vec3 operator *(float f, Vec3 a) => new Vec3(a.x * f, a.y * f, a.z * f);

        public static Vec3 operator/(Vec3 a, Vec3 b) => new Vec3(a.x / b.x, a.y / b.y, a.z / b.z);
        public static Vec3 operator /(Vec3 a, float f) => new Vec3(a.x / f, a.y / f, a.z / f);

        public static Vec3 reflect(Vec3 v, Vec3 n) => new Vec3(v - 2.0f * dot(v, n) * n);

        public float this[int i]
        {
            get {
                if (i == 0)
                {
                    return x;
                }else if (i == 1)
                {
                    return y;
                }else if(i == 2)
                {
                    return z;
                }
                return 0;
            }

            set
            {
                if (i == 0)
                {
                    x = value;
                }
                else if (i == 1)
                {
                    y = value;
                }
                else if (i == 2)
                {
                    z = value;
                }
            }
        }
    }

};