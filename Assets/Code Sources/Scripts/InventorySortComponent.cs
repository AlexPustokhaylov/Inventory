// Aleksey 061WR Pustohaylov [stilluswr@gmail.com]
// Last edit: 21-02-2014
using Assets.Classes;
using Assets.Interfaces;
using UnityEngine;

// ReSharper disable UnusedMember.Global
// ReSharper disable UnassignedField.Compiler

namespace Assets.Scripts
{
    internal class InventorySortComponent : MonoBehaviour
    {
        #region Data
        //===============================================================================================[]
        private IInventory Inventory { get { return EssenceManagerHelper.Inventory; } }
        //===============================================================================================[]
        #endregion




        #region MonoBehaviour
        //===============================================================================================[]
        public void OnClick()
        {
            Inventory.SortItems();
        }

        //===============================================================================================[]
        #endregion
    }
}