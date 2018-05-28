using System;
using System.IO;
using OpenTK;
using System.Diagnostics;
using OpenTK.Input;


namespace Template {

    class Application
    {
        public Surface screen;
        Raytracer raytracer;
        Stopwatch stopwatch;
        long prevMs = 0;
        int mouseDeadZone = 40;


        // initialize
        public void Init()
        {
            raytracer = new Raytracer(screen);
            stopwatch = Stopwatch.StartNew();
            
        }
        // tick: renders one frame
        public void Tick()
        {
            raytracer.Render();
            
            long curMs = stopwatch.ElapsedMilliseconds;
            Console.WriteLine(curMs - prevMs + " ms/frame");
            prevMs = curMs;
        }

        public void CameraActionKey(Key key)
        {
            if (key == Key.Down || key == Key.S)
            {
                if (raytracer.camera.E.Z <= 0.8)
                {
                    raytracer.camera.E.Z -= 0.1f;
                    raytracer.camera.T.Z -= 0.1f;
                }

            }
            if (key == Key.Up || key == Key.W)
            {
                raytracer.camera.E.Z += 0.1f;
                raytracer.camera.T.Z += 0.1f;
            }
            if (key == Key.Left || key == Key.A)
            {
                raytracer.camera.E.X -= 0.1f;
                raytracer.camera.T.X -= 0.1f;
            }
            if(key == Key.Right || key == Key.D)
            {
                raytracer.camera.E.X += 0.1f;
                raytracer.camera.T.X += 0.1f;
            }
            raytracer.camera.InitNewCameraDirection();
        }
        public void CameraActionMouse(int mouseX, int mouseY)
        {
            if (mouseX < screen.width/4 - mouseDeadZone)
            {
                raytracer.camera.T.X -= 0.003f;
                raytracer.camera.InitNewCameraDirection();
            }
            else if (mouseX > screen.width/4 - mouseDeadZone)
            {
                raytracer.camera.T.X += 0.003f;
                raytracer.camera.InitNewCameraDirection();
            }

            if(mouseY > screen.height/2 - mouseDeadZone)
            {
                raytracer.camera.T.Y -= 0.003f;
            }
            else if (mouseY < screen.height/2 - mouseDeadZone)
            {
                raytracer.camera.T.Y += 0.003f;
            }
        }
    } 
} 