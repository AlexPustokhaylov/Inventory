// Aleksey 061WR Pustohaylov [stilluswr@gmail.com]
// Last edit: 21-02-2014
using System.Collections.Generic;
using Assets.Classes;
using UnityEngine;

// ReSharper disable UnusedMember.Global
// ReSharper disable UnassignedField.Compiler

namespace Assets.Scripts
{
    internal class CraftingCreateNewItemComponent : MonoBehaviour
    {
        #region Data
        //===============================================================================================[]
        private readonly List<GameObject> _itemsList = new List<GameObject>();
        //===============================================================================================[]
        #endregion




        #region Public data
        //===============================================================================================[]
        public GameObject Item1X1Prefab;
        public GameObject Item2X1Prefab;
        public GameObject Item1X2Prefab;
        public GameObject Item2X2Prefab;
        //===============================================================================================[]
        #endregion




        #region MonoBehaviour
        //===============================================================================================[]
        public void Start()
        {
            EssenceManagerHelper.OnCraftNewItem += OnNeedCraftItem;

            _itemsList.Add( Item1X1Prefab );
            _itemsList.Add( Item2X1Prefab );
            _itemsList.Add( Item1X2Prefab );
            _itemsList.Add( Item2X2Prefab );
        }

        //===============================================================================================[]
        #endregion




        #region Routines
        //===============================================================================================[]
        private void OnNeedCraftItem()
        {
            if( !EssenceManagerHelper.ResultCraftingItemPlace.ContainsItem() ) {
                var itemIndex = Random.Range( 0, _itemsList.Count );
                var newItemPrefab = _itemsList[ itemIndex ];
                var newItemPrefabTransform = newItemPrefab.transform;
                var newItem = NGUITools.AddChild( gameObject, newItemPrefab );
                newItem.transform.localScale = new Vector3(
                    newItemPrefabTransform.localScale.x,
                    newItemPrefabTransform.localScale.y,
                    newItemPrefabTransform.localScale.z );

                var newItemId = UnityHelper.FindComponentWithoutCheck<DraggableItemComponent>().ItemId;

                EssenceManagerHelper.ResultCraftingItemPlace.AddItem( newItemId );
                newItem.name = newItemId + ". " + newItem.name;
            }
        }

        //===============================================================================================[]
        #endregion
    }
}