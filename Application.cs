using System;
using System.IO;
using OpenTK;

namespace Template {

    class Application
    {
        public Surface screen;
        Raytracer raytracer;
        // initialize
        public void Init()
        {
            raytracer = new Raytracer(screen);
        }
        // tick: renders one frame
        public void Tick()
        {
            raytracer.Render();
        }
    } 
} 