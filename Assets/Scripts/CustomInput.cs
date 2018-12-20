using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public struct MouseButtonsState
{

    public bool mouseButton0;
    public bool mouseButton1;
    public bool mouseButton2;
    public MouseButtonsState(bool btn0 = false, bool btn1 = false, bool btn2 = false)
    {
        mouseButton0 = btn0;
        mouseButton1 = btn1;
        mouseButton2 = btn2;
    }
    public MouseButtonsState(MouseButtonsState other)
    {
        if (other.Equals(null))
        {
            mouseButton0 = false;
            mouseButton1 = false;
            mouseButton2 = false;
        }
        else {
            mouseButton0 = other.mouseButton0;
            mouseButton1 = other.mouseButton1;
            mouseButton2 = other.mouseButton2;
        }
    }

    public bool this[int index]
    {
        get
        {
            switch (index)
            {
                case 0:
                    return mouseButton0;
                case 1:
                    return mouseButton1;
                case 2:
                    return mouseButton2;
            }
            return false;
        }
        set
        {
            switch (index)
            {
                case 0:
                    mouseButton0 = value;
                    break;
                case 1:
                    mouseButton1 = value;
                    break;
                case 2:
                    mouseButton2 = value;
                    break;
            }
        }
    }

    public override string ToString()
    {
        return  mouseButton0 + ";" + mouseButton1 + ";" + mouseButton2;
    }

    public static MouseButtonsState Parse(string str)
    {
        string [] splitted = str.Trim(' ').Split(';');
        return new MouseButtonsState(bool.Parse(splitted[0]), bool.Parse(splitted[1]), bool.Parse(splitted[2]));
    }
}


/*public struct CustomTouch
{
    public float altitudeAngle;
    public float azimuthAngle;
    public Vector2 deltaPosition;
    public float deltaTime;
    public int fingerId;
    public float maximumPossiblePressure;
    public TouchPhase phase;
    public Vector2 position;
    public float pressure;
    public float radius;
    public float radiusVariance;
    public Vector2 rawPosition;
    public int tapCount;
    public TouchType type;

    public CustomTouch(Touch touch)
    {
        altitudeAngle = touch.altitudeAngle;
        azimuthAngle = touch.azimuthAngle;
        deltaPosition = touch.deltaPosition;
        deltaTime = touch.deltaTime;
        fingerId = touch.fingerId;
        maximumPossiblePressure = touch.maximumPossiblePressure;
        phase = touch.phase;
        position = touch.position;
        pressure = touch.pressure;
        radius = touch.radius;
        radiusVariance = touch.radiusVariance;
        rawPosition = touch.rawPosition;
        tapCount = touch.tapCount;
        type = touch.type;
    }
}
*/
[DefaultExecutionOrder(-100)]
public class CustomInput : MonoBehaviour
{
    static public KeyCode[] allKeys;
    static public List<KeyCode> keysPressed = new List<KeyCode>();
    static public List<KeyCode> keysPressedDown = new List<KeyCode>();
    static public List<KeyCode> keysPressedUp = new List<KeyCode>();
    static public MouseButtonsState mouseButtonsPressed;
    static public MouseButtonsState mouseButtonsPressedDown;
    static public MouseButtonsState mouseButtonsPressedUp;

    //Variables
    static public Vector3 _acceleration;
    static public int _accelerationEventCount;
    static public AccelerationEvent[] _accelerationEvents;
    static public bool _anyKey;
    static public bool _anyKeyDown;
    static public bool _backButtonLeavesApp;
    static public Compass _compass;
    static public bool _compensateSensors;
    static public Vector2 _compositionCursorPos;
    static public string _compositionString;
    static public DeviceOrientation _deviceOrientation;
    static public Gyroscope _gyro;
    static public IMECompositionMode _imeCompositionMode;
    static public bool _imeIsSelected;
    static public string _inputString;
    static public LocationService _location;
    static public Vector3 _mousePosition;
    static public bool _mousePresent;
    static public Vector2 _mouseScrollDelta;
    static public bool _multiTouchEnabled;
    static public bool _simulateMouseWithTouches;
    static public bool _stylusTouchSupported;
    static public int _touchCount;
    static public List<Touch> _touches;
    static public bool _touchPressureSupported;
    static public bool _touchSupported;

