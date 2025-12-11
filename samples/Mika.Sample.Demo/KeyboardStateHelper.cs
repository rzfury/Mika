using Microsoft.Xna.Framework.Input;

namespace Mika.DevTest;

public static class KeyboardStateHelper
{
    private static KeyboardState _currentKeyState;
    private static KeyboardState _previousKeyState;

    internal static KeyboardState UpdateState()
    {
        _previousKeyState = _currentKeyState;
        _currentKeyState = Keyboard.GetState();
        return _currentKeyState;
    }

    /// <summary>
    /// Gets whether given key is released.
    /// </summary>
    public static bool GetKeyUp(Keys key)
    {
        return _currentKeyState.IsKeyUp(key);
    }

    /// <summary>
    /// Gets whether given key currently being pressed.
    /// </summary>
    public static bool GetKeyHold(Keys key)
    {
        return _currentKeyState.IsKeyDown(key);
    }

    /// <summary>
    /// Gets whether given key has been pressed once.
    /// </summary>
    public static bool GetKeyDown(Keys key)
    {
        return _currentKeyState.IsKeyDown(key) && !_previousKeyState.IsKeyDown(key);
    }
}

