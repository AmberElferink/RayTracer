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
            if (key == Key.Down)
            {
                Console.WriteLine(raytracer.camera.E);
                if(raytracer.camera.E.Z <= 0.8)
                raytracer.camera.E.Z -= 0.1f;
            }
            if (key == Key.Up)
            {
                raytracer.camera.E.Z += 0.1f;
            }
            if(key == Key.Left)
            {
                raytracer.camera.E.X -= 0.1f;
                raytracer.camera.T.X -= 0.1f;
            }
            if(key == Key.Right)
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
                raytracer.camera.T.X -= 0.01f;
                raytracer.camera.InitNewCameraDirection();
            }
            else if (mouseX > screen.width/ - mouseDeadZone)
            {
                raytracer.camera.T.X += 0.01f;
                raytracer.camera.InitNewCameraDirection();
            }

            if(mouseY > screen.height/2 - mouseDeadZone)
            {
                raytracer.camera.T.Y -= 0.01f;
            }
            else if (mouseY < screen.height/2 - mouseDeadZone)
            {
                raytracer.camera.T.Y += 0.01f;
            }
        }
    } 
} 