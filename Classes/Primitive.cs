using System;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

public abstract class Primitive
{
    public Material material; // primitive can be made of diffuse or reflective material
    public abstract Intersection Intersect(Ray ray); 
    // Method that calculates distance from ray to primitive, and updates ray.t if this distance is shorter than the actual value of ray.t.
}



public class Plane : Primitive
{
    public Vector3 N; // normal of plane
    public float d; // equation p.N + d = 0 for a point p on the plane

    public Plane(Vector3 N, float d, Material material)
    {
        this.N = Vector3.Normalize(N);
        this.d = d;
        this.material = material;
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
    }
}






public class CheckeredPlane : Plane
{
    public CheckeredPlane(Vector3 N, float d, Material material) : base(N, d, material)
    {
        this.N = Vector3.Normalize(N);
        this.d = d;
        this.material = material;
    }

    public Vector3 GetPixelColor(Vector3 Point)
    {
        return (((int)(Math.Floor((2 * Point.X)) + Math.Floor(Point.Z))) & 1) * Vector3.One;
    }
}







public class Sphere : Primitive
{
    public Vector3 center; // center of sphere
    public float r; // radius of sphere
    public float r2; // radius squared
    public float divr; // 1/r

    public Sphere(Vector3 center, float r, Material material)
    {
        this.center = center;
        this.r = r;
        this.r2 = r * r;
        this.divr = 1 / r;
        this.material = material;
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






public class Triangle : Primitive
{
    Vector3 V1, u, v, w;
    float dot1, dot2, dot3;
    float d;
    Vector3 N;

    public Triangle (Vector3 V1, Vector3 V2, Vector3 V3, Material material)
    {
        this.material = material;

        this.V1 = V1;
        u = V2 - V1;
        v = V3 - V1;

        dot1 = Vector3.Dot(u, v);
        dot2 = Vector3.Dot(u, u);
        dot3 = Vector3.Dot(v, v);


        N = Vector3.Cross(u, v);
        N.Normalize();

      

    }

    public override Intersection Intersect(Ray ray)
    {
        d = -Vector3.Dot(N, V1);
        //adapted plane intersection method
        float t = -(Vector3.Dot(ray.O, this.N) + d) / (Vector3.Dot(ray.D, this.N));
        if ((t < ray.t) && (t > 0))
        {
            Intersection intersection = new Intersection(t, ray.O + t * ray.D, this.N, this);
            if (IntersectIsInTriangle(intersection))
            {
                ray.t = t;
                return intersection;
            }
        }
        return null;

    }

    bool IntersectIsInTriangle(Intersection intersection)
    {
        //used formula from: http://geomalgorithms.com/a06-_intersect-2.html
        w = intersection.point - V1;

        float dot4 = Vector3.Dot(w, v);
        float dot5 = Vector3.Dot(w, u);

        float denom = dot1 * dot1 - dot2 * dot3;

        float s = (dot1 * dot4 - dot3 * dot5) / denom;
        if(s >= 0)
        {
            float t = (dot1 * dot5 - dot2 * dot4) / denom;
            if(t >= 0)
            {
                if (s + t <= 1)
                    return true;
            }
        }
        return false;
    }

    
}