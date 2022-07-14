using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace RoverVision
{
    class Program : GameWindow
    {

        public static Camera camera;
        private bool freeCamera = true;

        private int _vertexBufferObject;
        private bool _firstMove = true;
        private Vector2 _lastPos;
        //private bool textFramePaint = false;
        //private Vector3 carPosition = new Vector3(0, 0, 0);
        //private Vector3 textFramePosition = new Vector3(0, 0, 0);

        //private Vector3 LWheelPosition = new Vector3(0, 0, 1);
        //private Vector3 RWheelPosition = new Vector3(0, 0, 3);
        //private int countKeyF = 0;
        //здесь задаём все линии
        //private static float[] line1 = new float[]
        //{
        //    0f,0f,0f,
        //    0f,1f,0f,
        //    1f,1f,0f,
        //    1f,0f,0f,
        //};
        //private static float[] line2 = new float[]
        //{
        //    0f,0f,1f,
        //    0f,1f,1f,
        //    1f,1f,1f,
        //    1f,0f,1f,
        //};
        //список со всеми массивами с вершинами линий
        //private List<float[]> Lines = new List<float[]>() { line1, line2 };
        //список со всеми линиями - экземплярами класса Surface
        private List<Surface> Surfaces;
        //private Surface WheelLeft;
        //private Surface WheelRight;
        //private float[] circle;

        private float[] roverLine;

        private bool isForwardX = false;
        private bool isForwardY = false;

        //StreamWriter sw = new StreamWriter("C:\\Users\\Xiaomi\\Text.txt");

        //Terrain terrain;
        //TextFrame textFrame;
        //Car car;
        
        public Program()
            : base(800, 600, GraphicsMode.Default, "MoonSurface")
        {
            WindowState = WindowState.Maximized;//формат окна
            //Console.WriteLine(DisplayDevice.Default.Width+" "+DisplayDevice.Default.Height);
        }

        static void Main(string[] args)
        {
            using (Program program = new Program())
            {
                program.Run();
            }

            //Console.WriteLine()
            //Console.WriteLine(MathHelper.RadiansToDegrees(Math.Acos(0.8)) + " " + MathHelper.RadiansToDegrees(Math.Acos(-0.5)));
        }

        protected override void OnLoad(EventArgs e)
        {
            GL.Enable(EnableCap.DepthTest);
            GL.DepthFunc(DepthFunction.Less);
            //GL.Enable(EnableCap.DepthTest);
            //GL.DepthFunc(DepthFunction.Less);
            //_vertexBufferObject = GL.GenBuffer();
            //GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferObject);
            //GL.ClearColor(0.101f, 0.98f, 1.0f, 1.0f);
            GL.ClearColor(0.219f, 0.333f, 0.360f, 1.0f);


            //GL.ClearColor(0f, 0f, 1f,1f);
            //GL.Enable(EnableCap.DepthTest);
            //terrain = new Terrain(new FileInfo("./Resources/mshrpsc2.png"));//сама картинка
            camera = new Camera(new Vector3(0, 2/*terrain.getHeightAtPosition(256, 256)*/, 0), Width / (float)Height);//положение камеры начальное
            //textFrame = new TextFrame();
            //car = new Car();

            roverLine = new float[]
                    {
                        0f,0.7f,2f,
                        0.7f,0.3f,2f,

                        0.7f,0.3f,2f,
                        2.2f,1f,2f,

                        2.2f,1f,2f,
                        3.3f,0.2f,2f,

                        3.3f,0.2f,2f,
                        4f,2.5f,2f,

                        4f,2.5f,2f,
                        5.7f,2.7f,2f,

                        5.7f,2.7f,2f,
                        6.3f,2.9f,2f,

                        6.3f,2.9f,2f,
                        7.2f,3.3f,2f,

                        7.2f,3.3f,2f,
                        8.4f,2.5f,2f,

                        8.4f,2.5f,2f,
                        9f,2.3f,2f,

                        9f,2.3f,2f,
                        10f,2.1f,2f,
                    };
            //задаём Surfaces
            Surfaces = new List<Surface>()
            {
                //чтобы задать новую линию нужно дать массив вершин и цвет
                //первая линия
                new Surface(
                    new float[]
                    {
                        0f,0.5f,0f,
                        1f,1.4f,0f,

                        1f,1.4f,0f,
                        1.9f,0.4f,0f,

                        1.9f,0.4f,0f,
                        3.1f,1.5f,0f,

                        3.1f,1.5f,0f,
                        4.2f,3.1f,0f,
                        
                        4.2f,3.1f,0f,
                        5f,2.4f,0f,
                        
                        5f,2.4f,0f,
                        6.1f,1.9f,0f,
                        
                        6.1f,1.9f,0f,
                        6.7f,1.2f,0f,
                        
                        6.7f,1.2f,0f,
                        8f,0.7f,0f,
                        
                        8f,0.7f,0f,
                        9.2f,1.3f,0f,

                        9.2f,1.3f,0f,
                        10.1f,2.1f,0f,
                    },
                    new Vector3(1f, 0.980f, 0.058f),
                    "Line"),
                //вторая линия
                new Surface(
                    new float[]
                    {
                        0f,0.3f,1f,
                        0.9f,0f,1f,

                        0.9f,0f,1f,
                        2.4f,2f,1f,

                        2.4f,2f,1f,
                        3.5f,1.3f,1f,

                        3.5f,1.3f,1f,
                        3.8f,2.5f,1f,

                        3.8f,2.5f,1f,
                        5.1f,2.4f,1f,

                        5.1f,2.4f,1f,
                        5.7f,1.8f,1f,

                        5.7f,1.8f,1f,
                        6.5f,1.3f,1f,

                        6.5f,1.3f,1f,
                        8.1f,0.2f,1f,

                        8.1f,0.2f,1f,
                        9.4f,0.9f,1f,

                        9.4f,0.9f,1f,
                        10f,1.5f,1f,
                    },
                    new Vector3(0.058f, 0.203f, 1f),
                    "Line"),
                //третья линия
                new Surface(
                    new float[]
                    {
                        0f,0.7f,2f,
                        0.7f,0.3f,2f,

                        0.7f,0.3f,2f,
                        2.2f,1f,2f,

                        2.2f,1f,2f,
                        3.3f,0.2f,2f,

                        3.3f,0.2f,2f,
                        4f,2.5f,2f,

                        4f,2.5f,2f,
                        5.7f,2.7f,2f,

                        5.7f,2.7f,2f,
                        6.3f,2.9f,2f,

                        6.3f,2.9f,2f,
                        7.2f,3.3f,2f,

                        7.2f,3.3f,2f,
                        8.4f,2.5f,2f,

                        8.4f,2.5f,2f,
                        9f,2.3f,2f,

                        9f,2.3f,2f,
                        10f,2.1f,2f,
                    },
                    new Vector3(1f, 0.211f, 0.058f),
                    "Line"),
                //четвертая линия
                new Surface(
                    new float[]
                    {
                        0f,0.1f,3f,
                        0.5f,0.7f,3f,

                        0.5f,0.7f,3f,
                        1.4f,0.4f,3f,

                        1.4f,0.4f,3f,
                        2.9f,1.2f,3f,

                        2.9f,1.2f,3f,
                        4.3f,2.6f,3f,

                        4.3f,2.6f,3f,
                        5.1f,3.2f,3f,

                        5.1f,3.2f,3f,
                        6.2f,2.1f,3f,

                        6.2f,2.1f,3f,
                        6.9f,1.7f,3f,

                        6.9f,1.7f,3f,
                        8.3f,2.8f,3f,

                        8.3f,2.8f,3f,
                        8.8f,1.4f,3f,

                        8.8f,1.4f,3f,
                        10f,2.1f,3f,
                    },
                    new Vector3(0.066f, 1f, 0.058f),
                    "Line"),
                //пятая линия
                new Surface(
                    new float[]
                    {
                        0f,0.2f,4f,
                        1.3f,0.5f,4f,

                        1.3f,0.5f,4f,
                        1.7f,1.2f,4f,

                        1.7f,1.2f,4f,
                        3.1f,1.5f,4f,

                        3.1f,1.5f,4f,
                        3.8f,2f,4f,

                        3.8f,2f,4f,
                        5.1f,2.2f,4f,

                        5.1f,2.2f,4f,
                        6.5f,2.8f,4f,

                        6.5f,2.8f,4f,
                        7.1f,2.5f,4f,

                        7.1f,2.5f,4f,
                        8f,3.1f,4f,

                        8f,3.1f,4f,
                        8.8f,2.4f,4f,

                        8.8f,2.4f,4f,
                        10f,1.5f,4f,
                    },
                    new Vector3(1f, 0.058f, 0.984f),
                    "Line"),
            };
            //задаём окружность
            //circle = new float[] {};
            //List<float> cir = new List<float>();
            //for (double i = 0; i < 2 * Math.PI; i += 0.2d)
            //{
            //    //circle.Append((float)Math.Cos(i));
            //    //circle.Append((float)Math.Sin(i));
            //    cir.Add((float)Math.Cos(i));
            //    cir.Add((float)Math.Sin(i));
            //    cir.Add(0f);
            //}
            //circle = cir.ToArray();
            //
            //WheelLeft = new Surface(
            //    circle,
            //    new Vector3(1f, 1f, 1f),
            //    "Polygon");
            //
            //WheelRight = new Surface(
            //    circle,
            //    new Vector3(1f, 1f, 1f),
            //    "Polygon");


            //загружаем все Surface внутри Surfaces
            for (int i=0;i<Surfaces.Count;i++)
            {
                Surfaces[i].load();
            }

            //WheelLeft.load();
            //WheelRight.load();

            //car.load();
            //terrain.load();
            //textFrame.load();

            CursorVisible = false;

            base.OnLoad(e);
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);


            //textFrame.load();


            //textFrame = new TextFrame();

            var transform = Matrix4.Identity;
            //var transform1 = transform;
            //var transform2 = transform;

            //рендерим все Surface внутри Surfaces
            for (int i = 0; i < Surfaces.Count; i++)
            {
                Surfaces[i].render(e, transform);
            }

            float wheelscale = 0.1f;
            //матрицы преобразования для колёс
            //var Ltransform = Matrix4.CreateScale(wheelscale) * Matrix4.CreateTranslation(LWheelPosition);
            //var Rtransform = Matrix4.CreateScale(wheelscale) * Matrix4.CreateTranslation(RWheelPosition);

            //WheelLeft.render(e, Ltransform);
            //WheelRight.render(e, Rtransform);

            //carPosition.Y = terrain.returnHeightOnTriangle(new Vector2(carPosition.X, carPosition.Z));
            //transform1 *= Matrix4.CreateTranslation(carPosition);
            //transform1 *= Matrix4.CreateRotationZ(camera.return_pitch());
            //textFramePosition = camera.returnFront();
            //transform2 *= Matrix4.CreateTranslation(textFramePosition);
            //transform2 *= Matrix4.CreateTranslation(camera.Position + camera.Front);
            //transform2 *= Matrix4.CreateRotationZ(camera.return_pitch());
            //transform2*= Matrix4.CreateRotationZ(camera.return_yaw());

            //sw.Write(carPosition);
            //var obstacle = false;

            //sw.WriteLine(" "+obstacle);
            //if(textFramePaint) textFrame.render(e, transform2);
            //car.render(e, transform1);
            //terrain.render(e, transform);
            //if (textFramePaint) textFrame.render(e, Matrix4.CreateTranslation(camera.Position + camera.Front));
            //Console.WriteLine(carPosition+"\x020"+camera.Position);
            //Console.WriteLine(textFramePosition + "\x020" + camera.Position);
            //Console.WriteLine(camera.Front + "\x020" + camera.returnFront());
            //transform *= Matrix4.CreateTranslation(new Vector3(camera.Position.X-10,camera.Position.Y,camera.Position.Z-10));//положение окошка перед камерой
            //if (framePaint) textFrame.render(e, transform);
            //textFrame.render(e, transform);
            //surface1.render(e,transform);

            SwapBuffers();

            base.OnRenderFrame(e);
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            var input = Keyboard.GetState();

            if (input.IsKeyDown(Key.Escape))
            {
                Exit();
            }

            if (input.IsKeyDown(Key.F)) freeCamera = true;
            if (input.IsKeyDown(Key.G)) freeCamera = false;

            //if(input.IsKeyDown(Key.F)) textFramePaint = true;

            //if (input.IsKeyDown(Key.G)) textFramePaint = false;

            //if(input.IsKeyDown(Key.F))
            //{
            //    framePaint = true;
            //    //countKeyF++;
            //    //textFrame.load();
            //    //textFrame.render(e, Matrix4.Identity);
            //    //SwapBuffers();
            //}
            //if (input.IsKeyDown(Key.G))
            //{
            //    framePaint=false;
            //}



            //Console.WriteLine(maxX);
            //Console.WriteLine(maxY);

            const float cameraSpeed = 3f;
            const float sensitivity = 0.2f;

            const float carSpeed = 300f;

            //if (input.IsKeyDown(Key.Up))
            //{
            //    isForwardX = true;
            //    //carPosition.X += carSpeed * (float)e.Time;
            //}
            //
            //if (input.IsKeyDown(Key.Down))
            //{
            //    isForwardX = false;
            //    //carPosition.X -= carSpeed * (float)e.Time;
            //}
            //
            //if (input.IsKeyDown(Key.Right))
            //{
            //    isForwardY = true;
            //    //carPosition.Z += carSpeed * (float)e.Time;
            //}
            //
            //if (input.IsKeyDown(Key.Left))
            //{
            //    isForwardY = false;
            //    //carPosition.Z -= carSpeed * (float)e.Time;
            //}

            //Vector3 front = camera.Front;
            //front.Y = 0;
            if(freeCamera)
            {
                if (input.IsKeyDown(Key.W))
                {
                    camera.Position += camera.Front * cameraSpeed * (float)e.Time; // Forward
                }
                if (input.IsKeyDown(Key.S))
                {
                    camera.Position -= camera.Front * cameraSpeed * (float)e.Time; // Backwards
                }
                if (input.IsKeyDown(Key.A))
                {
                    camera.Position -= camera.Right * cameraSpeed * (float)e.Time; // Left
                }
                if (input.IsKeyDown(Key.D))
                {
                    camera.Position += camera.Right * cameraSpeed * (float)e.Time; // Right
                }
                if (input.IsKeyDown(Key.Space))
                {
                    camera.Position += camera.Up * cameraSpeed * (float)e.Time; // Up
                }
                if (input.IsKeyDown(Key.LShift))
                {
                    camera.Position -= camera.Up * cameraSpeed * (float)e.Time; // Down
                }
            }
            
            if(!freeCamera)
            {
                camera.Position =
                    new Vector3(camera.Position.X, camera.Ynofreecamera(roverLine, camera.Position.X) + 1f, 2f);
                if (input.IsKeyDown(Key.W))
                {
                    camera.Position += new Vector3(1f, 0f, 0f) * cameraSpeed * (float)e.Time; // Forward
                }
                if (input.IsKeyDown(Key.S))
                {
                    camera.Position -= new Vector3(1f, 0f, 0f) * cameraSpeed * (float)e.Time; // Backwards
                }
                if (camera.Position.X < roverLine[0]) camera.Position =
                        new Vector3(roverLine[0], camera.Position.Y, camera.Position.Z);
                if (camera.Position.X > roverLine[roverLine.Length - 3]) camera.Position =
                        new Vector3(roverLine[roverLine.Length - 3], camera.Position.Y, camera.Position.Z);
            }


            var mouse = Mouse.GetState();

            if (_firstMove)
            {
                _lastPos = new Vector2(mouse.X, mouse.Y);
                _firstMove = false;
            }
            else
            {

                var deltaX = mouse.X - _lastPos.X;
                var deltaY = mouse.Y - _lastPos.Y;
                _lastPos = new Vector2(mouse.X, mouse.Y);


                camera.Yaw += deltaX * sensitivity;
                camera.Pitch -= deltaY * sensitivity;
                //здесь ограничиваем угол обзора(тангажа и рыскания) у несвободной камеры в 2*20 градусов 
                if (!freeCamera && camera.Yaw < -20) camera.Yaw = -20;
                if (!freeCamera && camera.Yaw > 20) camera.Yaw = 20;
                if (!freeCamera && camera.Pitch < -20) camera.Pitch = -20;
                if (!freeCamera && camera.Pitch > 20) camera.Pitch = 20;
            }

            base.OnUpdateFrame(e);
        }

        protected override void OnMouseMove(MouseMoveEventArgs e)
        {
            if (Focused)
            {
                Mouse.SetPosition(X + Width / 2f, Y + Height / 2f);
            }

            base.OnMouseMove(e);
        }

        protected override void OnResize(EventArgs e)
        {
            GL.Viewport(0, 0, Width, Height);
            camera.AspectRatio = Width / (float)Height;
            base.OnResize(e);
        }

        protected override void OnUnload(EventArgs e)
        {
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            GL.BindVertexArray(0);
            GL.UseProgram(0);
            //terrain.destroy(e);
            //textFrame.destroy(e);
            //car.destroy(e);
            //surface1.destroy(e);

            //удаляем все Surface внутри Surfaces
            for (int i = 0; i < Surfaces.Count; i++)
            {
                Surfaces[i].destroy(e);
            }

            //WheelLeft.destroy(e);
            //WheelRight.destroy(e);

            GL.DeleteBuffer(_vertexBufferObject);
            base.OnUnload(e);
            //sw.Close();
        }
    }
}