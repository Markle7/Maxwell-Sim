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

        Texture2D[] textures;

        public ObjectsMenu(GameWindow window, GraphicsDevice gd, RectangleF bounds)
        {
            menuCanvas = new Canvas(window, gd, bounds);
            buttons = new Button[6];
            textures = new Texture2D[6];


        }

        private const float Width = 0.4f;
        public void LoadContent(ContentManager content)
        {
            textures[0] = content.Load<Texture2D>("Particles");
            textures[1] = content.Load<Texture2D>("Magnet");
            textures[2] = content.Load<Texture2D>("FieldButtons");
            textures[3] = content.Load<Texture2D>("SelectButtons");

            float aspectRatio = menuCanvas.AspectRatio;
            buttons[0] = new Button(new RectangleF(1 / 4f, 0.2f, Width, Width * aspectRatio), ButtonAlign.Centered, textures[0]);
            buttons[1] = new Button(new RectangleF(3 / 4f, 0.2f, Width, Width * aspectRatio), ButtonAlign.Centered, textures[1]);

            buttons[2] = new Button(new RectangleF(0.5f, 0.4f, Width, Width * aspectRatio), ButtonAlign.Centered, textures[2]);

            buttons[3] = new Button(new RectangleF(1 / 4f, 0.6f, Width, Width * aspectRatio), ButtonAlign.Centered, textures[3]);
            buttons[4] = new Button(new RectangleF(3 / 4f, 0.6f, Width, Width * aspectRatio), ButtonAlign.Centered, textures[3]);

            buttons[5] = new Button(new RectangleF(0.5f, 0.8f, Width, Width * aspectRatio), ButtonAlign.Centered, textures[0]);

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
            buttons[0].Draw(sp, new Rectangle(0, 0, 16, 16), 0.0f, Vector2.One,SpriteEffects.None, 1.0f, menuCanvas.Draw);
            buttons[1].Draw(sp, menuCanvas.Draw);
            buttons[2].Draw(sp, new Rectangle(0, 0, 57, 57), 0.0f, Vector2.One, SpriteEffects.None, 1.0f, menuCanvas.Draw);
            buttons[3].Draw(sp, new Rectangle(57, 0, 57, 57), 0.0f, Vector2.One, SpriteEffects.None, 1.0f, menuCanvas.Draw);
            buttons[4].Draw(sp, new Rectangle(57, 57, 57, 57), 0.0f, Vector2.One, SpriteEffects.None, 1.0f, menuCanvas.Draw);
            buttons[5].Draw(sp, new Rectangle(16, 0, 16, 16), 0.0f, Vector2.One, SpriteEffects.None, 1.0f, menuCanvas.Draw);
            menuCanvas.EndDraw(sp);

            gd.SetRenderTarget(null);
            sp.Begin();
            sp.Draw(menuCanvas.CanvasRT, menuCanvas.GetGlobalCanvasRect().Location, Color.White);
            sp.End();

        }


    }
}
