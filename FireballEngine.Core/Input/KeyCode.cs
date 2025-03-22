namespace FireballEngine.Core.Input
{
    /// <summary>
    /// Represents keyboard keys in a platform-independent way.
    /// </summary>
    public enum KeyCode
    {
        // Letters
        A, B, C, D, E, F, G, H, I, J, K, L, M,
        N, O, P, Q, R, S, T, U, V, W, X, Y, Z,
        
        // Numbers
        Alpha0, Alpha1, Alpha2, Alpha3, Alpha4,
        Alpha5, Alpha6, Alpha7, Alpha8, Alpha9,
        
        // Function keys
        F1, F2, F3, F4, F5, F6, F7, F8, F9, F10, F11, F12,
        
        // Arrow keys
        Left, Right, Up, Down,
        
        // Special keys
        Escape, Tab, CapsLock, Shift, Control, Alt,
        Space, Enter, Backspace, Delete, Insert,
        Home, End, PageUp, PageDown,
        
        // Numpad
        Numpad0, Numpad1, Numpad2, Numpad3, Numpad4,
        Numpad5, Numpad6, Numpad7, Numpad8, Numpad9,
        NumpadPlus, NumpadMinus, NumpadMultiply, NumpadDivide, NumpadEnter,
        
        // Other
        None
    }
}
