﻿using AssetRipper.Core.SourceGenExtensions;
using AssetRipper.Numerics;
using AssetRipper.SourceGenerated.Subclasses.CompressedMesh;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace AssetRipper.Tests
{
	internal class CompressedMeshTests
	{
		private static readonly Random random = new Random(57089);
		private static readonly Vector3[] vectors = MakeUnitVectors(20);
		private static readonly Vector4[] tangents = MakeTangents(22);
		private static readonly Matrix4x4[] matrices = MakeMatrices(11);
		private static readonly uint[] integers = MakeUInts(24);

		private static Vector3[] MakeUnitVectors(int count)
		{
			Vector3[] result = new Vector3[count];
			for (int i = 0; i < count; i++)
			{
				float x = random.NextSingle();
				float y = random.NextSingle();
				float z = random.NextSingle();
				result[i] = Vector3.Normalize(new Vector3(x, y, z));
			}
			return result;
		}

		private static Vector4[] MakeTangents(int count)
		{
			Vector3[] unitVectors = MakeUnitVectors(count);
			Vector4[] result = new Vector4[unitVectors.Length];
			for (int i = 0; i < unitVectors.Length; i++)
			{
				float w = random.NextSingle() < 0.5f ? -1f : 1f;
				result[i] = new Vector4(unitVectors[i], w);
			}
			return result;
		}

		private static uint[] MakeUInts(int count)
		{
			uint[] result = new uint[count];
			for (int i = 0; i < count; i++)
			{
				result[i] = unchecked((uint)random.Next());
			}
			return result;
		}

		private static Matrix4x4[] MakeMatrices(int count)
		{
			Matrix4x4[] result = new Matrix4x4[count];
			for (int i = 0; i < count; i++)
			{
				result[i] = new Matrix4x4(
					random.NextSingle(),
					random.NextSingle(),
					random.NextSingle(),
					random.NextSingle(),
					random.NextSingle(),
					random.NextSingle(),
					random.NextSingle(),
					random.NextSingle(),
					random.NextSingle(),
					random.NextSingle(),
					random.NextSingle(),
					random.NextSingle(),
					random.NextSingle(),
					random.NextSingle(),
					random.NextSingle(),
					random.NextSingle());
			}
			return result;
		}

		[Test]
		public void VertexAssignmentSymmetry()
		{
			CompressedMesh_5_0_0_f4 compressedMesh = new();
			compressedMesh.SetVertices(vectors);
			Vector3[] unpackedValues = compressedMesh.GetVertices();
			AreAlmostEqual(vectors, unpackedValues, 0.000001f);
		}

		[Test]
		public void NormalAssignmentSymmetry()
		{
			CompressedMesh_5_0_0_f4 compressedMesh = new();
			compressedMesh.SetNormals(vectors);
			Vector3[] unpackedValues = compressedMesh.GetNormals();
			AreAlmostEqual(vectors, unpackedValues, 0.00001f);
			//Note: this symmetry only happens because the vectors are already normalized.
			//This test would (and should) fail if non-normalized vectors are use.
		}

		[Test]
		public void TangentAssignmentSymmetry()
		{
			CompressedMesh_5_0_0_f4 compressedMesh = new();
			compressedMesh.SetTangents(tangents);
			Vector4[] unpackedValues = compressedMesh.GetTangents();
			AreAlmostEqual(tangents, unpackedValues, 0.00001f);
			//Note: this symmetry only happens because the vectors are already normalized.
			//This test would (and should) fail if non-normalized vectors are use.
		}

		[Test]
		public void FloatColorsNormalAssignmentSymmetry()
		{
			CompressedMesh_5_0_0_f4 compressedMesh = new();
			//These are technically invalid colors since they have values outside [0,1] but it doesn't matter for the test.
			ReadOnlySpan<ColorFloat> colors = MemoryMarshal.Cast<Vector4, ColorFloat>(tangents);
			compressedMesh.SetFloatColors(colors);
			ColorFloat[] unpackedValues = compressedMesh.GetFloatColors();
			AreAlmostEqual(MemoryMarshal.Cast<ColorFloat, Vector4>(colors), MemoryMarshal.Cast<ColorFloat, Vector4>(unpackedValues), 0.00001f);
		}

		[Test]
		public void BindPoseAssignmentSymmetry()
		{
			CompressedMesh_3_0_0_f5 compressedMesh = new();//BindPoses is only on versions before Unity 5
			compressedMesh.SetBindPoses(matrices);
			Matrix4x4[] unpackedValues = compressedMesh.GetBindPoses();
			AreAlmostEqual(matrices, unpackedValues, 0.000001f);
		}

		[Test]
		public void TriangleAssignmentSymmetry()
		{
			CompressedMesh_5_0_0_f4 compressedMesh = new();
			compressedMesh.SetTriangles(integers);
			uint[] unpackedValues = compressedMesh.GetTriangles();
			Assert.AreEqual(integers, unpackedValues);
		}

		private static void AreAlmostEqual(ReadOnlySpan<Vector3> expected, ReadOnlySpan<Vector3> actual, float maxDeviation)
		{
			if (expected.Length != actual.Length)
			{
				Assert.Fail($"Lengths were inequal.\nExpected: {expected.Length}\nBut was: {actual.Length}");
			}

			for (int i = 0; i < expected.Length; i++)
			{
				if (Vector3.Distance(expected[i], actual[i]) > maxDeviation)
				{
					Assert.Fail($"Values significantly differ at index {i}\nExpected: {expected[i]}\nBut was: {actual[i]}");
				}
			}
		}

		private static void AreAlmostEqual(ReadOnlySpan<Vector4> expected, ReadOnlySpan<Vector4> actual, float maxDeviation)
		{
			if (expected.Length != actual.Length)
			{
				Assert.Fail($"Lengths were inequal.\nExpected: {expected.Length}\nBut was: {actual.Length}");
			}

			for (int i = 0; i < expected.Length; i++)
			{
				if (Vector4.Distance(expected[i], actual[i]) > maxDeviation)
				{
					Assert.Fail($"Values significantly differ at index {i}\nExpected: {expected[i]}\nBut was: {actual[i]}");
				}
			}
		}

		private static void AreAlmostEqual(ReadOnlySpan<float> expected, ReadOnlySpan<float> actual, float maxDeviation)
		{
			if (expected.Length != actual.Length)
			{
				Assert.Fail($"Lengths were inequal.\nExpected: {expected.Length}\nBut was: {actual.Length}");
			}

			for (int i = 0; i < expected.Length; i++)
			{
				if (MathF.Abs(expected[i] - actual[i]) > maxDeviation)
				{
					Assert.Fail($"Values significantly differ at index {i}\nExpected: {expected[i]}\nBut was: {actual[i]}");
				}
			}
		}

		private static void AreAlmostEqual(ReadOnlySpan<Matrix4x4> expected, ReadOnlySpan<Matrix4x4> actual, float maxDeviation)
		{
			AreAlmostEqual(MemoryMarshal.Cast<Matrix4x4, float>(expected), MemoryMarshal.Cast<Matrix4x4, float>(actual), maxDeviation);
		}
	}
}
