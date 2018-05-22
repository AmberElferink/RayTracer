using OpenTK;

public class Material
{
    public enum materialType { diffuse, reflective }; // evt nog dielectric
    public int type;
    public Vector3 color;

    public Material(materialType type, Vector3 color)
    {
        this.type = (int)type;
        // 0 = diffuse, 1 = reflective
        if (this.type == 1)
            this.color = new Vector3(1, 1, 1); // reflective surfaces are always white
        else
            this.color = color; // diffuse surfaces can have any color
    }
}
