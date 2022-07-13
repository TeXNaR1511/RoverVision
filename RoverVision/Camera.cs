﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using OpenTK;

namespace RoverVision
{

    public class Camera
    {

        private Vector3 _front = -Vector3.UnitZ;

        private Vector3 _up = Vector3.UnitY;

        private Vector3 _right = Vector3.UnitX;


        private float _pitch;


        private float _yaw = MathHelper.PiOver2;


        private float _fov = MathHelper.PiOver2;

        public Camera(Vector3 position, float aspectRatio)
        {
            Position = position;
            AspectRatio = aspectRatio;
        }


        public Vector3 Position { get; set; }


        public float AspectRatio { private get; set; }

        public Vector3 Front => _front;

        public Vector3 Up => _up;

        public Vector3 Right => _right;


        public float Pitch
        {
            get => MathHelper.RadiansToDegrees(_pitch);
            set
            {



                var angle = MathHelper.Clamp(value, -89f, 89f);
                _pitch = MathHelper.DegreesToRadians(angle);
                UpdateVectors();
            }
        }


        public float Yaw
        {
            get => MathHelper.RadiansToDegrees(_yaw);
            set
            {
                _yaw = MathHelper.DegreesToRadians(value);
                UpdateVectors();
            }
        }




        public float Fov
        {
            get => MathHelper.RadiansToDegrees(_fov);
            set
            {
                var angle = MathHelper.Clamp(value, 1f, 45f);
                _fov = MathHelper.DegreesToRadians(angle);
            }
        }


        public Matrix4 GetViewMatrix()
        {
            return Matrix4.LookAt(Position, Position + _front, _up);
        }


        public Matrix4 GetProjectionMatrix()
        {
            //return Matrix4.CreateOrthographicOffCenter(_fo)
            return Matrix4.CreatePerspectiveFieldOfView(_fov, AspectRatio, 0.1f, 100000f);/*последний параметр дальность зрения*/
        }

        public Matrix4 GetOrthoProjectionMatrix()
        {
            return Matrix4.CreateOrthographic(DisplayDevice.Default.Width / 2, DisplayDevice.Default.Height / 2, 1f, 1000000f);
        }



        private void UpdateVectors()
        {

            _front.X = (float)Math.Cos(_pitch) * (float)Math.Cos(_yaw);
            _front.Y = (float)Math.Sin(_pitch);
            _front.Z = (float)Math.Cos(_pitch) * (float)Math.Sin(_yaw);


            _front = Vector3.Normalize(_front);




            _right = Vector3.Normalize(Vector3.Cross(_front, Vector3.UnitY));
            _up = Vector3.Normalize(Vector3.Cross(_right, _front));
        }

        public float return_pitch()
        {
            return _pitch;
        }

        public float return_yaw()
        {
            return _yaw;
        }

    }
}

