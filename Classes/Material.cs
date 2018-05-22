using OpenTK;

public class Material
{
    public enum materialType { diffuse, reflective }; // evt nog dielectric
    public int type;
    public Vector3 color;
    public float reflectiveness;

    public Material(materialType type, Vector3 color, float reflectiveness = 0)
    {
        this.type = (int)type; // 0 = diffuse, 1 = reflective
        this.color = color;
        this.reflectiveness = reflectiveness;
    }
}
