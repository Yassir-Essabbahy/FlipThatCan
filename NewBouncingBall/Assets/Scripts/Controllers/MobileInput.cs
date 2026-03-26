using UnityEngine;

public class MobileInput : MonoBehaviour
{
    // public output
    public static float move;      // -1 .. +1
    static bool _jumpDown;
    static bool _jumpUp;
    public static bool jumpHeld;

    // internal holds
    static bool leftHeld;
    static bool rightHeld;

    // ----- Move buttons -----
    public void LeftDown()  => leftHeld = true;
    public void LeftUp()    => leftHeld = false;

    public void RightDown() => rightHeld = true;
    public void RightUp()   => rightHeld = false;

    // ----- Jump button -----
    public void JumpDown()
    {
        _jumpDown = true;
        jumpHeld = true;
    }

    public void JumpUp()
    {
        _jumpUp = true;
        jumpHeld = false;
    }

    // Consume (so FixedUpdate won't miss)
    public static bool ConsumeJumpDown()
    {
        bool v = _jumpDown;
        _jumpDown = false;
        return v;
    }

    public static bool ConsumeJumpUp()
    {
        bool v = _jumpUp;
        _jumpUp = false;
        return v;
    }

    void Update()
    {
        // compute move from holds (supports move+jump together)
        if (leftHeld && !rightHeld) move = -1f;
        else if (rightHeld && !leftHeld) move = 1f;
        else move = 0f;
    }

    void OnDisable()
    {
        leftHeld = rightHeld = false;
        move = 0f;
        jumpHeld = false;
        _jumpDown = _jumpUp = false;
    }
}
