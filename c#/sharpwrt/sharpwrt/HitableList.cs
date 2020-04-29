using System.Collections.Generic;

namespace exunit
{
    class HitableList : Hitable
    {
        public HitableList() { }
        public HitableList(ref List<Hitable> hitables)
        {
            HitList = hitables;
        }

        public bool hit(Ray r, float t_min, float t_max, ref HitRecord rec)
        {
            HitRecord temp_rec = new HitRecord();
            bool hit_anything = false;
            float closest_so_far = t_max;

            foreach(Hitable hitable in HitList)
            {
                if(hitable.hit(r, t_min, closest_so_far, ref temp_rec))
                {
                    hit_anything = true;
                    closest_so_far = temp_rec.t;
                    rec = temp_rec;
                }
            }
            return hit_anything;
        }

        public List<Hitable> HitList { get; }

    }
}
