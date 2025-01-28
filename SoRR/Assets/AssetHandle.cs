namespace SoRR
{
    /// <summary>
    ///   <para>Represents a handle for an asset, loaded and managed by an <see cref="SoRR.AssetManager"/>.</para>
    /// </summary>
    public sealed class AssetHandle
    {
        /// <summary>
        ///   <para>Gets the asset manager that is responsible for this asset.</para>
        /// </summary>
        public AssetManager AssetManager { get; }
        /// <summary>
        ///   <para>Gets the asset's path, relative to its asset manager.</para>
        /// </summary>
        public string RelativePath { get; }

        /// <summary>
        ///   <para>Gets or sets the asset's current value. When changed, triggers the global <see cref="Assets.OnAssetReload"/> event.</para>
        /// </summary>
        public object? Value
        {
            get;
            set
            {
                if (field == value) return;
                field = value;
                Assets.TriggerAssetReload(this);
            }
        }

        internal AssetHandle(AssetManager assetManager, string relativePath, object? currentValue)
        {
            AssetManager = assetManager;
            RelativePath = relativePath;
            Value = currentValue;
        }

    }
}
