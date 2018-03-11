using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Port : MonoBehaviour {

    public string portname;
    public float reputation;
    public Ports ownPort;
    public Transform[] connectedPort;

    public Mission[] missions;

    [ContextMenu("Test")]
    public void Test() {
        Collider2D[] all = Physics2D.OverlapCircleAll(transform.position, 3, LayerMask.GetMask("Water"));
        List<Transform> l = new List<Transform>();
        for (int i = 0; i < all.Length; i++) {
            if(all[i].transform != transform)
                l.Add(all[i].transform);
        }
        connectedPort = l.ToArray();
    }

    void OnDrawGizmosSelected() {
        Gizmos.color = Color.cyan;
        for (int i = 0; i < connectedPort.Length; i++) {
            Gizmos.DrawLine(transform.position, connectedPort[i].position);
        }
    }
}
