using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using JetBrains.Annotations;

namespace SoRR
{
    /// <summary>
    ///   <para>Provides a set of static methods to manage asset managers and load assets.</para>
    /// </summary>
    public static class Assets
    {
        private static readonly StringKeyedDictionary<AssetManager> managers = new();

        /// <summary>
        ///   <para>Adds the specified asset <paramref name="assetManager"/> to the global registry under the specified <paramref name="prefix"/>.</para>
        /// </summary>
        /// <param name="assetManager">The asset manager to register under the specified <paramref name="prefix"/>.</param>
        /// <param name="prefix">The global prefix to register the specified asset <paramref name="assetManager"/> under.</param>
        /// <exception cref="ArgumentNullException"><paramref name="assetManager"/> or <paramref name="prefix"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentException"><paramref name="assetManager"/> already has a registered prefix, or the specified prefix is already occupied by another asset manager.</exception>
        public static void RegisterAssetManager(AssetManager assetManager, string prefix)
        {
            Guard.ThrowIfNull(assetManager);
            Guard.ThrowIfNull(prefix);
            if (assetManager.registeredPrefix is not null)
                throw new ArgumentException("The specified manager already has a registered prefix.", nameof(assetManager));

            managers.Add(prefix, assetManager);
            assetManager.registeredPrefix = prefix;
        }
        /// <summary>
        ///   <para>Removes the specified asset <paramref name="assetManager"/> from the global registry.</para>
        /// </summary>
        /// <param name="assetManager">The asset manager to remove from the global registry.</param>
        /// <returns><see langword="true"/>, if the specified asset <paramref name="assetManager"/> was successfully removed; otherwise, <see langword="false"/>.</returns>
        public static bool UnRegisterAssetManager([NotNullWhen(true)] AssetManager? assetManager)
        {
            if (assetManager?.registeredPrefix is { } prefix && managers.Remove(prefix))
            {
                assetManager.registeredPrefix = null;
                return true;
            }
            return false;
        }

        /// <summary>
        ///   <para>Returns the handle for an asset at the specified <paramref name="fullPath"/>.</para>
        /// </summary>
        /// <param name="fullPath">A fully qualified path to the asset to get the handle of.</param>
        /// <returns>The handle for the specified asset, or <see langword="null"/> if it is not found.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="fullPath"/> is <see langword="null"/>.</exception>
        [Pure] public static AssetHandle? GetHandle(string fullPath)
        {
            Guard.ThrowIfNull(fullPath);
            return GetHandle(fullPath.AsSpan());
        }
        /// <summary>
        ///   <para>Returns the handle for an asset at the specified <paramref name="fullPath"/>.</para>
        /// </summary>
        /// <param name="fullPath">A fully qualified path to the asset to get the handle of.</param>
        /// <returns>The handle for the specified asset, or <see langword="null"/> if it is not found.</returns>
        [Pure] public static AssetHandle? GetHandle(ReadOnlySpan<char> fullPath)
        {
            SplitPath(fullPath, out var prefix, out var relativePath);
            return managers.TryGetValue(prefix, out AssetManager? manager)
                ? manager.GetHandle(relativePath)
                : null;
        }

        /// <summary>
        ///   <para>Loads an asset at the specified <paramref name="fullPath"/> and returns it.</para>
        /// </summary>
        /// <typeparam name="T">The type of the asset to load.</typeparam>
        /// <param name="fullPath">A fully qualified path to the asset to load.</param>
        /// <returns>The specified asset.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="fullPath"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentException"><paramref name="fullPath"/> does not specify a known asset manager.</exception>
        /// <exception cref="AssetNotFoundException">An asset at the specified <paramref name="fullPath"/> could not be found.</exception>
        /// <exception cref="InvalidCastException">An asset at the specified <paramref name="fullPath"/> could not be cast to type <typeparamref name="T"/>.</exception>
        [Pure] public static T Load<T>(string fullPath)
        {
            Guard.ThrowIfNull(fullPath);
            return Load<T>(fullPath.AsSpan());
        }
        /// <summary>
        ///   <para>Loads an asset at the specified <paramref name="fullPath"/> and returns it.</para>
        /// </summary>
        /// <typeparam name="T">The type of the asset to load.</typeparam>
        /// <param name="fullPath">A fully qualified path to the asset to load.</param>
        /// <returns>The specified asset.</returns>
        /// <exception cref="ArgumentException"><paramref name="fullPath"/> does not specify a known asset manager.</exception>
        /// <exception cref="AssetNotFoundException">An asset at the specified <paramref name="fullPath"/> could not be found.</exception>
        /// <exception cref="InvalidCastException">An asset at the specified <paramref name="fullPath"/> could not be cast to type <typeparamref name="T"/>.</exception>
        [Pure] public static T Load<T>(ReadOnlySpan<char> fullPath)
        {
            SplitPath(fullPath, out var prefix, out var relativePath);
            return managers.TryGetValue(prefix, out AssetManager? manager)
                ? manager.Load<T>(relativePath)
                : throw new ArgumentException("Could not find specified asset manager prefix.", nameof(fullPath));
        }

