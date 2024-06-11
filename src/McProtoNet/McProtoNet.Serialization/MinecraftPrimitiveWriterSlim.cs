﻿using Cysharp.Text;
using DotNext.Buffers;
using McProtoNet.NBT;
using System.Buffers;
using System.Runtime.CompilerServices;

namespace McProtoNet.Serialization
{
	/// <summary>
	/// Represents stack-allocated writer for primitive types of Minecraft
	/// </summary>
	public ref struct MinecraftPrimitiveWriterSlim
	{
		private static readonly MemoryAllocator<byte> s_allocator = ArrayPool<byte>.Shared.ToAllocator<byte>();

		private BufferWriterSlim<byte> writerSlim = new BufferWriterSlim<byte>(10, s_allocator);
		public MinecraftPrimitiveWriterSlim()
		{

		}
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void WriteBoolean(bool value)
		{
			writerSlim.Write(value ? 1 : 0);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void WriteSignedByte(sbyte value)
		{
			writerSlim.Write((byte)value);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void WriteUnsignedByte(byte value)
		{
			writerSlim.Write(value);
		}


		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void WriteUnsignedShort(ushort value)
		{
			writerSlim.WriteBigEndian(value);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void WriteShort(short value)
		{
			writerSlim.WriteBigEndian(value);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void WriteInt(int value)
		{
			writerSlim.WriteBigEndian(value);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void WriteUnsignedInt(uint value)
		{
			writerSlim.WriteBigEndian(value);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void WriteLong(long value)
		{
			writerSlim.WriteBigEndian(value);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void WriteUnsignedLong(ulong value)
		{
			writerSlim.WriteBigEndian(value);
		}

		

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void WriteFloat(float value)
		{
			int val = BitConverter.SingleToInt32Bits(value);
			writerSlim.WriteBigEndian(val);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void WriteDouble(float value)
		{
			long val = BitConverter.DoubleToInt64Bits(value);
			writerSlim.WriteBigEndian(val);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void WriteUUID(Guid value)
		{
			Span<byte> span = writerSlim.GetSpan(16);
			if (!value.TryWriteBytes(span))
			{
				throw new InvalidOperationException("Guid no write");
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Write(ReadOnlySpan<byte> value)
		{
			writerSlim.Write(value);
		}


		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void WriteVarInt(int value)
		{
			Span<byte> data = stackalloc byte[5];

			var unsigned = (uint)value;

			byte len = 0;
			do
			{
				var temp = (byte)(unsigned & 127);
				unsigned >>= 7;

				if (unsigned != 0)
					temp |= 128;

				data[len++] = temp;
			}
			while (unsigned != 0);
			if (len > 5)
				throw new ArithmeticException("Var int is too big");

			writerSlim.Write(data.Slice(0, len));
		}


		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void WriteVarLong(long value)
		{
			var unsigned = (ulong)value;

			do
			{
				var temp = (byte)(unsigned & 127);

				unsigned >>= 7;

				if (unsigned != 0)
					temp |= 128;


				writerSlim.Write(temp);
			}
			while (unsigned != 0);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void WriteString(string value)
		{
			var builder = ZString.CreateUtf8StringBuilder();
			try
			{
				builder.Append(value);
				WriteVarInt(builder.Length);
				writerSlim.Write(builder.AsSpan());
			}
			finally
			{
				builder.Dispose();
			}
		}


		public void WriteNbt(NbtCompound value)
		{
			throw new InvalidOperationException();
		}

		public void Clear(bool reuseBuffer = false) => writerSlim.Clear(reuseBuffer);

		public MemoryOwner<byte> GetWrittenMemory()
		{
			if (!writerSlim.TryDetachBuffer(out var result))
				throw new InvalidOperationException("Don't detach buffer");
			return result;
		}
	}
}
