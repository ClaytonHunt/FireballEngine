using FireballEngine.Core.Math;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace FireballEngine.Core.Tests.Math
{
    public class Matrix4Tests
    {
        [Fact]
        public void Matrix4_LookAt_IsCalculatedCorrectly()
        {
            // Arrange
            var eye = new Vector3(0, 0, 1);
            var target = new Vector3(0, 0, 0);
            var up = new Vector3(0, 1, 0);

            // Act
            var result = Matrix4.LookAt(eye, target, up);

            // Assert
            Assert.Equal(1, result.M11);
            Assert.Equal(0, result.M12);
            Assert.Equal(0, result.M13);
            Assert.Equal(0, result.M14);
            Assert.Equal(0, result.M21);
            Assert.Equal(1, result.M22);
            Assert.Equal(0, result.M23);
            Assert.Equal(0, result.M24);
            Assert.Equal(0, result.M31);
            Assert.Equal(0, result.M32);
            Assert.Equal(1, result.M33);
            Assert.Equal(0, result.M34);
            Assert.Equal(0, result.M41);
            Assert.Equal(0, result.M42);
            Assert.Equal(-1, result.M43);
            Assert.Equal(1, result.M44);
        }
    }
}
