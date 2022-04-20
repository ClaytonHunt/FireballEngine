using FireballEngine.Core.Math;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace FireballEngine.Core.Tests.Math
{    
    public class Vector3Tests
    {
        [Fact]
        public void Vector3_Magnitude_IsCalculatedCorrectly()
        {
            // Arrange
            var vector = new Vector3(2, 3, 6);

            // Act
            var result = vector.Magnitude();

            // Assert
            Assert.Equal(7, result);
        }

        [Fact]
        public void Vector3_Normalize_IsCalculatedCorrectly()
        {
            // Arrange
            var vector = new Vector3(2, 3, 6);

            // Act
            var result = Vector3.Normalize(vector);

            // Assert
            Assert.Equal(.286, result.X, 3);
            Assert.Equal(.429, result.Y, 3);
            Assert.Equal(.857, result.Z, 3);
            Assert.Equal(1, result.Magnitude());
        }

        [Fact]
        public void Vector3_CrossProduct_IsCalculatedCorrectly()
        {
            // Arrange
            var up = new Vector3(0, 1, 0);
            var forward = new Vector3(0, 0, 1);

            // Act
            var result = Vector3.CrossProduct(up, forward);

            // Assert
            Assert.Equal(1, result.X);
            Assert.Equal(0, result.Y);
            Assert.Equal(0, result.Z);
        }
    }
}
