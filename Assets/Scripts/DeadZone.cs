using UnityEngine;
using System.Collections;

public class DeadZone : MonoBehaviour
{

    void OnTriggerEnter()
    {
        GM.instance.LoseLife();
    }
}