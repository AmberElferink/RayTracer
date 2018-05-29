using OpenTK;
using System;


public class Light
{
    public Vector3 position; // position of the light
    public Vector3 color; // color of the light

    public Light(Vector3 pos, Vector3 col)
    {
        position = pos;
        color = col;
    }
}



public class Spotlight:Light
{
    public Vector3 direction;
    public float maxcosangle;
    float a;
    public Spotlight(Vector3 pos, Vector3 T, float angle, Vector3 col) : base(pos, col)
    {
        this.a = (float)(angle * Math.PI / 180); // input a must be in degrees; it is then converted to radians
        this.direction = Vector3.Normalize(T - pos);
        this.maxcosangle = -(float)Math.Cos(a);
    }
}
