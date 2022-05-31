using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using Primitives;



namespace Maxwell_Sim
{
    class SimControl
    {
        List<Particle> particles;
        List<Magnet> magnets;
        const int maxParticles = 20;
        const int maxMagnets = 5;

        public bool fieldlLines = false;
        public bool electricField = true;
        public bool Move = false;

        public SimControl(List<Particle> p, List<Magnet> m)
        {
            particles = p;
            magnets = m;
        }

        public Particle AddParticle(Particle p)
        {
            if (particles.Count < maxParticles)
            {
                particles.Add(p);
                return p;
            }
            return null;
        }
        public Magnet AddMagnet(Magnet m)
        {
            if (magnets.Count < maxMagnets)
            {
                magnets.Add(m);
                return m;
            }
            return null;
        }
        public void RemoveParticle(Particle p)
        {
            int i = particles.IndexOf(p);
            particles.RemoveAt(i);
        }
        public void RemoveMagnet(Magnet m)
        {
            int i = magnets.IndexOf(m);
            magnets.RemoveAt(i);
        }

        public void Update(GraphicsDevice graphicsDevice)
        {
            Point mPos = Mouse.GetState().Position;
            Rectangle button;
            //In menu
            if (mPos.X > 500 && !Menu.openedMenu)
            {
                if (InputK.IsMouseLeftPressedOnce())
                {
                    //Check intersection with add particle
                    button = new Rectangle(512, 12, 57, 57);
                    if (Rectangle.Intersect(button, new Rectangle(mPos.X, mPos.Y, 1, 1)) != Rectangle.Empty)
                    {
                        Menu.CreateParticleMenu(AddParticle(new Particle(graphicsDevice, new Vector3(0.5f, 0.5f, 0), 0)));
                    }
                    //Check intersection with add magnet
                    button = new Rectangle(581, 12, 57, 57);
                    if (Rectangle.Intersect(button, new Rectangle(mPos.X, mPos.Y, 1, 1)) != Rectangle.Empty)
                    {
                        Menu.CreateMagnetMenu(AddMagnet(new Magnet(graphicsDevice, new Vector3(0.5f, 0.5f, 0), 0, 0)));
                    }
                    //Check intersection with add current
                    button = new Rectangle(512, 81, 57, 57);
                    if (Rectangle.Intersect(button, new Rectangle(mPos.X, mPos.Y, 1, 1)) != Rectangle.Empty)
                    {

                    }

                    //Check intersection with field line button
                    button = new Rectangle(581, 81, 57, 57);
                    if (Rectangle.Intersect(button, new Rectangle(mPos.X, mPos.Y, 1, 1)) != Rectangle.Empty)
                    {
                        fieldlLines = !fieldlLines;
                    }
                    //Check intersection with electric button
                    button = new Rectangle(512, 150, 57, 57);
                    if (Rectangle.Intersect(button, new Rectangle(mPos.X, mPos.Y, 1, 1)) != Rectangle.Empty)
                    {
                        electricField = true;
                    }
                    //Check intersection with magnetic button
                    button = new Rectangle(581, 150, 57, 57);
                    if (Rectangle.Intersect(button, new Rectangle(mPos.X, mPos.Y, 1, 1)) != Rectangle.Empty)
                    {
                        electricField = false;
                    }
                }
            }
            //In simulation
            else if (mPos.X < 500 && !Menu.openedMenu)
            {
                if (InputK.IsMouseLeftPressedOnce())
                {
                    var a = GetMouseObject(); if (a != null)
                    {
                        if (a.GetType() == typeof(Particle))
                        {
                            var p = (Particle)a;
                            if (p.Mode == "Drag")
                            {
                                p.Mode = "Fixed";
                            }
                            else if (p.Mode == "Fixed")
                            {
                                if (Move)
                                {
                                    p.Mode = "Move";
                                    p.velocity[0] = new Vector3(0, 0, 0);
                                }
                                else
                                    p.Mode = "Drag";
                            }
                        }
                        else
                        {
                            var m = (Magnet)a;
                            if (m.Mode == "Drag")
                                m.Mode = "Fixed";
                            else if (m.Mode == "Fixed")
                            {
                                if (Move)
                                    m.Mode = "Move";
                                else
                                    m.Mode = "Drag";
                            }
                        }
                    }
                }
                if (InputK.IsMouseRightJustReleased())
                {
                    var a = GetMouseObject();
                    if (a != null)
                    {
                        if (a.GetType() == typeof(Particle))
                        {
                            var p = (Particle)a;
                            RemoveParticle(p);
                        }
                        else
                        {
                            var m = (Magnet)a;
                            RemoveMagnet(m);
                        }
                    }
                }
            }
        }

        private Object GetMouseObject()
        {
            Vector3 mPos = new Vector3(Mouse.GetState().Position.ToVector2(), 0);
            Matrix mTransform;
            //Check mouse collision with particles
            for (int i = 0; i < particles.Count;++i)
            {
                if ((mPos - 500*particles[i].position[0]).LengthSquared() < Particle.size * Particle.size)
                {
                    return particles[i];
                }

            }
            for (int i = 0; i < magnets.Count;++i)
            {
                mTransform = Matrix.CreateTranslation(-500*magnets[i].origin)*
                    Matrix.CreateRotationZ(-magnets[i].angle)*Matrix.CreateTranslation(500*magnets[i].origin);
                var a = (Vector3.Transform(mPos, mTransform) - 500 * magnets[i].origin);
                if (a.X > -40 && a.X < 40 && a.Y > -20 && a.Y < 20)
                {
                    return magnets[i];
                }
            }
            return null;
        }
        
        public void DrawMenu(SpriteBatch spriteBatch, Texture2D[] buttons)
        {
            Primitives2D.FillRectangle(spriteBatch, new Rectangle(500, 0, 150, 500), Color.White);

            Primitives2D.DrawRectangle(spriteBatch, new Rectangle(512 - 0, 12 - 1, 57, 57), Color.Goldenrod, 2);

            Primitives2D.DrawRectangle(spriteBatch, new Rectangle(581 - 0, 12 - 1, 57, 57), Color.Goldenrod, 2);

            Primitives2D.DrawRectangle(spriteBatch, new Rectangle(512 - 0, 81 - 1, 57, 57), Color.Goldenrod, 2);

            spriteBatch.Draw(buttons[0], new Vector2(581, 81), new Rectangle(fieldlLines ? 57: 0, 0, 57, 57), Color.White);
            Primitives2D.DrawRectangle(spriteBatch, new Rectangle(581 - 0, 81 - 1, 57, 57), Color.Goldenrod, 2);

            spriteBatch.Draw(buttons[1], new Vector2(512, 150), new Rectangle(electricField ? 57 : 0, 0, 57, 57), Color.White);
            Primitives2D.DrawRectangle(spriteBatch, new Rectangle(512 - 0, 150 - 1, 57, 57), Color.Goldenrod, 2);

            spriteBatch.Draw(buttons[1], new Vector2(581, 150), new Rectangle(electricField ? 0 : 57, 57, 57, 57), Color.White);
            Primitives2D.DrawRectangle(spriteBatch, new Rectangle(581 - 0, 150 - 1, 57, 57), Color.Goldenrod, 2);
        }

    }
}
