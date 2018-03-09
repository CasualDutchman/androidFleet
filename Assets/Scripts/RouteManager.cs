using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RouteManager : MonoBehaviour {

    private static String[] portNames = new string[] { "New Amsterdam", "Charleston", "Havana", "Parimaribo", "Salvador", "Buenos Aires", "Porto", "Brest",
                    "Amsterdam", "Cadiz", "Casablanca", "Cape Coast", "Dakar", "Capetown", "Zanzibar", "Colombo", "Mumbai", "Batavia", "Makassar", "Hirado" };

    public Port[] ports;

    public int currentPortIndex = 0;

    Transform current;
    Transform working;

    public TextMeshProUGUI[] nameTexts;

    public Transform cameraTrans;

    void Start() {
        for (int i = 0; i < nameTexts.Length; i++) {
            nameTexts[i].text = i > 1 ? portNames[i - 2] : "";
        }
        current = ports[0].transform;
    }

    void Update() {

    }

    public void DownOnIndex() {
        currentPortIndex++;
        if (currentPortIndex >= ports.Length)
            currentPortIndex = 0;

        for (int i = 0; i < nameTexts.Length; i++) {
            nameTexts[i].text = i + currentPortIndex - 2 >= 0 && i + currentPortIndex - 2 < portNames.Length ? portNames[i + currentPortIndex - 2] : "";
        }

        StartCoroutine(IEChangeLocation(ports[currentPortIndex].transform.position));
    }

    public void UpOnIndex() {
        currentPortIndex--;
        if (currentPortIndex < 0)
            currentPortIndex = ports.Length - 1;

        for (int i = 0; i < nameTexts.Length; i++) {
            nameTexts[i].text = i + currentPortIndex - 2 >= 0 && i + currentPortIndex - 2 < portNames.Length ? portNames[i + currentPortIndex - 2] : "";
        }

        StartCoroutine(IEChangeLocation(ports[currentPortIndex].transform.position));
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
