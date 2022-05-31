using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Maxwell_Sim
{
    class ObjectsMenu
    {
        Canvas menuCanvas;
        Button[] buttons;

        public ObjectsMenu(GameWindow window, GraphicsDevice gd, RectangleF bounds)
        {
            menuCanvas = new Canvas(window, gd, bounds);
        }



    }
}
