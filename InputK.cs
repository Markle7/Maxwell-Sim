using Microsoft.Xna.Framework.Input;


namespace Maxwell_Sim
{
    static class InputK
    {
        public static KeyboardState oldState = Keyboard.GetState();
        public static KeyboardState newState;
        public static MouseState mOldState = Mouse.GetState();
        public static MouseState mNewState;

        static public void StartKey()
        {
            newState = Keyboard.GetState();
            mNewState = Mouse.GetState();
        }
        static public void EndKey()
        {
            oldState = newState;
            mOldState = mNewState;
        }

        static public bool IsKeyDown(Keys key)
        {
            return newState.IsKeyDown(key);
        }

        static public bool IsKeyUp(Keys key)
        {
            return newState.IsKeyUp(key);
        }

        static public bool IsKeyDownOnce(Keys key)
        {
            return newState.IsKeyDown(key) && !oldState.IsKeyDown(key);
        }

        static public bool IsKeyJustReleased(Keys key)
        {
            return !newState.IsKeyDown(key) && oldState.IsKeyDown(key);
        }

        static public bool IsKeyContinuos(Keys key)
        {
            return newState.IsKeyDown(key) && oldState.IsKeyDown(key);
        }


        static public bool IsMouseLeftPressed()
        {
            return mNewState.LeftButton == ButtonState.Pressed;
        }
        static public bool IsMouseRightPressed()
        {
            return mNewState.RightButton == ButtonState.Pressed;
        }
        static public bool IsMouseLeftReleased()
        {
            return mNewState.LeftButton == ButtonState.Released;
        }
        static public bool IsMouseRightReleased()
        {
            return mNewState.RightButton == ButtonState.Released;
        }
        static public bool IsMouseLeftPressedOnce()
        {
            return (mNewState.LeftButton == ButtonState.Pressed) && (mOldState.LeftButton == ButtonState.Released);
        }
        static public bool IsMouseRightPressedOnce()
        {
            return (mNewState.RightButton == ButtonState.Pressed) && (mOldState.RightButton == ButtonState.Released);
        }
        static public bool IsMouseLeftJustReleased()
        {
            return (mNewState.LeftButton == ButtonState.Released) && (mOldState.LeftButton == ButtonState.Pressed);
        }
        static public bool IsMouseRightJustReleased()
        {
            return (mNewState.RightButton == ButtonState.Released) && (mOldState.RightButton == ButtonState.Pressed);
        }


    }
}
