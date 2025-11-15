using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ObjectPlacer : MonoBehaviour
{
    [SerializeField] private GameObject placeableObjectPrefab;
    [SerializeField] private GameObject previewObjectPrefab;
    [SerializeField] private Camera playerCamera;
    [SerializeField] private LayerMask placementSurfaceLayerMask;
    [SerializeField] private float objectDistanceFromPlayer;
    [SerializeField] private float raycastStartvericalOffset;
    [SerializeField] private float raycastDistance;


    private GameObject _previewObject = null;
    private Vector3 _currentPlacementPosition = Vector3.zero;
    private bool _inPlacementMode = false;
    private void Update()
    {
        UpdateInput();
        if(_inPlacementMode)
        {
            UpdateCurrentPlacementPosition();
        }
    }
    private void UpdateCurrentPlacementPosition()
    {
        Vector3 cameraForward = new Vector3(playerCamera.transform.forward.x, 0f, playerCamera.transform.forward.z);
        cameraForward.Normalize();
        Vector3 startPos = playerCamera.transform.position + (cameraForward * objectDistanceFromPlayer);
        startPos.y += raycastStartvericalOffset;
        RaycastHit hitInfo;
        if (Physics.Raycast(startPos,Vector3.down, out hitInfo, raycastDistance, placementSurfaceLayerMask))
        {
            _currentPlacementPosition = hitInfo.point;
         
        }
        Quaternion rotation = Quaternion.Euler(0f, playerCamera.transform.eulerAngles.y, 0f);
        _previewObject.transform.position = _currentPlacementPosition; 
        _previewObject.transform.rotation = rotation;
    }
    private void UpdateInput()
    {
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            EnterPlacementMode();
        }
        else if(Input.GetKeyDown(KeyCode.Alpha2))
        {
            ExitPlacementMode();
        }
        else if (Input.GetMouseButtonDown(0))
        {
            PlaceObject();
        }
    }
    private void PlaceObject()
    {
        if (!_inPlacementMode)
            return;
        Quaternion rotation = Quaternion.Euler(0f, playerCamera.transform.eulerAngles.y,0f);
        Instantiate(placeableObjectPrefab, _currentPlacementPosition, rotation, transform);
        ExitPlacementMode();
    }
    private void EnterPlacementMode()
    {
        
        if (_inPlacementMode)
            return;

        

        Quaternion rotation = Quaternion.Euler(0f, playerCamera.transform.eulerAngles.y, 0f);
        _previewObject = Instantiate(previewObjectPrefab, _currentPlacementPosition, rotation,transform);
        _inPlacementMode = true;
    }


    private void ExitPlacementMode()
    {
       
        Destroy(_previewObject);
        _previewObject = null;
        _inPlacementMode = false;
    }

}
