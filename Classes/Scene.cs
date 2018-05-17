using System;
using System.Collections.Generic;
using OpenTK;

public class Scene
{
    List<Light> Lights = new List<Light>();
    List<Primitive> Primitives = new List<Primitive>();
    float eps = 0.00001f;

    public Scene()
	{
        Primitives.Add(new Sphere(new Vector3(-2, 4, 4), 2, new Vector3(1, 0.1f, 0.1f)));
        Primitives.Add(new Sphere(new Vector3(3, 0, 10), 2, new Vector3(0.1f, 1, 0.1f)));
        Lights.Add(new Light(new Vector3(1, 0, -1), new Vector3(25, 25, 25)));
        //Sphere sphere2 = new Sphere(new Vector3(2, 0, 5), 3);
    }

    ///<summary>
    ///Method that returns closest distance to an intersection with a primitive.
    ///</summary>
    /// <param name="ray">a ray that gets shot and maybe intersects a primitive</param>
    /// <returns></returns>
    public Intersection Intersect(Ray ray)
    {
        Intersection intersect = null;
        foreach(Primitive primitive in Primitives)
        {
            Intersection temp = primitive.Intersect(ray);
            if (temp != null)
                intersect = temp;
        }
        return intersect;
    }
    
    public Vector3 LightTransport(Intersection intersection) // hoort een kleur-vector (met helderheid) te returnen
    {
        Vector3 totalLight = new Vector3(0, 0, 0);
        foreach (Light light in Lights)
        {
            Vector3 L = light.position - intersection.point;
            float dist = L.Length;
            float tmax = dist - 2 * eps; // distance from intersection point to light, with correction for offset
            Ray ray = new Ray(intersection.point + eps * Vector3.Normalize(L), L, tmax);
            Intersection occluder = Intersect(ray);
            if (occluder == null)
            {
                float dotpr = Vector3.Dot(intersection.norm, Vector3.Normalize(L));
                if (dotpr > 0)
                    totalLight += light.color * Vector3.Dot(intersection.norm, Vector3.Normalize(L)) * (1 / (dist * dist)) * intersection.prim.color;
            }
                
            // TODO: hier moet nog de kleur van de primitieve in verwerkt worden. (Misschien vanuit andere methode)
        }
        return totalLight;
        // TODO: zorgen dat deze methode wordt aangeroepen vanuit Raytracer
        // TODO: zorgen dat totalLight altijd tussen 0 en 1 ligt. (Belangrijker als er meerdere lichtbronnen zijn)
    }
}
