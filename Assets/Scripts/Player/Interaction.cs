using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;

public class Interaction : MonoBehaviour
{
    public float checkRate = 0.05f;
    private float lastCheckTime;
    public float maxCheckDistance;
    public LayerMask layerMask;

    public GameObject curInteractGameObject;

    public TextMeshProUGUI promptText;
    public Camera camera;
    private Player player;

    public float dropForward = 1f;

    public Transform cube;

    
    private Transform carried;

      

    // Start is called before the first frame update
    void Start()
    {
        camera = Camera.main;
        player = PlayerManager.Instance.Player;
    }

    // Update is called once per frame
    void Update()
    {
        Ray ray = camera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
        RaycastHit hit;
        Debug.Log("Update good");
        if(Physics.Raycast(ray, out hit, maxCheckDistance, layerMask))
        {
            Debug.Log("RayCast good");
            if (hit.collider.gameObject != curInteractGameObject)
            {
                curInteractGameObject = hit.collider.gameObject;
                Debug.Log("gameobject good");
            }
            
        }
        else
        {
            curInteractGameObject = null;
            Debug.Log("else good");
        }
    }


    public void OnInteractInput(InputAction.CallbackContext context)
    {
        if (!context.started) return;

        if(curInteractGameObject && HasTagInParents(curInteractGameObject.transform, "Item"))
            {
            Transform itemRoot = curInteractGameObject.GetComponentInParent<Transform>();
            if(carried == itemRoot)
            {
                DropCarried();
                return;
            }

            if (carried) DropCarried();
            PickUp(itemRoot);
            return;
        }

        if(carried) DropCarried();


    }

    static bool HasTagInParents(Transform tr, string tag)
    {
        while (tr)
        {
            if (tr.CompareTag(tag)) return true;
            tr = tr.parent;
        }
        return false;
    }

    void PickUp(Transform item)
    {
        if(item.TryGetComponent<Rigidbody>(out var rb))
        {
            rb.isKinematic = true;
            rb.detectCollisions = false;
        }
        var parent = cube ? cube : transform;
        item.SetParent(parent, worldPositionStays: false);
        item.localPosition = Vector3.zero;
        item.localRotation = Quaternion.identity;
        item.localScale = Vector3.one;

        carried = item;
    }

    void DropCarried()
    {
        if (!carried) return;
        var t = carried;
        carried = null;

        t.SetParent(null, true);
        Vector3 pos = (player ?  player.transform.position : transform.position) + (player? player.transform.forward : transform.forward) * dropForward;
        Quaternion rot = Quaternion.LookRotation((player ? player.transform.forward : transform.forward), Vector3.up);
        t.SetPositionAndRotation(pos, rot);

        if(t.TryGetComponent<Rigidbody>(out var rb))
        {
            rb.isKinematic = false;
            rb.detectCollisions = true;
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
    }
}
