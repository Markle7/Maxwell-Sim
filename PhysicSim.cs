using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Primitives;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Maxwell_Sim
{
    class PhysicSim
    {
        List<Particle> particles;
        List<Magnet> magnets;
        Effect getForce;
        RenderTarget2D forceTex;
        Texture2D blank;
        SimInterface sim;

        public PhysicSim(List<Particle> particles, List<Magnet> magnets, Effect force, GraphicsDevice graphicsDevice, Texture2D b, SimInterface s)
        {
            this.particles = particles;
            this.magnets = magnets;
            getForce = force;
            forceTex = new RenderTarget2D(graphicsDevice, 1, 1, false, SurfaceFormat.Vector4, DepthFormat.Depth24);
            blank = b;
            sim = s;
        }

        public void InvertCurrent()
        {
            for (int i = 0; i < particles.Count; ++i)
            {
                particles[i].position[0] = new Vector3(0, 0, particles[i].position[0].X - 0.5f);
                particles[i].Mode = "CurrentZ";
            }
        }
        public void Update(float time, float timestep, bool context, GraphicsDevice device, SpriteBatch spriteBatch, float steptime)
        {
            if (context)
            {
                for (int i = 0; i < particles.Count; ++i)
                {
                    if (particles[i].Mode == "Drag")
                    {
                        particles[i].position[0] = new Vector3(Mouse.GetState().Position.ToVector2(), 0) * 0.002f;
                        particles[i].velocity[0] = Vector3.Zero;
                    }
                    if (particles[i].Mode == "Move")
                    {
                        GetForce(particles[i], device, spriteBatch, steptime);
                    }
                }
                for (int i = 0; i < magnets.Count; ++i)
                {
                    if (magnets[i].Mode == "Drag")
                    {
                        magnets[i].origin = new Vector3(Mouse.GetState().Position.ToVector2(), 0) * 0.002f;
                    }
                    if (magnets[i].Mode == "Move")
                    {
                        GetForce(magnets[i], device, spriteBatch, steptime);
                    }

                }
            }
        }

        private void GetForce(Particle p, GraphicsDevice device, SpriteBatch spriteBatch, float steptime)
        {
            device.SetRenderTarget(forceTex);
            device.Clear(Color.Transparent);
            spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, getForce, null);
            getForce.CurrentTechnique = getForce.Techniques["Particle"];
            getForce.Parameters["OwnFieldE"].SetValue(p.eField);
            getForce.Parameters["OwnFieldM"].SetValue(p.mField);
            getForce.Parameters["TotalFieldE"].SetValue(sim.totalEField);
            getForce.Parameters["TotalFieldM"].SetValue(sim.totalMField);
            Vector3[] data = new Vector3[128];
            data[0] = p.position[0];
            getForce.Parameters["position"].SetValue(data);
            getForce.Parameters["velocity"].SetValue(p.velocity[0]);
            getForce.Parameters["charge"].SetValue(p.charge);
            getForce.CurrentTechnique.Passes[0].Apply();
            spriteBatch.Draw(blank, Vector2.Zero, Color.White);
            spriteBatch.End();


            forceTex.GetData(dot);
            p.velocity[0] += 0.00000001f * new Vector3(dot[0].X, dot[0].Y, dot[0].Z) * steptime;
            p.position[0] += p.velocity[0] * steptime;



        }
        Vector4[] dot = { new Vector4(0, 0, 0, 0) };
        private void GetForce(Magnet m, GraphicsDevice device, SpriteBatch spriteBatch, float steptime)
        {
            device.SetRenderTarget(forceTex);
            device.Clear(Color.Transparent);
            spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, getForce, null);
            getForce.CurrentTechnique = getForce.Techniques["Magnet"];
            getForce.Parameters["OwnFieldM"].SetValue(m.mField);
            getForce.Parameters["TotalFieldM"].SetValue(sim.totalMField);

            getForce.Parameters["position"].SetValue(m.position);
            getForce.Parameters["velocity"].SetValue(m.angle);
            getForce.Parameters["charge"].SetValue(m.charge);
            getForce.CurrentTechnique.Passes[0].Apply();
            spriteBatch.Draw(blank, Vector2.Zero, Color.White);
            spriteBatch.End();

            forceTex.GetData(dot);
            m.angle += 0.5f * dot[0].Z * steptime * steptime;


        }


    }
}
