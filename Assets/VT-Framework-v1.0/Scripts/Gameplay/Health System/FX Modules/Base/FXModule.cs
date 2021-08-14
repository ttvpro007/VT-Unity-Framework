using DG.Tweening;
using UnityEngine;
using VT.Gameplay.HealthSystem;

public abstract class FXModule
{
    protected Health healthSystem;
    protected Transform fxBarTransform;
    protected Tween fxBarTween;
    protected bool playedLastPart;

    public FXModule(Transform fxBarTransform, Health healthSystem)
    {
        this.healthSystem = healthSystem;
        this.fxBarTransform = fxBarTransform;
        healthSystem.OnHealthAdded += HealthSystem_OnHealthAdded;
        healthSystem.OnHealthSubtracted += HealthSystem_OnHealthSubtracted;
    }

    public virtual void Kill()
    {
        healthSystem.OnHealthAdded -= HealthSystem_OnHealthAdded;
        healthSystem.OnHealthSubtracted -= HealthSystem_OnHealthSubtracted;
    }

    public virtual void Play()
    {
        if (healthSystem.IsAlive || !playedLastPart)
        {
            PlayRoutine();
        }
    }

    protected abstract void PlayRoutine();

    protected virtual void HealthSystem_OnHealthAdded()
    {
        if (healthSystem.IsAlive)
            playedLastPart = false;
    }

    protected virtual void HealthSystem_OnHealthSubtracted()
    {
        Play();

        if (!healthSystem.IsAlive)
            playedLastPart = true;
    }
}
