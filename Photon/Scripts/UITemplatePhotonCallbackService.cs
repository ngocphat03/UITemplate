namespace UITemplate.Photon.Scripts
{
    using System;
    using System.Collections.Generic;
    using global::Photon.Pun;
    using UnityEngine;

    public class UITemplatePhotonCallbackService : MonoBehaviourPunCallbacks
    {
        private readonly Dictionary<string, Action<object[]>> callbackObjectDictionary = new();
        private readonly Dictionary<string, Action>           callbackDictionary       = new();

        private string nameCurrentAction;

        public void AddCallback(Action<object[]> newAction, string nameAction = "")
        {
            nameAction = nameAction == "" ? nameof(newAction) : nameAction;

            if (!this.callbackObjectDictionary.TryAdd(nameAction, newAction))
            {
                Debug.LogError($"You had call back with name: {nameAction}. Don't resign again!");

                return;
            }

            Debug.Log($"Subscribe success photon action callback with name: \"{nameAction}\"");
        }

        public void AddCallback(Action newAction, string nameAction = "")
        {
            nameAction = nameAction == "" ? nameof(newAction) : nameAction;

            if (!this.callbackDictionary.TryAdd(nameAction, newAction))
            {
                Debug.LogError($"You had call back with name: {nameAction}. Don't resign again!");

                return;
            }

            Debug.Log($"Subscribe success photon action callback with name: \"{nameAction}\"");
        }

        public void CallFunction(string nameAction, RpcTarget target = RpcTarget.All, object[] parameters = null)
        {
            parameters ??= new object[] { };

            // Add reference key
            var newParameters = new object[parameters.Length + 1];
            Array.Copy(parameters, newParameters, parameters.Length);
            newParameters[^1] = nameAction;

            this.photonView.RPC(nameof(this.CallbackObjectAction), target, (object[])newParameters);
        }

        [PunRPC]
        private void CallbackObjectAction(object[] parameters)
        {
            if (parameters.Length < 1)
            {
                Debug.LogError("No action name provided in parameters.");

                return;
            }

            var nameAction = (string)parameters[^1];
            Debug.Log($"Call action with key: {nameAction}");

            if (this.callbackObjectDictionary.TryGetValue(nameAction, out var actionObject) && parameters.Length > 1)
            {
                var actionParameters = new object[parameters.Length - 1];
                Array.Copy(parameters, actionParameters, parameters.Length - 1);
                actionObject(actionParameters);
            }
            else if (this.callbackDictionary.TryGetValue(nameAction, out var action) && parameters.Length <= 1)
            {
                action();
            }
            else
            {
                Debug.LogError($"Can't find action with name: {nameAction}");
            }
        }

        // HOT FIX: Add this method to fix error: RPC method 'CallbackObjectAction' found on object with PhotonView 2 but has wrong parameters. Implement as 'CallbackObjectAction(String)'. PhotonMessageInfo is optional as final parameter.Return type must be void or IEnumerator (if you enable RunRpcCoroutines).
        [PunRPC]
        public void CallbackObjectAction(string stringValue)
        {
            Debug.Log("Received string value: " + stringValue);
            this.callbackDictionary.TryGetValue(stringValue, out var action);

            action();
        }
    }
}