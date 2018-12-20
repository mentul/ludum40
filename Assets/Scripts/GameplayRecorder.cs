using System.Collections.Generic;
using UnityEngine;

public struct IndexedData<T>
{
    public int index;
    public T data;

    public IndexedData(int ind, T dat)
    {
        index = ind;
        data = dat;
    }

    public void SetData(T newData)
    {
        data = newData;
    }
}
public struct IndexedDataList<T>
{
    List<IndexedData<T>> _list;
    List<IndexedData<T>> list
    {
        get
        {
            if (_list == null) _list = new List<IndexedData<T>>();
            return _list;
        }
        set
        {
            _list = value;
        }
    }

    List<int> _indexes;
    List<int> indexes
    {
        get
        {
            if (_indexes == null) _indexes = new List<int>();
            return _indexes;
        }
        set
        {
            _indexes = value;
        }
    }

    public IndexedDataList(List<IndexedData<T>> argList = null)
    {
        if (argList == null) _list = new List<IndexedData<T>>();
        else _list = argList;
        _indexes = new List<int>();
        UpdateIndexes();
    }

    void UpdateIndexes()
    {
        indexes.Clear();
        foreach (IndexedData<T> id in list)
        {
            indexes.Add(id.index);
        }
    }

    IndexedData<T> GetIndexedDataByIndex(int index)
    {
        for (int i = 0; i < list.Count; i++)
        {
            if (list[i].index == index) return list[i];
        }
        return default(IndexedData<T>);
    }

    void SetIndexedDataByIndex(int index, T data)
    {
        for (int i = 0; i < list.Count; i++)
        {
            if (list[i].index == index) list[i].SetData(data);
        }
    }
    void RemoveIndexedDataByIndex(int index)
    {
        for (int i = 0; i < list.Count; i++)
        {
            if (list[i].index == index)
            {
                indexes.Remove(list[i].index);
                list.Remove(list[i]);
                return;
            }
        }
    }

    public void Clear()
    {
        if (list != null) list.Clear();
        if (indexes != null) indexes.Clear();
    }

    public T this[int index]
    {
        get
        {
            if (indexes.Contains(index))
            {
                for (int i = 0; i < list.Count; i++)
                {
                    if (list[i].index == index) return list[i].data;
                }
            }

            return default(T);
        }
        set
        {
            if (indexes.Contains(index))
            {
                if (value == null)
                {
                    RemoveIndexedDataByIndex(index);
                }
                else
                {
                    SetIndexedDataByIndex(index, value);
                }
            }
            else
            {
                if (value != null)
                {
                    list.Add(new IndexedData<T>(index, value));
                    indexes.Add(index);
                }
            }
        }
    }

}



[DefaultExecutionOrder(-50)]
public class GameplayRecorder : MonoBehaviour
{
    public bool loopPlayback = false;
    [Tooltip("Script that implements IGameDataRestorer interface")]
    public MonoBehaviour dataRestorer;
    [Tooltip("FPSCounter will start on playback start and will stop at the end of playback")]
    public FPSCounter fpsCounter;
    static public bool recording = false;
    static public bool playback = false;

    IndexedDataList<Touch[]> recordedTouches = new IndexedDataList<Touch[]>();
    IndexedDataList<List<KeyCode>> recordedKeys = new IndexedDataList<List<KeyCode>>();
    IndexedDataList<List<KeyCode>> recordedKeysDown = new IndexedDataList<List<KeyCode>>();
    IndexedDataList<List<KeyCode>> recordedKeysUp = new IndexedDataList<List<KeyCode>>();
    IndexedDataList<MouseButtonsState> recordedMouseButtonsPressed = new IndexedDataList<MouseButtonsState>();
    IndexedDataList<MouseButtonsState> recordedMouseButtonsPressedDown = new IndexedDataList<MouseButtonsState>();
    IndexedDataList<MouseButtonsState> recordedMouseButtonsPressedUp = new IndexedDataList<MouseButtonsState>();
    IndexedDataList<Vector3> recordedMousePositions = new IndexedDataList<Vector3>();

