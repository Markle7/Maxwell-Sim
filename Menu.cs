using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Primitives;
using System;

namespace Maxwell_Sim
{
    static class Menu
    {
        static string menuType = "";
        static string keyboardText = "";
        public static bool openedMenu = false;
        static Point position;

        static public string Text { get => keyboardText; set => keyboardText = value; }


        static string[] textFields = { "", "" };


        static int clicked = -1;
        static Object activeObject = null;

        static public void CreateParticleMenu(Particle particle)
        {
            menuType = "P";
            openedMenu = true;
            activeObject = particle;
            position = new Point(250, 250);
        }
        static public void CreateMagnetMenu(Magnet magnet)
        {
            menuType = "M";
            openedMenu = true;
            activeObject = magnet;
            position = new Point(250, 250);
        }
        static public void Update()
        {
            //Depurate keyboard text
            for (int i = 0; i < keyboardText.Length; ++i)
            {
                if (keyboardText[i] == '\b')
                {
                    keyboardText = keyboardText.Remove(i, 1);
                    if (i != 0)
                    {
                        keyboardText = keyboardText.Remove(i - 1, 1);
                        i -= 2;
                    }
                    else i -= 1;
                }
            }

            if (openedMenu)
            {
                if (clicked != -1)
                {
                    textFields[clicked] = keyboardText;
                }
                if (menuType == "P")
                {
                    if (InputK.IsMouseLeftPressedOnce())
                    {
                        //Check for collsion in any text field of menu
                        if (MouseIntersecting(new Rectangle(180, 150, 150, 15)))
                        {
                            if (clicked != 0) { keyboardText = ""; }
                            clicked = 0;
                            keyboardText = textFields[clicked];
                        }
                        else
                        {
                            clicked = -1;
                        }
                    }

                }
                else if (menuType == "M")
                {
                    if (InputK.IsMouseLeftPressedOnce())
                    {
                        //Check for collsion in any text field of menu
                        if (MouseIntersecting(new Rectangle(180, 150, 150, 15)))
                        {
                            if (clicked != 0) { keyboardText = ""; }
                            clicked = 0;
                            keyboardText = textFields[clicked];
                        }
                        else if (MouseIntersecting(new Rectangle(180, 200, 150, 15)))
                        {
                            if (clicked != 0) { keyboardText = ""; }
                            clicked = 1;
                            keyboardText = textFields[clicked];
                        }
                        else
                        {
                            clicked = -1;
                        }
                    }
                }
                if (InputK.IsKeyDownOnce(Keys.Enter))
                {
                    if (activeObject != null)
                    {
                        //First validate inputs
                        float value1 = 0;
                        float value2 = 0;
                        if (float.TryParse(textFields[0], out value1))
                        {
                            openedMenu = false;
                            textFields[0] = "";
                            keyboardText = "";

                            if (activeObject.GetType() == typeof(Particle))
                            {
                                Particle p = (Particle)activeObject;
                                p.charge = value1;
                            }
                            if (activeObject.GetType() == typeof(Magnet) && float.TryParse(textFields[1], out value2))
                            {
                                textFields[1] = "";
                                Magnet m = (Magnet)activeObject;
                                m.charge = value1;
                                m.angle = value2;
                            }
                            activeObject = null;
                        }
                    }
                    
                    

                }
            }
        }

        static public void DrawMenu(SpriteBatch spriteBatch, SpriteFont font)
        {
            if (openedMenu)
            {
                if (menuType == "P")
                {
                    Primitives2D.FillRectangle(spriteBatch, CreateCentered(200, 300, 250, 250, 0, 0), Color.Gainsboro);
                    Primitives2D.FillRectangle(spriteBatch, CreateCentered(200, 25, 250, 250, 0, -150 + 12), Color.LightSlateGray);

                    font.DefaultCharacter = ' ';
                    Vector2 textM = font.MeasureString("Particula");
                    spriteBatch.DrawString(font, "Particula", new Vector2(250 - textM.X * 0.5f, 100 + textM.Y * 0.5f), Color.White);

                    spriteBatch.DrawString(font, "Carga", new Vector2(150, 125), Color.Black);
                    spriteBatch.DrawString(font, "q:", new Vector2(160, 150), Color.Black);

                    //Text Fields
                    Primitives2D.FillRectangle(spriteBatch, new Rectangle(180, 150, 150, 15), Color.White);
                    spriteBatch.DrawString(font, textFields[0], new Vector2(190, 150), Color.Black);

                }
                if (menuType == "M")
                {
                    Primitives2D.FillRectangle(spriteBatch, CreateCentered(200, 300, 250, 250, 0, 0), Color.Gainsboro);
                    Primitives2D.FillRectangle(spriteBatch, CreateCentered(200, 25, 250, 250, 0, -150 + 12), Color.LightSlateGray);

                    font.DefaultCharacter = ' ';
                    Vector2 textM = font.MeasureString("Iman");
                    spriteBatch.DrawString(font, "Iman", new Vector2(250 - textM.X * 0.5f, 100 + textM.Y * 0.5f), Color.White);

                    spriteBatch.DrawString(font, "Carga equivalente", new Vector2(150, 125), Color.Black);
                    spriteBatch.DrawString(font, "q:", new Vector2(160, 150), Color.Black);

                    spriteBatch.DrawString(font, "Angulo", new Vector2(150, 175), Color.Black);
                    spriteBatch.DrawString(font, "a:", new Vector2(160, 200), Color.Black);

                    Primitives2D.FillRectangle(spriteBatch, new Rectangle(180, 150, 150, 15), Color.White);
                    spriteBatch.DrawString(font, textFields[0], new Vector2(190, 150), Color.Black);

                    Primitives2D.FillRectangle(spriteBatch, new Rectangle(180, 200, 150, 15), Color.White);
                    spriteBatch.DrawString(font, textFields[1], new Vector2(190, 200), Color.Black);

                }
            }
        }

        static private Rectangle CreateCentered(int width, int height, int x, int y, int dx, int dy)
        {
            return new Rectangle(x + dx - width / 2, y + dy - height / 2, width, height);
        }

        static private bool MouseIntersecting(Rectangle rect)
        {
            return Rectangle.Intersect(rect, new Rectangle(Mouse.GetState().Position, new Point(1, 1))) != Rectangle.Empty;
        }




    }
}
