using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


// Warning, if this object has and post death processes, DON'T use destroythis script
//  on the object until those process is finished.
// (possibly use destory by time, or just deactivate the game object)
public class Damageable : MonoBehaviour
{
    [SerializeField] protected Progressive _health;

    // Destroy or spawn something depending on the event
    public UnityEvent OnDie;


    public virtual void Damage(
        float damage, GameObject attacker = null)
    {
        
        //if (_health.Current <= 0)
        //{
        //    return;
        //}

        _health.Sub(damage);

        if (_health.Current <= 0)
        {
            //Die();
            StartCoroutine(SlowDie());
        }
    }
    public virtual void Heal(float heal)
    {
        _health.Add(heal);

        //if (_health.Current > _health.Initial)
        //{
        //    _health.Current = _health.Initial;
        //}
    }

    protected void Die()
    {
        OnDie.Invoke();
    }

    protected IEnumerator SlowDie()
    {
        yield return null;
        OnDie.Invoke();
    }
}
