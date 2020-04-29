using System;

namespace exunit
{
    class Camera
    {
        public Camera(Vec3 from ,Vec3 to, Vec3 up, float vfov, float aspect, float apperature, float focus_dist)
        {
            lend_radius = apperature / 2.0f;

            
            float theta = vfov * (float)Math.PI / 180.0f;
            float half_height = (float)Math.Tan(theta / 2);
            float half_width = aspect * half_height;

            origin = from;
            w = Vec3.unit_vector(from - to);
            u = Vec3.unit_vector(Vec3.cross(up, w));
            v = Vec3.cross(w, u);

            lower_left_corner = origin - half_width * focus_dist * u - half_height * focus_dist * v - focus_dist * w;
            horizontal = 2 * half_width * focus_dist * u;
            vertical = 2 * half_height * focus_dist * v;
        }

        public Ray get_ray(float s, float t)
        {
            Vec3 rd = lend_radius * Util.random_in_unit_disk();
            Vec3 offset = u * rd.x + v * rd.y;
            return new Ray(origin + offset, lower_left_corner + s * horizontal + t * vertical - origin - offset);
        }

        public Vec3 origin;
        public Vec3 lower_left_corner;
        public Vec3 horizontal;
        public Vec3 vertical;
        public float lend_radius;
        public Vec3 u;
        public Vec3 v;
        public Vec3 w;
    }


}
