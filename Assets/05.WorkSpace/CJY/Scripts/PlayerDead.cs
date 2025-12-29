using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDead : MonoBehaviour
{
    private void OnDisable()
    {
        GameManager.PlayerisDead();
    }
}
