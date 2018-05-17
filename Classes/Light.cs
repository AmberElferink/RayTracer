
using OpenTK;

public class Light
{
    public Vector3 position;
    public Vector3 color;

    public Light(Vector3 pos, Vector3 col)
    {
        position = pos;
        color = col;
        //= new Vector3(0, -15, 5); //5 behind and 5 above the camera
        // = new Vector3(255, 255, 255);
    }

    public Color(Intersection intersection)
    {
        Vector3 L = this.position - intersection.point;
        float dist = L.Length;
        L *= (1.0f / dist);
        
    }
}
