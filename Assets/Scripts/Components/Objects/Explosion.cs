using UnityEngine;

/// <summary>
/// Simple class for an explosion
/// </summary>
public class Explosion : MonoBehaviour {

    /// <summary>
    /// Set the scale of the explosion
    /// </summary>
    /// <param name="scale"></param>
	public void SetScaleExplosion(float scale) {
        if (transform.childCount > 0) {
            transform.GetChild(0).localScale = Vector3.one * scale;
        }        
    }
}
