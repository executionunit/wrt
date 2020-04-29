using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using exunit;

namespace exunit
{
    class Ray
    {
        public Ray()
        {
        }

        public Ray(Vec3 a, Vec3 b)
        {
            A = a;
            B = b;
        }

        public Vec3 origin() => A;
        public Vec3 direction() => B;

        public Vec3 point_at_parameter(float t) => A + t * B;

        /* origin */
        public Vec3 A { get; set; }
        /* direction */
        public Vec3 B { get; set; }
    }
}
