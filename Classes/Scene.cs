using System;
using System.Collections.Generic;
using OpenTK;


namespace Template
{
    public class Scene
    {
        List<Light> lights = new List<Light>(); // all the lights in the scene
        List<Primitive> primitives = new List<Primitive>(); // all the primitives in the scene
        float eps = 0.00001f; // offset for calculating secondary rays and shadow rays
        int recursionDepth = 0;
        int maxRecursionDepth = 10;
        public Debug debug;

        public Scene()
        {

            Primitives.Add(
    new CheckeredPlane(
        new Vector3(0, 1, 0), 0,
        new Material(
            Material.materialType.diffuse,
            new Vector3(0.95f, 0.95f, 0.95f), 0)));


            Primitives.Add(
    new Plane(
        new Vector3(0, 1, 0), 5, // normal to the plane; d = -p(dot)N (p a point in the plane)
        new Material( // material of the plane
            Material.materialType.diffuse, // type of the material
            new Vector3(1,1,1f), 0.8f))); // color and reflectiveness of the material (color is irrelevant for CheckeredPlane)

            Primitives.Add(
              new Triangle(
                  new Vector3(-2f, 0.5f, 9f), // bottom left of triangle
                  new Vector3(2f, 0.5f, 9f), // bottom right of triangle
                  new Vector3(0f, 3.5f, 7.5f), // top of triangle
                  new Material(Material.materialType.reflective,
                  new Vector3(1, 1, 1), 1)));



            /*Primitives.Add(
              new Sphere(
                  new Vector3(0, 0.5f, 5.5f), 0.5f,
                  new Material(
                      Material.materialType.dielectric,
                      new Vector3(), 0.6f, 1.5f)));*/


            Primitives.Add(
              new Sphere(
                  new Vector3(0, 0.5f, 5.5f), 0.5f,
                  new Material(
                      Material.materialType.reflective,
                      new Vector3(0.05f, 1, 0.05f), 1f)));
            Primitives.Add(
  new Sphere(
      new Vector3(0.5f, 0.5f, 6.5f), 0.5f,
      new Material(
          Material.materialType.reflective,
          new Vector3(1f, 0.05f, 0.05f), 1f)));

            Primitives.Add(
new Sphere(
new Vector3(-0.5f, 0.5f, 6.5f), 0.5f,
new Material(
Material.materialType.reflective,
new Vector3(0.05f, 0.05f, 1f), 1f)));



            Primitives.Add(
                new Sphere(
                    new Vector3(0, 1f, 6f), 0.5f,
                    new Material(
                        Material.materialType.dielectric, new Vector3(), 0f, 1.5f)));
            /*
            Primitives.Add(
  new Sphere(
      new Vector3(0, 1f, 6f), 0.5f,
      new Material(
          Material.materialType.reflective,
          new Vector3(0.05f, 1, 0.05f), 1f)));*/


            Primitives.Add(
    new Plane(
        new Vector3(0, 0, 1), 10,
        new Material(
            Material.materialType.diffuse,
            new Vector3(1f, 1f, 1f), 0)));

            /*Primitives.Add(new CheckeredPlane(
    new Vector3(0, 1, 0), 2, // normal to the plane; d = -p(dot)N (p a point in the plane)
    new Material( // material of the plane
        Material.materialType.reflective, // type of the material
        new Vector3(), 0.8f))); // color and reflectiveness of the material (color is irrelevant for CheckeredPlane)*/
            Primitives.Add(
                new CheckeredPlane(
                    new Vector3(0, 1, 0), -5,
                    new Material(
                        Material.materialType.diffuse,
                        new Vector3(0.95f, 0.95f, 0.95f), 0)));
            Primitives.Add(
                new Plane(
                    new Vector3(0, 0, -1), 10,
                    new Material(
                        Material.materialType.diffuse,
                        new Vector3(1, 0.1f, 0.1f), 0)));
            Primitives.Add(
                new Plane(
                    new Vector3(-1, 0, 0), 6,
                    new Material(Material.materialType.diffuse,
                    new Vector3(0.7f, 0.1f, 0.7f), 0)));


            lights.Add(new Light(new Vector3(1, 6, 3), new Vector3(10, 10, 10))); // position and color of the light
            lights.Add(new Light(new Vector3(1, 6, 7), new Vector3(10, 10, 10))); // position and color of the light
            lights.Add(new Light(new Vector3(0, 5, 0), new Vector3(37, 35, 30))); // position and color of the light
            lights.Add(new Spotlight(new Vector3(0, 3, 6), new Vector3(0, 1, 6), 30, new Vector3(1000, 1000, 1000)));

            /*Primitives.Add(
                new Sphere(
                    new Vector3(1.5f, 0, 4), 0.8f, // center and radius of the sphere 
                    new Material( // material of the sphere
                        Material.materialType.reflective, // type of the material
                        new Vector3(1, 1, 1), 1))); // color, reflectiveness and index of refraction of the material (here: no index of refraction)
            Primitives.Add(
                new Sphere(
                    new Vector3(1.5f, -0.5f, 2), 0.4f, 
                    new Material( 
                        Material.materialType.dielectric,
                        new Vector3(), 0.6f, 1.5f)));
            Primitives.Add(
                new Sphere(
                    new Vector3(0.2f, -0.7f, 2.5f), 0.5f,
                    new Material(
                        Material.materialType.reflective,
                        new Vector3(0.1f, 1, 0.1f), 0.7f)));
            Primitives.Add(
                new Sphere(new Vector3(-0.3f, 0.15f, 4), 0.8f,
                new Material(
                    Material.materialType.reflective,
                    new Vector3(0.1f, 0.1f, 1), 0.7f)));
            Primitives.Add(
                new Triangle(
                    new Vector3(-1.5f, -1, 5.4f), // bottom left of triangle
                    new Vector3(3.2f, -1, 5.4f), // bottom right of triangle
                    new Vector3(0.7f, 3.3f, 4), // top of triangle
                    new Material(Material.materialType.reflective,
                    new Vector3(1, 1, 1), 1)));
            Primitives.Add(
                new CheckeredPlane(
                    new Vector3(0, 1, 0), 2, // normal to the plane; d = -p(dot)N (p a point in the plane)
                    new Material( // material of the plane
                        Material.materialType.reflective, // type of the material
                        new Vector3(), 0.8f))); // color and reflectiveness of the material (color is irrelevant for CheckeredPlane)
            Primitives.Add(
                new CheckeredPlane(
                    new Vector3(0, 1, 0), -5,
                    new Material(
                        Material.materialType.diffuse,
                        new Vector3(0.95f, 0.95f, 0.95f), 0)));
            Primitives.Add(
                new Plane(
                    new Vector3(0, 0, -1), 12,
                    new Material(
                        Material.materialType.diffuse,
                        new Vector3(1, 0.1f, 0.1f), 0)));
            Primitives.Add(
                new Plane(
                    new Vector3(-1, 0, 0), 6,
                    new Material(Material.materialType.diffuse,
                    new Vector3(0.7f, 0.1f, 0.7f), 0)));
            lights.Add(new Light(new Vector3(1, 6, 3), new Vector3(10, 10, 10))); // position and color of the light
            lights.Add(new Light(new Vector3(-3, 3, 3.2f), new Vector3(10, 10, 10)));
            lights.Add(new Light(new Vector3(0, 1, 0), new Vector3(15, 15, 15))); 
            lights.Add(new Spotlight(
                new Vector3(-1, 2, -1), // position of spotlight
                new Vector3(0.2f, -0.7f, 2.5f), 30, // direction and angle of spotlight
                new Vector3(80f, 80f, 80f))); // color of spotlight*/
        }



