using System;
using System.Collections.Generic;
using OpenTK;

public class Scene
{
    List<Light> lights = new List<Light>();
    List<Primitive> primitives = new List<Primitive>();
    float eps = 0.00001f;

    public Scene()
	{
        Primitives.Add(new Sphere(new Vector3(-2, 0, 4), 1, new Material(Material.materialType.diffuse, new Vector3(1, 0.1f, 0.1f)), false));
        Primitives.Add(new Sphere(new Vector3(-6, 1, 6), 1.5f, new Material(Material.materialType.diffuse, new Vector3(0.1f, 1, 0.1f)), false));
        Primitives.Add(new Sphere(new Vector3(1, 2, 7), 1.5f, new Material(Material.materialType.diffuse, new Vector3(0.1f, 0.1f, 1)), false));
        Primitives.Add(new Plane(new Vector3(0, 1, 0), 3, new Material(Material.materialType.diffuse, new Vector3(0.3f, 0.75f, 1)), true));
        // TODO: testen of het werkt voor andere planes. (Geen problemen met minteken normaal?)
        Primitives.Add(new Plane(new Vector3(0, 0, -1), 12, new Material(Material.materialType.diffuse, new Vector3(1, 1, 0.7f)), false));
        lights.Add(new Light(new Vector3(1, 0, -1), new Vector3(25, 25, 25)));
        lights.Add(new Light(new Vector3(0, 6, 0), new Vector3(12, 12, 12)));
        lights.Add(new Light(new Vector3(2, 3, 4), new Vector3(10, 10, 30)));
    }

    ///<summary>
    ///Method that returns closest distance to an intersection with a primitive.
    ///</summary>
    /// <param name="ray">a ray that gets shot and maybe intersects a primitive</param>
    /// <returns></returns>
    public Intersection Intersect(Ray ray)
    {
        Intersection intersect = null;
        foreach(Primitive primitive in primitives)
        {
            Intersection temp = primitive.Intersect(ray);
            if (temp != null)
                intersect = temp;
        }
        return intersect;
    }
    
    public Vector3 LightTransport(Intersection intersection) // geeft kleur 
    {
        Vector3 totalLight = new Vector3(0, 0, 0);
        foreach (Light light in lights)
        {
            Vector3 L = light.position - intersection.point;
            float dist = L.Length;
            if(Vector3.Dot(intersection.norm, L) > 0)
            {
                L = Vector3.Normalize(L);
            }

            float tmax = dist - 2 * eps; // distance from intersection point to light, with correction for offset
            Ray ray = new Ray(intersection.point + eps * L, L, tmax);
            Intersection occluder = Intersect(ray);
            if(occluder != null) //shadowray intersects, go to next lightsource
            {
                continue;
            }
            else //shadowray doesn't intersect. Add light value for this.
            {
                float dotpr = Vector3.Dot(intersection.norm, L);
                if (dotpr > 0)
                    if(!intersection.prim.checkerboard)
                        totalLight += light.color * Vector3.Dot(intersection.norm, L) * (1 / (dist * dist)) * intersection.prim.material.color;
                    else
                        totalLight += light.color * Vector3.Dot(intersection.norm, L) * (1 / (dist * dist)) * (((int)(2 * intersection.point.X) + (int)intersection.point.Z) & 1);
                // TODO: aanpassen met materialen
            }
        }
        if (totalLight.X > 1)
            totalLight.X = 1;
        if (totalLight.Y > 1)
            totalLight.Y = 1;
        if (totalLight.Z > 1)
            totalLight.Z = 1;
        return totalLight;
    }

    public List<Primitive> Primitives
    {
        get { return primitives;}
    }
}
