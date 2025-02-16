using FireballEngine.Core.Math;
using FireballEngine.Core.Utilities;

namespace FireballEngine.Core;

public class Camera
{
    public Matrix4 ViewMatrix { get; private set; }
    public Matrix4 ProjectionMatrix { get; private set; }

    public Vector3 Position { get; set; } = Vector3.Zero;
    public Vector3 Target { get; set; } = new Vector3(0, 0, -1);
    public Vector3 Up { get; set; } = Vector3.Up;

    private ProjectionType _projectionType;
    private float _width, _height;
    private float _near = -1.0f, _far = 1.0f;
    private float _fov = MathF.PI / 4; // 45 degrees

    public Camera(ProjectionType projectionType, float width, float height)
    {
        _projectionType = projectionType;
        _width = width;
        _height = height;

        UpdateProjection();
        UpdateView();
    }

    public void UpdateProjection()
    {
        if (_projectionType == ProjectionType.Orthographic)
        {
            ProjectionMatrix = Matrix4.CreateOrthographicOffCenter(0, _width, 0, _height, _near, _far);
        }
        else
        {
            ProjectionMatrix = Matrix4.CreatePerspectiveFieldOfView(_fov, _width / _height, _near, _far);
        }
    }

    public void UpdateView()
    {
        ViewMatrix = Matrix4.CreateLookAt(Position, Target, Up);
    }

    public void SetPerspective(float fov, float near, float far)
    {
        _projectionType = ProjectionType.Perspective;
        _fov = fov;
        _near = near;
        _far = far;

        UpdateProjection();
    }

    public void SetOrthographic(float near, float far)
    {
        _projectionType = ProjectionType.Orthographic;
        _near = near;
        _far = far;

        UpdateProjection();
    }
}

