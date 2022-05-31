using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Maxwell_Sim
{
    /// <summary>
    /// Describes where the button will be aligned to.
    /// </summary>
    public enum ButtonAlign
    {
        TopLeft,
        TopRight,
        BottomLeft,
        BottomRight,
        Centered
    }
    class Button
    {
        /// <summary>
        /// Button size and position, in relative coordinates [0,1] with respect to a canvas
        /// </summary>
        RectangleF buttonRect;
        ButtonAlign buttonAlign;
        Texture2D buttonTexture;
        public event EventHandler ButtonClicked;

        protected virtual void OnButtonClicked(EventArgs e)
        {
            EventHandler handler = ButtonClicked;
            handler?.Invoke(this, e);
        }

        public Button(RectangleF rect, ButtonAlign alignment, Texture2D texture)
        {
            buttonRect = rect;
            buttonAlign = alignment;
            buttonTexture = texture;
        }

        public void Draw(SpriteBatch sp,
              Action<SpriteBatch, Texture2D, RectangleF, RectangleF?, float, Vector2, SpriteEffects, float> DrawFunc)
        {
            Draw(sp, null, 0.0f,Vector2.One,SpriteEffects.None,1.0f, DrawFunc);
        }
        public void Draw(SpriteBatch sp, Rectangle? source, float angle, Vector2 scale, SpriteEffects effect, float depthLayer, 
            Action<SpriteBatch, Texture2D, RectangleF, RectangleF?, float, Vector2, SpriteEffects, float> DrawFunc)
        {
            Vector2 origin = buttonAlign.GetOrigin() * buttonTexture.Bounds.Size.ToVector2();
            DrawFunc(sp, buttonTexture, buttonRect, source, angle, origin, effect, depthLayer);
        }

        /// <summary>
        /// Checks if the button is being clicked.
        /// </summary>
        /// <param name="mousePos">Recieves a position in relative coordinates to the canvas.</param>
        /// <param name="leftButton">State of mouse left button.</param>
        public bool IsClicked(RectangleF mousePos, ButtonState leftButton)
        {
            if (leftButton == ButtonState.Pressed)
            {
                Vector2 origin = buttonAlign.GetOrigin() * buttonRect.Location;
                mousePos.Offset(origin * 0.5f);
                if (buttonRect.IntersectsWith(mousePos))
                {
                    OnButtonClicked(EventArgs.Empty);
                    return true;
                }
            }
            return false;
        }


    }
}
