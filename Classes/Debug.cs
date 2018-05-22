using System.Linq;
using OpenTK;
using System;

namespace Template
{

    public class Debug
    {
        Surface screen;
        Scene scene;
        public Debug(Surface screenApp, Scene scene)
        {
            screen = screenApp;
            this.scene = scene;
            DrawPrimitives();
            
        }

        void DrawPrimitives()
        {
            foreach (Primitive primitive in scene.Primitives)
            {
                //if(primitive == sphere)
               // sphere.center.X, sphere.center.Z
            }
        }

        public void Render()
        {
            for (int y = 0; y < screen.height; y++)
            {
                for (int x = screen.width/2; x < screen.width; x++)
                {
                       
                      //  screen.pixels[x + y * screen.width] = CreateColor(scene.LightTransport(intersection));
                    
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

    }

}