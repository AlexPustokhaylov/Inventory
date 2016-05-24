// Aleksey 061WR Pustohaylov [stilluswr@gmail.com]
// Last edit: 21-02-2014
using Assets.Classes;
using Assets.Interfaces;
using UnityEngine;

// ReSharper disable UnusedMember.Global

namespace Assets.Scripts
{
    internal class ComponentEssenceManagerComponent : MonoBehaviour
    {
        #region Public data
        //===============================================================================================[]
        public IEssenceWorld EssenceWorldInstance { get; private set; }
        //===============================================================================================[]
        #endregion




        #region MonoBehaviour
        //===============================================================================================[]
        public void Awake()
        {
            EssenceWorldInstance = new EssenceWorld();
        }

        //===============================================================================================[]
        #endregion
    }
}