using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using Recursive.Components;
using UnityEngine;

namespace Recursive
{
    public class Door : MonoBehaviour
    {
        [SerializeField] private GameObject _actuator;
        [SerializeField] private GameObject _model;

        private void OnEnable() => _actuator.GetComponent<IActuator>().Bind(SetState);
        private void OnDisable() => _actuator.GetComponent<IActuator>().Unbind(SetState);

        private void SetState(bool state)
        {
            if (state) Open();
            else Close();
        }

        private void Open()
        {
            _model.SetActive(false);
        }

        private void Close()
        {
            _model.SetActive(true);
        }
    }
}
