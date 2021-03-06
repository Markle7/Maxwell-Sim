//------------------------------------------------------------------------------ 
// 
//     Copyright (c) Microsoft Corporation.  All rights reserved.
// 
//----------------------------------------------------------------------------- 

namespace Maxwell_Sim
{
    using System;
    using Microsoft.Xna.Framework;

    public struct RectangleF
    {
        public static readonly RectangleF Empty = new RectangleF();

        private float x;
        private float y;
        private float width;
        private float height;

        public RectangleF(float x, float y, float width, float height)
        {
            this.x = x;
            this.y = y;
            this.width = width;
            this.height = height;
        }

        public RectangleF(Vector2 location, Vector2 size)
        {
            this.x = location.X;
            this.y = location.Y;
            this.width = size.X;
            this.height = size.Y;
        }

        public static RectangleF FromLTRB(float left, float top, float right, float bottom)
        {
            return new RectangleF(left,
                                 top,
                                 right - left,
                                 bottom - top);
        }

        public Vector2 Location
        {
            get
            {
                return new Vector2(X, Y);
            }
            set
            {
                X = value.X;
                Y = value.Y;
            }
        }
        public Vector2 Size
        {
            get
            {
                return new Vector2(Width, Height);
            }
            set
            {
                this.Width = value.X;
                this.Height = value.Y;
            }
        }

        public float X
        {
            get
            {
                return x;
            }
            set
            {
                x = value;
            }
        }
        public float Y
        {
            get
            {
                return y;
            }
            set
            {
                y = value;
            }
        }
        public float Width
        {
            get
            {
                return width;
            }
            set
            {
                width = value;
            }
        }
        public float Height
        {
            get
            {
                return height;
            }
            set
            {
                height = value;
            }
        }
        public float Left
        {
            get
            {
                return X;
            }
        }
        public float Top
        {
            get
            {
                return Y;
            }
        }
        public float Right
        {
            get
            {
                return X + Width;
            }
        }
        public float Bottom
        {
            get
            {
                return Y + Height;
            }
        }
        public bool IsEmpty
        {
            get
            {
                return (Width <= 0) || (Height <= 0);
            }
        }

        public override bool Equals(object obj)
        {
            if (!(obj is RectangleF))
                return false;

            RectangleF comp = (RectangleF)obj;

            return (comp.X == this.X) &&
                   (comp.Y == this.Y) &&
                   (comp.Width == this.Width) &&
                   (comp.Height == this.Height);
        }
        public static bool operator ==(RectangleF left, RectangleF right)
        {
            return (left.X == right.X
                     && left.Y == right.Y
                     && left.Width == right.Width
                     && left.Height == right.Height);
        }
        public static bool operator !=(RectangleF left, RectangleF right)
        {
            return !(left == right);
        }

        public bool Contains(float x, float y)
        {
            return this.X <= x &&
            x < this.X + this.Width &&
            this.Y <= y &&
            y < this.Y + this.Height;
        }
        public bool Contains(Vector2 pt)
        {
            return Contains(pt.X, pt.Y);
        }
        public bool Contains(RectangleF rect)
        {
            return (this.X <= rect.X) &&
                   ((rect.X + rect.Width) <= (this.X + this.Width)) &&
                   (this.Y <= rect.Y) &&
                   ((rect.Y + rect.Height) <= (this.Y + this.Height));
        }


        public override int GetHashCode()
        {
            return (int)((UInt32)X ^
            (((UInt32)Y << 13) | ((UInt32)Y >> 19)) ^
            (((UInt32)Width << 26) | ((UInt32)Width >> 6)) ^
            (((UInt32)Height << 7) | ((UInt32)Height >> 25)));
        }


        public void Inflate(float x, float y)
        {
            this.X -= x;
            this.Y -= y;
            this.Width += 2 * x;
            this.Height += 2 * y;
        }
        public void Inflate(Vector2 size)
        {
            Inflate(size.X, size.Y);
        }
        public static RectangleF Inflate(RectangleF rect, float x, float y)
        {
            RectangleF r = rect;
            r.Inflate(x, y);
            return r;
        }


        public void Intersect(RectangleF rect)
        {
            RectangleF result = RectangleF.Intersect(rect, this);

            this.X = result.X;
            this.Y = result.Y;
            this.Width = result.Width;
            this.Height = result.Height;
        }
        public static RectangleF Intersect(RectangleF a, RectangleF b)
        {
            float x1 = Math.Max(a.X, b.X);
            float x2 = Math.Min(a.X + a.Width, b.X + b.Width);
            float y1 = Math.Max(a.Y, b.Y);
            float y2 = Math.Min(a.Y + a.Height, b.Y + b.Height);

            if (x2 >= x1
                && y2 >= y1)
            {

                return new RectangleF(x1, y1, x2 - x1, y2 - y1);
            }
            return RectangleF.Empty;
        }
        public bool IntersectsWith(RectangleF rect)
        {
            return (rect.X < this.X + this.Width) &&
                   (this.X < (rect.X + rect.Width)) &&
                   (rect.Y < this.Y + this.Height) &&
                   (this.Y < rect.Y + rect.Height);
        }

  
        public static RectangleF Union(RectangleF a, RectangleF b)
        {
            float x1 = Math.Min(a.X, b.X);
            float x2 = Math.Max(a.X + a.Width, b.X + b.Width);
            float y1 = Math.Min(a.Y, b.Y);
            float y2 = Math.Max(a.Y + a.Height, b.Y + b.Height);

            return new RectangleF(x1, y1, x2 - x1, y2 - y1);
        }

        public void Offset(Vector2 pos)
        {
            Offset(pos.X, pos.Y);
        }
        public void Offset(float x, float y)
        {
            this.X += x;
            this.Y += y;
        }

        public static implicit operator RectangleF(Rectangle r)
        {
            return new RectangleF(r.X, r.Y, r.Width, r.Height);
        }

        public static explicit operator Rectangle(RectangleF r)
        {
            return new Rectangle((int)r.X, (int)r.Y, (int)r.Width, (int)r.Height);
        }
        public override string ToString()
        {
            return "{X=" + X.ToString() + ",Y=" + Y.ToString() +
            ",Width=" + Width.ToString() +
            ",Height=" + Height.ToString() + "}";
        }
    }
}

// File provided for Reference Use Only by Microsoft Corporation (c) 2007.
// Copyright (c) Microsoft Corporation. All rights reserved.
