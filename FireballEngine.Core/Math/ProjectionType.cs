namespace FireballEngine.Core.Math
{
    /// <summary>
    /// Specifies the type of projection to use for rendering.
    /// </summary>
    public enum ProjectionType
    {
        /// <summary>
        /// Perspective projection, which simulates 3D perspective with depth and foreshortening.
        /// </summary>
        Perspective,
        
        /// <summary>
        /// Orthographic projection, which renders objects at their actual size regardless of distance.
        /// </summary>
        Orthographic
    }
}
