using Mathematics = System.MathF;

namespace FireballEngine.Core.Math
{
    public struct Matrix4 : IEquatable<Matrix4>
    {
        public float M11;
        public float M12;
        public float M13;
        public float M14;
        public float M21;
        public float M22;
        public float M23;
        public float M24;
        public float M31;
        public float M32;
        public float M33;
        public float M34;
        public float M41;
        public float M42;
        public float M43;
        public float M44;

        public static Matrix4 Identity => new Matrix4(
            1, 0, 0, 0,
            0, 1, 0, 0,
            0, 0, 1, 0,
            0, 0, 0, 1
        );

        public Matrix4(Vector4 row1, Vector4 row2, Vector4 row3, Vector4 row4)
        {
            M11 = row1.X;
            M12 = row1.Y;
            M13 = row1.Z;
            M14 = row1.W;
            M21 = row2.X;
            M22 = row2.Y;
            M23 = row2.Z;
            M24 = row2.W;
            M31 = row3.X;
            M32 = row3.Y;
            M33 = row3.Z;
            M34 = row3.W;
            M41 = row4.X;
            M42 = row4.Y;
            M43 = row4.Z;
            M44 = row4.W;
        }

        public Matrix4(float m11, float m12, float m13, float m14,
                       float m21, float m22, float m23, float m24,
                       float m31, float m32, float m33, float m34,
                       float m41, float m42, float m43, float m44)
        {
            M11 = m11;
            M12 = m12;
            M13 = m13;
            M14 = m14;
            M21 = m21;
            M22 = m22;
            M23 = m23;
            M24 = m24;
            M31 = m31;
            M32 = m32;
            M33 = m33;
            M34 = m34;
            M41 = m41;
            M42 = m42;
            M43 = m43;
            M44 = m44;
        }

        public Matrix4(Matrix4 seed)
        {
            M11 = seed.M11;
            M12 = seed.M12;
            M13 = seed.M13;
            M14 = seed.M14;
            M21 = seed.M21;
            M22 = seed.M22;
            M23 = seed.M23;
            M24 = seed.M24;
            M31 = seed.M31;
            M32 = seed.M32;
            M33 = seed.M33;
            M34 = seed.M34;
            M41 = seed.M41;
            M42 = seed.M42;
            M43 = seed.M43;
            M44 = seed.M44;
        }

        public Matrix4 RotateX(float degrees)
        {
            var radians = Mathematics.Tan(degrees * Mathematics.PI / 360.0f);

            var cos = MathF.Cos(radians);
            var sin = MathF.Sin(radians);

            var result = new Matrix4(this);

            result.M21 = this.M21 * cos + this.M31 * sin;
            result.M22 = this.M22 * cos + this.M32 * sin;
            result.M23 = this.M23 * cos + this.M33 * sin;
            result.M24 = this.M24 * cos + this.M34 * sin;

            result.M31 = this.M31 * cos - this.M21 * sin;
            result.M32 = this.M32 * cos - this.M22 * sin;
            result.M33 = this.M33 * cos - this.M23 * sin;
            result.M34 = this.M34 * cos - this.M24 * sin;

            return result;
        }

        public Matrix4 RotateY(float degrees)
        {
            var radians = Mathematics.Tan(degrees * Mathematics.PI / 360.0f);

            var cos = MathF.Cos(radians);
            var sin = MathF.Sin(radians);

            var result = new Matrix4(this);            

            result.M11 = this.M11 * cos - this.M31 * sin;
            result.M12 = this.M12 * cos - this.M32 * sin;
            result.M13 = this.M13 * cos - this.M33 * sin;
            result.M14 = this.M14 * cos - this.M34 * sin;

            result.M31 = this.M11 * sin + this.M31 * cos;
            result.M32 = this.M12 * sin + this.M32 * cos;
            result.M33 = this.M13 * sin + this.M33 * cos;
            result.M34 = this.M14 * sin + this.M34 * cos;

            return result;
        }

        public Matrix4 RotateZ(float degrees)
        {
            var radians = Mathematics.Tan(degrees * Mathematics.PI / 360.0f);

            var cos = MathF.Cos(radians);
            var sin = MathF.Sin(radians);

            var result = new Matrix4(this);

            result.M11 = this.M11 * cos + this.M21 * sin;
            result.M12 = this.M12 * cos + this.M22 * sin;
            result.M13 = this.M13 * cos + this.M23 * sin;
            result.M14 = this.M14 * cos + this.M24 * sin;

            result.M21 = this.M21 * cos - this.M11 * sin;
            result.M22 = this.M22 * cos - this.M12 * sin;
            result.M23 = this.M23 * cos - this.M13 * sin;
            result.M24 = this.M24 * cos - this.M14 * sin;

            return result;
        }

