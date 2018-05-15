﻿using System;
using System.IO;

namespace Template {

class RayTracer
{
	// member variables
	public Surface screen;
    public Camera camera;
	// initialize
	public void Init()
	{
        camera = new Camera();
	}
	// tick: renders one frame
	public void Tick()
	{
		screen.Clear( 0 );
		screen.Print( "hello world", 2, 2, 0xffffff );
        screen.Line(2, 20, 160, 20, 0xff0000);
	}
}

} // namespace Template