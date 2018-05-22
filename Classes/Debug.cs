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
        }

        public void Render()
        {
            DrawCircles();

        }

        void DrawCircles()
        {
            int x;
            int y;
            foreach (Primitive primitive in scene.Primitives)
            {
                if(primitive is Sphere)
                {
                    Sphere sphere = (Sphere)primitive;
                    for (float theta = 0; theta < (float)2 * Math.PI; theta += (float)(2 * Math.PI / 100))   //2PI for a circle. Existing of 100 line pieces, so 100 steps
                    {

                        x = (int)(sphere.center.X + Math.Cos(theta) * sphere.r);
                        y = (int)(sphere.center.Z + Math.Sin(theta) * sphere.r);

                        screen.pixels[screen.width / 2 + x + (y + screen.height / 2) * screen.width] = CreateColor(new Vector3(1, 1, 1));
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

    }

}