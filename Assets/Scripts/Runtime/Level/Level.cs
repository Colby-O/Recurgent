using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Recursive
{
    public class Level : MonoBehaviour
    {
        public List<Components.IComponent> Components = new();
        public Transform StartPosition;

        public void Initialize()
        {
            Components = GetComponentsInChildren<Components.IComponent>().ToList();
        }
    }
}
