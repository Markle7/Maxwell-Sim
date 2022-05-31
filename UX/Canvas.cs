using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Maxwell_Sim
{
    class Canvas
    {
        RectangleF canvasRectangle;
        RenderTarget2D canvasRT;
        GameWindow window;
        GraphicsDevice graphics;
        float windowAspectRatio;
        float canvasAspectRatio;
        public RenderTarget2D CanvasRT { get => canvasRT; }
        public RectangleF CanvasRectangle { get => canvasRectangle;}
        public float AspectRatio { get => windowAspectRatio * canvasAspectRatio; }

        /// <summary>
        /// Constructor for a canvas
        /// </summary>
        /// <param name="window">Instance of the GameWindow object to render.</param>
        /// <param name="canvasBounds">Bounds of the canvas with respect to the total window area. Coordinates go from [0,1]</param>
        public Canvas(GameWindow window, GraphicsDevice gd, RectangleF canvasBounds)
        {
            this.window = window;
            graphics = gd;
            canvasRectangle = canvasBounds;
            canvasAspectRatio = canvasRectangle.Width / canvasRectangle.Height;
            windowAspectRatio = window.ClientBounds.Width / (float)window.ClientBounds.Height;
            window.ClientSizeChanged += OnResize;
            CreateRenderTarget();
        }

        private void OnResize(object sender, EventArgs e)
        {
            CreateRenderTarget();

            //Debug.WriteLine("Hey, I'm a canvas and I have detected that the client size has changed!");
        }

        void CreateRenderTarget()
        {
            //Calculate the canvas world position and size based on the canvasBounds (Normalized coordinates, [0,1]→[0,Width/Height])
            Rectangle bounds = (Rectangle)canvasRectangle.MapRectangle(window.ClientBounds.Size);
            if (bounds != Rectangle.Empty)
            {
                windowAspectRatio = window.ClientBounds.Width / (float)window.ClientBounds.Height;
                canvasRT?.Dispose();
                canvasRT = new RenderTarget2D(graphics, bounds.Width, bounds.Height);
            }
        }

        /// <summary>
        /// Creates a RectangleF in local coordinates [0,1] from a global vector 2
        /// </summary>
        /// <param name="global">Vector2 in global coordinates [0,Width/Height]</param>
        /// <returns></returns>
        public RectangleF LocalRectFromVector2(Vector2 global)
        {
            var canvasWorldRect = CanvasRectangle.MapRectangle(window.ClientBounds.Size);
            var globalPosRel = global - canvasWorldRect.Location;
            var inverse = canvasWorldRect.Size.Inverse();
            return new RectangleF(globalPosRel, Vector2.One).MapRectangle(inverse);
        }

        public RectangleF GlobalRectFromVector2(Vector2 local)
        {
            return new RectangleF(local * window.ClientBounds.Size.ToVector2(), Vector2.One);
        }

        public RectangleF GetGlobalCanvasRect()
        {
            return canvasRectangle.MapRectangle(window.ClientBounds.Size);
        }

        public void BeginDraw(GraphicsDevice gd, SpriteBatch sp, Color clear)
        {
            gd.SetRenderTarget(canvasRT);
            gd.Clear(clear);
            sp.Begin();
        }

        public void Draw(SpriteBatch sp, Texture2D texture, RectangleF destination, RectangleF? source, float angle, Vector2 origin, SpriteEffects effect, float depthLayer)
        {

            destination = destination.MapRectangle(canvasRectangle.MapRectangle(window.ClientBounds.Size).Size);
            sp.Draw(texture, (Rectangle)destination, (Rectangle?)source, Color.White, angle, origin, effect, depthLayer);
        }

        public void EndDraw(SpriteBatch sp)
        {
            sp.End();
        }

    }
}
