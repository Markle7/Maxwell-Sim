using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Maxwell_Sim
{
    /// <summary>
    /// This class defines and designs the menu for the simulation.
    /// </summary>
    class ObjectsMenu
    {
        Canvas menuCanvas;
        Button[] buttons;

        public ObjectsMenu(GameWindow window, GraphicsDevice gd, RectangleF bounds)
        {
            menuCanvas = new Canvas(window, gd, bounds);
            buttons = new Button[6];


        }


        public void LoadContent(Texture2D texture)
        {
            Texture2D[] textures = new Texture2D[6];
            textures[0] = texture;

            float aspectRatio = menuCanvas.AspectRatio;
            buttons[0] = new Button(new RectangleF(1 / 3f, 0.2f, 0.25f, 0.25f * aspectRatio), ButtonAlign.Centered, textures[0]);
            buttons[1] = new Button(new RectangleF(2 / 3f, 0.2f, 0.25f, 0.25f * aspectRatio), ButtonAlign.Centered, textures[0]);

            buttons[2] = new Button(new RectangleF(0.5f, 0.4f, 0.25f, 0.25f * aspectRatio), ButtonAlign.Centered, textures[0]);

            buttons[3] = new Button(new RectangleF(1 / 3f, 0.6f, 0.25f, 0.25f * aspectRatio), ButtonAlign.Centered, textures[0]);
            buttons[4] = new Button(new RectangleF(2 / 3f, 0.6f, 0.25f, 0.25f * aspectRatio), ButtonAlign.Centered, textures[0]);

            buttons[5] = new Button(new RectangleF(0.5f, 0.8f, 0.25f, 0.25f * aspectRatio), ButtonAlign.Centered, textures[0]);

            buttons[0].ButtonClicked += OnButton1Pressed;
            buttons[1].ButtonClicked += OnButton2Pressed;
            buttons[2].ButtonClicked += OnButton3Pressed;
            buttons[3].ButtonClicked += OnButton4Pressed;
            buttons[4].ButtonClicked += OnButton5Pressed;
            buttons[5].ButtonClicked += OnButton6Pressed;

        }

        public void OnButton1Pressed(object sender, EventArgs e)
        {
            Debug.WriteLine("Button 1 is being pressed");
        }
        public void OnButton2Pressed(object sender, EventArgs e)
        {
            Debug.WriteLine("Button 2 is being pressed");
        }
        public void OnButton3Pressed(object sender, EventArgs e)
        {
            Debug.WriteLine("Button 3 is being pressed");
        }
        public void OnButton4Pressed(object sender, EventArgs e)
        {
            Debug.WriteLine("Button 4 is being pressed");
        }
        public void OnButton5Pressed(object sender, EventArgs e)
        {
            Debug.WriteLine("Button 5 is being pressed");
        }
        public void OnButton6Pressed(object sender, EventArgs e)
        {
            Debug.WriteLine("Button 6 is being pressed");
        }

        public void Update()
        {
            //Debug.WriteLine(menuCanvas.LocalRectFromVector2(Mouse.GetState().Position.ToVector2()));
            if (InputK.IsMouseLeftPressedOnce())
            {
                for(int i = 0; i < buttons.Length;++i)
                {
                    buttons[i].IsClicked(menuCanvas.LocalRectFromVector2(Mouse.GetState().Position.ToVector2()), Mouse.GetState().LeftButton);
                }
            }
        }

        public void Draw(GraphicsDevice gd, SpriteBatch sp)
        {
            menuCanvas.BeginDraw(gd, sp, Color.White);
            for (int i = 0; i < buttons.Length;++i)
            {
                buttons[i].Draw(sp, menuCanvas.Draw);
            }
            menuCanvas.EndDraw(sp);

            gd.SetRenderTarget(null);
            sp.Begin();
            sp.Draw(menuCanvas.CanvasRT, menuCanvas.GetGlobalCanvasRect().Location, Color.White);
            sp.End();

        }


    }
}
