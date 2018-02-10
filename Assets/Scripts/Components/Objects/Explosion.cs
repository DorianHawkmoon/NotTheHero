using UnityEngine;

public class Explosion : MonoBehaviour {

	public void SetScaleExplosion(float scale) {
        if (transform.childCount > 0) {
            transform.GetChild(0).localScale = Vector3.one * scale;
        }        
    }
}
