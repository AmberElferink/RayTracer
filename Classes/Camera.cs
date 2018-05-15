using System;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

class Camera
{
    // member variables
    public Vector3 E; // camera position
    public Vector3 V; // view direction; must be normalized
    //public double a; // field of view-angle in degrees // convert fov to radians: multiply by Math.PI/180
    public Vector3 C; // center of the screen 
    public Vector3 p0; // screen corners
    public Vector3 p1;
    public Vector3 p2;
    // later: make screen corners 

    public Camera()
    {
        this.E = new Vector3(0, -10, 0);
        this.V = new Vector3(0, 0, 1);
        this.C = E + 1 * V; // later: maak van 1 d, berekenen mbv field of view-angle
        this.p0 = C + new Vector3(-1, -1, 0);
        this.p1 = C + new Vector3(1, -1, 0);
        this.p2 = C + new Vector3(-1, 1, 0);
    }
}