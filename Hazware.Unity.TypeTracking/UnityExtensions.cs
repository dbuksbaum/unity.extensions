using System.Collections.Generic;
using Microsoft.Practices.Unity;

namespace Hazware.Unity.TypeTracking
{
  /// <summary>
  /// Extension methods to simplify using the TypeTrackingExtension for Unity.
  /// Requires .NET 3.5
  /// </summary>
  public static class UnityExtensions
  {
    #region CanResolve
    /// <summary>
    /// Determines whether this type can be resolved as the default.
    /// </summary>
    /// <typeparam name="T">The type to test for resolution</typeparam>
    /// <param name="container">The unity container.</param>
    /// <returns>
    ///     <c>true</c> if this instance can resolve; otherwise, <c>false</c>.
    /// </returns>
    public static bool CanResolve<T>(this IUnityContainer container)
    {
      return container.Configure<TypeTrackingExtension>().CanResolve<T>();
    }
    /// <summary>
    /// Determines whether this type can be resolved with the specified name.
    /// </summary>
    /// <typeparam name="T">The type to test for resolution</typeparam>
    /// <param name="container">The unity container.</param>
    /// <param name="name">The name associated with the type.</param>
    /// <returns>
    ///     <c>true</c> if this instance can resolve; otherwise, <c>false</c>.
    /// </returns>
    public static bool CanResolve<T>(this IUnityContainer container, string name)
    {
      return container.Configure<TypeTrackingExtension>().CanResolve<T>(name);
    }
    /// <summary>
    /// Determines whether this instance can be resolved at all with or without a name.
    /// </summary>
    /// <typeparam name="T">The type to test for resolution</typeparam>
    /// <param name="container">The unity container.</param>
    /// <returns>
    ///     <c>true</c> if this instance can resolve; otherwise, <c>false</c>.
    /// </returns>
    public static bool CanResolveAny<T>(this IUnityContainer container)
    {
      return container.Configure<TypeTrackingExtension>().CanResolveAny<T>();
    }
    #endregion

    #region TryResolve
    /// <summary>
    /// Tries to resolve the type, returning null if not found.
    /// </summary>
    /// <typeparam name="T">The type to try and resolve</typeparam>
    /// <param name="container">The unity container.</param>
    /// <returns>An object of type <see cref="T"/> if found, or <c>null</c> if not.</returns>
    public static T TryResolve<T>(this IUnityContainer container)
    {
      return container.Configure<TypeTrackingExtension>().TryResolve<T>();
    }
    /// <summary>
    /// Tries to resolve the type with the specified of name, returning null if not found.
    /// </summary>
    /// <typeparam name="T">The type to try and resolve</typeparam>
    /// <param name="container">The unity container.</param>
    /// <param name="name">The name associated with the type.</param>
    /// <returns>An object of type <see cref="T"/> if found, or <c>null</c> if not.</returns>
    public static T TryResolve<T>(this IUnityContainer container, string name)
    {
      return container.Configure<TypeTrackingExtension>().TryResolve<T>(name);
    }
    /// <summary>
    /// Tries to resolve the type, returning the passed in defaultValue if not found.
    /// </summary>
    /// <typeparam name="T">The type to try and resolve</typeparam>
    /// <param name="container">The unity container.</param>
    /// <param name="defaultValue">The default value.</param>
    /// <returns>An object of type <see cref="T"/> if found, or the <see cref="defaultValue"/> if not.</returns>
    public static T TryResolve<T>(this IUnityContainer container, T defaultValue)
    {
      return container.Configure<TypeTrackingExtension>().TryResolve<T>(defaultValue);
    }
    /// <summary>
    /// Tries to resolve the type, returning the passed in defaultValue if not found.
    /// </summary>
    /// <typeparam name="T">The type to try and resolve</typeparam>
    /// <param name="container">The unity container.</param>
    /// <param name="name">The name associated with the type.</param>
    /// <param name="defaultValue">The default value.</param>
    /// <returns>An object of type <see cref="T"/> if found, or the <see cref="defaultValue"/> if not.</returns>
    public static T TryResolve<T>(this IUnityContainer container, string name, T defaultValue)
    {
      return container.Configure<TypeTrackingExtension>().TryResolve<T>(name, defaultValue);
    }
    #endregion

    #region ResolveAll
    /// <summary>
    /// Resolves all elements of a type, including the default.
    /// </summary>
    /// <typeparam name="T">The type to resolve</typeparam>
    /// <param name="container">The unity container.</param>
    /// <returns><see cref="IEnumerable"/> of T</returns>
    public static IEnumerable<T> ResolveAllToEnumerable<T>(this IUnityContainer container)
    {
      return container.Configure<TypeTrackingExtension>().ResolveAll<T>();
    }
    /// <summary>
    /// Resolves all registered T in the container, conditionally including the default unnamed
    /// registered T. When includeDefault is false, this is the same as the normal Unity
    /// ResolveAll.
    /// </summary>
    /// <typeparam name="T">The type to resolve</typeparam>
    /// <param name="container">The unity container.</param>
    /// <param name="includeDefault">if set to <c>true</c> include default value, else do not include default.</param>
    /// <returns><see cref="IEnumerable"/> of T</returns>
    public static IEnumerable<T> ResolveAllToEnumerable<T>(this IUnityContainer container, bool includeDefault)
    {
      return container.Configure<TypeTrackingExtension>().ResolveAll<T>(includeDefault);
    }
    /// <summary>
    /// Resolves all elements of a type, including the default, and returns 
    /// as an array of T.
    /// </summary>
    /// <typeparam name="T">The type to resolve</typeparam>
    /// <param name="container">The unity container.</param>
    /// <returns>Array of T</returns>
    public static T[] ResolveAllToArray<T>(this IUnityContainer container)
    {
      return container.Configure<TypeTrackingExtension>().ResolveAllToArray<T>();
    }
    /// <summary>
    /// Resolves all registered T in the container, conditionally including the default unnamed
    /// registered T. When includeDefault is false, this is the same as the normal Unity
    /// ResolveAll.
    /// </summary>
    /// <typeparam name="T">The type to resolve</typeparam>
    /// <param name="container">The unity container.</param>
    /// <param name="includeDefault">if set to <c>true</c> include default value, else do not include default.</param>
    /// <returns>Array of T</returns>
    public static T[] ResolveAllToArray<T>(this IUnityContainer container, bool includeDefault)
    {
      return container.Configure<TypeTrackingExtension>().ResolveAllToArray<T>(includeDefault);
    }
    #endregion
  }
}
