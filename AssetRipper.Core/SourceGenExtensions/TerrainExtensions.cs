﻿using AssetRipper.SourceGenerated.Classes.ClassID_218;
using AssetRipper.SourceGenerated.Enums;

namespace AssetRipper.Core.SourceGenExtensions
{
	public static class TerrainExtensions
	{
		public static void ConvertToEditorFormat(this ITerrain terrain)
		{
			terrain.ScaleInLightmap_C218 = 0.0512f;
		}

		public static ShadowCastingMode GetShadowCastingMode(this ITerrain terrain)
		{
			if (terrain.Has_ShadowCastingMode_C218())
			{
				return terrain.ShadowCastingMode_C218E;
			}
			else
			{
				return terrain.CastShadows_C218 ? ShadowCastingMode.TwoSided : ShadowCastingMode.Off;
			}
		}

		public static bool GetCastShadows(this ITerrain terrain)
		{
			if (terrain.Has_CastShadows_C218())
			{
				return terrain.CastShadows_C218;
			}
			else
			{
				return terrain.ShadowCastingMode_C218E != ShadowCastingMode.Off;
			}
		}
	}
}
