using UnityEngine;
using UnityEngine.UIElements;

public class LineOfSightCheck : MonoBehaviour
{
    public bool inSight = false;

    [SerializeField] private LayerMask coverL;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            inSight = true;
        }
    }

    public bool IsCharacterCovered(Vector3 targetDirection, float distance)
    {
        RaycastHit[] hits = new RaycastHit[5];
        Ray ray = new Ray(transform.position, targetDirection);
        int amountOfHits = Physics.RaycastNonAlloc(ray, hits, distance, coverL);

        if (amountOfHits > 0)
        {
            return true;
        }
        //RaycastHit[] hits = Physics.RaycastAll(transform.position, targetDirection, distance, coverL);
        return false;
    }
}
