﻿namespace SangoScripts_Server
{
    public abstract class BaseSystem<T> : ServerSingleton<T> where T : class, new()
    {
        public override void Update()
        {
            base.Update();
            OnUpdate();
        }

        public virtual void OnInit()
        {

        }

        protected virtual void OnUpdate()
        {

        }

        public virtual void OnDispose()
        {

        }
    }
}
