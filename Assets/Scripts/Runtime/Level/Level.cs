using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Recursive
{
    public class Level : MonoBehaviour
    {
        public List<Components.IComponent> Components = new();
        public Transform StartPosition;

        private void Start()
        {
            Components = GetComponentsInChildren<Components.IComponent>().ToList();
        }
    }
}
