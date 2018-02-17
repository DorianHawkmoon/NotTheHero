using UnityEngine;

public class test : MonoBehaviour {

    public float distance = 1.0f;
 public int theAngle = 45;
 public float segmentsDistance = 0.2f;
 
 void Update() {
        Debug.DrawLine(transform.position, transform.position + Vector3.left, Color.cyan);
        if (Input.GetMouseButtonDown(0)) {
            RaycastSweep();
        }
    }

    void RaycastSweep() {
        Vector3 startPos = transform.position; // umm, start position !
        Vector3 targetPos = Vector3.zero;// transform.position + Vector3.left; // variable for calculated end position

        int startAngle = (int)(-theAngle * 0.5); // half the angle to the Left of the forward
        int finishAngle =(int)(theAngle * 0.5); // half the angle to the Right of the forward

        float b = distance;
        float c = distance;
        float distanceCO = Mathf.Sqrt(b * b + c * c - 2 * b * c * Mathf.Cos(theAngle));
        int segments = Mathf.RoundToInt(Mathf.Ceil( distanceCO / segmentsDistance));

        // the gap between each ray (increment)
        int inc = (int)(theAngle / segments);

        RaycastHit hit;

        // step through and find each target point
        for (int i = startAngle; i < finishAngle; i += inc ) // Angle from forward
     {
            targetPos = (Quaternion.Euler(0, 0, i) * transform.up).normalized * distance;

            // linecast between points
            if (Physics.Linecast(startPos, targetPos, out hit)) {
                Debug.Log("Hit " + hit.collider.gameObject.name);
            }

            // to show ray just for testing
            Debug.DrawLine(startPos, targetPos, Color.green,5);
        }
    }
}
