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
        int raycounter = 0;

        public Raytracer(Surface screenApp)
        {
            screen = screenApp;
            camera = new Camera(new Vector3(0, 0.2f, -2.2f), new Vector3(0.5f, -1, 0.8f), 40);
            scene = new Scene();
            debug = new Debug(screen, scene);
            RscreenWidth = screen.width / 2;
        }
        public void Render()
        {

            screen.Clear(0);
            debug.Render();

            for (int y = 0; y < screen.height; y++)
            {
                for (int x = 0; x < RscreenWidth; x++)
                {
                    Vector3 D = (float)x / (float)RscreenWidth* (camera.p1 - camera.p0) + (float)y / (float)screen.height * (camera.p2 - camera.p0) + camera.p0 - camera.E;
                    D.Normalize();
                    int raynumber = 0;
                    Ray ray = new Ray(camera.E, D, 1E30f);
                    Intersection intersection = scene.Intersect(ray);
                    screen.pixels[x + y * screen.width] = CreateColor(scene.Trace(ray, intersection, debug, raynumber));
                    
                    //Debug output
                    if (y == screen.height / 2)
                    {
                        
                        raycounter++;
                        if(raycounter >= 15)
                        {
                            if (intersection.prim is Sphere)
                            {
                                    debug.DrawRay(camera.E, intersection.point, raynumber);
                            }
                            else
                            {
                                debug.DrawRay(camera.E, ray.D * ray.t, raynumber);
                            }
                            raycounter = 0;
                        }

                    }

                }
            }
        }

        int CreateColor(Vector3 color)
        {
            int r = (int)(Math.Min(1, color.X) * 255);
            int g = (int)(Math.Min(1, color.Y) * 255);
            int b = (int)(Math.Min(1, color.Z) * 255);
            return (r << 16) + (g << 8) + b;
        }
    } //class raytracer
} //namespace template
