using System.Linq;
using System.Collections.Generic;
using ProjectFukalite.Handlers;
using ProjectFukalite.Interfaces;
using ProjectFukalite.Data;
using UnityEngine;
namespace ProjectFukalite.Systems
{
    public class InteractionSystem : MonoBehaviour
    {
        [SerializeField] private float reach = 5f;

        [SerializeField] private Transform camTransform;

        private void Update()
        {
            if (PlayerUI.singleton.isPaused)
            { return; }

            Collider[] colliders = Physics.OverlapSphere(camTransform.position, reach);

            foreach (IDisplay interactable in FindObjectsOfType<MonoBehaviour>().OfType<IDisplay>())
            {
                interactable.Undisplay();
            }

            foreach (Collider col in colliders)
            {
                IDisplay display = col.transform.GetComponent<IDisplay>();
                if (display != null)
                {
                    display.Display();
                }
            }

            if (Input.GetKeyDown(KeyHandler.PickupKey))
            {
                RaycastHit _hitInfo;
                if (Physics.Raycast(camTransform.position, camTransform.forward, out _hitInfo, reach))
                {
                    IInteractable interactable = _hitInfo.transform.GetComponent<IInteractable>();
                    if (interactable != null)
                    {
                        interactable.Interact();
                    }
                }
            }
            else if (Input.GetKeyDown(KeyHandler.TriggerKey))
            {
                RaycastHit _hitInfo;
                if (Physics.Raycast(camTransform.position, camTransform.forward, out _hitInfo, reach))
                {
                    ITrigger interactable = _hitInfo.transform.GetComponent<ITrigger>();
                    if (interactable != null)
                    {
                        interactable.Trigger();
                    }
                }
            }
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(camTransform.position, reach);
        }
    }
}