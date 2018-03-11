using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using UnityEditor;

public class MissionManager : MonoBehaviour {

    Mission[] activeMissions;

    public TimeScale[] scale;

    float timer;

    void Start () {
        scale = new TimeScale[20];
        for (int i = 0; i < 20; i++) {
            scale[i] = new TimeScale(1, 0, 0);
        }
    }

    void Update() {
        timer += Time.deltaTime;
        if (timer >= 1) {
            timer -= 1;
            for (int i = 0; i < 20; i++) {
                scale[i].RemoveSecond();
            }
        }


        //UpdateTime();
    }

    void UpdateTime() {
        foreach (Mission mission in activeMissions) {

        }
    }
}

[System.Serializable]
public class Mission {
    public static Mission Wine = new Mission("We need wine", "Deliver wine to fancy people", 1000, new TimeScale(1, 0), new Item(Resource.Wine, 20));

    public string missionName;
    public string missionDesc;
    public int price;
    public TimeScale maxTime;
    public Item[] selling;

    public Mission(string name, string desc, int _price, TimeScale time, params Item[] items) {
        missionName = name;
        missionDesc = desc;
        price = _price;
        maxTime = time;
        selling = items;
    }
}

#region TimeScale class and drawer

[System.Serializable]
public struct TimeScale {
    public int days;
    public int hours;
    public int minutes;
    public int seconds;

    public TimeScale(int d, int h, int m, int s) {
        days = d;
        hours = h;
        minutes = m;
        seconds = s;
    }

    public TimeScale(int h, int m, int s) {
        days = 0;
        hours = h;
        minutes = m;
        seconds = s;
    }

    public TimeScale(int m, int s) {
        days = 0;
        hours = 0;
        minutes = m;
        seconds = s;
    }

    public TimeScale RemoveSecond() {
        return this.Subtract(new TimeScale(0, 1));
    }

    public TimeScale Subtract(TimeScale sub) {
        seconds -= sub.seconds;
        if (seconds < 0) {
            minutes -= 1;
            seconds += 60;
        }

        minutes -= sub.minutes;
        if (minutes < 0) {
            hours -= 1;
            minutes += 60;
        }

        hours -= sub.minutes;
        if (hours < 0) {
            days -= 1;
            hours += 24;
        }

        if (days < 0) {
            return new TimeScale(0, 0, 0, 0);
        }

        return this;
    }

    public int TotalSeconds() {
        return (days * 86400) + (hours * 3600) + (minutes * 60) + seconds;
    }

    public float Percentage(TimeScale other) {
        return (float)this.TotalSeconds() / (float)other.TotalSeconds() ;
    }

    public bool HasTime() {
        return days > 0 || hours > 0 || minutes > 0 || seconds > 0;
    }
}

[CustomPropertyDrawer(typeof(TimeScale))]
public class TimeScaleDrawer : PropertyDrawer {
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
        EditorGUI.BeginProperty(position, label, property);

        position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

        var indent = EditorGUI.indentLevel;
        EditorGUI.indentLevel = 0;

        var daysNameRect = new Rect(position.x, position.y, 30, position.height);
        var daysRect = new Rect(position.x + 20, position.y, 30, position.height);

        var hoursNameRect = new Rect(position.x + 35 + 20, position.y, 30, position.height);
        var hoursRect = new Rect(position.x + 35 + 40, position.y, 30, position.height);

        var minutesNameRect = new Rect(position.x + 70 + 40, position.y, 30, position.height);
        var minutesRect = new Rect(position.x + 70 + 60, position.y, 30, position.height);

        var secondsNameRect = new Rect(position.x + 105 + 60, position.y, 30, position.height);
        var secondsRect = new Rect(position.x + 105 + 80, position.y, 30, position.height);

        EditorGUI.LabelField(daysNameRect, "D:");
        EditorGUI.PropertyField(daysRect, property.FindPropertyRelative("days"), GUIContent.none);
        EditorGUI.LabelField(hoursNameRect, "H:");
        EditorGUI.PropertyField(hoursRect, property.FindPropertyRelative("hours"), GUIContent.none);
        EditorGUI.LabelField(minutesNameRect, "M:");
        EditorGUI.PropertyField(minutesRect, property.FindPropertyRelative("minutes"), GUIContent.none);
        EditorGUI.LabelField(secondsNameRect, "S:");
        EditorGUI.PropertyField(secondsRect, property.FindPropertyRelative("seconds"), GUIContent.none);

        EditorGUI.indentLevel = indent;

        EditorGUI.EndProperty();
    }
}

#endregion
