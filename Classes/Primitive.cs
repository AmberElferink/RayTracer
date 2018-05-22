using System;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

public abstract class Primitive
{
    public Vector3 color; // later: in material implementatie
    public bool checkerboard; // checks if a plane has a checkerboard pattern
    // TODO: can we define this so that only Plane has this bool? (see class Scene)

    ///<summary>
    ///Method that calculates distance from ray to primitive, and updates ray.t if this distance is shorter than the actual value of ray.t.
    ///</summary>
    /// <param name="ray">a ray that gets shot and maybe intersects a primitive</param>
    /// <returns></returns>
    public abstract Intersection Intersect(Ray ray);
}

public class Plane : Primitive
{
    public Vector3 N; // normal of plane
    public float d; // equation p.N + d = 0 for a point p on the plane

    public Plane(Vector3 N, float d, Vector3 color, bool checkerboard)
    {
        this.N = N;
        this.d = d;
        this.color = color;
        this.checkerboard = checkerboard;
    }

    public override Intersection Intersect(Ray ray)
    {
        float t = -(Vector3.Dot(ray.O, this.N) + d) / (Vector3.Dot(ray.D, this.N));
        if ((t < ray.t) && (t > 0))
        {
            ray.t = t;
            return new Intersection(t, ray.O + t * ray.D, this.N, this);
        }
        else return null;
        // TODO: checken of er een probleem is als de ray parallel is aan het vlak
    }
}

public class Sphere : Primitive
{
    public Vector3 center; // center of sphere
    public float r; // radius of sphere
    public float r2; // radius squared
    public float divr; // 1/r

    public Sphere(Vector3 center, float r, Vector3 color, bool checkerboard)
    {
        this.center = center;
        this.r = r;
        this.r2 = r * r;
        this.divr = 1 / r;
        this.color = color;
        this.checkerboard = checkerboard;
    }

    public override Intersection Intersect(Ray ray)
    {
        Vector3 c = this.center - ray.O;
        float t = Vector3.Dot(c, ray.D);
        Vector3 q = c - t * ray.D;
        float p2 = Vector3.Dot(q, q);
        if (p2 > this.r2) return null;
        t -= (float)(Math.Sqrt(this.r2 - p2));
        if ((t < ray.t) && (t > 0))
        {
            ray.t = t;
            Vector3 P = ray.O + t * ray.D; // intersection point of ray with sphere
            return new Intersection(t, P, (P - this.center) * divr, this);
        }
        else return null;
    }
}