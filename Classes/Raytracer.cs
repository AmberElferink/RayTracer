using OpenTK;
using System;

namespace Template
{

    public class Raytracer
    {
        // member variables
        Surface screen;
        public Camera camera;
        public Scene scene;

        public Raytracer(Surface screenApp)
        {
            screen = screenApp;
            camera = new Camera(new Vector3(0, 0, 0), new Vector3(0, 0, 1), 90);
            scene = new Scene();
        }
        public void Render()
        {
            Ray ray;

            for (int y = 0; y < screen.height; y++)
            {

                for (int x = 0; x < screen.width; x++)
                {
                    Vector3 D = (float)x / (float)screen.width * (camera.p1 - camera.p0) + (float)y / (float)screen.height * (camera.p2 - camera.p0) + camera.p0 - camera.E;
                    D.Normalize();

                    ray = new Ray(camera.E, D, 1E30f);
                    Intersection intersection = scene.Intersect(ray);

                    if (intersection != null)
                        screen.pixels[x + y * screen.width] = CreateColor(intersection.prim.color);
                }
            }
        }

        int CreateColor(Vector3 color)
        {
            int r = (int)color.X*255;
            int g = (int)color.Y*255;
            int b = (int)color.Z*255;
            return (r << 16) + (g << 8) + b;
        }


    } //class raytracer
} //namespace template
