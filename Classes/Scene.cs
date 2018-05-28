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
        public Debug debug;

        public Scene()
        {
            Primitives.Add(
                new Sphere(
                    new Vector3(1.5f, 0, 4), 0.9f, // center and radius of the sphere (WAS: 1.5f, 0, 4)
                    new Material( // material of the sphere
                        Material.materialType.reflective, // type of the material
                        new Vector3(1, 0.1f, 0.1f), 0.6f))); // color, reflectiveness and index of refraction of the material WAS: 0.6f
            Primitives.Add(
                new Sphere(
                    new Vector3(1.6f, -0.5f, 2), 0.4f, 
                    new Material( 
                        Material.materialType.dielectric,
                        new Vector3(), 0.6f, 1.5f)));
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
                new Triangle(
                    new Vector3(-0.3f, 0.95f, 1.6f), //bottom left
                    new Vector3(1.5f, 0.1f, 5.4f), //bottom right
                    new Vector3(-0.7f, 5.3f, 4), //top
                    new Material(Material.materialType.reflective,
                    new Vector3(0.2f, 0.2f, 0.95f), 0.6f)));
            Primitives.Add(
                new CheckeredPlane(
                    new Vector3(0, 1, 0), 2, // normal to the plane; d = -N DOT P (P a point in the plane)
                    new Material( // material of the plane
                        Material.materialType.reflective, // type of the material
                        new Vector3(), 0.5f))); // color and reflectiveness of the material (color is irrelevant for CheckeredPlane)
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
                        new Vector3(1, 1, 0.7f), 0)));
            Primitives.Add(
                new Plane(
                    new Vector3(-1, 0, 0), 6,
                    new Material(Material.materialType.diffuse,
                    new Vector3(1, 1, 0.7f), 0)));
            lights.Add(new Light(new Vector3(-1, 2, -1), new Vector3(10, 10, 10))); // position and color of the light
            lights.Add(new Light(new Vector3(1, 6, 3), new Vector3(10, 10, 10)));
            lights.Add(new Light(new Vector3(2, 3, -4), new Vector3(2, 2, 10)));
            lights.Add(new Light(new Vector3(0, 1, 0), new Vector3(15, 15, 15)));*/
            lights.Add(new Spotlight(new Vector3(-1, 2, -1), new Vector3(0.2f, -0.7f, 2.5f), 30, new Vector3(100f, 80f, 20f)));
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
        public Vector3 Trace(Ray ray, Intersection intersection)
        {
            // if the ray finds does not intersect any primitive, return black
            if (intersection == null)
                return new Vector3(0, 0, 0);

            // if the ray intersects a (partially) reflective material, we start tracing the reflected ray (recursively)
            if (intersection.Type == (int)Material.materialType.reflective && recursionDepth < maxRecursionDepth)
            {
                recursionDepth++;
                // NIEUW: ik heb hier al het (1-r)*TotalLight gedeelte gestopt, dan hoeft dat niet in Reflection en hoef je daar geen onderscheid in te maken
                // tussen dielectric en reflective. Zie ook Reflection.
                float reflectiveness = intersection.Reflectiveness;
                return Reflection(ray, intersection, debug, raynumber, reflectiveness) + (1 - reflectiveness) * TotalLight(intersection);
            }

            // if the ray intersects a dielectric, we start tracing the reflected ray and possibly the refracted ray
            if(intersection.Type == (int)Material.materialType.dielectric && recursionDepth < maxRecursionDepth)
            {
                recursionDepth++;
                return Fresnel(ray, intersection, debug, raynumber);
            }

            // if the ray intersects a diffuse material, we directly calculate the light transport from all the light sources to the intersection point
            recursionDepth = 0;
            return TotalLight(intersection);
        }

        public Vector3 Fresnel(Ray ray, Intersection intersection, Debug debug, int raynumber)
        {
            float cos, n, r0, r; // cos(theta), index of refraction, reflectance at normal incidence
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
            /*if (inRoot < 0) // in this case, there is total internal reflection; there is no refracted ray
                return Reflection(ray, intersection, debug, raynumber, r);
            else
                return Reflection(ray, intersection, debug, raynumber, r) + Refraction(ray, intersection, dotpr, n, inRoot, r, debug, raynumber);*/

            // NIEUW: in plaats van de bovenstaande regels, hieronder een iets andere formulering. Als r = 1 zal Refraction sowieso 0 opleveren, dus het komt op hetzelfde neer.
            // Voordeel: we hebben nu één regel voor beide gevallen, ziet er wellicht mooier uit. Nadeel: we moeten nu altijd de functie Refraction aanroepen, ook als daar
            // niks uit gaat komen, wat extra tijd kost. We kunnen hier dus nog even kiezen welk van de twee we doen.

            if (inRoot < 0) r = 1; // in this case, there is total internal reflection; there is no refracted ray
            return Reflection(ray, intersection, debug, raynumber, r) + Refraction(ray, intersection, dotpr, n, inRoot, r, debug, raynumber);
        }

        public Vector3 Reflection(Ray ray, Intersection intersection, Debug debug, int raynumber, float r) // if a ray gets reflected
        {
            Vector3 R = ray.D - 2 * intersection.norm * Vector3.Dot(ray.D, intersection.norm); // the direction of the reflected ray
            Ray newray = new Ray(intersection.point + eps * R, R, 1E30f); // the reflected ray

            Intersection newIntersection = Intersect(newray);

            
            if (newIntersection != null && (newIntersection.prim is Sphere || newIntersection.prim is Triangle))
                    debug.DrawRay(intersection.point, newIntersection.point, 2);


            // NIEUW: alles wat tussen /* */ staat hierboven én hieronder kunnen we vervangen door de regel return r* Trace(....).
            // We hebben namelijk gezorgd dat het (1-reflectiveness)*color gedeelte al wordt aangeroepen in de functie Trace bij het geval materialtype = reflective.
            // Bovendien zorgen we daar, door TotalLight aan te roepen, ervoor dat we ook meenemen dat die kleur alleen gereturnd wordt als er ook licht op schijnt
            // (dat is hieronder nog niet zo). Deze functie wordt hiermee een stuk korter, mooier, duidelijker.

            return r * Trace(newray, newIntersection, debug, raynumber);
            /*if (intersection.prim is CheckeredPlane)
            {
                CheckeredPlane checkplane = (CheckeredPlane)intersection.prim;
                Vector3 checkeredColor = checkplane.GetPixelColor(intersection.point);
                return (1 - reflectiveness) * checkeredColor + reflectiveness * Trace(newray, newIntersection);
            }
            else
                return (1 - reflectiveness) * intersection.Color + reflectiveness * Trace(newray, newIntersection, debug, raynumber);*/
        }

        public Vector3 Refraction(Ray ray, Intersection intersection, float dotpr, float ior, float inRoot, float r, Debug debug, int raynumber) // if a ray gets refracted
        {
            Vector3 t = (ray.D - intersection.norm * dotpr) / ior - intersection.norm * (float)Math.Sqrt(inRoot);
            //t = Vector3.Normalize(t);
            Ray newray = new Ray(intersection.point + eps * t, t, 1E30f); // the refracted ray
            Intersection newIntersection = Intersect(newray);
            raynumber++; // weet nog niet of dit moet
            // misschien moet er nog iets met de debug gebeuren hier.
            return (1 - r) * Trace(newray, newIntersection, debug, raynumber);
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
                    bool occluded = Occlusion(L, dist, intersection, light);
                    if (occluded) continue;
                    else totalLight = LightTransport(L, dist, intersection, light.color, totalLight);
                }
            }
            if (totalLight.X > 1) totalLight.X = 1;
            if (totalLight.Y > 1) totalLight.Y = 1;
            if (totalLight.Z > 1) totalLight.Z = 1;
            return totalLight;
        }

        public bool Occlusion(Vector3 L, float dist, Intersection intersection, Light light) // checks if an intersection point is occluded from a light source by shooting a shadow ray
        {
            float tmax = dist - 2 * eps; // distance from intersection point to light, with correction for offset
            Ray shadowray = new Ray(intersection.point + eps * L, L, tmax);
            Intersection occluder = Intersect(shadowray);
            if (occluder != null)
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
