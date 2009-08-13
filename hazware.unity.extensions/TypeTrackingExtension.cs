using System;
using System.Collections.Generic;
using Microsoft.Practices.Unity;

namespace hazware.unity.extensions
{
  /// <summary>
  /// Extension class for Unity that tracks types that are registered.
  /// </summary>
  /// <example>
  /// var container = new UnityContainer();
  /// container.AddNewExtension<TypeTrackingExtension>();
  /// container.RegisterType<ITest, TestClass>();
  /// var obj = container.Configure<TypeTrackingExtension>().TryResolve<ITest>();
  /// </example>
  public class TypeTrackingExtension : UnityContainerExtension
  {
    #region Fields
    private readonly Dictionary<Type, HashSet<string>> _registeredTypes = new Dictionary<Type, HashSet<string>>();
    #endregion

    #region Overrides
    protected override void Initialize()
    {
      Context.RegisteringInstance += OnNewInstance;
      Context.Registering += OnNewType;
    }
    public override void Remove()
    {
      Context.Registering -= OnNewType;
      Context.RegisteringInstance -= OnNewInstance;
    }
    #endregion

    #region Private Methods
    private void OnNewInstance(object sender, RegisterInstanceEventArgs e)
    {
      HashSet<string> names;
      string name = string.IsNullOrEmpty(e.Name) ? string.Empty : e.Name;

      if (!_registeredTypes.TryGetValue(e.RegisteredType, out names))
      { //  not found, so add it
        _registeredTypes.Add(e.RegisteredType, new HashSet<string>(new string[] { name }));
      }
      else
      { //  already added type, so add name
        names.Add(name);
      }
    }
    private void OnNewType(object sender, RegisterEventArgs e)
    {
      HashSet<string> names;
      string name = string.IsNullOrEmpty(e.Name) ? string.Empty : e.Name;
      if (!_registeredTypes.TryGetValue(e.TypeFrom, out names))
      { //  not found, so add it
        _registeredTypes.Add(e.TypeFrom, new HashSet<string>(new string[] { name }));
      }
      else
      { //  already added type, so add name
        names.Add(name);
      }
    }
    #endregion

    #region CanResolve
    /// <summary>
    /// Determines whether this type can be resolved as the default.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns>
    ///     <c>true</c> if this instance can resolve; otherwise, <c>false</c>.
    /// </returns>
    public bool CanResolve<T>()
    {
      return CanResolve<T>(null);
    }
    /// <summary>
    /// Determines whether this type can be resolved with the specified name.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="name">The name.</param>
    /// <returns>
    ///     <c>true</c> if this instance can be resolved with the specified name; otherwise, <c>false</c>.
    /// </returns>
    public bool CanResolve<T>(string name)
    {
      HashSet<string> names;
      if (_registeredTypes.TryGetValue(typeof(T), out names))
      {
        return names.Contains(name ?? string.Empty);
      }
      return false;
    }
    /// <summary>
    /// Determines whether this instance can be resolved at all.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns>
    ///     <c>true</c> if this instance can be resolved at all; otherwise, <c>false</c>.
    /// </returns>
    public bool CanResolveAny<T>()
    {
      return _registeredTypes.ContainsKey(typeof(T));
    }
    #endregion

    #region TryResolve
    /// <summary>
    /// Tries to resolve the type, returning null if not found.
    /// </summary>
    /// <typeparam name="T">The type to try and resolve.</typeparam>
    /// <returns>An object of type <see cref="T"/> if found, or <c>null</c> if not.</returns>
    public T TryResolve<T>()
    {
      return TryResolve<T>(default(T));
    }
    /// <summary>
    /// Tries to resolve the type with the specified of name, returning null if not found.
    /// </summary>
    /// <typeparam name="T">The type to try and resolve.</typeparam>
    /// <param name="name">The name associated with the type.</param>
    /// <returns>An object of type <see cref="T"/> if found, or <c>null</c> if not.</returns>
    public T TryResolve<T>(string name)
    {
      return TryResolve<T>(name, default(T));
    }
    /// <summary>
    /// Tries to resolve the type, returning null if not found.
    /// </summary>
    /// <typeparam name="T">The type to try and resolve.</typeparam>
    /// <param name="defaultValue">The default value to return if type not found.</param>
    /// <returns>An object of type <see cref="T"/> if found, or the <see cref="defaultValue"/> if not.</returns>
    public T TryResolve<T>(T defaultValue)
    {
      if (!CanResolve<T>())
        return defaultValue;
      return Container.Resolve<T>();
    }
    /// <summary>
    /// Tries to resolve the type with the specified of name, returning null if not found.
    /// </summary>
    /// <typeparam name="T">The type to try and resolve.</typeparam>
    /// <param name="name">The name associated with the type.</param>
    /// <param name="defaultValue">The default value to return if type not found.</param>
    /// <returns>An object of type <see cref="T"/> if found, or the <see cref="defaultValue"/> if not.</returns>
    public T TryResolve<T>(string name, T defaultValue)
    {
      if (!CanResolve<T>(name))
        return defaultValue;
      return Container.Resolve<T>(name);
    }
    #endregion

    #region ResolveAllToArray
    /// <summary>
    /// Resolves all elements of a type, including the default.
    /// </summary>
    /// <typeparam name="T">The type to resolve</typeparam>
    /// <returns><see cref="IEnumerable"/> of T</returns>
    public IEnumerable<T> ResolveAll<T>()
    {
      return ResolveAll<T>(true);
    }
    /// <summary>
    /// Resolves all registered T in the container, conditionally including the default unnamed
    /// registered T. When includeDefault is false, this is the same as the normal Unity
    /// ResolveAll.
    /// </summary>
    /// <typeparam name="T">The type to resolve</typeparam>
    /// <param name="includeDefault">if set to <c>true</c> include default value, else do not include default.</param>
    /// <returns><see cref="IEnumerable"/> of T</returns>
    public IEnumerable<T> ResolveAll<T>(bool includeDefault)
    {
      List<T> elements = new List<T>(Container.ResolveAll<T>());
      if (includeDefault && CanResolve<T>()) //  can resolve default element?
        elements.Add(Container.Resolve<T>()); // then add it
      return elements;
    }
    /// <summary>
    /// Resolves all elements of a type, including the default, and returns 
    /// as an array of T.
    /// </summary>
    /// <typeparam name="T">The type to resolve</typeparam>
    /// <returns>Array of T</returns>
    public T[] ResolveAllToArray<T>()
    {
      return ResolveAllToArray<T>(true);
    }
    /// <summary>
    /// Resolves all registered T in the container, conditionally including the default unnamed
    /// registered T. When includeDefault is false, this is the same as the normal Unity
    /// ResolveAll.
    /// </summary>
    /// <typeparam name="T">The type to resolve</typeparam>
    /// <param name="includeDefault">if set to <c>true</c> include default value, else do not include default.</param>
    /// <returns>Array of T</returns>
    public T[] ResolveAllToArray<T>(bool includeDefault)
    {
      List<T> elements = new List<T>(Container.ResolveAll<T>());
      if (includeDefault && CanResolve<T>()) //  can resolve default element?
        elements.Add(Container.Resolve<T>()); // then add it
      return elements.ToArray();
    }
    #endregion
  }
}
