using System.Linq;
using OpenTK;
using System;

namespace Template
{

    public class Debug
    {
        Surface screen;
        Scene scene;
        int translateX, translateY, offsetX;
        int DscreenWidth;
        float scale;
        public Debug(Surface screenApp, Scene scene)
        {
            screen = screenApp;
            this.scene = scene;
            DscreenWidth = screen.width / 2; //width of debugscreen and left edge of debugscreen.

            scale = DscreenWidth / 10.0f;
            offsetX = DscreenWidth;
            translateX = DscreenWidth / 2;
            translateY = DscreenWidth / 2;

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

                    int prevX = (int)((sphere.center.X + sphere.r) * scale + offsetX + translateX);
                    int prevY = (int)(-sphere.center.Z * scale + translateY);
                    for (float theta = 0; theta <= (float)2 * Math.PI + 1; theta += (float)(2 * Math.PI / 100))   //2PI for a circle. Existing of 100 line pieces, so 100 steps
                    {
                        x = (int)((sphere.center.X + Math.Cos(theta) * sphere.r) *scale + offsetX + translateX);
                        y = (int)((-sphere.center.Z + Math.Sin(theta) * sphere.r) * scale + translateY);
                        screen.Line(prevX, prevY, x, y, CreateColor(sphere.material.color));
                        prevX = x;
                        prevY = y;
                    }
                }
            }
        }

        public void DrawRay(Vector3 start, Vector3 end)
        {
            screen.Line((int)(offsetX + translateX + start.X*scale), (int)(translateY + start.Z*-scale), (int)(translateX + offsetX + end.X*scale), (int)(translateY + end.Z*-scale), CreateColor(new Vector3(1, 1, 0)));
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