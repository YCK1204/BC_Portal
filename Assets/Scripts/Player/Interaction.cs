using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Interaction : MonoBehaviour
{
    public float checkRate = 0.05f;
    private float lastCheckTime;
    public float maxCheckDistance;
    public LayerMask layerMask;

    public GameObject curInteractGameObject;
    //private IInteractable curInteractable; ItemData를 강의와 같은 구조로 제작한다면 사용 가능

    public TextMeshProUGUI promptText;
    private Camera camera;

    // Start is called before the first frame update
    void Start()
    {
        camera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        Ray ray = camera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
        RaycastHit hit;

        if(Physics.Raycast(ray, out hit, maxCheckDistance, layerMask))
        {
            if(hit.collider.gameObject != curInteractGameObject)
            {
                curInteractGameObject = hit.collider.gameObject;
                //curInteractable = hit.collider.GetComponent<IInteractable>();
                //출력해줘라.
            }
        }
        else
        {
            curInteractGameObject = null;
            //curInteractable = null;
            promptText.gameObject.SetActive(false);
        }
    }
}
