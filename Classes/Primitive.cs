using System;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

class Primitive
{
    Vector3 color = new Vector3(100, 40, 50);

    public virtual void Intersect(Ray ray)
    { }
}

class Plane : Primitive
{
    public Vector3 N; // normal of plane
    public float d; // equation p.N + d = 0 for a point p on the plane

    public Plane(Vector3 N, float d)
    {
        this.N = N;
        this.d = d;
    }

    public override void Intersect(Ray ray)
    {
        float t = -(Vector3.Dot(ray.O, this.N) + d) / (Vector3.Dot(ray.D, this.N));
        if ((t < ray.t) && (t > 0)) ray.t = t;
    }
}

class Sphere : Primitive
{
    public Vector3 pos; // center of sphere
    public double r; // radius of sphere
    public float r2; // radius squared

    public Sphere(Vector3 pos, float r)
    {
        this.pos = pos;
        this.r = r;
        this.r2 = r * r;
    }

    public override void Intersect(Ray ray)
    {
        Vector3 c = this.pos - ray.O;
        float t = Vector3.Dot(c, ray.D);
        Vector3 q = c - t * ray.D;
        float p2 = Vector3.Dot(q, q);
        if (p2 > this.r2) return;
        t = (float)(-Math.Sqrt(this.r2 - p2));
        if ((t < ray.t) && (t > 0)) ray.t = t;
    }
}