using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedZombie : Zombie
{
    [SerializeField] Transform throwTransform;
    [SerializeField] GameObject projectile;

    public override void AttackPoint()
    {
        base.AttackPoint();
        GameObject target = GetComponent<AIController>().Target;
        Vector3 throwDir = throwTransform.forward;
        float up = Vector3.Dot(throwDir, Vector3.up);
        float forward = Vector3.Dot(throwDir, transform.forward);
        float distance = Vector3.Distance(target.transform.position, transform.position);
        float gravityAcc = Physics.gravity.magnitude;
        float speed = Mathf.Sqrt(Mathf.Abs(gravityAcc * distance / (2 * up * forward)));
        Vector3 throwVel = speed * throwDir;
        GameObject newProjectile = Instantiate(projectile, throwTransform.position, throwTransform.rotation);
        newProjectile.GetComponent<Rigidbody>().AddForce(throwVel , ForceMode.VelocityChange);
    }
}
