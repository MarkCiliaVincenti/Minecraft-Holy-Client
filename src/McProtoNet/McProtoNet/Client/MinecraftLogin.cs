﻿

using DotNext.Buffers;
using McProtoNet.Cryptography;
using McProtoNet.Protocol;
using System.Buffers;
using System.Runtime.CompilerServices;
using System.Text;
using System.Xml.Linq;

namespace McProtoNet.Client
{
	public sealed class MinecraftLogin
	{
		private readonly static byte[] VarIntLoginIntent;
		private static MemoryAllocator<byte> s_allocator = ArrayPool<byte>.Shared.ToAllocator();

		static MinecraftLogin()
		{
			MemoryStream ms = new MemoryStream();
			ms.WriteVarInt(2);
			VarIntLoginIntent = ms.ToArray();
		}


		public async Task<LoginizationResult> Login(Stream source, LoginOptions options, CancellationToken cancellationToken = default)
		{
			AesStream mainStream = new AesStream(source);

			using MinecraftPacketSender sender = new MinecraftPacketSender();
			using MinecraftPacketReader reader = new MinecraftPacketReader();

			sender.BaseStream = mainStream;
			reader.BaseStream = mainStream;

			using var handshake = CreateHandshake(options.Host, options.Port, options.ProtocolVersion);
			await sender.SendPacketAsync(handshake, cancellationToken).ConfigureAwait(false);


			using var loginStart = CreateLoginStart(options.Username);
			await sender.SendPacketAsync(loginStart, cancellationToken).ConfigureAwait(false);

			int threshold = 0;


			while (true)
			{
				InputPacket inputPacket = await reader.ReadNextPacketAsync().ConfigureAwait(false);

				bool needBreak = false;

				switch (inputPacket.Id)
				{
					case 0x00:
						//Disconnect
						Console.WriteLine("Disconnect");
						inputPacket.Data.TryReadString(out string reason, out _);
						throw new LoginRejectedException(reason);
						break;
					case 0x01:
						Console.WriteLine("Encrypt");

						var encryptBegin = ReadEncryptionPacket(inputPacket);

						var RSAService = CryptoHandler.DecodeRSAPublicKey(encryptBegin.PublicKey);
						var secretKey = CryptoHandler.GenerateAESPrivateKey();


						byte[] sharedSecret = RSAService.Encrypt(secretKey, false);
						byte[] verifyToken = RSAService.Encrypt(encryptBegin.VerifyToken, false);

						using (var response = CreateEncryptionResponse(sharedSecret, verifyToken))
						{
							await sender.SendPacketAsync(response, cancellationToken);
						}

						mainStream.SwitchEncryption(secretKey);

						break;
					case 0x02:
						//Success
						Console.WriteLine("Succes");
						needBreak = true;
						break;
					case 0x03:
						//Compress
						Console.WriteLine("Compress");
						inputPacket.Data.TryReadVarInt(out threshold, out _);
						reader.SwitchCompression(threshold);
						sender.SwitchCompression(threshold);
						break;
					case 0x04:
						//Login plugin request
						ReadOnlySequence<byte> buffer = inputPacket.Data;
						int offset = 0;
						buffer.TryReadVarInt(out int messageId, out offset);
						buffer = buffer.Slice(offset);
						buffer.TryReadString(out string channel, out offset);
						ReadOnlySequence<byte> data = buffer.Slice(offset);
						break;

					default: throw new Exception("Unknown packet: " + inputPacket.Id);
				}

				if (needBreak)
					break;
			}



			return new LoginizationResult(mainStream, threshold);
		}


		private static EncryptionBeginPacket ReadEncryptionPacket(InputPacket inputPacket)
		{
			scoped SequenceReader<byte> reader = new SequenceReader<byte>(inputPacket.Data);

			reader.TryReadString(out string serverId);

			reader.TryReadVarInt(out int len, out _);

			byte[] publicKey = reader.UnreadSequence.Slice(0, len).ToArray();
			reader.Advance(len);

			reader.TryReadVarInt(out len, out _);
			byte[] verifyToken = reader.UnreadSequence.Slice(0, len).ToArray();


			return new EncryptionBeginPacket(serverId, publicKey, verifyToken);
		}

		private static OutputPacket CreateEncryptionResponse(byte[] sharedSecret, byte[] verifyToken)
		{
			int length = sharedSecret.Length + verifyToken.Length + 4;
			scoped BufferWriterSlim<byte> writer = new BufferWriterSlim<byte>(length, s_allocator);

			try
			{
				writer.WriteVarInt(0x01);
				writer.WriteBuffer(sharedSecret);
				writer.WriteBuffer(verifyToken);


				writer.TryDetachBuffer(out MemoryOwner<byte> buffer);
				return new OutputPacket(buffer);
			}
			finally
			{
				writer.Dispose();
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static OutputPacket CreateLoginStart(string name)
		{
			if (name.Length > 16)
			{
				throw new ArgumentOutOfRangeException();
			}
			scoped BufferWriterSlim<byte> writer = new BufferWriterSlim<byte>(10, s_allocator);

			try
			{
				writer.WriteVarInt(0x00); //Packet Id
				writer.WriteString(name);
				writer.TryDetachBuffer(out MemoryOwner<byte> buffer);
				return new OutputPacket(buffer);
			}
			finally
			{
				writer.Dispose();
			}

		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static OutputPacket CreateHandshake(string host, ushort port, int version)
		{
			if (host.Length > 255)
			{
				throw new ArgumentOutOfRangeException();
			}
			scoped BufferWriterSlim<byte> writer = new BufferWriterSlim<byte>(10, s_allocator);
			try
			{
				writer.WriteVarInt(0x00); //Packet Id

				writer.WriteVarInt(version);
				writer.WriteString(host);
				writer.WriteBigEndian(port);
				writer.Write(VarIntLoginIntent);
				writer.TryDetachBuffer(out MemoryOwner<byte> owner);
				return new OutputPacket(owner);
			}
			finally
			{
				writer.Dispose();
			}
		}

		internal readonly struct EncryptionBeginPacket
		{
			public readonly string ServerId;
			public readonly byte[] PublicKey;
			public readonly byte[] VerifyToken;

			public EncryptionBeginPacket(string serverId, byte[] publicKey, byte[] verifyToken)
			{
				ServerId = serverId;
				PublicKey = publicKey;
				VerifyToken = verifyToken;
			}
		}
	}

	public class LoginizationResult
	{
		public readonly Stream Stream;
		public readonly int CompressionThreshold;

		public LoginizationResult(Stream stream, int compressionThreshold)
		{
			Stream = stream;
			CompressionThreshold = compressionThreshold;
		}
	}
}
