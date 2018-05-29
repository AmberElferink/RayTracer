using OpenTK;
using System;
using OpenTK.Input;

namespace Template
{
    public class Raytracer
    {
        // member variables
        Debug debug;
        Surface screen;
        public Camera camera;
        public Scene scene;
        int RscreenWidth;

        public Raytracer(Surface screenApp)
        {
            screen = screenApp;
            camera = new Camera(new Vector3(0, 0.5f, -2.2f), new Vector3(0.5f, -0.15f, 0.8f), 40); 
            scene = new Scene();
            debug = new Debug(screen, scene);
            scene.debug = debug;
            RscreenWidth = screen.width / 2;
        }



        public void Render()
        {
            screen.Clear(0);
            debug.Render();

            //loops over all pixels on the screen
            for (int y = 0; y < screen.height; y++)
            {
                for (int x = 0; x < RscreenWidth; x++)
                {
                    int raynumber = 0;

                     Ray ray1 = new Ray(camera.E, D(x - 0.5f, y + 0.5f), 1E30f);
                     Ray ray2 = new Ray(camera.E, D(x - 0.5f, y - 0.5f), 1E30f);
                     Ray ray3 = new Ray(camera.E, D(x + 0.5f, y + 0.5f), 1E30f);
                     Ray ray4 = new Ray(camera.E, D(x + 0.5f, y - 0.5f), 1E30f);
                     Ray ray5 = new Ray(camera.E, D(x, y), 1E30f);

                    //Ray ray = new Ray(camera.E, D, 1E30f);
                    Intersection intersection1 = scene.Intersect(ray1);
                    Intersection intersection2 = scene.Intersect(ray2);
                    Intersection intersection3 = scene.Intersect(ray3);
                    Intersection intersection4 = scene.Intersect(ray4);
                    Intersection intersection5 = scene.Intersect(ray5);

                    Vector3 color1 = scene.Trace(ray1, intersection1, debug, raynumber);
                    Vector3 color2 = scene.Trace(ray2, intersection2, debug, raynumber);
                    Vector3 color3 = scene.Trace(ray3, intersection3, debug, raynumber);
                    Vector3 color4 = scene.Trace(ray4, intersection4, debug, raynumber);
                    Vector3 color5 = scene.Trace(ray5, intersection4, debug, raynumber);

                    Vector3 AverageColor; 
                    AverageColor.X = (color1.X + color2.X + color3.X + color4.X + color5.X) / 5;
                    AverageColor.Y = (color1.Y + color2.Y + color3.Y + color4.Y + color5.Y) / 5;
                    AverageColor.Z = (color1.Z + color2.Z + color3.Z + color4.Z + color5.Z) / 5;

                    screen.pixels[x + y * screen.width] = CreateColor(AverageColor);


                    //if there is an intersection, draw the debug output
                    if (intersection != null)
                    {
                        if (y == screen.height / 2) //only draw the rays of one row
                        {
                            //only draw the rays with intersections for spheres and triangles, otherwise, draw it als long as the ray is in the beginning.
                            if (intersection.prim is Sphere || intersection.prim is Triangle)
                            {
                                debug.DrawRay(camera.E, intersection.point, 0);
                            }
                            else
                            {
                                debug.DrawRay(camera.E, ray.D * ray.t, 0);
                            }
                        }
                    }

                }
            }
        }



        //convert the 0 to 1 (or higher for more light) float to values between 0 and 255
        int CreateColor(Vector3 color)
        {
            int r = (int)(Math.Min(1, color.X) * 255);
            int g = (int)(Math.Min(1, color.Y) * 255);
            int b = (int)(Math.Min(1, color.Z) * 255);
            return (r << 16) + (g << 8) + b;
        }
    } 
} 
