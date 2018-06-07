using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraZoom : MonoBehaviour {

    public float CameraHeight = 175f;

    public Vector3 toPosition;
    public Vector3 fromPosition;

    private Camera Camera;
    private Vector3 DesiredPosition;
    private Vector3 OriginalPosition;

    private void Awake()
    {
        Camera = GetComponent<Camera>();

        OriginalPosition = transform.position;
    }

    //move the position of the camera
    public void Move()
    {
        FindPosition();

        transform.position = Vector3.Lerp(transform.position, DesiredPosition, 1);
    }

    //Find desired camera position
    private void FindPosition()
    {
        Vector3 pos = new Vector3();

        if(CountryManager.instance.attackClickCount == 0)
        {
            pos = fromPosition;
        }

        if (CountryManager.instance.attackClickCount == 1)
        {
            pos = (toPosition + fromPosition) / 2;
        }

        pos.z = pos.z - CameraHeight;

        DesiredPosition = pos;
    }

    //Reset the camera position
    public void ResetPosition()
    {
        transform.position = Vector3.Lerp(transform.position, OriginalPosition, 1);
    }
}
