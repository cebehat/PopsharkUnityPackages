using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Cebt
{
    [AddComponentMenu("PopShark/Local/Character Controller")]
    [RequireComponent(typeof(CharacterController))]
#if ENABLE_INPUT_SYSTEM
    [RequireComponent(typeof(PlayerInput))]
#endif

    public class LocalCharacterMovement : MonoBehaviour
    {
        [SerializeField]
        [Tooltip("Input action assets to affect when inputs are enabled or disabled.")]
        List<InputActionAsset> m_ActionAssets;

        [SerializeField]
        public InputActionReference MoveAction;

        [SerializeField]
        public InputActionReference LookAction;



        Transform cameraTransform;
        float pitch = 0f;

        [Range(1f, 90f)]
        public float maxPitch = 85f;
        [Range(-1f, -90f)]
        public float minPitch = -85f;
        [Range(0.01f, 0.15f)]
        public float mouseSensitivity = 0.08f;
        [Range(0.1f, 5f)]
        public float movementSpeed = 3f;

        CharacterController characterController;
        Collider collider;
        // Start is called before the first frame update
        void Start()
        {
            cameraTransform = GetComponentInChildren<Camera>().transform;
            characterController = GetComponent<CharacterController>();
        }

        //private IInteractible targettedInteractible = null;
        // Update is called once per frame
        void Update()
        {

            Look();
            Move();
            //HandleInteractions();
        }

        void Look()
        {
            var action = LookAction.ToInputAction();
            if (action != null && action.phase == InputActionPhase.Started)
            {
                Vector2 input = action.ReadValue<Vector2>();
                float xInput = input.x * mouseSensitivity;
                float yInput = input.y * mouseSensitivity;

                this.transform.Rotate(0, xInput, 0);

                pitch -= yInput;
                pitch = Mathf.Clamp(pitch, minPitch, maxPitch);

                Quaternion rot = Quaternion.Euler(pitch, 0, 0);
                cameraTransform.localRotation = rot;
                
            }
        }

        void Move()
        {
            var action = MoveAction.ToInputAction();
            if (action != null && action.phase == InputActionPhase.Started)
            {
                Vector2 value = action.ReadValue<Vector2>();

                float hAxis = value.x;
                float vAxis = value.y;

                var forward = transform.forward;
                var right = transform.right;

                forward.y = 0f;
                right.y = 0f;
                forward.Normalize();
                right.Normalize();

                var move = forward * vAxis + right * hAxis;
                //var move = new Vector3(hAxis, 0f, vAxis);

                if (move != Vector3.zero)
                {
                    characterController.Move(move.normalized * Time.deltaTime * movementSpeed);
                    //transform.Translate(move * Time.deltaTime * 5f);
                }
            }
        }

        //private void HandleInteractions()
        //{
        //    LayerMask mask = LayerMask.GetMask("Interactible");
        //    RaycastHit hitInfo;

        //    Ray ray = new Ray(cameraTransform.position, cameraTransform.forward);

        //    if (Physics.Raycast(ray, out hitInfo, 20f) && hitInfo.collider.gameObject.layer == LayerMask.NameToLayer("Interactible"))
        //    {
        //        var interactible = hitInfo.collider.GetComponent<IInteractible>();
        //        if (targettedInteractible != interactible)
        //        {
        //            Debug.Log("Hit Interactible: " + hitInfo.collider.name);
        //            interactible.Target(true);
        //            if (targettedInteractible != null)
        //            {
        //                targettedInteractible.Target(false);
        //            }

        //        }
        //        targettedInteractible = interactible;
        //    }
        //    else
        //    {
        //        if (targettedInteractible != null)
        //        {
        //            targettedInteractible.Target(false);
        //            targettedInteractible = null;
        //        }
        //    }
        //}
    }
}