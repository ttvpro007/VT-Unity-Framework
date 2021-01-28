/* 
    ------------------- Code Monkey -------------------

    Thank you for downloading this package
    I hope you find it useful in your projects
    If you have any questions let me know
    Cheers!

               unitycodemonkey.com
    --------------------------------------------------
 */

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TractorBehaviour : MonoBehaviour {

    private PlayerOvermap playerOvermap;
    private float tractorDistance;
    private float tractorSpeed;
    private Action onReachedPlayer;

    public void Setup(PlayerOvermap playerOvermap, float tractorDistance, float tractorSpeed, Action onReachedPlayer) {
        this.playerOvermap = playerOvermap;
        this.tractorDistance = tractorDistance;
        this.tractorSpeed = tractorSpeed;
        this.onReachedPlayer = onReachedPlayer;
    }

    private void Update() {
        if (Vector3.Distance(GetPosition(), playerOvermap.GetPosition()) < tractorDistance) {
            // Within Tractor distance, get pulled towards player
            float tractorMoveSpeed = Vector3.Distance(GetPosition(), playerOvermap.GetPosition()) * tractorSpeed;
            transform.position += (playerOvermap.GetPosition() - GetPosition()).normalized * tractorMoveSpeed * Time.deltaTime;

            float grabDistance = 2f;
            if (Vector3.Distance(GetPosition(), playerOvermap.GetPosition()) < grabDistance) {
                // Within Grab distance, grab and destroy
                Destroy(gameObject);
                onReachedPlayer();
            }
        }
    }

    private Vector3 GetPosition() {
        return transform.position;
    }

}
