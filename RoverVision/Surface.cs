using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace RoverVision
{
    class Surface
    {
        private readonly float[] surface;
        //private int _vertexBufferObject;
        private Shader surfaceShader;
        private int surfaceVAO;
        //private int _vertexBufferObject;
        private int surfaceBufferObject;

        private Vector3 objectColor = new Vector3();

        public Surface(float[] surface, Vector3 objectColor)
        {
            this.surface = surface;
            this.objectColor = objectColor;
            //_vertexBufferObject = _vBO;
            //_vertexBufferObject = GL.GenBuffer();
            surfaceShader = new Shader("./Shaders/shader2.vert", "./Shaders/shader2.frag");
        }

        public void load()
        {
            //GL.Enable(EnableCap.DepthTest);
            //GL.DepthFunc(DepthFunction.Less);
            //GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferObject);
            surfaceShader.Use();
            GL.GenVertexArrays(1, out surfaceVAO);
            GL.BindVertexArray(surfaceVAO);
            GL.GenBuffers(1, out surfaceBufferObject);
            GL.BindBuffer(BufferTarget.ArrayBuffer, surfaceBufferObject);
            GL.BufferData(BufferTarget.ArrayBuffer, surface.Length * sizeof(float), surface, BufferUsageHint.StaticDraw);

            //var vertexLocation = _frameshader.GetAttribLocation("aPosition");
            GL.EnableVertexAttribArray(0);
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);
        }

        public void render(FrameEventArgs e, Matrix4 model)
        {
            //_frameshader.SetMatrix4("model", model);

            surfaceShader.Use();

            surfaceShader.SetMatrix4("view", Program.camera.GetViewMatrix());
            surfaceShader.SetMatrix4("projection", Program.camera.GetProjectionMatrix());
            surfaceShader.SetVector3("objectColor", objectColor);//цвет окошка перед камерой
            //_shader.SetVector3("objectColor", new Vector3(0.83f, 0.06f, 0.06f));

            surfaceShader.SetMatrix4("model", model);
            GL.BindVertexArray(surfaceVAO);
            GL.DrawArrays(PrimitiveType.Lines, 0, surface.Length);
        }

        public void destroy(EventArgs e)
        {
            GL.DeleteProgram(surfaceShader.Handle);
            //GL.DeleteBuffer(_vertexBufferObject);
        }

    }
}

