using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace Maxwell_Sim
{
    class Magnet
    {
        public Vector3[] position = new Vector3[128];
        public float velocity;
        public float charge;
        public float angle;
        public RenderTarget2D eField;
        public RenderTarget2D mField;

        public Vector3 origin;
        public float area = 0.01f;
        public static Point size = new Point(16, 8);

        public string Mode = "Fixed";


        public Magnet(GraphicsDevice graphicsDevice, Vector3 origin, float charge, float a)
        {
            this.origin = origin;
            eField = new RenderTarget2D(graphicsDevice, 500, 500, false, SurfaceFormat.Vector4, DepthFormat.Depth24);
            mField = new RenderTarget2D(graphicsDevice, 500, 500, false, SurfaceFormat.Vector4, DepthFormat.Depth24);

            for (int i = 0; i < 128; ++i)
            {
                position[i] = new Vector3((i % 16) * area - 16 * area * 0.5f, (i / 16) * area - 8 * 0.5f * area, 0);
            }
            velocity = 1;
            this.charge = charge;
            angle = a;
        }

        public Matrix rotate, translate;
        public void Simulate()
        {
            translate = Matrix.CreateTranslation(origin);
            rotate = Matrix.CreateTranslation(-origin) * Matrix.CreateRotationZ(angle) * Matrix.CreateTranslation(origin);
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

        public void CalculateField(SpriteBatch spriteBatch, GraphicsDevice graphicsDevice, Effect fieldCalc, float timeStep, float c, Texture2D field)
        {
            graphicsDevice.SetRenderTarget(mField);
            graphicsDevice.Clear(Color.Transparent);
            spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, null, fieldCalc, null);
            fieldCalc.CurrentTechnique = fieldCalc.Techniques["mField"];
            fieldCalc.Parameters["positions"].SetValue(position);
            fieldCalc.Parameters["velocity"].SetValue(velocity);
            fieldCalc.Parameters["charge"].SetValue(charge);
            fieldCalc.Parameters["angle"].SetValue(angle);
            fieldCalc.Parameters["rotation"].SetValue(rotate);
            fieldCalc.Parameters["translation"].SetValue(translate);
            fieldCalc.Parameters["constants"].SetValue(new Vector4(timeStep, c, area, 0));
            fieldCalc.CurrentTechnique.Passes[0].Apply();
            spriteBatch.Draw(field, Vector2.Zero, Color.White);

            spriteBatch.End();
            
        }

        public void Draw(SpriteBatch spriteBatch, Texture2D magnetTexture)
        {
            spriteBatch.Draw(magnetTexture, new Rectangle((int)(500 * origin.X), (int)(500 * origin.Y), (int)(16 * 500 * area), (int)(8 * 500 * area)),
                new Rectangle(0, 0, 16, 8), Color.White, angle, new Vector2(8, 4), SpriteEffects.None, 0.0f);
        }
    }
}
