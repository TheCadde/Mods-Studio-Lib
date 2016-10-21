using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

using ModsStudioLib.Annotations;

namespace ModsStudioLib.Utils.Helpers {
    /// <summary>
    /// This is a wrapper for INotifyPropertyChanged that adds a method to raise a PropertyChanged events easily.
    /// </summary>
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public class BindableAttributeClass : Attribute, INotifyPropertyChanged {
        /// <summary>
        /// Fires when a property has changed on the derived class.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Raises the PropertyChanged event and returns the value supplied to it.
        /// <para>Used as "field = Set(value);" in property set block.</para>
        /// </summary>
        /// <typeparam name="T">Type of value, inferred from usage.</typeparam>
        /// <param name="value">The value to pass through.</param>
        /// <param name="result">What the value will be assigned to.</param>
        /// <param name="propertName">The property name to use in the PropertyChangedEvent(string).
        /// <para>This is inferred from usage and should be left unspecified unless one wants to override.</para></param>
        /// <returns></returns>
        protected T Set<T>(T value, out T result, [CallerMemberName] string propertName = null) {
            result = value;
            // ReSharper disable once ExplicitCallerInfoArgument
            Notify(propertName);
            return value;
        }

        /// <summary>
        /// Raises the PropertyChanged event for the (optionally) specified property name(s).
        /// </summary>
        /// <param name="propertyName">[OPTIONAL] If unspecified, the caller name is used.</param>
        /// <param name="additionalPropertyNames">[OPTIONAL] Additional property names to raise events for.</param>
        protected void Notify([CallerMemberName] string propertyName = null, params string[] additionalPropertyNames) {
            if (PropertyChanged == null)
                return;

            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            foreach (var name in additionalPropertyNames)
                PropertyChanged(this, new PropertyChangedEventArgs(name));
        }
    }
}
