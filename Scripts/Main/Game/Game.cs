using UnityEngine;
using Core.MessageBus;
using System.Collections;
using Core.MessageBus.MessageDataTemplates;

namespace Main.Game
{
    public class Game : MonoBehaviour
    {
        // Use this for initialization
        void Start()
        { 
            Application.targetFrameRate = 120;
            QualitySettings.vSyncCount = 0;

            StartCoroutine(StartAction());
        }

        public IEnumerator StartAction()
        {
            yield return new WaitForSeconds(2);

            MessageBus.SendMessage(CommonMessage.Get(Network.API.Messages.CREATE_CHARACTER.ToString(),
                IntData.GetIntData(1)));

            MessageBus.SendMessage(CommonMessage.Get(Network.API.Messages.CREATE_AIM_TARGET.ToString(),
                AimTarget.API.InstanceData.GetData("UnarmedAimTarget", 2, 1)));
        }
    }
}