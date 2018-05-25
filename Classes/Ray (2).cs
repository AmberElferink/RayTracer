using System;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

struct Ray
{
    Vector3 O; // ray origin
    Vector3 D; // ray direction (normalized)
    float t; // distance

    public Ray(Vector3 O, Vector3 D, float t)
    {
        this.O = O;
        this.D = D;
        this.t = t;
    }
}