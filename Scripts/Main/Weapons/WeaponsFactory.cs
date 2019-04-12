using Core.MessageBus;
using Core.MessageBus.MessageDataTemplates;
using Core.Services;
using Main.AimTarget.API;
using Main.Weapons.API;
using Main.Weapons.Data;
using UnityEngine;

namespace Main.Weapons
{
    public class WeaponsFactory : SubscriberBehaviour
    {
        public static void InstantiateFactory()
        {
            var container = new GameObject("__Weapons_FACTORY");
            container.AddComponent<WeaponsFactory>();
        }
        
        [Subscribe(API.Messages.INSTANTIATE_L2)]
        private void InstantiateWeapon(Message msg)
        {
            Debug.Log("Create weapon!");

            var weaponData = msg.Data as WeaponInstantiateData;

            ServiceLocator.GetService<ResourceLoaderService>()
                .InstantiatePrefabByPathName("Weapons/" + weaponData.Type.ToString(),
                    weapon =>
                    {
                        weapon.transform.parent = weaponData.Parent;
                        weapon.transform.localPosition = new Vector3(0.001f, 0.079f, 0.025f);
                        weapon.transform.localEulerAngles = new Vector3(94.46698f, 51.211f, 44.17799f);

                        var sub = weapon.GetComponent<SubscriberBehaviour>();
                        var channel = weapon.GetComponent<Channel>();
                        channel.ChannelIds.Add(SubscribeType.Channel, weaponData.ChannelId);
                        sub.ReSubscribe();

                        MessageBus.SendMessage(SubscribeType.Channel, weaponData.ChannelId,
                            CommonMessage.Get(Characters.API.Messages.SET_ARMED));

                        MessageBus.SendMessage(SubscribeType.Channel, weaponData.ChannelId,
                            CommonMessage.Get(API.Messages.INSTANTIATED, ObjectData.GetObjectData(weapon)));

                        if (!weaponData.CreateAimTarget) return;

                        MessageBus.SendMessage(SubscribeType.Channel, weaponData.ChannelId,
                            CommonMessage.Get(AimTarget.API.Messages.HIDE));

                        //MessageBus.SendMessage(CommonMessage.Get(
                        //    AimTarget.API.Messages.INSTANTIATE,
                        //        InstantiateData.GetInstantiateData(
                        //            weapon.GetComponent<WeaponData>().WeaponConfig.aimTargetPrefab,
                        //            weaponData.ChannelId)));
                    });
        }
        

    }
}