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
            camera = new Camera();
            scene = new Scene();
	    }
	    // tick: renders one frame
	    public void Tick()
	    {
		    screen.Clear( 0 );
		    screen.Print( "hello world", 2, 2, 0xffffff );
            screen.Line(2, 20, 160, 20, 0xff0000);
	    }

        public void Render()
        {

            float addAngle = (camera.Viewangle.Y - camera.Viewangle.X)/screen.width;
            for (int y = 0; y < screen.height; y++)
            {
                float angle = camera.Viewangle.X;
                for (int x = 0; x < screen.width; x++)
                {
                    Vector3 D = x/screen.width


                    Ray ray = new Ray()
                    scene.Intersect(ray);


                    screen.pixels[x + y * screen.width] = ...;
                    angle += addAngle;
                }
            }
        }
    } // class raytracer

} // namespace Template