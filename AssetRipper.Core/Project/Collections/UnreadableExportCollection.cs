﻿using AssetRipper.Assets;
using AssetRipper.Assets.Collections;
using AssetRipper.Assets.Export;
using AssetRipper.Assets.Metadata;
using AssetRipper.Core.Classes;
using AssetRipper.Core.Project.Exporters;
using AssetRipper.IO.Files.SerializedFiles;
using System.Collections.Generic;
using System.IO;

namespace AssetRipper.Core.Project.Collections
{
	public sealed class UnreadableExportCollection : ExportCollection
	{
		UnreadableObject Asset { get; }
		public override IAssetExporter AssetExporter { get; }

		public UnreadableExportCollection(IAssetExporter exporter, UnreadableObject asset)
		{
			Asset = asset;
			AssetExporter = exporter;
		}

		public override AssetCollection File => Asset.Collection;

		public override TransferInstructionFlags Flags => Asset.Collection.Flags;

		public override IEnumerable<IUnityObjectBase> Assets
		{
			get { yield return Asset; }
		}

		public override string Name => Asset.NameString;

		public override MetaPtr CreateExportPointer(IUnityObjectBase asset, bool isLocal)
		{
			return MetaPtr.NullPtr;
		}

		public override bool Export(IProjectAssetContainer container, string projectDirectory)
		{
			string resourcePath = Path.Combine(projectDirectory, "AssetRipper", "UnreadableAssets", Asset.ClassName, $"{Asset.NameString}.unreadable");
			string subPath = Path.GetDirectoryName(resourcePath)!;
			Directory.CreateDirectory(subPath);
			string resFileName = Path.GetFileName(resourcePath);
			string fileName = GetUniqueFileName(subPath, resFileName);
			string filePath = Path.Combine(subPath, fileName);
			return AssetExporter.Export(container, Asset, filePath);
		}

		public override long GetExportID(IUnityObjectBase asset)
		{
			throw new NotSupportedException();
		}

		public override bool IsContains(IUnityObjectBase asset)
		{
			return asset == Asset;
		}
	}
}
