using OpenTK;

public class Material
{
    public enum materialType { diffuse, reflective, dielectric }; 
    public int type; // type of material (0 = diffuse, 1 = reflective, 2 = dielectric)
    public Vector3 color; // color of the material
    public float reflectiveness; // reflectiveness of the material (only relevant if the material is reflective)
    public float ior; // index of refraction (only relevant if the material is a dielectric)


    public Material(materialType type, Vector3 color, float reflectiveness, float ior = 1.0f)
    {
        this.type = (int)type; 
        this.color = color;
        this.reflectiveness = reflectiveness;
        this.ior = ior;
    }
}
