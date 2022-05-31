using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Maxwell_Sim
{
    class Particle
    {
        public Vector3[] position = new Vector3[32];
        public Vector3[] velocity = new Vector3[32];
        public Vector3[] oldVelocity = new Vector3[32];
        public float charge;

        public RenderTarget2D eField;
        public RenderTarget2D mField;

        public string Mode = "Fixed";
        public static int size = 16;

        public Particle(GraphicsDevice graphicsDevice, Vector3 pos, float charge)
        {
            eField = new RenderTarget2D(graphicsDevice, 500, 500, false, SurfaceFormat.Vector4, DepthFormat.Depth24);
            mField = new RenderTarget2D(graphicsDevice, 500, 500, false, SurfaceFormat.Vector4, DepthFormat.Depth24);
            for (int i = 0; i < 32; ++i)
            {
                position[i] = pos;
                velocity[i] = Vector3.Zero;
                oldVelocity[i] = Vector3.Zero;
            }
            this.charge = charge;
        }

        public void Simulate()
        {
            //Shift values after timeStep
            for (int i = 31; i > 0; --i)
            {
                position[i] = position[i - 1];
                velocity[i] = velocity[i - 1];
                oldVelocity[i] = oldVelocity[i - 1];
            }
            for (int i = 0; i < 32; ++i)
            {
                oldVelocity[i] = velocity[i];
            }
            


        }
        public void CalculateField(SpriteBatch spriteBatch, GraphicsDevice graphicsDevice, Effect fieldCalc, float timeStep, float c, Texture2D field)
        {
            if (charge != 0)
            {
                graphicsDevice.SetRenderTarget(eField);
                graphicsDevice.Clear(Color.Transparent);
                spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, null, fieldCalc, null);
                fieldCalc.CurrentTechnique = fieldCalc.Techniques["eField"];
                fieldCalc.Parameters["positions"].SetValue(position);
                fieldCalc.Parameters["velocities"].SetValue(velocity);
                fieldCalc.Parameters["oldvelocities"].SetValue(oldVelocity);
                fieldCalc.Parameters["charge"].SetValue(charge);
                fieldCalc.Parameters["constants"].SetValue(new Vector2(timeStep, c));
                fieldCalc.CurrentTechnique.Passes[0].Apply();
                spriteBatch.Draw(field, Vector2.Zero, Color.White);

                spriteBatch.End();

                graphicsDevice.SetRenderTarget(mField);
                graphicsDevice.Clear(Color.Transparent);
                spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, null, fieldCalc, null);
                fieldCalc.CurrentTechnique = fieldCalc.Techniques["mField"];
                fieldCalc.CurrentTechnique.Passes[0].Apply();
                spriteBatch.Draw(field, Vector2.Zero, Color.White);

                spriteBatch.End();
            }

        }

        public void DrawParticle(SpriteBatch spriteBatch, Texture2D particleTexture)
        {
            Point pos = new Point((int)(500*position[0].X), (int)(500*position[0].Y));
            int chargeType = charge > 0 ? 0 : (charge == 0 ? 1 : 2); 
            spriteBatch.Draw(particleTexture, new Rectangle(pos.X, pos.Y, size, size), 
                new Rectangle(size * chargeType, 0, size, size), Color.White, 0.0f, 
                new Vector2(size/2, size/2), SpriteEffects.None, 0.0f); 
        }

        public void GetForce(GraphicsDevice graphicsDevice, SimInterface sI, SpriteBatch spriteBatch, RenderTarget2D blank)
        {
            BlendState bs = new BlendState();
            BlendState oldbs = graphicsDevice.BlendState;

            bs.ColorBlendFunction = BlendFunction.Subtract;

            bs.AlphaSourceBlend = Blend.One;
            bs.ColorSourceBlend = Blend.One;

            bs.AlphaDestinationBlend = Blend.One;
            bs.ColorDestinationBlend = Blend.One;

            graphicsDevice.BlendState = bs;

            graphicsDevice.SetRenderTarget(blank);
            graphicsDevice.Clear(Color.Transparent);
            spriteBatch.Begin();
            spriteBatch.Draw(mField, Vector2.Zero, Color.White);
            spriteBatch.Draw(sI.totalMField, Vector2.Zero, Color.White);
            spriteBatch.End();





            graphicsDevice.BlendState = oldbs;


        }

    }
}
