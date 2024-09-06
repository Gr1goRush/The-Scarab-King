using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UniversalInput : PlayerInput
{
    [SerializeField] private Button _spinBtn;
    private bool _pointerDown;

    private void Awake()
    {
        _spinBtn.onClick.AddListener(() => GetEvent(EventKey.PointerDown)?.Invoke());
    }

    public override bool GetInputAction(InputKey key)
    {
        switch (key)
        {
            case InputKey.PointerDown:
                return _pointerDown;
            default:
                return false;
        }
    }

    public override Vector2 GetInputAxis(AxisKey key)
    {
        switch (key)
        {
            default:
                return Vector2.zero;
        }
    }

    public override Vector2 GetInputAxisRaw(AxisKey key)
    {
        switch (key)
        {
            default:
                return Vector2.zero;
        }
    }
}
