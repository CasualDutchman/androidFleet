using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FleetManager : MonoBehaviour {

    public Sprite shippy;

    public List<Ship> ships = new List<Ship>();
    List<Transform> shipGraphics = new List<Transform>();

    float timer;

    void Update() {
        timer += Time.deltaTime;
        if (timer >= 1) {
            for (int i = 0; i < ships.Count; i++) {
                Ship ship = ships[i];
                Transform shipTransform = shipGraphics[i];

                ship.currentTime.RemoveSecond();

                float percentDone = ship.currentTime.Percentage(ship.maxTime);
                Port origin = RouteManager.instance.GetPort(ship.originPort);
                Port endPoint = RouteManager.instance.GetPort(ship.destinationPort);

                Vector3 newPos = Vector3.Lerp(origin.transform.position, endPoint.transform.position, 1f - percentDone);
                newPos.z = -0.02f;
                shipTransform.position = newPos;
            }
            timer -= 1;
        }
    }

    void LoadGraphics() {
        foreach (Ship ship in ships) {

            GameObject currentGraphic = new GameObject();
            currentGraphic.name = ship.name;

            Port p = RouteManager.instance.GetPort(ship.originPort);
            currentGraphic.transform.position = p.transform.position + new Vector3(0, 0, -0.01f);

            if (ship.currentTime.HasTime()) {
                float percentDone = ship.currentTime.Percentage(ship.maxTime);
                Port origin = RouteManager.instance.GetPort(ship.originPort);
                Port endPoint = RouteManager.instance.GetPort(ship.destinationPort);

                Vector3 newPos = Vector3.Lerp(origin.transform.position, endPoint.transform.position, 1f - percentDone);
                newPos.z = -0.02f;
                currentGraphic.transform.position = newPos;
            }

            SpriteRenderer spriteRenderer = currentGraphic.AddComponent<SpriteRenderer>();
            spriteRenderer.sprite = shippy;

            shipGraphics.Add(currentGraphic.transform);
        }
    }

    public void Save() {
        int i = 0;
        foreach (Ship ship in ships) {
            string s = "";
            s += (int)ship.type + "/";
            s += ship.name + "/";
            s += ship.maxHitPoints + "/";
            s += ship.hitPoints + "/";
            s += ship.attackDamage + "/";
            s += ship.speed + "/";
            s += ship.cargoSize;

            PlayerPrefs.SetString("Ship" + i, s);
            i++;
        }
        PlayerPrefs.SetInt("ShipCount", i);
    }

    public void Load() {
        if (!PlayerPrefs.HasKey("ShipCount")) {
            Ship ship = new Ship() {
                type = ShipType.Schooner,
                name = "Saint Lois",
                maxHitPoints = 100,
                hitPoints = 100,
                attackDamage = 10,
                speed = 2,
                cargoSize = 20,
                originPort = Ports.New_Amsterdam,
                destinationPort = Ports.Amsterdam,
                maxTime = new TimeScale(1, 0, 0),
                currentTime = new TimeScale(42, 0)
            };

            ships.Add(ship);

            LoadGraphics();
            return;
        }

        int shipcount = PlayerPrefs.GetInt("ShipCount");

        for (int i = 0; i < shipcount; i++) {
            string[] s = PlayerPrefs.GetString("Ship" + i).Split('/');

            Ship ship = new Ship() {
                type = (ShipType)int.Parse(s[0]),
                name = s[1],
                maxHitPoints = float.Parse(s[2]),
                hitPoints = float.Parse(s[3]),
                attackDamage = float.Parse(s[4]),
                speed = float.Parse(s[5]),
                cargoSize = int.Parse(s[6])
            };

            ships.Add(ship);
        }

        LoadGraphics();
    }
}

public enum ShipType { Schooner, Brig, Frigate, Galleon }

[System.Serializable]
public class Ship {
    public ShipType type;
    public string name;
    public float maxHitPoints;
    public float hitPoints;
    public float attackDamage;
    public float speed;
    public int cargoSize;

    //Mission related
    public Ports originPort;
    public Ports destinationPort;
    public TimeScale maxTime;
    public TimeScale currentTime;
    public Item[] cargo;
}
