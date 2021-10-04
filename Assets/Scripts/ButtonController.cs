using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class ButtonController : MonoBehaviour
{
    public ActuatorBase[] actuators;
    public ButtonType buttonType;
    public float intensity = 1f;
    public MeshRenderer buttonTopRenderer;
    
    [Header("Audio")]
    public AudioClip buttonDownSfx;
    public AudioClip buttonUpSfx;

    private Animator animator;

    public enum ButtonType
    {
        SingleAction,
        Toggle,
        PressurePlate,
    }

    private bool buttonActivated;

    // Start is called before the first frame update
    void Start()
    {
        this.buttonActivated = false;
        this.animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Horse"))
        {
            switch (this.buttonType)
            {
                case ButtonType.SingleAction:
                case ButtonType.PressurePlate:
                    if (!this.buttonActivated)
                        this.AnimateButtonDown();
                        this.ButtonActivated();
                    break;
                case ButtonType.Toggle:
                    this.AnimateButtonDown();
                    if (!this.buttonActivated)
                        this.ButtonActivated();
                    else this.ButtonDeactivated();
                    break;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Horse"))
        {
            switch (this.buttonType)
            {
                case ButtonType.SingleAction:
                    break;
                case ButtonType.Toggle:
                    this.AnimateButtonUp();
                    break;
                case ButtonType.PressurePlate:
                    this.AnimateButtonUp();
                    this.ButtonDeactivated();
                    break;
            }
        }
    }

    private void AnimateButtonDown()
    {
        // need to animate button being pressed down
        GameManager.Instance.sfx.PlaySoundEffect(buttonUpSfx);
        this.animator.SetBool("Pressed", true);
    }

    private void AnimateButtonUp()
    {
        // need to animate button raising back up
        GameManager.Instance.sfx.PlaySoundEffect(buttonDownSfx);
        this.animator.SetBool("Pressed", false);
    }

    private void ButtonActivated()
    {
        this.buttonActivated = true;

        foreach (ActuatorBase actuator in this.actuators)
        {
            actuator.Actuate();
        }
    }

    private void ButtonDeactivated()
    {
        this.buttonActivated = false;

        foreach (ActuatorBase actuator in this.actuators)
        {
            actuator.ResetActuator();
        }
    }
}
