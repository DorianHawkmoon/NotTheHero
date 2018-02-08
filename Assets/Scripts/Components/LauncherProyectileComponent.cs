using UnityEngine;

public class LauncherProyectileComponent : MonoBehaviour {
    /// <summary>
    /// The proyectile to launch
    /// </summary>
    [SerializeField]
    private GameObject prefabProyectile;
    /// <summary>
    /// Adjust it to have the proyectile start in another place
    /// </summary>
    [SerializeField]
    private Vector3 offsetLauncher = Vector3.zero;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Launch(Vector3 direction) {
        if (prefabProyectile == null) return;
        //create the proyectile
        //give it the direction
        //it will have the velocity and a KillOnCollisionComponent (under folder collisionsComponents)
        GameObject proyectile = Instantiate(prefabProyectile);
        proyectile.transform.position = transform.position + offsetLauncher;
        /*proyectile.name = "draging object";
        proyectile.transform.localScale = item.ScaleItem;

        rendererDragged = proyectile.AddComponent<SpriteRenderer>();
        rendererDragged.sprite = item.GetSprite();

        GameControllerTemporal.AddTemporal(proyectile);
        return proyectile;*/
    }
}
