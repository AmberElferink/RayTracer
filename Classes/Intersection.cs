﻿using OpenTK;

public class Intersection
{
    public float t; // distance from starting point to intersection point
    public Vector3 point; // intersection point
    public Vector3 norm; // normal at intersection point
    public Primitive prim; // primitive where the intersection is

    public Intersection(float t, Vector3 point, Vector3 norm, Primitive prim)
    {
        this.t = t;
        this.point = point;
        this.norm = norm;
        this.prim = prim;
    }

    public Vector3 Color
    {
        get { return prim.material.color; }
    }

    public float Reflectiveness
    {
        get { return prim.material.reflectiveness; }
    }

    public int Type
    {
        get { return prim.material.type; }
    }

    public float IndexOfRefraction
    {
        get { return prim.material.ior; }
    }
}

