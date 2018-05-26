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
                    new Vector3(1.5f, 0, 4), 0.9f, // center and radius of the sphere
                    new Material( // material of the sphere
                        Material.materialType.reflective, // type of the material
                        new Vector3(1, 0.1f, 0.1f), 0.6f))); // color and reflectiveness of the material
            Primitives.Add(
                new Sphere(
                    new Vector3(0.2f, -0.7f, 2.5f), 0.55f,
                    new Material(
                        Material.materialType.reflective, 
                        new Vector3(0.1f, 1, 0.1f), 0.6f)));
            Primitives.Add(
                new Sphere(new Vector3(-0.3f, 0.15f, 4), 0.8f,
                new Material(
                    Material.materialType.reflective, 
                    new Vector3(0.1f, 0.1f, 1), 0.6f)));
            Primitives.Add(
                new CheckeredPlane(
                    new Vector3(0, 1, 0), 2, // normal to the plane; d = -N.P (P a point in the plane)
                    new Material( // material of the plane
                        Material.materialType.reflective, // type of the material
                        new Vector3(), 0.5f))); // color and reflectiveness of the material (color is irrelevant for CheckeredPlane)
            Primitives.Add(
                new Plane(
                    new Vector3(0, 1, 0), -5,
                    new Material(
                        Material.materialType.diffuse, 
                        new Vector3(0.95f, 0.95f, 0.95f)))); // color of the material
            Primitives.Add(
                new Plane(
                    new Vector3(0, 0, -1), 12, 
                    new Material(
                        Material.materialType.diffuse, 
                        new Vector3(1, 1, 0.7f))));
            Primitives.Add(
                new Plane(
                    new Vector3(-1, 0, 0), 6, 
                    new Material(Material.materialType.diffuse, 
                    new Vector3(1, 1, 0.7f))));
            lights.Add(new Light(new Vector3(-1, 2, -1), new Vector3(10, 10, 10))); // position and color of the light
            lights.Add(new Light(new Vector3(1, 6, 3), new Vector3(10, 10, 10)));
            lights.Add(new Light(new Vector3(2, 3, -4), new Vector3(2, 2, 10)));
            lights.Add(new Light(new Vector3(0, 1, 0), new Vector3(15, 15, 15)));
        }

        // Method that returns closest distance to an intersection with a primitive
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
        public Vector3 Trace(Ray ray, Intersection intersection, Debug debug, int raynumber)
        {
            // if the ray finds does not intersect any primitive, return black
            if (intersection == null)
                return new Vector3(0, 0, 0);

            // if the ray intersects a (partially) reflective material, we start tracing the reflected ray (recursively)
            if (intersection.Type == (int)Material.materialType.reflective && recursionDepth < maxRecursionDepth)
            {
                recursionDepth++;
                return Reflection(ray, intersection, debug, raynumber);
            }

            // if the ray intersects a diffuse material, we directly calculate the light transport from all the light sources to the intersection point
            recursionDepth = 0;
            return TotalLight(intersection);
        }

        public Vector3 Reflection(Ray ray, Intersection intersection, Debug debug, int raynumber) // if a ray gets reflected
        {
            Vector3 R = ray.D - 2 * intersection.norm * Vector3.Dot(ray.D, intersection.norm); // the direction of the reflected ray
            Ray newray = new Ray(intersection.point + eps * R, R, 1E30f); // the reflected ray


            Intersection newIntersection = Intersect(newray);

            raynumber++;
            if (newIntersection != null && newIntersection.prim is Sphere)
                if (raynumber >= 3)
                {
                    debug.DrawRay(intersection.point, newIntersection.point, raynumber);
                }

            float reflectiveness = intersection.Reflectiveness;

            if (intersection.prim is CheckeredPlane)
            {
                CheckeredPlane checkplane = (CheckeredPlane)intersection.prim;
                Vector3 checkeredColor = checkplane.GetPixelColor(intersection.point);
                return (1 - reflectiveness) * checkeredColor + reflectiveness * Trace(newray, newIntersection, debug, raynumber);
            }
            else
                return (1 - reflectiveness) * intersection.Color + reflectiveness * Trace(newray, newIntersection, debug, raynumber);
        }

        public Vector3 TotalLight(Intersection intersection) // the total light on a point
        {
            Vector3 totalLight = new Vector3(0, 0, 0);
            foreach (Light light in lights)
            {
                Vector3 L = light.position - intersection.point;
                float dist = L.Length;
                L = Vector3.Normalize(L);
                float dotpr = Vector3.Dot(intersection.norm, L);
                if (dotpr > 0)
                {
                    bool occluded = occlusion(L, dist, intersection);
                    if (occluded) continue;
                    else totalLight = LightTransport(L, dist, intersection, light.color, totalLight);
                }
            }
            if (totalLight.X > 1) totalLight.X = 1;
            if (totalLight.Y > 1) totalLight.Y = 1;
            if (totalLight.Z > 1) totalLight.Z = 1;
            return totalLight;
        }

        public bool occlusion(Vector3 L, float dist, Intersection intersection) // checks if an intersection point is occluded from a light source by shooting a shadow ray
        {
            float tmax = dist - 2 * eps; // distance from intersection point to light, with correction for offset
            Ray shadowray = new Ray(intersection.point + eps * L, L, tmax);
            Intersection occluder = Intersect(shadowray);
            if (occluder != null) return true;
            else return false;
        }

        public Vector3 LightTransport(Vector3 L, float dist, Intersection intersection, Vector3 lightColor, Vector3 totalLight) // the light transport from all the light sources illuminating a point
        {
            float dotpr = Vector3.Dot(intersection.norm, L);
            if (dotpr > 0)
                if (intersection.prim is CheckeredPlane)
                {
                    CheckeredPlane checkplane = (CheckeredPlane)intersection.prim;
                    Vector3 checkeredColor = checkplane.GetPixelColor(intersection.point);
                    totalLight += lightColor * dotpr * (1 / (dist * dist)) * checkeredColor;
                }
                else
                    totalLight += lightColor * dotpr * (1 / (dist * dist)) * intersection.Color;
            return totalLight;
        }

        public List<Primitive> Primitives
        {
            get { return primitives; }
        }
    }
}
