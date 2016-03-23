using System;
using UnityEngine;

public class TimedObjectDestructor : MonoBehaviour, IGetAnimationTimeToFin {
        [SerializeField] private float m_TimeOut = 1.0f;
        [SerializeField] private bool m_DetachChildren = false;


        private void Awake()
        {
            Invoke("DestroyNow", m_TimeOut);
        }

	#region IGetAnimationTimeToFin implementation
	public float getAnimationTime ()
	{
		return m_TimeOut;
	}
	#endregion

        private void DestroyNow()
        {
            if (m_DetachChildren)
            {
                transform.DetachChildren();
            }
            DestroyObject(gameObject);
        }
    }
