using OpenTK;

public class Material
{
    public enum materialType { diffuse, reflective }; // evt nog dielectric
    public int type;
    public Vector3 color;

    public Material(materialType type, Vector3 color)
    {
        this.type = (int)type; // 0 = diffuse, 1 = reflective
        this.color = color;
    }
}
