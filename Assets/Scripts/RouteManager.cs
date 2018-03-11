using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RouteManager : MonoBehaviour {

    public static RouteManager instance;

    public Port[] ports;

    Port missionPort;

    public int currentPortIndex = 0;
    public int currentMissionIndex = 0;

    Transform current;
    Transform working;

    public TextMeshProUGUI[] nameTexts;

    public Transform cameraTrans;

    void Awake() {
        instance = this;
    }

    void Start() {
        for (int i = 0; i < nameTexts.Length; i++) {
            nameTexts[i].text = i > 1 ? ports[i - 2].portname : "";
        }
        current = ports[0].transform;
    }

    void Update() {

    }

    public void BackToPortView() {
        missionPort = null;
        for (int i = 0; i < nameTexts.Length; i++) {
            nameTexts[i].text = i + currentPortIndex - 2 >= 0 && i + currentPortIndex - 2 < ports.Length ? ports[i + currentPortIndex - 2].portname : "";
        }
    }

    public void ShowMissions() {
        missionPort = ports[currentPortIndex];
        for (int i = 0; i < nameTexts.Length; i++) {
            nameTexts[i].text = i > 1 && i - 2 < missionPort.missions.Length ? missionPort.missions[i - 2].missionName : "";
        }
        currentMissionIndex = 0;
    }

    public Port GetPort(Ports port) {
        foreach (Port p in ports) {
            if (p.ownPort.Equals(port)) {
                return p;
            } else {
                continue;
            }
        }

        return null;
    }

    public void ChangeIndex(int index) {
        if (missionPort != null) {
            currentMissionIndex += index;
                if (currentMissionIndex >= missionPort.missions.Length)
                currentMissionIndex = 0;

            if (currentMissionIndex < 0)
                currentMissionIndex = missionPort.missions.Length - 1;

            for (int i = 0; i < nameTexts.Length; i++) {
                nameTexts[i].text = i + currentMissionIndex - 2 >= 0 && i + currentMissionIndex - 2 < missionPort.missions.Length ? missionPort.missions[i + currentMissionIndex - 2].missionName : "";
            }

        } else {
            currentPortIndex += index;
            if (currentPortIndex >= ports.Length)
                currentPortIndex = 0;

            if (currentPortIndex < 0)
                currentPortIndex = ports.Length - 1;

            for (int i = 0; i < nameTexts.Length; i++) {
                nameTexts[i].text = i + currentPortIndex - 2 >= 0 && i + currentPortIndex - 2 < ports.Length ? ports[i + currentPortIndex - 2].portname : "";
            }

            StartCoroutine(IEChangeLocation(ports[currentPortIndex].transform.position));
        }
    }

    IEnumerator IEChangeLocation(Vector3 newPos) {
        float timer = 0;
        Vector3 currentCameraPos = cameraTrans.position;
        bool b = false;

        newPos.z = -10;

        while (!b) {
            timer += Time.deltaTime;

            cameraTrans.position = Vector3.Lerp(currentCameraPos, newPos, timer);

            if (timer >= 1) {
                b = true;
            }
            yield return new WaitForEndOfFrame();
        }
        yield return null;
    }

    public void Save() {
        
    }

    public void Load() {
        
    }

}

public enum Ports { New_Amsterdam, Charleston, Havana, Parimaribo, Salvador, Buenos_Aires, Porto, Brest, Amsterdam, Cadiz, Casablanca, Cape_Coast,
                    Dakar, Capetown, Zanzibar, Colombo, Mumbai, Batavia, Makassar, Hirado }
