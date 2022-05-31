using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Maxwell_Sim
{
    public class Game1 : Game
    {
        readonly GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        #region Before
        /*
        float timestep = 0.1f;

        Texture2D particleTexture;
        Texture2D magnetTexture;
        Texture2D[] menuButtons;
        RenderTarget2D blankRender;
        SpriteFont menuFont;

        List<Particle> particles = new List<Particle>(10);
        List<Magnet> magnets = new List<Magnet>(2);
        Effect force;
        SimInterface simulationInterface;
        SimControl controlSimulation;
        PhysicSim physicsSim;
        */
        #endregion

        private void TextInputHandler(object sender, TextInputEventArgs args)
        {
            //var pressedKey = args.Key;
            var character = args.Character;
            Menu.Text += character;
        }

        public Game1()
        {
            this.IsFixedTimeStep = false;
            this.IsMouseVisible = true;

            graphics = new GraphicsDeviceManager(this)
            {
                PreferredBackBufferWidth = 800,
                PreferredBackBufferHeight = 600,
                GraphicsProfile = GraphicsProfile.HiDef
            };
            graphics.ApplyChanges();

            Content.RootDirectory = "Content";

            Window.AllowUserResizing = true;
            Window.TextInput += TextInputHandler;
        }

        Button button1;
        Canvas simulationCanvas;
        Texture2D magnetTexture;
        protected override void Initialize()
        {
            #region Before
            /*
            menuButtons = new Texture2D[2];
            simulationInterface = new SimInterface();
            controlSimulation = new SimControl(particles, magnets);
            */
            #endregion

            simulationCanvas = new Canvas(Window, GraphicsDevice, new RectangleF(0.25f, 0.25f, 0.5f, 0.5f));

            base.Initialize();
        }

        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);


            magnetTexture = Content.Load<Texture2D>("Magnet");
            button1 = new Button(new RectangleF(0.5f, 0.5f, 0.25f, 0.25f), ButtonAlign.Centered, magnetTexture);
            button1.ButtonClicked += ClickButton1Handle;

            #region Before
            /*
            particleTexture = Content.Load<Texture2D>("Particles");
            magnetTexture = Content.Load<Texture2D>("Magnet");
            menuButtons[0] = Content.Load<Texture2D>("FieldButtons");
            menuButtons[1] = Content.Load<Texture2D>("SelectButtons");

            blankRender = new RenderTarget2D(GraphicsDevice, 500, 500, false, SurfaceFormat.Vector4, DepthFormat.Depth24);

            menuFont = Content.Load<SpriteFont>("MenuFont");

            simulationInterface.Load(Content, GraphicsDevice);
            force = Content.Load<Effect>("GetForceE");
            physicsSim = new PhysicSim(particles, magnets, force, GraphicsDevice, blankRender, simulationInterface);
            */
            #endregion

        }


        protected override void UnloadContent()
        {
        }

        #region Before
        /*
        float stepTimer = 0;
        float timer = 0;
        bool simulate = false;
        */
        #endregion

        protected override void Update(GameTime gameTime)
        {
            #region Before
            /*
            InputK.startKey();

            if (Keyboard.GetState().IsKeyDown(Keys.Escape)) Exit();

            if (simulate)
            {
                timer += gameTime.ElapsedGameTime.Milliseconds / 1000.0f;
                stepTimer += gameTime.ElapsedGameTime.Milliseconds / 1000.0f;
                if (stepTimer > timestep)
                {
                    stepTimer -= timestep;
                    Simulate();

                }
            }
            
            if (InputK.IsKeyDownOnce(Keys.S))
            {
                simulate = !simulate;
            }
            if (InputK.IsKeyDownOnce(Keys.E))
            {
                controlSimulation.Move = !controlSimulation.Move;
            }

            if (InputK.IsKeyDownOnce(Keys.L))
            {
                for (int i = 0; i < particles.Count; ++i)
                {
                    particles[i].Mode = "Move";
                }
            }

            Menu.Update();
            controlSimulation.Update(GraphicsDevice);

            InputK.endKey();
            */
            #endregion

            InputK.StartKey();
            if (InputK.IsMouseLeftPressedOnce())
            {
                button1.IsClicked(simulationCanvas.LocalRectFromVector2(Mouse.GetState().Position.ToVector2()), Mouse.GetState().LeftButton);
            }

           


            InputK.EndKey();

            base.Update(gameTime);
        }

        public void ClickButton1Handle(object sender, EventArgs e )
        {
            Debug.WriteLine("I have recived that the button 1 has been clicked");
        }

        /*
        private void Simulate()
        {
            simulationInterface.Simulate(particles, magnets);
            simulationInterface.Calculate(particles, magnets, spriteBatch, GraphicsDevice, timestep, 10000);
            simulationInterface.TotalField(spriteBatch, GraphicsDevice, particles, magnets);
            simulationInterface.DrawFieldLines(GraphicsDevice, controlSimulation.electricField, controlSimulation.fieldlLines);
            physicsSim.Update(timer, timestep, simulate, GraphicsDevice, spriteBatch, timestep);
        }
        */

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.SetRenderTarget(null);
            GraphicsDevice.Clear(Color.Transparent);

            #region Before
            /*
            spriteBatch.Begin();
            spriteBatch.Draw(simulationInterface.SimulationTarget, Vector2.Zero, Color.White);

            for (int i = 0; i < particles.Count; ++i)
            {
                particles[i].DrawParticle(spriteBatch, particleTexture);
            }
            for (int i = 0; i < magnets.Count; ++i)
            {
                magnets[i].Draw(spriteBatch, magnetTexture);
            }

            controlSimulation.DrawMenu(spriteBatch, menuButtons);

            Menu.DrawMenu(spriteBatch, menuFont);

            spriteBatch.End();
            */
            #endregion

            simulationCanvas.BeginDraw(GraphicsDevice, spriteBatch, Color.White);
            button1.Draw(spriteBatch,simulationCanvas.Draw);
            simulationCanvas.EndDraw(spriteBatch);


            GraphicsDevice.SetRenderTarget(null);
            spriteBatch.Begin();
            spriteBatch.Draw(simulationCanvas.CanvasRT, simulationCanvas.GetGlobalCanvasRect().Location , Color.White);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
