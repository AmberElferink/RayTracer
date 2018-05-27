using OpenTK;

public class Material
{
    public enum materialType { diffuse, reflective, dielectric }; 
    public int type;
    public Vector3 color;
    public float reflectiveness;
    public float ior; // index of refraction

    public Material(materialType type, Vector3 color, float reflectiveness, float ior = 1.0f)
    {
        this.type = (int)type; // 0 = diffuse, 1 = reflective, 2 = dielectric
        this.color = color;
        this.reflectiveness = reflectiveness;
        this.ior = ior;
    }
}
