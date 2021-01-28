using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey;
using CodeMonkey.Utils;
using CodeMonkey.MonoBehaviours;
using GridPathfindingSystem;

public class GameHandler_Setup : MonoBehaviour {

    public static GridPathfinding gridPathfinding;
    [SerializeField] private CameraFollow cameraFollow;
    [SerializeField] private PlayerOvermap playerOvermap;

    private void Awake() {
        new OvermapHandler(playerOvermap);
    }

    private void Start() {
        //Sound_Manager.Init();
        cameraFollow.Setup(GetCameraPosition, () => 70f, true, true);

        gridPathfinding = new GridPathfinding(new Vector3(-400, -400), new Vector3(400, 400), 5f);
        gridPathfinding.RaycastWalkable();

        //OvermapHandler.SpawnFollower();
        //OvermapHandler.SpawnEnemy(new Vector3(150, 0));
    }

    private Vector3 GetCameraPosition() {
        return playerOvermap.GetPosition();
    }

}
