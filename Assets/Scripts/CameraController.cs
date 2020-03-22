using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraController : MonoBehaviour
{
    public GameObject player;

    Transform playerT;
    Transform transform;

    public BoxCollider2D bound;

    private Vector3 minBound;
    private Vector3 maxBound;

    private float halfWidth;
    private float halfHeight;

    private Camera theCamera;

    private void Awake()
    {
        transform = GetComponent<Transform>();
        playerT = player.transform;
    }

    // Start is called before the first frame update
    void Start()
    {
        theCamera = GetComponent<Camera>();
        minBound = bound.bounds.min;
        maxBound = bound.bounds.max;
        halfHeight = theCamera.orthographicSize;
        halfWidth = halfHeight * Screen.width / Screen.height;// 해상도
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(playerT.position.x + 1, playerT.position.y + 2.5f, playerT.position.z - 10);

        float clampedX = Mathf.Clamp(this.transform.position.x, minBound.x + halfWidth, maxBound.x - halfWidth);  //Clamp('값', '최솟값', '최댓값') 값이 무조건 최솟값 최댓값 사이만 나옴
        float clampedY = Mathf.Clamp(this.transform.position.y, minBound.y + halfHeight, maxBound.y - halfHeight);

        this.transform.position = new Vector3(clampedX, clampedY, this.transform.position.z);
    }
}
