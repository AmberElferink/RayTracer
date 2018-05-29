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
            camera = new Camera(new Vector3(0, 0.75f, 0), new Vector3(0, 8.5f, 7f), 40); 
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
                    //generates a ray from the camera eye in the direction of the position of the pixel on the R3 plane
                    Vector3 D = (float)x / (float)RscreenWidth * (camera.p1 - camera.p0) + (float)y / (float)screen.height * (camera.p2 - camera.p0) + camera.p0 - camera.E;
                    D.Normalize();
                    Ray ray = new Ray(camera.E, D, 1E30f);

                    //Intersect this ray, and draw the corresponding color to the screen.
                    Intersection intersection = scene.Intersect(ray);
                    screen.pixels[x + y * screen.width] = CreateColor(scene.Trace(ray, intersection));


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
