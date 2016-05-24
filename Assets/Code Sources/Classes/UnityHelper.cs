// Aleksey 061WR Pustohaylov [stilluswr@gmail.com]
// Last edit: 21-02-2014
using System.Linq;
using UnityEngine;

namespace Assets.Scripts
{
    internal static class UnityHelper
    {
        internal static TComponent FindComponentWithoutCheck< TComponent >() where TComponent : MonoBehaviour
        {
            var type = typeof( TComponent );
            var component = Object.FindObjectOfType( type );
            return ( TComponent )component;
        }

        public static T GetInterface< T >( this GameObject inObj ) where T : class
        {
            return inObj.GetComponents<Component>().OfType<T>().FirstOrDefault();
        }
    }
}