    //Properties
    static public Vector3 acceleration
    {
        get
        {
            return _acceleration;
        }
        set
        {
            _acceleration = value;
        }
    }
    static public int accelerationEventCount
    {
        get
        {
            return _accelerationEventCount;
        }
        set
        {
            _accelerationEventCount = value;
        }
    }
    static public AccelerationEvent[] accelerationEvents
    {
        get
        {
            return _accelerationEvents;
        }
        set
        {
            _accelerationEvents = value;
        }
    }
    static public bool anyKey
    {
        get
        {
            return _anyKey;
        }
        set
        {
            _anyKey = value;
        }
    }
    static public bool anyKeyDown
    {
        get
        {
            return _anyKeyDown;
        }
        set
        {
            _anyKeyDown = value;
        }
    }
    static public bool backButtonLeavesApp
    {
        get
        {
            return _backButtonLeavesApp;
        }
        set
        {
            _backButtonLeavesApp = value;
            Input.backButtonLeavesApp = value;
        }
    }
    static public Compass compass
    {
        get
        {
            return _compass;
        }
        set
        {
            _compass = value;
        }
    }
    static public bool compensateSensors
    {
        get
        {
            return _compensateSensors;
        }
        set
        {
            _compensateSensors = value;
            Input.compensateSensors = value;
        }
    }
    static public Vector2 compositionCursorPos
    {
        get
        {
            return _compositionCursorPos;
        }
        set
        {
            _compositionCursorPos = value;
            Input.compositionCursorPos = value;
        }
    }
    static public string compositionString
    {
        get
        {
            return _compositionString;
        }
        set
        {
            _compositionString = value;
        }
    }
    static public DeviceOrientation deviceOrientation
    {
        get
        {
            return _deviceOrientation;
        }
        set
        {
            _deviceOrientation = value;
        }
    }
    static public Gyroscope gyro
    {
        get
        {
            return _gyro;
        }
        set
        {
            _gyro = value;
        }
    }
    static public IMECompositionMode imeCompositionMode
    {
        get
        {
            return _imeCompositionMode;
        }
        set
        {
            _imeCompositionMode = value;
            Input.imeCompositionMode = value;
        }
    }
    static public bool imeIsSelected
    {
        get
        {
            return _imeIsSelected;
        }
        set
        {
            _imeIsSelected = value;
        }
    }
    static public string inputString
    {
        get
        {
            return _inputString;
        }
        set
        {
            _inputString = value;
        }
    }
    static public LocationService location
    {
        get
        {
            return _location;
        }
        set
        {
            _location = value;
        }
    }
    static public Vector3 mousePosition
    {
        get
        {
            return _mousePosition;
        }
        set
        {
            _mousePosition = value;
        }
    }
    static public bool mousePresent
    {
        get
        {
            return _mousePresent;
        }
        set
        {
            _mousePresent = value;
        }
    }
    static public Vector2 mouseScrollDelta
    {
        get
        {
            return _mouseScrollDelta;
        }
        set
        {
            _mouseScrollDelta = value;
        }
    }
    static public bool multiTouchEnabled
    {
        get
        {
            return _multiTouchEnabled;
        }
        set
        {
            _multiTouchEnabled = value;
            Input.multiTouchEnabled = value;
        }
    }
    static public bool simulateMouseWithTouches
    {
        get
        {
            return _simulateMouseWithTouches;
        }
        set
        {
            _simulateMouseWithTouches = value;
            Input.simulateMouseWithTouches = value;
        }
    }
    static public bool stylusTouchSupported
    {
        get
        {
            return _stylusTouchSupported;
        }
        set
        {
            _stylusTouchSupported = value;
        }
    }
    static public int touchCount
    {
        get
        {
            return _touchCount;
        }
        set
        {
            _touchCount = value;
        }
    }
    static public List<Touch> touches
    {
        get
        {
            return _touches;
        }
        set
        {
            _touches = value;
            _touchCount = _touches.Count;
        }
    }
    static public bool touchPressureSupported
    {
        get
        {
            return _touchPressureSupported;
        }
        set
        {
            _touchPressureSupported = value;
        }
    }
    static public bool touchSupported
    {
        get
        {
            return _touchSupported;
        }
        set
        {
            _touchSupported = value;
        }
    }


    static public AccelerationEvent GetAccelerationEvent(int index)
    {
        return Input.GetAccelerationEvent(index);
    }

    static public float GetAxis(string axisName)
    {
        return Input.GetAxis(axisName);
    }

    static public float GetAxisRaw(string axisName)
    {
        return Input.GetAxisRaw(axisName);
    }

    static public bool GetButton(string buttonName)
    {
        return Input.GetButton(buttonName);
    }

    public static bool GetButtonDown(string buttonName)
    {
        return Input.GetButtonDown(buttonName);
    }

    public static bool GetButtonUp(string buttonName)
    {
        return Input.GetButtonUp(buttonName);
    }

    public static string[] GetJoystickNames()
    {
        return Input.GetJoystickNames();
    }

    public static bool GetKey(string name)
    {
        return Input.GetKey(name);
        //return keysPressed.Contains((KeyCode)System.Enum.Parse(typeof(KeyCode), name));
    }

    public static bool GetKey(KeyCode key)
    {
        //return Input.GetKey(key);
        return keysPressed.Contains(key);
    }

    public static bool GetKeyDown(string name)
    {
        return Input.GetKeyDown(name);
        //return keysPressedDown.Contains((KeyCode)System.Enum.Parse(typeof(KeyCode), name));
    }

