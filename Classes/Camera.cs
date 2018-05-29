using System;
using OpenTK;

public class Camera
{
    public Vector3 E; // camera position
    public Vector3 T; // target the camera is looking at
    public Vector3 V; // view direction; must be normalized
    public double a; // field of view-angle in degrees
    public Vector3 C; // center of screen 
    public Vector3 p0; // upper left corner of screen
    public Vector3 p1; // upper right corner of screen
    public Vector3 p2; // bottom left corner of screen


    public Camera(Vector3 E, Vector3 T, double a)
    {
        this.E = E;
        this.T = T;
        this.a = a * Math.PI / 180; // input a must be in degrees; it is then converted to radians
        InitNewCameraDirection();
    }



    public void InitNewCameraDirection()
    {
        V = Vector3.Normalize(T - E); 
        float w = (float)(2 * Math.Tan(this.a / 2)); // width of screen
        float h = w; // height of screen
        C = E + V; // screen center
        Vector3 y = new Vector3(0, 1, 0);
        Vector3 right = Vector3.Normalize(Vector3.Cross(y, V)); // direction 'right' on the screen
        Vector3 up = Vector3.Cross(V, right); // direction 'up' on the screen (already normalized)
        p0 = C - (w / 2) * right + (h / 2) * up;
        p1 = p0 + w * right;
        p2 = p0 - h * up;
    }
}