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

        public void CameraAction(Key key)
        {
            if (key == Key.Down)
            {
                Console.WriteLine(raytracer.camera.E);
                if(raytracer.camera.E.Z <= 0.8)
                raytracer.camera.E.Z += 0.1f;
            }
            if (key == Key.Up)
            {
                raytracer.camera.E.Z -= 0.1f;
            }

        }
    } 
} 