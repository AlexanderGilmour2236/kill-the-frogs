using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace KillTheFrogs
{
    public class TapStrategy : MonoBehaviour, IPointerClickHandler
    {
        public event Action<TrapView> trapViewDestroyed;
        public event Action<TapStrategy> tap;
        public event Action<FrogView, TrapView> onTrapCollideWithFrog;

        public virtual void onTap()
        {
        }
        
        public virtual void OnPointerClick(PointerEventData eventData)
        {
            tap?.Invoke(this);
        }

        public virtual void invokeTrapCollideWithFrog(FrogView frogView, TrapView trapView)
        {
            onTrapCollideWithFrog?.Invoke(frogView, trapView);
        }        
        
        public virtual void invokeTrapViewDestroyed(TrapView trapView)
        {
            trapViewDestroyed?.Invoke(trapView);
        }
    }
}