        // method that returns Intersection of a certain ray (primitive it intersects, distance, normal at intersection point etc.)
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



        // method that traces a ray and returns the color of the pixel the ray is shot through
        public Vector3 Trace(Ray ray, Intersection intersection)
        {
            // if the ray finds does not intersect any primitive, return black
            if (intersection == null)
                return new Vector3(0, 0, 0);

            // if the ray intersects a (partially) reflective material, we start tracing the reflected ray (recursively)
            if (intersection.Type == (int)Material.materialType.reflective && recursionDepth < maxRecursionDepth)
            {
                recursionDepth++;
                float reflectiveness = intersection.Reflectiveness;
                return Reflection(ray, intersection, reflectiveness) + (1 - reflectiveness) * TotalLight(intersection);
            }

            // if the ray intersects a dielectric, we start tracing the reflected ray and (possibly) the refracted ray
            if(intersection.Type == (int)Material.materialType.dielectric && recursionDepth < maxRecursionDepth)
            {
                recursionDepth++;
                return Fresnel(ray, intersection);
            }

            // if the ray intersects a diffuse material, we directly calculate the light transport from all the light sources to the intersection point
            recursionDepth = 0;
            return TotalLight(intersection);
        }



        // method that starts tracing the reflected and refracted ray (for dielectrics)
        public Vector3 Fresnel(Ray ray, Intersection intersection)
        {
            // source of formulas: 'Fundamentals of Computer Graphics' by Peter Shirley
            float cos, n, r0, r; // cosine of the angle of incidence, index of refraction, reflectance at normal incidence
            float dotpr = Vector3.Dot(ray.D, intersection.norm);
            if (dotpr < 0)
            {
                cos = -dotpr;
                n = intersection.IndexOfRefraction;
            }
            else
            {
                cos = dotpr;
                n = 1 / intersection.IndexOfRefraction;
            }
            r0 = ((n - 1) * (n - 1)) / ((n + 1) * (n + 1)); // reflectance at normal incidence
            r = r0 + (1 - r0) * (float)Math.Pow(1 - cos, 5); // reflectance
            float inRoot = 1 - (1 - dotpr * dotpr) / (n * n);

            if (inRoot < 0) r = 1; // in this case, there is total internal reflection; there is no refracted ray
            return Reflection(ray, intersection, r) + Refraction(ray, intersection, dotpr, n, inRoot, r); // part of the light gets reflected, part gets refracted
        }



