using UnityEngine;
using System.Collections.Generic;


public class OverlapItem : MonoBehaviour {
    /// <summary>
    /// The layer mask of colliders for where the item can't be put down
    /// </summary>
    [SerializeField]
    private LayerMask[] layerColliders;

    /// <summary>
    /// An extended region of the layer colliders (for example, near range for obstacles and bigger range for heroes)
    /// </summary>
    [SerializeField]
    private LayerMask[] layerCollidersExtended;

    /// <summary>
    /// Offset of the overlap of collider
    /// </summary>
    [SerializeField]
    private Vector3 centerOffset;

    /// <summary>
    /// If the shape of collider is a sphere
    /// </summary>
    [SerializeField]
    private bool isSphere = true;

    /// <summary>
    /// radius of sphere
    /// </summary>
    [SerializeField]
    private float radius=1;

    /// <summary>
    /// radius of sphere for an extended check
    /// </summary>
    [SerializeField]
    private float radiusExtended = 1;

    /// <summary>
    /// If the shape of collider is a box
    /// </summary>
    [SerializeField]
    private bool isBox=false;

    /// <summary>
    /// Size of the box
    /// </summary>
    [SerializeField]
    private Vector3 sizeBox=Vector3.one;

    /// <summary>
    /// Size of the box for an extended check
    /// </summary>
    [SerializeField]
    private Vector3 sizeBoxExtended = Vector3.one;


    /// <summary>
    /// Return the collider layers
    /// </summary>
    public LayerMask[] LayersColliders {
        get { return layerColliders; }
    }

    /// <summary>
    /// Return the collider layers for extended range
    /// </summary>
    public LayerMask[] LayersCollidersExtended {
        get { return layerCollidersExtended; }
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="colliders"></param>
    /// <param name="layerMask"></param>
    public void OverlapColliders(Vector3 position, List<Collider> colliders, int layerMask) {
        position += centerOffset;
        if (isSphere) {
            colliders.AddRange(Physics.OverlapSphere(position, radius, layerMask));
        } else if (isBox) {
            colliders.AddRange(Physics.OverlapBox(position, sizeBox/2, Quaternion.identity, layerMask));   
        }
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="colliders"></param>
    /// <param name="layerMask"></param>
    public void OverlapCollidersExtended(Vector3 position, List<Collider> colliders, int layerMask) {
        position += centerOffset;
        if (isSphere) {
            colliders.AddRange(Physics.OverlapSphere(position, radiusExtended, layerMask));
        } else if (isBox) {
            colliders.AddRange(Physics.OverlapBox(position, sizeBoxExtended/2, Quaternion.identity, layerMask));
        }
    }

    /// <summary>
    /// Get all the colliders overlaping the shape of this item
    /// </summary>
    /// <param name="layerMask"></param>
    /// <returns></returns>
    public List<Collider> OverlapColliders(Vector3 position, int layerMask) {
        List<Collider> colliders = new List<Collider>();
        OverlapColliders(position, colliders, layerMask);
        return colliders;
    }

    /// <summary>
    /// Get all the colliders overlaping the shape of this item
    /// using the extended version of the colliders
    /// </summary>
    /// <param name="layerMask"></param>
    /// <returns></returns>
    public List<Collider> OverlapCollidersExtended(Vector3 position, int layerMask) {
        List<Collider> colliders = new List<Collider>();
        OverlapCollidersExtended(position, colliders, layerMask);
        return colliders;
    }

    /// <summary>
    /// Draw the overlap size in use
    /// </summary>
    public void OnDrawGizmos() {
        Gizmos.color = Color.magenta;
        if (isSphere) {
            Gizmos.DrawWireSphere(transform.position + centerOffset, radius);
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position + centerOffset, radiusExtended);
        } else if (isBox) {
            Gizmos.DrawWireCube(transform.position + centerOffset, sizeBox);
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireCube(transform.position + centerOffset, sizeBoxExtended);
        }
    }
}
