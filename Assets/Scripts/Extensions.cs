using UnityEngine;

public static class Extensions
{
    /// <summary>
    /// Returns copy of tab array with obj element addon at the end of array.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="tab">Source array</param>
    /// <param name="obj">Element to add to copy</param>
    /// <param name="checkIfAlreadyInArray">If true, allow more occurencies of element in array.</param>
    /// <returns>Copy of source array with obj element at the end.</returns>
    public static T[] Add<T>(this T[] tab, T obj, bool checkIfAlreadyInArray = false)
    {
        if (checkIfAlreadyInArray)
        {
            for (int i = 0; i < tab.Length; i++)
            {
                if (tab[i].Equals(obj)) return tab;
            }
        }
        T[] newTab = new T[tab.Length + 1];
        for (int i = 0; i < tab.Length; i++)
        {
            newTab[i] = tab[i];
        }
        newTab[newTab.Length - 1] = obj;
        return newTab;
    }

    /// <summary>
    /// Returns copy of tab array without obj element.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="tab">Source array</param>
    /// <param name="obj">Element to ignore</param>
    /// <returns>Copy of source array without obj element.</returns>
    public static T[] Remove<T>(this T[] tab, T obj)
    {
        int elementCount = 0;
        for (int i = 0; i < tab.Length; i++)
        {
            if (tab[i].Equals(obj)) elementCount++;
        }
        T[] newTab = new T[tab.Length - elementCount];
        int j = 0;
        for (int i = 0; i < tab.Length; i++)
        {
            if (!tab[i].Equals(obj))
            {
                newTab[j] = tab[i];
                j++;
            }
        }
        //newTab[newTab.Length - 1] = obj;
        return newTab;
    }


    public static float CalculateAlphaValue(this Vector3 center, Vector3 point)
    {
        if (center == point) return float.PositiveInfinity;
        Vector3 pointRelative = point - center;
        float ret = -1;
        if (pointRelative.x >= 0 && pointRelative.z >= 0)
        {
            ret = pointRelative.z / (Vector3.Distance(center, point));
        }
        else if (pointRelative.x < 0 && pointRelative.z >= 0)
        {
            ret = 2 - (pointRelative.z / (Vector3.Distance(center, point)));
        }
        else if (pointRelative.x < 0 && pointRelative.z < 0)
        {
            ret = 2 + (System.Math.Abs(pointRelative.z) / (Vector3.Distance(center, point)));
        }
        else if (pointRelative.x >= 0 && pointRelative.z < 0)
        {
            ret = 4 - (System.Math.Abs(pointRelative.z) / (Vector3.Distance(center, point)));
        }
        return ret;
    }


    public static float CalculateDeterminant(this Vector3 a, Vector3 b, Vector3 c)
    {
        return a.x * b.z + a.z * c.x + b.x * c.z - (b.z * c.x + a.x * c.z + a.z * b.x);
    }


    public static int RoundToClosestInt(this float toRound)
    {
        float ceiling = Mathf.Ceil(toRound);
        float floor = Mathf.Floor(toRound);

        if (ceiling - toRound > toRound - floor)
        {
            return (int)floor;
        }
        return (int)ceiling;
    }

    public static float RoundToClosestFloat(this float toRound, int precision = 1)
    {
        float tempToRound = toRound;
        for (int i = 0; i < precision; i++)
        {
            tempToRound *= 10;
        }
        float ceiling = Mathf.Ceil(tempToRound);
        float floor = Mathf.Floor(tempToRound);

        if (ceiling - tempToRound > tempToRound - floor)
        {
            tempToRound = floor;
        }
        else {
            tempToRound = ceiling;
        }

        for (int i = 0; i < precision; i++)
        {
            tempToRound *= 0.1f;
        }
        return tempToRound;
    }


    static public void SetObjectVisibility(this GameObject targetObject, bool visibility)
    {
        if (targetObject != null)
            SetVisibility(targetObject, visibility);
    }

    static private void SetVisibility(GameObject targetObject, bool visibility)
    {
        if (targetObject.GetComponent<Renderer>() != null) targetObject.GetComponent<Renderer>().enabled = visibility;
        for (int i = 0; i < targetObject.transform.childCount; i++)
        {
            SetVisibility(targetObject.transform.GetChild(i).gameObject, visibility);
        }
        return;
    }

    /*static public Vector3 ParseVector3(string vector)
    {
        vector = vector.Replace("(", "");
        vector = vector.Replace(")", "");
        string[] vectortext = vector.Split(',');
        Vector3 ret = Vector3.zero;
        ret.x = (float)System.Convert.ChangeType(vectortext[0], typeof(float));
        ret.y = (float)System.Convert.ChangeType(vectortext[1], typeof(float));
        ret.z = (float)System.Convert.ChangeType(vectortext[2], typeof(float));
        return ret;
    }
    */

    static public void RemoveComponentIncludingChildren<T>(this GameObject gameObject) where T : UnityEngine.Object
    {
        gameObject.RemoveComponent<T>();
        for (int i = 0; i < gameObject.transform.childCount; i++)
        {
            if (gameObject.transform.GetChild(i))
            {
                gameObject.transform.GetChild(i).gameObject.RemoveComponentIncludingChildren<T>();
            }
        }
        return;
    }

    static public void RemoveComponent<T>(this GameObject gameObject) where T : UnityEngine.Object
    {
        T component = gameObject.GetComponent<T>();
        if (component != null) MonoBehaviour.Destroy(component);
    }


    public static Vector3 ParseVector3(string str)
    {
        Vector3 ret = Vector3.zero;
        char[] toTrim = { ' ', '(', ')' };
        string[] splittedLine = str.Trim(toTrim).Split(',');
        ret.x = float.Parse(splittedLine[0].Trim(toTrim));
        ret.y = float.Parse(splittedLine[1].Trim(toTrim));
        ret.z = float.Parse(splittedLine[2].Trim(toTrim));
        return ret;
    }

    public static Vector2 ParseVector2(string str)
    {
        Vector2 ret = Vector2.zero;
        char[] toTrim = { ' ', '(', ')' };
        string[] splittedLine = str.Trim(toTrim).Split(',');
        ret.x = float.Parse(splittedLine[0].Trim(toTrim));
        ret.y = float.Parse(splittedLine[1].Trim(toTrim));
        return ret;
    }
}