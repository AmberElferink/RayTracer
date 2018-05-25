using System;
using System.Collections.Generic;
using OpenTK;


namespace Template
{
    public class Scene
    {
        List<Light> lights = new List<Light>();
        List<Primitive> primitives = new List<Primitive>();
        float eps = 0.00001f;
        int recursionDepth = 0;
        int maxRecursionDepth = 10;

        public Scene()
        {
            Primitives.Add(
                new Sphere(
                    new Vector3(2, 0, 4), 1, // center and radius of the sphere
                    new Material( // material of the sphere
                        Material.materialType.reflective, // type of the material
                        new Vector3(1, 0.1f, 0.1f), 0.6f), // color and reflectiveness of the material
                    false)); // is it a checkerboard
            Primitives.Add(
                new Sphere(
                    new Vector3(0.2f, -0.7f, 2.5f), 0.5f, 
                    new Material(
                        Material.materialType.reflective, 
                        new Vector3(0.1f, 1, 0.1f), 0.6f), 
                    false));
            Primitives.Add(
                new Sphere(new Vector3(-0.3f, 0.2f, 4), 1, 
                new Material(
                    Material.materialType.reflective, 
                    new Vector3(0.1f, 0.1f, 1), 0.6f), 
                false));
            Primitives.Add(
                new CheckeredPlane( 
                    new Vector3(0, 1, 0), 2, // normal to the plane; d = -N.P (P a point in the plane)
                    new Material( // material of the plane
                        Material.materialType.reflective, // type of the material
                        new Vector3(0.3f, 0.75f, 1), // color of the material
                        0.5f), // reflectiveness of the material
                    true)); // is it a checkerboard
            Primitives.Add(
                new Plane(
                    new Vector3(0, 1, 0), -5, 
                    new Material(
                        Material.materialType.diffuse, 
                        new Vector3(0.95f, 0.95f, 0.95f)), 
                    false));
            Primitives.Add(
                new Plane(
                    new Vector3(0, 0, -1), 12, 
                    new Material(
                        Material.materialType.diffuse, 
                        new Vector3(1, 1, 0.7f)), 
                    false));
            Primitives.Add(
                new Plane(
                    new Vector3(-1, 0, 0), 6, 
                    new Material(Material.materialType.diffuse, 
                    new Vector3(1, 1, 0.7f)), 
                    false));
            lights.Add(new Light(new Vector3(-1, 2, -1), new Vector3(20, 20, 20))); // position and color of the light
            lights.Add(new Light(new Vector3(1, 6, 3), new Vector3(10, 10, 10)));
            lights.Add(new Light(new Vector3(2, 3, -4), new Vector3(2, 2, 10)));
            lights.Add(new Light(new Vector3(0, 1, 0), new Vector3(25, 25, 25)));
        }

        // Method that returns closest distance to an intersection with a primitive.
        public Intersection Intersect(Ray ray)
        {
            Intersection intersect = null;
            foreach (Primitive primitive in primitives)
            {
                Intersection temp = primitive.Intersect(ray);
                if (temp != null)
                    intersect = temp;
            }
            return intersect;
        }

        // Method that traces a ray
        public Vector3 LightTransport(Ray ray, Intersection intersection)
        {
            // if the ray finds does not intersect any primitive, return black
            if (intersection == null)
                return new Vector3(0, 0, 0);

            // if the ray intersects a reflective material, we start tracing the reflected ray (recursively)
            if (intersection.Type == (int)Material.materialType.reflective && recursionDepth < maxRecursionDepth)
            {
                recursionDepth++;
                Vector3 R = ray.D - 2 * intersection.norm * Vector3.Dot(ray.D, intersection.norm);
                Ray newray = new Ray(intersection.point + eps * R, R, 1E30f);

                if(intersection.prim is CheckeredPlane)
                {
                    CheckeredPlane checkplane = (CheckeredPlane)intersection.prim;
                    Vector3 checkeredColor = checkplane.GetPixelColor(intersection.point);
                    return checkeredColor * (1 - intersection.Reflectiveness) + 
                        (intersection.Reflectiveness) * LightTransport(newray, Intersect(newray));
                }
                else
                    return intersection.Color * (1 - intersection.Reflectiveness) +
                        (intersection.Reflectiveness) * LightTransport(newray, Intersect(newray));
            }

            // if the ray intersects a diffuse material, we start tracing shadow rays to all the light sources
            recursionDepth = 0;
            Vector3 totalLight = new Vector3(0, 0, 0);
            foreach (Light light in lights)
            {
                Vector3 L = light.position - intersection.point;
                float dist = L.Length;
                if (Vector3.Dot(intersection.norm, L) > 0)
                {
                    L = Vector3.Normalize(L);
                }
                float tmax = dist - 2 * eps; // distance from intersection point to light, with correction for offset
                Ray shadowray = new Ray(intersection.point + eps * L, L, tmax);
                Intersection occluder = Intersect(shadowray);
                if (occluder != null) //shadowray intersects, go to next lightsource
                {
                    continue;
                }
                else //shadowray doesn't intersect. Add light value for this.
                {
                    float dotpr = Vector3.Dot(intersection.norm, L);
                    if (dotpr > 0)
                        if (intersection.prim is CheckeredPlane)
                        {
                            CheckeredPlane checkplane = (CheckeredPlane)intersection.prim;
                            Vector3 checkeredColor = checkplane.GetPixelColor(intersection.point);
                            totalLight += light.color * dotpr * (1 / (dist * dist)) * checkeredColor;
                        }
                        else
                            totalLight += light.color * dotpr * (1 / (dist * dist)) * intersection.Color;
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
            get { return primitives; }
        }
    }
}