        public static Matrix4 CreateOrthorgraphicProjection(float width, float height, float near, float far)
        {
            var left = -width / 2;
            var right = width / 2;
            var bottom = -height / 2;
            var top = height / 2;

            var invRL = 1.0f / (right - left);
            var invTB = 1.0f / (top - bottom);
            var invFN = 1.0f / (far - near);

            return new Matrix4(
                2 * invRL, 0, 0, 0,
                0, 2 * invTB, 0, 0,
                0, 0, -2 * invFN, 0,
                -(right + left) * invRL, -(top + bottom) * invTB, -(far + near) * invFN, 1
            );
        }

        public static Matrix4 CreatePerspectiveProjection(int fieldOfView, float width, float height, float nearClip, float farClip)
        {
            if (width == 0) width = 1;
            if (height == 0) height = 1;

            var aspect = width / height;
            var top = nearClip * Mathematics.Tan(fieldOfView * Mathematics.PI / 360.0f);
            var bottom = -top;
            var left = bottom * aspect;
            var right = top * aspect;

            var lengthX = right - left;
            var lengthY = top - bottom;
            var lengthZ = farClip - nearClip;

            return new Matrix4(
                2 * nearClip / lengthX, 0, 0, 0,
                0, 2 * nearClip / lengthY, 0, 0,
                (right + left) / lengthX, (top + bottom) / lengthY, -(farClip + nearClip) / lengthZ, -1,
                0, 0, -2 * farClip * nearClip / lengthZ, 0
            );
        }

        public static Matrix4 LookAt(Vector3 eye, Vector3 target, Vector3 up)
        {
            var z = Vector3.Normalize(eye - target);
            var x = Vector3.Normalize(Vector3.CrossProduct(up, z));
            var y = Vector3.Normalize(Vector3.CrossProduct(z, x));

            return new Matrix4(
                x.X, y.X, z.X, 0,
                x.Y, y.Y, z.Y, 0,
                x.Z, y.Z, z.Z, 0,
                -Dot(eye, x), -Dot(eye, y), -Dot(eye, z), 1
            );
        }

        private static float Dot(Vector3 eye, Vector3 x)
        {
            return ((x.X * eye.X) + (x.Y * eye.Y) + (x.Z * eye.Z));
        }

        public bool Equals(Matrix4 other)
        {
            throw new NotImplementedException();
        }

        public static Matrix4 operator *(Matrix4 left, Matrix4 right)
        {
            return new Matrix4(
                (left.M11 * right.M11) + (left.M12 * right.M21) + (left.M13 * right.M31) + (left.M14 * right.M41),
                (left.M11 * right.M12) + (left.M12 * right.M22) + (left.M13 * right.M32) + (left.M14 * right.M42),
                (left.M11 * right.M13) + (left.M12 * right.M23) + (left.M13 * right.M33) + (left.M14 * right.M43),
                (left.M11 * right.M14) + (left.M12 * right.M24) + (left.M13 * right.M34) + (left.M14 * right.M44),
                (left.M21 * right.M11) + (left.M22 * right.M21) + (left.M23 * right.M31) + (left.M24 * right.M41),
                (left.M21 * right.M12) + (left.M22 * right.M22) + (left.M23 * right.M32) + (left.M24 * right.M42),
                (left.M21 * right.M13) + (left.M22 * right.M23) + (left.M23 * right.M33) + (left.M24 * right.M43),
                (left.M21 * right.M14) + (left.M22 * right.M24) + (left.M23 * right.M34) + (left.M24 * right.M44),
                (left.M31 * right.M11) + (left.M32 * right.M21) + (left.M33 * right.M31) + (left.M34 * right.M41),
                (left.M31 * right.M12) + (left.M32 * right.M22) + (left.M33 * right.M32) + (left.M34 * right.M42),
                (left.M31 * right.M13) + (left.M32 * right.M23) + (left.M33 * right.M33) + (left.M34 * right.M43),
                (left.M31 * right.M14) + (left.M32 * right.M24) + (left.M33 * right.M34) + (left.M34 * right.M44),
                (left.M41 * right.M11) + (left.M42 * right.M21) + (left.M43 * right.M31) + (left.M44 * right.M41),
                (left.M41 * right.M12) + (left.M42 * right.M22) + (left.M43 * right.M32) + (left.M44 * right.M42),
                (left.M41 * right.M13) + (left.M42 * right.M23) + (left.M43 * right.M33) + (left.M44 * right.M43),
                (left.M41 * right.M14) + (left.M42 * right.M24) + (left.M43 * right.M34) + (left.M44 * right.M44)
            );
        }
    }
}
