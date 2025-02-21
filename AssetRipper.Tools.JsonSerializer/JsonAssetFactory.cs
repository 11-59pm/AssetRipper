﻿using AssetRipper.Assets;
using AssetRipper.Assets.Exceptions;
using AssetRipper.Assets.IO;
using AssetRipper.Assets.IO.Reading;
using AssetRipper.Assets.Metadata;
using AssetRipper.IO.Files.SerializedFiles.Parser;
using System;

namespace AssetRipper.Tools.JsonSerializer;

public sealed class JsonAssetFactory : AssetFactoryBase
{
	public override IUnityObjectBase? ReadAsset(AssetInfo assetInfo, AssetReader reader, int size, SerializedType type)
	{
		if (type.OldType.Nodes.Count > 0)
		{
			long basePosition = reader.BaseStream.Position;
			SerializableEntry entry = SerializableEntry.FromTypeTree(type.OldType);
			JsonAsset asset = new JsonAsset(assetInfo);
			asset.Read(reader, entry);
			IncorrectByteCountException.ThrowIf(reader.BaseStream, basePosition, size);
			return asset;
		}
		else
		{
			Console.WriteLine($"Asset could not be read because it has no type tree. ClassID: {assetInfo.ClassID} PathID: {assetInfo.PathID}");
			return null;
		}
	}
}
