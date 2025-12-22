using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraAxisController : MonoBehaviour
{
    public Transform player;
    private float currentAxisX = 0;
    private void Start()
    {
        currentAxisX = player.position.x;
    }

    private void LateUpdate()
    {
        if(Mathf.Abs(transform.position.x - player.position.x) > 10f) WarpToPlayer();
        else transform.position = new Vector3(currentAxisX, transform.position.y, player.position.z);

    }

    public void WarpToPlayer()
    {
        currentAxisX = player.position.x;
        transform.position = new Vector3(currentAxisX, transform.position.y, player.position.z);
    }
}
