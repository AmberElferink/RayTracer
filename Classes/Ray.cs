using System;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

struct Ray
{
    float3 O; // ray origin
    float3 D; // ray direction (normalized)
    float t; // distance
}