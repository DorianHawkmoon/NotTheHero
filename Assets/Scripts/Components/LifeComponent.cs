﻿using System;
using UnityEngine;

public class LifeComponent : MonoBehaviour {
    /// <summary>
    /// Life of the entity
    /// </summary>
    [SerializeField]
    private int life;

    private bool died = false;

    private Action onDeath;
    private Action<int> onLifeChange;
    

    public void Damage(int damage) {
        life -= damage;

        if (onLifeChange != null) {
            onLifeChange(life);
        }
        if(life<=0 && !died) {
            died = true;
            if (onDeath != null) {
                onDeath();
                Die();
            }
        }
    }
    

    public bool IsDead() {
        return died;
    }

    private void Die() {
        //TODO
        //init animation Die
        //else, remove gameobject
    }

    private void AnimationDied() {
        //TODO
    }


    public void RegisterOnDeath(Action callback) {
        onDeath += callback;
    }

    public void UnregisterOnDeath(Action callback) {
        onDeath -= callback;
    }

    public void RegisterOnLifeChange(Action<int> callback) {
        onLifeChange += callback;
    }

    public void UnregisterOnLifeChange(Action<int> callback) {
        onLifeChange -= callback;
    }
}
