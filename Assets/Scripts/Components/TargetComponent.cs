﻿using UnityEngine;
    public float treshold=0.5f;

    private Vector3 position;
        if (!init) {
            init = true;
            if (targetObject != null) {
                SetTargetObject(targetObject);
            }
        }

        Vector3 newPosition = targetObject.transform.position;
        if (Mathf.Abs(Vector3.Distance(newPosition, position)) > treshold) {
            SetTargetObject(targetObject);
        }
    }
        targetObject = newTarget;
        position = targetObject.transform.position;
        onTargetChange();
    }
        onTargetChange += callback;
    }
        onTargetChange -= callback;
    }