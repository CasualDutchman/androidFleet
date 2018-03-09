using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    Location currentLocation = Location.World;
    Location workingLocation = Location.World;

    public Transform[] locations;
    Transform current;
    Transform working;
    bool relocating = false;

    public Transform cameraTransform;

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
        current = locations[0];
        UpdateTexts();
    }
	
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

    void UpdateTexts() {
        coinText.text = coin + "C";
        wheatText.text = wheat.ToString("F0");
        wineText.text = wine.ToString("F0");
        fruitText.text = fruit.ToString("F0");
        toolsText.text = tools.ToString("F0");
        tobaccoText.text = tobacco.ToString("F0");
    }

    public bool HasEnoughCoin(int c) {
        return coin >= c;
    }

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

    public void OnQuit() {
        Save();
        fleetmanager.Save();
        routemanager.Save();
        PlayerPrefs.Save();

        Application.Quit();
    }

    void Update() {
        if (Input.GetMouseButtonDown(0)) {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

            if (hit.collider != null) {
                if (currentLocation == Location.World) {
                    //Location clickedLocation = (Location)System.Enum.Parse(typeof(Location), hit.collider.name);
                    //ChangeLocation(clickedLocation);
                }else {
                    Debug.Log(hit.collider.name);
                }
            }
        }
        if (Input.GetKeyDown(KeyCode.Escape) && currentLocation != Location.World) {
            ChangeLocation(Location.World);
        }
    }

    void ChangeLocation(Location loc) {
        if (currentLocation == loc || relocating)
            return;

        if (loc == Location.World) {
            working = locations[0];
            ZoomToWorld();
        }
        else if (loc == Location.N_America) {
            working = locations[1];
        } 
        else if (loc == Location.S_America) {
            working = locations[2];
        } 
        else if (loc == Location.Europe) {
            working = locations[3];
        } 
        else if (loc == Location.W_Africa) {
            working = locations[4];
        } 
        else if (loc == Location.S_Africa) {
            working = locations[5];
        } 
        else if (loc == Location.Indian_Ocean) {
            working = locations[6];
        }

        workingLocation = loc;

        relocating = true;
        StartCoroutine(IEChangeLocation());
    }

    IEnumerator IEChangeLocation() {
        float timer = 0;
        while (workingLocation != currentLocation) {
            timer += Time.deltaTime;

            cameraTransform.position = Vector3.Lerp(current.position, working.position, timer) + new Vector3(0, 0, -10);

            if (currentLocation == Location.World)
                cameraTransform.GetComponent<Camera>().orthographicSize = Mathf.Lerp(5f, 1.5f, timer);

            if (workingLocation == Location.World)
                cameraTransform.GetComponent<Camera>().orthographicSize = Mathf.Lerp(1.5f, 5f, timer);

            if (timer >= 1) {
                if (currentLocation == Location.World) {
                    ZoomedIn(workingLocation, working);
                }else {
                    ZoomedOut(currentLocation, current);
                }

                currentLocation = workingLocation;
                current = working;
            }
            yield return new WaitForEndOfFrame();
        }

        relocating = false;
    }

    void ZoomToWorld() {
        
        ContinentColliders(true);
    }

    void ZoomedOut(Location loc, Transform obj) {
        foreach (Transform child in obj) {
            if (child.GetComponent<Collider2D>()) {
                child.GetComponent<Collider2D>().enabled = false;
            }
        }
    }

    void ZoomedIn(Location loc, Transform obj) {
        
        ContinentColliders(false);
        foreach (Transform child in obj) {
            if (child.GetComponent<Collider2D>()) {
                child.GetComponent<Collider2D>().enabled = true;
            }
        }
    }

    void ContinentColliders(bool b) {
        foreach (Transform loc in locations) {
            if (loc.GetComponent<Collider2D>()) {
                loc.GetComponent<Collider2D>().enabled = b;
            }
        }
    }
}
