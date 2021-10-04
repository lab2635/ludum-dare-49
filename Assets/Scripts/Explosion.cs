using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    //private void OnCollisionEnter(Collision collision)
    //{
    //    ContactPoint[] contacts = new ContactPoint[10];
    //    collision.GetContacts(contacts);
    //    for (int i = 0; i < collision.contactCount; i++)
    //    {
    //        ContactPoint contact = collision.GetContact(i);
    //        if (contact.otherCollider.gameObject.CompareTag("Horse"))
    //        {
    //            HorseBehavior otherHorse = contact.otherCollider.gameObject.GetComponent<HorseBehavior>();
    //            if (otherHorse.Active)
    //            {
    //                otherHorse.Explode();
    //            }
    //        }
    //    }
    //}

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Horse"))
        {
            HorseBehavior otherHorse = other.gameObject.GetComponent<HorseBehavior>();
            if (otherHorse.Active)
            {
                otherHorse.Explode();
            }
        }
    }
}
