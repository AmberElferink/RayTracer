using System;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

public class Camera
{
    public Vector3 E; // camera position
    public Vector3 V; // view direction; must be normalized
    public double a; // field of view-angle in degrees
    public float w; // width of screen
    public float h; // height of screen
    public Vector3 C; // center of screen 
    public Vector3 p0; // upper left corner of screen
    public Vector3 p1; // upper right corner of screen
    public Vector3 p2; // bottom left corner of screen

    public Camera(Vector3 E, Vector3 T, double a)
    {
        this.E = E;
        this.V = Vector3.Normalize(T - E); // T = target
        this.a = a * Math.PI / 180; // in radians
        this.w = (float)(2 * Math.Tan(this.a / 2));
        this.h = this.w;
        this.C = E + V;
        Vector3 y = new Vector3(0, 1, 0);
        Vector3 right = Vector3.Normalize(Vector3.Cross(this.V, y));
        Vector3 up = Vector3.Cross(right, this.V); // already normalized
        this.p0 = this.C - (w / 2) * right + (h / 2) * up;
        this.p1 = this.p0 + w * right;
        this.p2 = this.p0 - h * up;

        /* this.E = new Vector3(0, 0, 0);
        this.V = new Vector3(0, 0, 1);
        this.C = E + 1 * V; // later: maak van 1 d, berekenen mbv field of view-angle
        this.p0 = C + new Vector3(-1, -1, 0);
        this.p1 = C + new Vector3(1, -1, 0);
        this.p2 = C + new Vector3(-1, 1, 0); */
    }
}