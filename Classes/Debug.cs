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
        int raycounter = 0;
        int raycounter1 = 0;
        int raycounter2 = 0;
        int color = 0;

        public Debug(Surface screenApp, Scene scene)
        {
            screen = screenApp;
            this.scene = scene;
            DscreenWidth = screen.width / 2; //width of debugscreen and left edge of debugscreen.

            scale = DscreenWidth / 10.0f;
            offsetX = DscreenWidth;
            translateX = DscreenWidth / 2;
            translateY = DscreenWidth / 2;

            color = CreateColor(new Vector3(1, 1, 0));

        }



        public void Render()
        {
            DrawCircles();
        }



        //draws the spheres of the scene
        void DrawCircles()
        {
            int x;
            int y;

            foreach (Primitive primitive in scene.Primitives)
            {
                if (primitive is Sphere)
                {
                    Sphere sphere = (Sphere)primitive;

                    int prevX = (int)((sphere.center.X + sphere.r) * scale + offsetX + translateX);
                    int prevY = (int)(-sphere.center.Z * scale + translateY);
                    for (float theta = 0; theta <= (float)2 * Math.PI + 1; theta += (float)(2 * Math.PI / 100))   //2PI for a circle. Existing of 100 line pieces, so 100 steps
                    {
                        x = (int)((sphere.center.X + Math.Cos(theta) * sphere.r) * scale + offsetX + translateX);
                        y = (int)((-sphere.center.Z + Math.Sin(theta) * sphere.r) * scale + translateY);
                        screen.Line(prevX, prevY, x, y, CreateColor(sphere.material.color));
                        prevX = x;
                        prevY = y;
                    }
                }
            }
        }



        //draws all different kind of rays. Is called from the classes where the rays are shot.
        public void DrawRay(Vector3 start, Vector3 end, int raynumber)
        {
            int startX = (int)(offsetX + translateX + start.X * scale);
            int endX = (int)(translateX + offsetX + end.X * scale);
            int startY = (int)(translateY + start.Z * -scale);
            int endY = (int)(translateY + end.Z * -scale);
            raycounter2 = 0;
            if (startX < DscreenWidth)
                startX = DscreenWidth;
            if (endX < DscreenWidth)
                endX = DscreenWidth;

            //per kind of ray, the raycounter is increased and the wanted amount of rays to be drawn in the debug can be selected by setting the racounter >= value
            if (raynumber == 0) //primary ray
            {
                color = CreateColor(new Vector3(1, 1, 0));
                raycounter++;
                if (raycounter >= 50)
                {
                    raycounter = 0;
                    screen.Line(startX,startY, endX, endY, color);
                }
            }
            else if (raynumber == 1) //shadowray
            {
                color = CreateColor(new Vector3(0, 0, 1));
                raycounter1++;
                if (raycounter1 >= 10000)
                {
                    raycounter1 = 0;
                    screen.Line(startX, startY, endX, endY, color);
                }
            }
            else if (raynumber == 2)  //reflection
            {
                color = CreateColor(new Vector3(0, 1, 1));
                raycounter2++;
                if(raycounter2 >= 800)
                {

                    screen.Line(startX, startY, endX , endY, color);
                }
            }
        }



        //converts the floats from 0 to 1 (or a bit more if too much light) in the vector to values of 0 to 255
        int CreateColor(Vector3 color)
        {
            int r = (int)(Math.Min(1, color.X) * 255);
            int g = (int)(Math.Min(1, color.Y) * 255);
            int b = (int)(Math.Min(1, color.Z) * 255);
            return (r << 16) + (g << 8) + b;
        }
    }
}