using UnityEngine;

public class RayToClickEnemy : MonoBehaviour
{
    public Camera mainCamera;
    public LayerMask enemyLayerMask;    //Click Object in only ENEMY layer
    public Transform player;

    void Awake()
    {
        if (mainCamera == null)
            mainCamera = Camera.main;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && player.GetComponent<Player>().inCodeWorld()) //left click
        {
            
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            //Debug.Log($"Casting ray from {mainCamera.name} at {Input.mousePosition}");

            if (Physics.Raycast(ray, out RaycastHit hitInfo, Mathf.Infinity, enemyLayerMask))
            {
                GameObject hitObject = hitInfo.collider.gameObject;
                //Debug.Log($"Hit object: {hitObject.name}, layer: {hitObject.layer}", hitObject);

                EnemyPropertyManager enemyScript = hitObject.GetComponent<EnemyPropertyManager>();
                if (enemyScript != null)
                {
                    Debug.Log($"Calling SpawnProperty on: {hitObject.name}");
                    enemyScript.SpawnPropertyUI();
                }
                else
                {
                    Debug.LogWarning($"Hit enemy {hitObject.name} does not have Property Info!");
                }
            }
            else
            {
                //Debug.Log("Raycast did not hit any enemy layer object");
            }
        }
    }

}
