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
    private IInteractable curInteractable;

    public TextMeshProUGUI promptText;
    private Camera camera; //강의에도 경고문구가 나오긴하는데 일단 보류해두고 추후 수정할 수 있는지 확인해보겠습니다.
    private Player player;

    private float _nextInteractTime = 0f;
    [SerializeField] private float interactCooldown = 0.12f;

      

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

        if(Physics.Raycast(ray, out hit, maxCheckDistance, layerMask))
        {
            if(hit.collider.gameObject != curInteractGameObject)
            {
                curInteractGameObject = hit.collider.gameObject;
                curInteractable = hit.collider.GetComponent<IInteractable>();
                promptText.gameObject.SetActive(false);
            }
        }
        else
        {
            curInteractGameObject = null;
            curInteractable = null;
            promptText.gameObject.SetActive(false);
        }
    }

    private void SetPromptText()
    {
        promptText.gameObject.SetActive(true);
        promptText.text = curInteractable.GetInteractPrompt();
    }

    public void OnInteractInput(InputAction.CallbackContext context)
    {
        if (!context.started) return;
        if (Time.time < _nextInteractTime) return;
        _nextInteractTime = Time.time + interactCooldown;

        if (player == null) player = PlayerManager.Instance.Player;
        if (curInteractable != null)
        {
            if(curInteractable is ItemObject io)
            {
                player.PickUpItem(io.transform, io.data);
            }

            curInteractGameObject = null;
            curInteractable = null;
            if (promptText) promptText.gameObject.SetActive(false);
        }
        else
        {
            if (player.HasItem) player.DropCarried();
        }
        
    }
}
