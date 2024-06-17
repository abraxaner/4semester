using System.Collections;
using Cinemachine;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBehaviour : MonoBehaviour
{
   private NavMeshAgent agent;
   [SerializeField] private Transform target;

   private LineOfSightCheck lineOfSight;
   [SerializeField] private float followTime = 10f;

   [SerializeField] private bool isRotating;
   [SerializeField] private float rotationSpeed = 50f;

   private bool isSeen = false;

   private void Awake()
   {
      lineOfSight = GetComponentInChildren<LineOfSightCheck>();
   }

   private void Start()
   {
      agent = gameObject.GetComponent<NavMeshAgent>();
   }

   private void Update()
   {
      if (lineOfSight.inSight && !isSeen)
      {
         StartCoroutine(FollowPlayer());
      }
      
      if (isRotating)
      {
         transform.Rotate(transform.up, rotationSpeed * Time.deltaTime);
      }
   }

   private void Follow()
   {
      CheckIfHidden();
      agent.SetDestination(target.position);
   }

   IEnumerator FollowPlayer()
   {
      Follow();
      yield return new WaitForSeconds(followTime);
      lineOfSight.inSight = false;
   }

   private void CheckIfHidden()
   {
      Vector3 direction = target.position - transform.position;
      float distance = Vector3.Distance(gameObject.transform.position, target.transform.position);

      float targetAngle = Vector3.Angle(transform.forward, direction);

      isSeen = targetAngle > 60 || lineOfSight.IsCharacterCovered(direction, distance);
      Debug.Log("Is covered? " + isSeen);

      if (isSeen)
      {
         lineOfSight.inSight = false;
      }
   }
}
