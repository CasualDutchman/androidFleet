using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

public enum Resource { Wheat, Wine, Fruit, Tools, Tobacco }
public enum Location { World, N_America, S_America, Europe, W_Africa, S_Africa, Indian_Ocean }

[RequireComponent(typeof(FleetManager))]
[RequireComponent(typeof(RouteManager))]
public class Manager : MonoBehaviour {

    public static Manager instance;

    FleetManager fleetmanager;
    RouteManager routemanager;

    public int coin;
    public int wheat, wine, fruit, tools, tobacco;

    [Header("UI")]
    public Text coinText;
    public Text wheatText;
    public Text wineText;
    public Text fruitText;
    public Text toolsText;
    public Text tobaccoText;

    void Awake() {
        instance = this;
        fleetmanager = GetComponent<FleetManager>();
        routemanager = GetComponent<RouteManager>();
        DontDestroyOnLoad(gameObject);
    }

    void Start () {
        Load();
        fleetmanager.Load();
        routemanager.Load();
        UpdateTexts();
    }
	
    /// <summary>
    /// Buy food.
    /// </summary>
    /// <param name="code">The food type to be sold</param>
    /// <param name="value">Amount of food bought</param>
    /// <param name="cost">the cost of the food</param>
    /// <returns>True if transaction was able to work</returns>
    public bool Buy(Resource code, int value, int cost) {
        if (!HasEnoughCoin(cost)) {
            return false;
        }

        bool b = false;

        if (code == Resource.Wheat) {
            wheat += value;
            coin -= cost;

            b = true;
        }
        else if (code == Resource.Wine) {
            wine += value;
            coin -= cost;

            b = true;
        } 
        else if (code == Resource.Fruit) {
            fruit += value;
            coin -= cost;

            b = true;
        } 
        else if (code == Resource.Tools) {
            tools += value;
            coin -= cost;

            b = true;
        } 
        else if (code == Resource.Tobacco) {
            tobacco += value;
            coin -= cost;

            b = true;
        }

        if(b)
            UpdateTexts();

        return false;
    }

    /// <summary>
    /// Sell food.
    /// </summary>
    /// <param name="code">The food type to be sold</param>
    /// <param name="value">Amount of food sold</param>
    /// <param name="cost">coin gained of the food</param>
    /// <returns>True if transaction was able to work</returns>
    public bool Sell(Resource code, int value, int gain) {
        bool b = false;

        if (code == Resource.Wheat) {
            if (wheat < value)
                return false;

            wheat -= value;
            coin += gain;

            b = true;
        } 
        else if (code == Resource.Wine) {
            if (wine < value)
                return false;

            wine -= value;
            coin += gain;

            b = true;
        } 
        else if (code == Resource.Fruit) {
            if (fruit < value)
                return false;

            fruit -= value;
            coin += gain;

            b = true;
        } 
        else if (code == Resource.Tools) {
            if (tools < value)
                return false;

            tools -= value;
            coin += gain;

            b = true;
        } 
        else if (code == Resource.Tobacco) {
            if (tobacco < value)
                return false;

            tobacco -= value;
            coin += gain;

            b = true;
        }

        if (b)
            UpdateTexts();

        return false;
    }

    /// <summary>
    /// Update all TopBanner text components
    /// </summary>
    void UpdateTexts() {
        coinText.text = coin + "C";
        wheatText.text = wheat.ToString("F0");
        wineText.text = wine.ToString("F0");
        fruitText.text = fruit.ToString("F0");
        toolsText.text = tools.ToString("F0");
        tobaccoText.text = tobacco.ToString("F0");
    }

    /// <summary>
    /// Check if player has enough coin
    /// </summary>
    /// <param name="_coin">the amount of coind</param>
    /// <returns>True if player has the amount of coin or more</returns>
    public bool HasEnoughCoin(int _coin) {
        return coin >= _coin;
    }

    /// <summary>
    /// Load from the PlayerPrefs system
    /// </summary>
    void Load() {
        if (!PlayerPrefs.HasKey("items")) {
            coin = 1000;
            wheat = 10;
            wine = 0;
            fruit = 0;
            tools = 0;
            tobacco = 0;
        } 
        else {
            string[] s = PlayerPrefs.GetString("items").Split('/');
            coin = int.Parse(s[0]);
            wheat = int.Parse(s[1]);
            wine = int.Parse(s[2]);
            fruit = int.Parse(s[3]);
            tools = int.Parse(s[4]);
            tobacco = int.Parse(s[5]);
        }
    }

    /// <summary>
    /// Save to the playerPrefs system
    /// </summary>
    void Save() {
        string s = "";
        s += coin + "/";
        s += wheat + "/";
        s += wine + "/";
        s += fruit + "/";
        s += tools + "/";
        s += tobacco;

        PlayerPrefs.SetString("items", s);
    }

    /// <summary>
    /// When the player quits the game
    /// </summary>
    public void OnQuit() {
        Save();
        fleetmanager.Save();
        routemanager.Save();
        PlayerPrefs.Save();

        Application.Quit();
    }

    void Update() {

    }
}
#region Item class and drawer

[System.Serializable]
public struct Item {
    public Resource item;
    public int amount;
    public Item(Resource _item, int _amount) {
        item = _item;
        amount = _amount;
    }
}

[CustomPropertyDrawer(typeof(Item))]
public class ItemDrawer : PropertyDrawer {
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
        EditorGUI.BeginProperty(position, label, property);

        position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

        var indent = EditorGUI.indentLevel;
        EditorGUI.indentLevel = 0;

        var itemTypeRect = new Rect(position.x, position.y, 100, position.height);
        var amountRect = new Rect(position.x + 110, position.y, 30, position.height);

        EditorGUI.PropertyField(itemTypeRect, property.FindPropertyRelative("item"), GUIContent.none);
        EditorGUI.PropertyField(amountRect, property.FindPropertyRelative("amount"), GUIContent.none);

        EditorGUI.indentLevel = indent;

        EditorGUI.EndProperty();
    }
}

#endregion