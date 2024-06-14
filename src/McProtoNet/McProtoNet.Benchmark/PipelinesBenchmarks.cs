﻿namespace McProtoNet.Benchmark;
//[MemoryDiagnoser(true)]
//public class PipelinesBenchmarks
//{

//	public int CompressionThreshold { get; set; } = 128;

//	public int PacketsCount { get; set; } = 100_000;

//	private Pipe pipe;

//	private long TotalBytes;
//	private MemoryStream mainStream;

//	[GlobalSetup]
//	public async Task Setup()
//	{

//		using var mainStream = File.Open("data.bin", FileMode.Create, FileAccess.Write, FileShare.Write);
//		//mainStream = new();

//		var sender = new MinecraftPacketSender();
//		sender.SwitchCompression(CompressionThreshold);
//		sender.BaseStream = mainStream;
//		for (int i = 0; i < PacketsCount; i++)
//		{
//			var data = new byte[Random.Shared.Next(10, 512)];

//			int id = Random.Shared.Next(0, 60);

//			TotalBytes += data.Length;
//			Random.Shared.NextBytes(data);

//			MemoryStream ms = new MemoryStream(data);

//			ms.Position = 0;

//			var packet = new Packet(id, ms);

//			await sender.SendPacketAsync(packet);
//		}
//		await mainStream.FlushAsync();

//		pipe = new Pipe();
//	}
//	[GlobalCleanup]
//	public void Clean()
//	{

//	}
//	[Benchmark]
//	public async Task ReadStream1()
//	{
//		using var mainStream = File.OpenRead("data.bin");
//		//mainStream.Position = 0;
//		var native_reader = new MinecraftPacketReader();

//		native_reader.BaseStream = mainStream;
//		native_reader.SwitchCompression(CompressionThreshold);

//		for (int i = 0; i < PacketsCount; i++)
//		{
//			using var packet = await native_reader.ReadNextPacketAsync();

//		}

//	}
//	[Benchmark]
//	public async Task ReadStream2()
//	{
//		using var mainStream = File.OpenRead("data.bin");
//		//mainStream.Position = 0;

//		using var native_reader = new MinecraftPacketReaderNew();

//		native_reader.BaseStream = mainStream;
//		native_reader.SwitchCompression(CompressionThreshold);

//		for (int i = 0; i < PacketsCount; i++)
//		{
//			var packet = await native_reader.ReadNextPacketAsync();
//			packet.Dispose();
//		}
//	}

//	private readonly ZlibDecompressor decompressor = new();

//	[Benchmark]
//	public async Task ReadWithPipelines()
//	{

//		MinecraftPacketPipeReader pipeReader = new MinecraftPacketPipeReader(pipe.Reader, decompressor);

//		pipeReader.CompressionThreshold = this.CompressionThreshold;
//		var fill = FillPipe();
//		var read = ProcessPackets(pipeReader);

//		await Task.WhenAll(fill, read);

//		pipe.Reset();

//	}
//	private async Task ProcessPackets(MinecraftPacketPipeReader reader)
//	{
//		int count = 0;
//		await foreach (var packet in reader.ReadPacketsAsync())
//		{
//			count++;
//		}
//	}

//	[MethodImpl(MethodImplOptions.AggressiveInlining)]
//	private async Task FillPipe()
//	{
//		using var mainStream = File.OpenRead("data.bin");
//		//mainStream.Position = 0;
//		try
//		{
//			while (true)
//			{
//				Memory<byte> memory = pipe.Writer.GetMemory(512);
//				try
//				{
//					int count = await mainStream.ReadAsync(memory);

//					if (count == 0)
//						return;

//					pipe.Writer.Advance(count);

//					FlushResult flushResult = await pipe.Writer.FlushAsync();

//					if (flushResult.IsCanceled || flushResult.IsCompleted)
//						return;
//				}
//				catch (Exception ex)
//				{
//					throw;
//				}

//			}
//		}
//		finally
//		{
//			await pipe.Writer.CompleteAsync();
//		}
//	}

//}