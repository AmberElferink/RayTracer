using System;
using System.Collections.Generic;
using OpenTK;

public class Scene
{
    List<Light> Lights = new List<Light>();
    List<Primitive> Primitives = new List<Primitive>();
    
	public Scene()
	{
        
	}

    ///<summary>
    ///Method that returns closest distance to an intersection with a primitive.
    ///</summary>
    /// <param name="ray">a ray that gets shot and maybe intersects a primitive</param>
    /// <returns></returns>
    public float Intersect(Ray ray)
    {
        foreach(Light light in Lights)
        {
            foreach(Primitive primitive in Primitives)
            {
                primitive.Intersect(ray);

            }
        }
        return ray.t;
    }
    
}
