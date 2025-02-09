﻿using AssetRipper.Assets;
using AssetRipper.Assets.Collections;
using AssetRipper.Assets.Export;
using AssetRipper.Assets.Interfaces;
using AssetRipper.Assets.Metadata;
using AssetRipper.Core.Logging;
using AssetRipper.Core.Project.Exporters;
using AssetRipper.IO.Files;
using AssetRipper.IO.Files.SerializedFiles;
using System.Collections.Generic;

namespace AssetRipper.Core.Project.Collections
{
	public class FailExportCollection : IExportCollection
	{
		public FailExportCollection(IAssetExporter assetExporter, IUnityObjectBase asset)
		{
			AssetExporter = assetExporter ?? throw new ArgumentNullException(nameof(assetExporter));
			m_asset = asset ?? throw new ArgumentNullException(nameof(asset));
		}

		public bool Export(IProjectAssetContainer container, string projectDirectory)
		{
			Logger.Log(LogType.Warning, LogCategory.Export, $"Unable to export asset {Name}");
			return false;
		}

		public bool IsContains(IUnityObjectBase asset)
		{
			return asset == m_asset;
		}

		public long GetExportID(IUnityObjectBase asset)
		{
			if (asset == m_asset)
			{
				return ExportIdHandler.GetMainExportID(m_asset);
			}
			throw new ArgumentException(null, nameof(asset));
		}

		public UnityGUID GetExportGUID(IUnityObjectBase _)
		{
			throw new NotSupportedException();
		}

		public MetaPtr CreateExportPointer(IUnityObjectBase asset, bool isLocal)
		{
			if (isLocal)
			{
				throw new ArgumentException(null, nameof(isLocal));
			}

			long exportId = GetExportID(asset);
			AssetType type = AssetExporter.ToExportType(asset);
			return new MetaPtr(exportId, UnityGUID.MissingReference, type);
		}

		public IAssetExporter AssetExporter { get; }
		public AssetCollection File => m_asset.Collection;
		public TransferInstructionFlags Flags => File.Flags;
		public IEnumerable<IUnityObjectBase> Assets
		{
			get { yield return m_asset; }
		}
		public string Name => m_asset is IHasNameString namedAsset ? namedAsset.GetNameNotEmpty() : m_asset.ClassName;

		private readonly IUnityObjectBase m_asset;
	}
}