        /// <summary>
        ///   <para>Tries to load an asset at the specified <paramref name="fullPath"/>, and returns a value indicating whether the operation was successful.</para>
        /// </summary>
        /// <typeparam name="T">The type of the asset to load.</typeparam>
        /// <param name="fullPath">A fully qualified path to the asset to load.</param>
        /// <param name="asset">When this method returns, contains the specified asset, if it was successfully loaded, or <see langword="default"/> if it could not be loaded.</param>
        /// <returns><see langword="true"/>, if the specified asset was successfully loaded; otherwise, <see langword="false"/>.</returns>
        [Pure] public static bool TryLoad<T>(string fullPath, [NotNullWhen(true)] out T? asset)
        {
            if (fullPath is null)
            {
                asset = default;
                return false;
            }
            return TryLoad(fullPath.AsSpan(), out asset);
        }
        /// <summary>
        ///   <para>Tries to load an asset at the specified <paramref name="fullPath"/>, and returns a value indicating whether the operation was successful.</para>
        /// </summary>
        /// <typeparam name="T">The type of the asset to load.</typeparam>
        /// <param name="fullPath">A fully qualified path to the asset to load.</param>
        /// <param name="asset">When this method returns, contains the specified asset, if it was successfully loaded, or <see langword="default"/> if it could not be loaded.</param>
        /// <returns><see langword="true"/>, if the specified asset was successfully loaded; otherwise, <see langword="false"/>.</returns>
        [Pure] public static bool TryLoad<T>(ReadOnlySpan<char> fullPath, [NotNullWhen(true)] out T? asset)
        {
            SplitPath(fullPath, out var prefix, out var relativePath);
            if (managers.TryGetValue(prefix, out AssetManager? manager))
                return manager.TryLoad(relativePath, out asset);
            asset = default;
            return false;
        }

        private static void SplitPath(ReadOnlySpan<char> query, out ReadOnlySpan<char> prefix, out ReadOnlySpan<char> path)
        {
            int separatorIndex = query.IndexOf(":/");
            if (separatorIndex == -1)
            {
                if (query.Length > 0 && query[0] == '/')
                    query = query[1..];
                // [ '/' ] <path>
                prefix = default;
                path = query;
                return;
            }
            // <prefix> ':/' <path>
            prefix = query[..separatorIndex];
            path = query[(separatorIndex + 2)..];
        }

        private static ValueBag<Action<AssetHandle>> _reloadListeners = new();

        /// <summary>
        ///   <para>Specifies an event that is called when an asset is reloaded.</para>
        /// </summary>
        public static event Action<AssetHandle> OnAssetReload
        {
            add => _reloadListeners.Add(value);
            remove => _reloadListeners.Remove(value);
        }

        internal static void TriggerAssetReload(AssetHandle handle)
        {
            List<Exception>? exceptions = null;
            var listeners = _reloadListeners.Span;

            for (int i = 0; i < listeners.Length; i++)
            {
                try
                {
                    listeners[i](handle);
                }
                catch (Exception exception)
                {
                    (exceptions ??= []).Add(exception);
                }
            }
            if (exceptions is not null)
                throw new AggregateException(exceptions);
        }

    }
}
