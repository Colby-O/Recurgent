using System;
using UnityEngine;

namespace Recursive.Components
{
    public interface IActuator
    {
        public void Bind(Action<bool> callback);
        public void Unbind(Action<bool> callback);
    }
}
