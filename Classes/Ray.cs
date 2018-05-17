﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;

public class Ray
{
    public Vector3 O; // ray origin
    public Vector3 D; // ray direction (normalized)
    public float t; // distance to nearest primitive

    public Ray(Vector3 O, Vector3 D, float t)
    {
        this.O = O;
        this.D = Vector3.Normalize(D);
        this.t = t;
    }
}