    int startFrame = 0;
    public int currentFrame = 0;
    public int recordedFrames = 0;


    void Update()
    {
        if (recording)
        {
            currentFrame = Time.frameCount - startFrame;
            recordedTouches[currentFrame] = new Touch[CustomInput.touchCount];
            for (int i = 0; i < CustomInput.touchCount; ++i)
            {
                recordedTouches[currentFrame][i] = (CustomInput.GetTouch(i)).Clone();
            }
            recordedMousePositions[currentFrame] = CustomInput.mousePosition;
            recordedKeys[currentFrame] = new List<KeyCode>(CustomInput.keysPressed);
            recordedKeysDown[currentFrame] = new List<KeyCode>(CustomInput.keysPressedDown);
            recordedKeysUp[currentFrame] = new List<KeyCode>(CustomInput.keysPressedUp);
            recordedMouseButtonsPressed[currentFrame] = CustomInput.mouseButtonsPressed;
            recordedMouseButtonsPressedDown[currentFrame] = CustomInput.mouseButtonsPressedDown;
            recordedMouseButtonsPressedUp[currentFrame] = CustomInput.mouseButtonsPressedUp;

            if (Input.GetKeyDown(KeyCode.Space))
                TurnRecordingOff();
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.Space))
                TurnRecordingOn();
        }
        if (playback)
        {
            if (currentFrame >= recordedFrames - 1)
            {
                if (loopPlayback)
                {
                    startFrame = Time.frameCount;
                }
                else
                {
                    TurnPlaybackOff();
                    return;
                }
            }

            currentFrame = Time.frameCount - startFrame;
            CustomInput.mousePosition = recordedMousePositions[currentFrame];
            CustomInput.touches = recordedTouches[currentFrame] == null ? null : new List<Touch>(recordedTouches[currentFrame]);
            CustomInput.keysPressed = recordedKeys[currentFrame] == null ? null : new List<KeyCode>(recordedKeys[currentFrame]);
            CustomInput.keysPressedDown = recordedKeysDown[currentFrame] == null ? null : new List<KeyCode>(recordedKeysDown[currentFrame]);
            CustomInput.keysPressedUp = recordedKeysUp[currentFrame] == null ? null : new List<KeyCode>(recordedKeysUp[currentFrame]);
            CustomInput.mouseButtonsPressed = new MouseButtonsState(recordedMouseButtonsPressed[currentFrame]);
            CustomInput.mouseButtonsPressedDown = new MouseButtonsState(recordedMouseButtonsPressedDown[currentFrame]);
            CustomInput.mouseButtonsPressedUp = new MouseButtonsState(recordedMouseButtonsPressedUp[currentFrame]);

            if (Input.GetKeyDown(KeyCode.Return))
                TurnPlaybackOff();
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.Return))
                TurnPlaybackOn();
        }
    }

    void OnGUI()
    {
        GUI.TextArea(new Rect(Screen.width * 0.5f - 50, Screen.height-20, 100, 20), "Frame: " + (currentFrame + 1) + "/" + recordedFrames);
        if (GUI.Button(new Rect(0, 0, 100, 30), "Record"))
        {
            if (!recording)
                TurnRecordingOn();
            else
                TurnRecordingOff();
        }
        if (GUI.Button(new Rect(Screen.width-100, 0, 100, 30), "Playback"))
        {
            if (!playback)
                TurnPlaybackOn();
            else
                TurnPlaybackOff();
        }
    }
    public void TurnRecordingOn()
    {
        recordedFrames = 0;
        recordedMousePositions.Clear();
        recordedTouches.Clear();
        recordedKeys.Clear();
        recordedKeysDown.Clear();
        recordedKeysUp.Clear();
        recordedMouseButtonsPressed.Clear();
        recordedMouseButtonsPressedDown.Clear();
        recordedMouseButtonsPressedUp.Clear();
        startFrame = Time.frameCount;
        recording = true;
        playback = false;
        if (dataRestorer as IGameDataRestorer != null) (dataRestorer as IGameDataRestorer).StoreGameData();
    }
    public void TurnRecordingOff()
    {
        recordedFrames = currentFrame;
        recording = false;
        playback = false;
        SaveRecordedData();
    }
    public void TurnPlaybackOn()
    {
        LoadRecordedData();
        startFrame = Time.frameCount;
        currentFrame = Time.frameCount - startFrame;
        playback = true;
        recording = false;
        if (dataRestorer as IGameDataRestorer != null) (dataRestorer as IGameDataRestorer).RestoreGameData();
        fpsCounter.StopCounter();
        fpsCounter.ResetCounter();
        fpsCounter.StartCounter();
    }
    public void TurnPlaybackOff()
    {
        playback = false;
        recording = false;
        fpsCounter.StopCounter();
    }

    string gameplayRecordFile = "Gameplay.gpr";
    public void SaveRecordedData()
    {
        string filepath = Application.persistentDataPath + '/' + gameplayRecordFile;
        List<string> str = new List<string>();

        str.Add(recordedFrames.ToString());

        for (int i = 0; i < recordedFrames; ++i)
        {
            if (recordedTouches[i] != null)
            {
                str.Add(recordedTouches[i].Length.ToString());
                for (int j = 0; j < recordedTouches[i].Length; ++j)
                {
                    str.Add(
                        recordedTouches[i][j].altitudeAngle + ";" +
                        recordedTouches[i][j].azimuthAngle + ";" +
                        recordedTouches[i][j].deltaPosition + ";" +
                        recordedTouches[i][j].deltaTime + ";" +
                        recordedTouches[i][j].fingerId + ";" +
                        recordedTouches[i][j].maximumPossiblePressure + ";" +
                        recordedTouches[i][j].phase + ";" +
                        recordedTouches[i][j].position + ";" +
                        recordedTouches[i][j].pressure + ";" +
                        recordedTouches[i][j].radius + ";" +
                        recordedTouches[i][j].radiusVariance + ";" +
                        recordedTouches[i][j].rawPosition + ";" +
                        recordedTouches[i][j].tapCount + ";" +
                        recordedTouches[i][j].type
                        );
                }
            }
            else str.Add("0");

            if (recordedKeys[i] != null)
            {
                str.Add(recordedKeys[i].Count.ToString());
                for (int j = 0; j < recordedKeys[i].Count; ++j)
                {
                    str.Add(recordedKeys[i][j].ToString());
                }
            }
            else str.Add("0");

            if (recordedKeysDown[i] != null)
            {
                str.Add(recordedKeysDown[i].Count.ToString());
                for (int j = 0; j < recordedKeysDown[i].Count; ++j)
                {
                    str.Add(recordedKeysDown[i][j].ToString());
                }
            }
            else str.Add("0");

            if (recordedKeysUp[i] != null)
            {
                str.Add(recordedKeysUp[i].Count.ToString());
                for (int j = 0; j < recordedKeysUp[i].Count; ++j)
                {
                    str.Add(recordedKeysUp[i][j].ToString());
                }
            }
            else str.Add("0");

            str.Add(recordedMouseButtonsPressed[i].ToString());
            str.Add(recordedMouseButtonsPressedDown[i].ToString());
            str.Add(recordedMouseButtonsPressedUp[i].ToString());
            str.Add(recordedMousePositions[i].ToString());

        }


        try
        {
            System.IO.File.WriteAllLines(filepath, str.ToArray());
        }
        catch (System.Exception e)
        {
            print(e.Message + "; " + e.StackTrace + "\n");
        }
    }

    public void LoadRecordedData()
    {
        string filepath = Application.persistentDataPath + '/' + gameplayRecordFile;
        if (!System.IO.File.Exists(filepath)) return;

        string[] fileLines = System.IO.File.ReadAllLines(filepath);
        int lineIndex = 0;
        string line = fileLines[lineIndex].Trim(' ');
        recordedFrames = int.Parse(line);

        for (int i = 0; i < recordedFrames; ++i)
        {
            line = fileLines[++lineIndex].Trim(' ');
            int touchesLength = int.Parse(line);
            recordedTouches[i] = new Touch[touchesLength];
            for (int j = 0; j < touchesLength; ++j)
            {
                line = fileLines[++lineIndex].Trim(' ');
                string[] splittedLine = line.Split(';');

                recordedTouches[i][j].altitudeAngle = float.Parse(splittedLine[0]);
                recordedTouches[i][j].azimuthAngle = float.Parse(splittedLine[1]);
                recordedTouches[i][j].deltaPosition = Extensions.ParseVector2(splittedLine[2]);
                recordedTouches[i][j].deltaTime = float.Parse(splittedLine[3]);
                recordedTouches[i][j].fingerId = int.Parse(splittedLine[4]);
                recordedTouches[i][j].maximumPossiblePressure = float.Parse(splittedLine[5]);
                recordedTouches[i][j].phase = (TouchPhase)System.Enum.Parse(typeof(TouchPhase), splittedLine[6]);
                recordedTouches[i][j].position = Extensions.ParseVector2(splittedLine[7]);
                recordedTouches[i][j].pressure = float.Parse(splittedLine[8]);
                recordedTouches[i][j].radius = float.Parse(splittedLine[9]);
                recordedTouches[i][j].radiusVariance = float.Parse(splittedLine[10]);
                recordedTouches[i][j].rawPosition = Extensions.ParseVector2(splittedLine[11]);
                recordedTouches[i][j].tapCount = int.Parse(splittedLine[12]);
                recordedTouches[i][j].type = (TouchType)System.Enum.Parse(typeof(TouchType), splittedLine[13]);

            }

            line = fileLines[++lineIndex].Trim(' ');
            int recordedKeysCount = int.Parse(line);
            recordedKeys[i] = new List<KeyCode>(recordedKeysCount);
            for (int j = 0; j < recordedKeysCount; ++j)
            {
                line = fileLines[++lineIndex].Trim(' ');
                recordedKeys[i].Add((KeyCode)System.Enum.Parse(typeof(KeyCode), line));
            }

            line = fileLines[++lineIndex].Trim(' ');
            int recordedKeysDownCount = int.Parse(line);
            recordedKeysDown[i] = new List<KeyCode>(recordedKeysDownCount);
            for (int j = 0; j < recordedKeysDownCount; ++j)
            {
                line = fileLines[++lineIndex].Trim(' ');
                recordedKeysDown[i].Add((KeyCode)System.Enum.Parse(typeof(KeyCode), line));
            }

            line = fileLines[++lineIndex].Trim(' ');
            int recordedKeysUpCount = int.Parse(line);
            recordedKeysUp[i] = new List<KeyCode>(recordedKeysUpCount);
            for (int j = 0; j < recordedKeysUpCount; ++j)
            {
                line = fileLines[++lineIndex].Trim(' ');
                recordedKeysUp[i].Add((KeyCode)System.Enum.Parse(typeof(KeyCode), line));
            }

            line = fileLines[++lineIndex].Trim(' ');
            recordedMouseButtonsPressed[i] = MouseButtonsState.Parse(line);
            line = fileLines[++lineIndex].Trim(' ');
            recordedMouseButtonsPressedDown[i] = MouseButtonsState.Parse(line);
            line = fileLines[++lineIndex].Trim(' ');
            recordedMouseButtonsPressedUp[i] = MouseButtonsState.Parse(line);
            line = fileLines[++lineIndex].Trim(' ');
            recordedMousePositions[i] = Extensions.ParseVector3(line);

        }

    }

}
