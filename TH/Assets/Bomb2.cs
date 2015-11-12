using UnityEngine;
using System.Collections;

public class Bomb2 : MonoBehaviour {
    public string bombName;
    private string privateBombName;

    // Use this for initialization
    void Start()
    {
        Debug.Log("Bomb2 Start !!" + bombName.ToString());
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log("Bomb2 update !!");
    }

    void BombSound() {
        Debug.Log("Bomb 2 sound !!");
    }
}
