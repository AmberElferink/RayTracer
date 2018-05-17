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
    public Vector3 C; // center of screen 
    public Vector3 p0; // upper left corner of screen
    public Vector3 p1; // upper right corner of screen
    public Vector3 p2; // bottom left corner of screen


    public Camera(Vector3 E, Vector3 T, double a)
    {
        this.E = E;
        V = Vector3.Normalize(T - E); // T = target
        this.a = a * Math.PI / 180; // input a must be in degrees; it is then converted to radians
        float w = (float)(2 * Math.Tan(this.a / 2)); // width of screen
        float h = w; // height of screen
        C = E + V;
        Vector3 y = new Vector3(0, 1, 0);
        Vector3 right = Vector3.Normalize(Vector3.Cross(y, V));
        Vector3 up = Vector3.Cross(V, right); // already normalized
        p0 = C - (w / 2) * right + (h / 2) * up;
        p1 = p0 + w * right;
        p2 = p0 - h * up;

        /* this.E = new Vector3(0, 0, 0);
        this.V = new Vector3(0, 0, 1);
        this.C = E + 1 * V; // later: maak van 1 d, berekenen mbv field of view-angle
        this.p0 = C + new Vector3(-1, -1, 0);
        this.p1 = C + new Vector3(1, -1, 0);
        this.p2 = C + new Vector3(-1, 1, 0); 
        this.p2 = C + new Vector3(-1, 1, 0);*/
    }

    public double Viewangle
    {
        get { return a; }
    }
}