using FireballEngine.Core.Math;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace FireballEngine.Core.Tests.Math
{    
    public class Vector2Tests
    {
        [Fact]
        public void Vector2_Magnitude_IsCalculatedCorrectly()
        {
            // Arrange
            var vector = new Vector2(3, 4);

            // Act
            var result = vector.Magnitude();

            // Assert
            Assert.Equal(5, result);
        }

        [Fact]
        public void Vector2_Normalize_IsCalculatedCorrectly()
        {
            // Arrange
            var vector = new Vector2(3, 4);

            // Act
            var result = Vector2.Normalize(vector);

            // Assert
            Assert.Equal(.6, result.X, 3);
            Assert.Equal(.8, result.Y, 3);
            Assert.Equal(1, result.Magnitude());
        }

        [Fact]
        public void Vector2_CrossProduct_IsCalculatedCorrectly()
        {
            // Arrange
            var vector1 = new Vector2(40, 0);
            var vector2 = new Vector2(0, 40);

            // Act
            var result = Vector2.crossProduct(vector1, vector2);

            // Assert
            Assert.Equal(1600, result);
        }
    }
}
