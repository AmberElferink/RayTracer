using System;
using System.Collections.Generic;
using OpenTK;

public class Scene
{
    List<Light> Lights = new List<Light>();
    List<Primitive> Primitives = new List<Primitive>();
    
	public Scene()
	{
        Primitives.Add(new Sphere(new Vector3(3, 0, 5), 2));
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
    
}
