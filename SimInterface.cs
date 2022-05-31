using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;


namespace Maxwell_Sim
{
    class SimInterface
    {
        bool inContext = true;
        public bool InContext { get => inContext; set => inContext = value; }
        public RenderTarget2D SimulationTarget { get => simulationTarget; }

        Effect particleFieldCalc;
        Effect magnetFieldCalc;
        Effect lineCalc;

        public RenderTarget2D totalEField;
        public RenderTarget2D totalMField;

        Texture2D tempField;

        RenderTarget2D simulationTarget;

        FieldLines fieldLines = new FieldLines();


        public void Load(ContentManager Content, GraphicsDevice graphicsDevice)
        {
            particleFieldCalc = Content.Load<Effect>("ParticleFieldCalculator");
            magnetFieldCalc = Content.Load<Effect>("LoopFieldCalculator");
            lineCalc = Content.Load<Effect>("FieldLine");

            tempField = new Texture2D(graphicsDevice, 500, 500);
            totalEField = new RenderTarget2D(graphicsDevice, 500, 500, false, SurfaceFormat.Vector4, DepthFormat.Depth24);
            totalMField = new RenderTarget2D(graphicsDevice, 500, 500, false, SurfaceFormat.Vector4, DepthFormat.Depth24);
            simulationTarget = new RenderTarget2D(graphicsDevice, 500, 500, false, SurfaceFormat.Vector4, DepthFormat.Depth24);

            fieldLines.Effect = lineCalc;
            fieldLines.Init(graphicsDevice);
        }



        Vector4[] array = new Vector4[500 * 500];
        public void Simulate(List<Particle> particles, List<Magnet> magnets)
        {
            for (int i = 0;i < particles.Count;++i)
            {
                particles[i].Simulate();
            }
            for (int i = 0; i < magnets.Count; ++i)
            {
                magnets[i].Simulate();
            }
        }
        public void Calculate(List<Particle> particles, List<Magnet> magnets, SpriteBatch spriteBatch, GraphicsDevice graphicsDevice, float timestep, float c)
        {
            for (int i = 0; i < particles.Count;++i)
            {
                particles[i].CalculateField(spriteBatch, graphicsDevice, particleFieldCalc, timestep, c, tempField);
            }
            for (int i = 0; i < magnets.Count; ++i)
            {
                magnets[i].CalculateField(spriteBatch, graphicsDevice, magnetFieldCalc, timestep, c, tempField);
            }
        }
        public void TotalField (SpriteBatch spriteBatch, GraphicsDevice graphicsDevice, List<Particle> particles, List<Magnet> magnets)
        {
            graphicsDevice.SetRenderTarget(totalEField);
            graphicsDevice.Clear(Color.Transparent);
            spriteBatch.Begin();
            for (int i = 0; i < particles.Count;++i)
            {
                spriteBatch.Draw(particles[i].eField, Vector2.Zero, Color.White);
            }
            spriteBatch.End();

            graphicsDevice.SetRenderTarget(totalMField);
            graphicsDevice.Clear(Color.Transparent);
            spriteBatch.Begin();
            for (int i = 0; i < particles.Count; ++i)
            {
                spriteBatch.Draw(particles[i].mField, Vector2.Zero, Color.White);
            }
            for (int i = 0; i < magnets.Count; ++i)
            {
                spriteBatch.Draw(magnets[i].mField, Vector2.Zero, Color.White);
            }
            spriteBatch.End();
        }
        

        public void DrawFieldLines(GraphicsDevice graphicsDevice, bool eField, bool draw)
        {
            graphicsDevice.SetRenderTarget(simulationTarget);
            graphicsDevice.Clear(Color.Black);
            if (draw)
                fieldLines.Draw(graphicsDevice, eField ? totalEField : totalMField);

        }

    }
}
