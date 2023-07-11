/*
 * Character controller
 */
using System;
using Unity.Netcode;
using UnityEngine;

public class PlayerController : NetworkBehaviour
{
    [SerializeField] private float speed = 3f;

    //avoid repeat calling camera.main
    private Camera _mainCamera;
    private Vector3 _mouseInput = Vector3.zero;

    private void Initialize()
    {
        _mainCamera = Camera.main;
    }

    //private void Start()
    //{
    //    Initialize(); // Call the initializer in Start()
    //}

    //private void Initialize()
    //{
    //    _mainCamera = Camera.main;
    //    if (_mainCamera == null)
    //    {
    //        Debug.LogError("Main camera is null!");
    //    }
    //}


    public override void OnNetworkSpawn()
    {
        base.OnNetworkDespawn();
        Initialize(); //call this initializer when network is spawned
    }

    private void Update()
    {

        if (!IsOwner || !Application.isFocused) return; //***for easier testing


        //movement
        _mouseInput.x = Input.mousePosition.x;
        _mouseInput.y = Input.mousePosition.y;
        _mouseInput.z = _mainCamera.nearClipPlane; //near clipping plane, where z = 0
        Vector3 mouseWorldCoordinates = _mainCamera.ScreenToWorldPoint(_mouseInput);
        transform.position = Vector3.MoveTowards(transform.position, mouseWorldCoordinates, Time.deltaTime * speed);

        //rotate
        if (mouseWorldCoordinates != transform.position)
        {
            Vector3 targetDirection = mouseWorldCoordinates - transform.position;
            targetDirection.z = 0f;
            transform.up = targetDirection;
        }
    }
}
