using DG.Tweening;
using UnityEngine;
using VT.Utilities.GameObjectPooling.PooledGameObjectSpawnSystem;

namespace VT.Utilities.GameObjectPooling
{
    public class PooledGameObject : MonoBehaviour
    {
        private void Awake()
        {
            SetInactive();
        }

        public void SetInactive()
        {
            PooledGameObjectIntervalSpawnerManager.Instance?.Remove(this);
            gameObject.SetActive(false);

            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.identity;
        }

        public PooledGameObject SetActive()
        {
            PooledGameObjectIntervalSpawnerManager.Instance?.Add(this);
            gameObject.SetActive(true);
            return this;
        }

        public PooledGameObject SetActiveDuration(float duration)
        {
            if (repeaterTween != null)
            {
                repeaterTween.Kill();
            }

            repeaterTween = DOVirtual.DelayedCall(duration, () => SetInactive(), false).OnComplete(() => repeaterTween = null);
            return this;
        }

        private Tween repeaterTween;

        private void OnDestroy()
        {
            repeaterTween?.Kill();
        }
    }
}