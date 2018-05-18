﻿using System;
using System.Collections.Generic;
using OpenTK;

public class Scene
{
    List<Light> lights = new List<Light>();
    List<Primitive> primitives = new List<Primitive>();
    float eps = 0.00001f;

    public Scene()
	{
        Primitives.Add(new Sphere(new Vector3(-2, 0, 4), 2, new Vector3(1, 0.1f, 0.1f)));
        Primitives.Add(new Sphere(new Vector3(3, 0, 10), 2, new Vector3(0.1f, 1, 0.1f)));
        Primitives.Add(new Sphere(new Vector3(1, 2, 8), 2.5f, new Vector3(0.1f, 0.1f, 1)));
        Primitives.Add(new Plane(new Vector3(0, 1, 0), 3, new Vector3(0.3f, 0.75f, 1)));
        // TODO: testen of het werkt voor andere planes. (Geen problemen met minteken normaal?)
        Primitives.Add(new Plane(new Vector3(0, 0, -1), 11, new Vector3(1, 1, 0.1f)));
        // Update: dit vlak werkt ook!
        Lights.Add(new Light(new Vector3(1, 0, -1), new Vector3(25, 25, 25)));
        Lights.Add(new Light(new Vector3(0, 6, 0), new Vector3(12, 12, 12)));
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
            L = Vector3.Normalize(L);
            float tmax = dist - 2 * eps; // distance from intersection point to light, with correction for offset
            Ray ray = new Ray(intersection.point + eps * L, L, tmax);
            Intersection occluder = Intersect(ray);
            if (occluder == null)
            {
                float dotpr = Vector3.Dot(intersection.norm, L);
                if (dotpr > 0)
                    totalLight += light.color * Vector3.Dot(intersection.norm, L) * (1 / (dist * dist)) * intersection.prim.color;
                // IDEE: prim.color: voor een textured plane moet je weten welk punt je precies raakt, en daaruit bepalen wat de kleur is (schaakbord)
                // (dus in plaats van dat de hele plane één kleur heeft) Dit kan mbv twee parameters lambda1 en lambda2.
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
