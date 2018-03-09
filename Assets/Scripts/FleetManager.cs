using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FleetManager : MonoBehaviour {

    public List<Ship> ships = new List<Ship>();

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
        if (!PlayerPrefs.HasKey("ShipCount"))
            return;

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
}
