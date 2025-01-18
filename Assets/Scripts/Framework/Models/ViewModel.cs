using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace MarkFramework
{
	public class ViewModel : MonoBehaviour
    {
        protected bool ChangePropertyAndNotify<T>(ref T currentValue, T newValue, [CallerMemberName] string propertyName = null)
        {
            if (newValue == null && currentValue == null)
            {
                return false;
            }

            if (newValue != null && newValue.Equals(currentValue))
            {
                return false;
            }

            currentValue = newValue;

            RaisePropertyChanged(propertyName, newValue);

            return true;
        }

        protected virtual void RaisePropertyChanged(string propertyName, object value = null)
        {
            EventCenter.Instance.EventTrigger(E_EventType.E_Raise_Property, new PropertyValueChangedEventArgs(propertyName, value));
        }
    }
}
