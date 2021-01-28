using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey;
using CodeMonkey.Utils;
using CodeMonkey.MonoBehaviours;
using GridPathfindingSystem;

public class GameHandler_Overmap : MonoBehaviour {

    private static GameHandler_Overmap instance;

    public static GridPathfinding gridPathfinding;
    [SerializeField] private CameraFollow cameraFollow;
    [SerializeField] private PlayerOvermap playerOvermap;

    private void Awake() {
        instance = this;
        new OvermapHandler(playerOvermap);
    }

    private void Start() {
        //Sound_Manager.Init();

        gridPathfinding = new GridPathfinding(new Vector3(-400, -400), new Vector3(400, 400), 5f);
        gridPathfinding.RaycastWalkable();

        OvermapHandler.GetInstance().Start(transform);
        
        cameraFollow.Setup(GetCameraPosition, () => 70f, true, true);
    }

    private void Update() {
        OvermapHandler.GetInstance().Update();
    }

    private Vector3 GetCameraPosition() {
        return playerOvermap.GetPosition();
    }

}