    public static bool GetKeyDown(KeyCode key)
    {
        //return Input.GetKeyDown(key);
        return keysPressedDown.Contains(key);
    }

    public static bool GetKeyUp(string name)
    {
        return Input.GetKeyUp(name);
        //return keysPressedUp.Contains((KeyCode)System.Enum.Parse(typeof(KeyCode), name));
    }

    public static bool GetKeyUp(KeyCode key)
    {
        //return Input.GetKeyUp(key);
        return keysPressedUp.Contains(key);
    }

    public static bool GetMouseButton(int button)
    {
        //return Input.GetMouseButton(button);
        return mouseButtonsPressed[button];
    }

    public static bool GetMouseButtonDown(int button)
    {
        //return Input.GetMouseButtonDown(button);
        return mouseButtonsPressedDown[button];
    }

    public static bool GetMouseButtonUp(int button)
    {
        //return Input.GetMouseButtonUp(button);
        return mouseButtonsPressedUp[button];
    }

    public static Touch GetTouch(int index)
    {
        return touches[index];
        //return Input.GetTouch(index);
    }

#if UNITY_STANDALONE_LINUX
    public static bool IsJoystickPreconfigured(string joystickName)
    {
        return Input.IsJoystickPreconfigured(joystickName);
    }
#endif
    public static void ResetInputAxes()
    {
        Input.ResetInputAxes();
    }

    void Awake()
    {
        allKeys = System.Enum.GetValues(typeof(KeyCode)) as KeyCode[];
    }

    void Update()
    {
        _acceleration = Input.acceleration;
        _accelerationEventCount = Input.accelerationEventCount;
        _accelerationEvents = Input.accelerationEvents;
        _anyKey = Input.anyKey;
        _anyKeyDown = Input.anyKeyDown;
        _backButtonLeavesApp = Input.backButtonLeavesApp;
        _compass = Input.compass;
        _compensateSensors = Input.compensateSensors;
        _compositionCursorPos = Input.compositionCursorPos;
        _compositionString = Input.compositionString;
        _deviceOrientation = Input.deviceOrientation;
        _gyro = Input.gyro;
        _imeCompositionMode = Input.imeCompositionMode;
        _imeIsSelected = Input.imeIsSelected;
        _inputString = Input.inputString;
        _location = Input.location;
        _mousePosition = Input.mousePosition;
        _mousePresent = Input.mousePresent;
        _mouseScrollDelta = Input.mouseScrollDelta;
        _multiTouchEnabled = Input.multiTouchEnabled;
        _simulateMouseWithTouches = Input.simulateMouseWithTouches;
        _stylusTouchSupported = Input.stylusTouchSupported;
        _touchCount = Input.touchCount;
        _touches = Input.touches.ToList();
        _touchPressureSupported = Input.touchPressureSupported;
        _touchSupported = Input.touchSupported;

        keysPressed.Clear();
        keysPressedDown.Clear();
        keysPressedUp.Clear();
        for (int i = 0; i < allKeys.Length; i++)
        {
            if (Input.GetKey(allKeys[i]))
                keysPressed.Add(allKeys[i]);
            if (Input.GetKeyDown(allKeys[i]))
                keysPressedDown.Add(allKeys[i]);
            if (Input.GetKeyUp(allKeys[i]))
                keysPressedUp.Add(allKeys[i]);
        }

        mouseButtonsPressed = new MouseButtonsState(Input.GetMouseButton(0), Input.GetMouseButton(1), Input.GetMouseButton(2));
        mouseButtonsPressedDown = new MouseButtonsState(Input.GetMouseButtonDown(0), Input.GetMouseButtonDown(1), Input.GetMouseButtonDown(2));
        mouseButtonsPressedUp = new MouseButtonsState(Input.GetMouseButtonUp(0), Input.GetMouseButtonUp(1), Input.GetMouseButtonUp(2));
    }

    /*private void LateUpdate()
    {
        print("P: " + mouseButtonsPressed + "  D: " + mouseButtonsPressedDown + "  U: " + mouseButtonsPressedUp);
    }*/
}

public static class InputExtensions
{
    static public Touch Clone(this Touch touch)
    {
        Touch ret = new Touch();

        ret.altitudeAngle = touch.altitudeAngle;
        ret.azimuthAngle = touch.azimuthAngle;
        ret.deltaPosition = touch.deltaPosition;
        ret.deltaTime = touch.deltaTime;
        ret.fingerId = touch.fingerId;
        ret.maximumPossiblePressure = touch.maximumPossiblePressure;
        ret.phase = touch.phase;
        ret.position = touch.position;
        ret.pressure = touch.pressure;
        ret.radius = touch.radius;
        ret.radiusVariance = touch.radiusVariance;
        ret.rawPosition = touch.rawPosition;
        ret.tapCount = touch.tapCount;
        ret.type = touch.type;

        return ret;
    }
}
