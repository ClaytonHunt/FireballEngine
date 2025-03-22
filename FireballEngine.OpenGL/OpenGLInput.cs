using FireballEngine.Core.Input;
using OpenTK.Windowing.Common;

namespace FireballEngine.OpenGL;

public class OpenGLInput : IInput
{
    private readonly Dictionary<KeyCode, bool> _keyStates = new();

    public void OnKeyDown(KeyboardKeyEventArgs e)
    {
        if(Enum.TryParse(e.Key.ToString(), true, out KeyCode key))
        {
            _keyStates[key] = true;
        }
    }

    public void OnKeyUp(KeyboardKeyEventArgs e)
    {
        if(Enum.TryParse(e.Key.ToString(), true, out KeyCode key))
        {
            _keyStates[key] = false;
        }
    }

    public bool IsKeyDown(KeyCode key)
    {
        return _keyStates.TryGetValue(key, out bool isDown) && isDown;
    }
}
