using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Maxwell_Sim
{
    class FieldLines
    {
        private VertexDeclaration instanceVertexDeclaration;

        private VertexBuffer instanceBuffer;
        private VertexBuffer geometryBuffer;
        private IndexBuffer indexBuffer;

        private VertexBufferBinding[] bindings;
        private LineInfo[] instances;

        struct LineInfo
        {
            public Vector2 World;
        };

        public Matrix View { get; set; }
        public Matrix Projection { get; set; }
        public Effect Effect { get; set; }

        private void InitializeInstanceVertexBuffer()
        {
            VertexElement[] _instanceStreamElements = new VertexElement[1];

            // Position
            _instanceStreamElements[0] = new VertexElement(0, VertexElementFormat.Vector2,
                        VertexElementUsage.Position, 1);


            this.instanceVertexDeclaration = new VertexDeclaration(_instanceStreamElements);
        }

        int count = 50 * 50;
        int grid = 50;
        private void InitializeInstances(GraphicsDevice graphicsDevice)
        {
            this.instances = new LineInfo[count];

            // Set the position for each cube.
            for (int j = 0; j < grid; ++j)
            {
                for (int i = 0; i < grid; ++i)
                {
                    this.instances[j * grid + i].World = new Vector2(500/grid* i, 500/grid * j);
                }
            }

            // Set the instace data to the instanceBuffer.
            this.instanceBuffer = new VertexBuffer(graphicsDevice, instanceVertexDeclaration, count, BufferUsage.WriteOnly);
            this.instanceBuffer.SetData(this.instances);
        }

        private void GenerateCommonGeometry(GraphicsDevice graphicsDevice)
        {
            VertexPositionColor[] vertices = new VertexPositionColor[2];

            vertices[0].Position = new Vector3(0, 0, 0);
            vertices[0].Color = new Color(1, 0, 0);
            vertices[1].Position = new Vector3(1, 0, 0);
            vertices[1].Color = new Color(1, 0, 0);

            geometryBuffer = new VertexBuffer(graphicsDevice, VertexPositionColor.VertexDeclaration,
                                              2, BufferUsage.WriteOnly);
            this.geometryBuffer.SetData(vertices);

            int[] indices = new int[2];
            indices[0] = 0; indices[1] = 1;

            this.indexBuffer = new IndexBuffer(graphicsDevice, typeof(int), 2, BufferUsage.WriteOnly);
            this.indexBuffer.SetData(indices);

        }

        public void Init(GraphicsDevice graphicsDevice)
        {
            InitializeInstanceVertexBuffer();
            GenerateCommonGeometry(graphicsDevice);
            InitializeInstances(graphicsDevice);

            // Creates the binding between the geometry and the instances.
            this.bindings = new VertexBufferBinding[2];
            this.bindings[0] = new VertexBufferBinding(geometryBuffer);
            this.bindings[1] = new VertexBufferBinding(instanceBuffer, 0, 1);

        }

        public void Draw(GraphicsDevice graphicsDevice, Texture2D field)
        {
            View = Matrix.CreateLookAt(new Vector3(0, 0, -100), new Vector3(0, 0, 0), new Vector3(0,-1,0));
            Projection = Matrix.CreateOrthographicOffCenter(-0.5f, 500, -500, -0.5f, 0.001f, 1000f);

            var a = View * Projection;
            // Set the effect technique and parameters
            Effect.CurrentTechnique = Effect.Techniques["Instancing"];
            Effect.Parameters["WVP"].SetValue(View * Projection);
            Effect.Parameters["rotationData"].SetValue(field);

            // Set the indices in the graphics device.
            graphicsDevice.Indices = indexBuffer;

            // Apply the current technique pass.
            Effect.CurrentTechnique.Passes[0].Apply();

            // Set the vertex buffer and draw the instanced primitives.
            graphicsDevice.SetVertexBuffers(bindings);
            graphicsDevice.DrawInstancedPrimitives(PrimitiveType.LineList, 0, 0, 2, count);
            
        }

    }
}
