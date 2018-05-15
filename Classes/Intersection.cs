using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;

class Intersection
{
    public float t; // shortest intersection distance
    public Vector3 norm; // normal at intersection point
    public Primitive prim; // nearest primitive

    public Intersection(float t, Vector3 norm, Primitive prim)
    {
        this.t = t;
        this.norm = norm;
        this.prim = prim;
    }
}

