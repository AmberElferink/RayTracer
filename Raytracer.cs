using System;
using System.IO;
using OpenTK;

namespace Template {

    class RayTracer
    {
	    // member variables
	    public Surface screen;
        public Camera camera;
        public Scene scene;
	    // initialize
	    public void Init()
	    {
            camera = new Camera(new Vector3(0,0,0), new Vector3(0,0,1), 45);
            scene = new Scene();
	    }
	    // tick: renders one frame
	    public void Tick()
	    {
            Render();
	    }

        public void Render()
        {

            for (int y = 0; y < screen.height; y++)
            {
                
                for (int x = 0; x < screen.width; x++)
                {
                    Vector3 D = (float)x / (float)screen.width * (camera.p1 - camera.p0) + (float)y / (float)screen.height * (camera.p2 - camera.p0) + camera.p0 - camera.E;
                    D.Normalize();

                    Ray ray = new Ray(camera.E, D, 1E30f);
                    Intersection intersection = scene.Intersect(ray);

                    if(intersection != null)
                        screen.pixels[x + y * screen.width] = CreateColor(intersection.prim.color);
                }
            }
        }

        int CreateColor(Vector3 color)
        {
            int r = (int)color.X;
            int g = (int)color.Y;
            int b = (int)color.Z;
            return (r << 16) + (g << 8) + b;
        }
    } // class raytracer

} // namespace Template