        // method that calculates and traces a reflected ray
        public Vector3 Reflection(Ray ray, Intersection intersection, float r)
        {
            Vector3 R = ray.D - 2 * intersection.norm * Vector3.Dot(ray.D, intersection.norm); // the direction of the reflected ray
            Ray newray = new Ray(intersection.point + eps * R, R, 1E30f); // the reflected ray

            Intersection newIntersection = Intersect(newray);

            if (newIntersection != null && (newIntersection.prim is Sphere || newIntersection.prim is Triangle))
                    debug.DrawRay(intersection.point, newIntersection.point, 2);

            if (intersection.prim is CheckeredPlane)
            {
                CheckeredPlane checkplane = (CheckeredPlane)intersection.prim;
                Vector3 checkeredColor = checkplane.GetPixelColor(intersection.point);
                return r * checkeredColor * Trace(newray, newIntersection);
            }
            else
                return r * intersection.Color * Trace(newray, newIntersection);
        }



        // method that calculates and traces a refracted ray
        public Vector3 Refraction(Ray ray, Intersection intersection, float dotpr, float ior, float inRoot, float r)
        {
            // source of formulas: 'Fundamentals of Computer Graphics' by Peter Shirley
            Vector3 t = (ray.D - intersection.norm * dotpr) / ior - intersection.norm * (float)Math.Sqrt(inRoot); // the direction of the refracted ray
            Ray newray = new Ray(intersection.point + eps * t, t, 1E30f); // the refracted ray
            Intersection newIntersection = Intersect(newray);
            return (1 - r) * Trace(newray, newIntersection);
        }



        // method that returns the total light on an intersection point
        public Vector3 TotalLight(Intersection intersection)
        {
            Vector3 totalLight = new Vector3(0, 0, 0);
            foreach (Light light in lights)
            {
                Vector3 L = light.position - intersection.point; // direction of a ray from the intersection point to the light source
                float dist = L.Length;
                L = Vector3.Normalize(L);
                float dotpr = Vector3.Dot(intersection.norm, L);
                if (dotpr > 0)
                {
                    bool occluded = Occlusion(L, dist, intersection, light);
                    if (occluded) continue; // if the intersection point is not reached by light from this source, we move on to the next light source
                    else totalLight = LightTransport(L, dist, intersection, light.color, totalLight);
                }
            }
            if (totalLight.X > 1) totalLight.X = 1; // the value of each color component is clamped to 1
            if (totalLight.Y > 1) totalLight.Y = 1;
            if (totalLight.Z > 1) totalLight.Z = 1;
            return totalLight;
        }



        // method that casts a shadow ray to check if an intersection point is occluded from a light source
        public bool Occlusion(Vector3 L, float dist, Intersection intersection, Light light) 
        {
            float tmax = dist - 2 * eps; // distance from intersection point to light, with correction for offset
            Ray shadowray = new Ray(intersection.point + eps * L, L, tmax);
            Intersection occluder = Intersect(shadowray);
            if (occluder != null) // if the shadow ray finds an intersection with another primitive, the original intersection point is occluded
                return true;
            else if (light is Spotlight)
            {
                Spotlight light2 = (Spotlight)light;
                float cosShadowrayLight = Vector3.Dot(light2.direction, shadowray.D) / (light2.direction.Length * shadowray.D.Length);
                if (cosShadowrayLight > light2.maxcosangle)
                    return true;
            }
            debug.DrawRay(intersection.point, light.position, 1);
            return false;
        }



        // method that calculates the light transport from all the light sources illuminating a point
        public Vector3 LightTransport(Vector3 L, float dist, Intersection intersection, Vector3 lightColor, Vector3 totalLight) 
        {
            // source of formulas: slides lecture 5+6
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
