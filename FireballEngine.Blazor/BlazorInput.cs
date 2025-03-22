using FireballEngine.Core.Input;
using Microsoft.AspNetCore.Components.Web;

namespace FireballEngine.Blazor.Input
{
    public class BlazorInputManager : IInput
    {
        private readonly Dictionary<KeyCode, bool> _keyStates = new();

        public void OnKeyDown(KeyboardEventArgs e)
        {
            if (TryGetKeyCode(e.Key, out KeyCode key))
            {
                _keyStates[key] = true;
            }
        }

        public void OnKeyUp(KeyboardEventArgs e)
        {
            if (TryGetKeyCode(e.Key, out KeyCode key))
            {
                _keyStates[key] = false;
            }
        }

        public bool IsKeyDown(KeyCode key)
        {
            return _keyStates.TryGetValue(key, out bool isDown) && isDown;
        }

        private bool TryGetKeyCode(string key, out KeyCode keyCode)
        {
            switch (key)
            {
                case "ArrowLeft":
                    keyCode = KeyCode.Left;
                    return true;
                case "ArrowRight":
                    keyCode = KeyCode.Right;
                    return true;
                case "ArrowUp":
                    keyCode = KeyCode.Up;
                    return true;
                case "ArrowDown":
                    keyCode = KeyCode.Down;
                    return true;
                case "w":
                case "W":
                    keyCode = KeyCode.W;
                    return true;
                case "a":
                case "A":
                    keyCode = KeyCode.A;
                    return true;
                case "s":
                case "S":
                    keyCode = KeyCode.S;
                    return true;
                case "d":
                case "D":
                    keyCode = KeyCode.D;
                    return true;
                case " ":
                    keyCode = KeyCode.Space;
                    return true;
                case "Escape":
                    keyCode = KeyCode.Escape;
                    return true;
                default:
                    keyCode = default;
                    return false;
            }
        }
    }
}