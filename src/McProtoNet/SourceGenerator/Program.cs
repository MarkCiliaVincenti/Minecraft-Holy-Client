﻿using System.Text.Json;
using SourceGenerator.MCDataModels;
using SourceGenerator.ProtoDefTypes;

public class Program
{
    public static Dictionary<string, string> MapWriteMethods = new()
    {
        { "", "" }
    };


    public static string Testpath = @"C:\Users\Title\OneDrive\Рабочий стол\Test.cs";

    public static string Root =
        "C:\\Users\\Title\\source\\repos\\Minecraft-Holy-Client\\src\\McProtoNet\\McProtoNet.Protocol";

    public static string dataPath = @"minecraft-data\data";

    private static async Task Main(string[] args)
    {
        var dataPathsJson = Path.Combine(dataPath, "dataPaths.json");

        dataPathsJson = await File.ReadAllTextAsync(dataPathsJson);

        var paths = JsonSerializer.Deserialize<DataPaths>(dataPathsJson);


        var allVersions = JsonSerializer.Deserialize<ProtocolVersion[]>(
            await File.ReadAllTextAsync(
                Path.Combine(dataPath, "pc", "common", "protocolVersions.json")));


        var filter = allVersions
            .Where(x => x.Version >= 754)
            .Where(x => x.ReleaseType != "snapshot");

        ProtocolCollection collection = new();

        foreach (var item in paths.Pc)
        {
            var protocol_path = Path.Combine(dataPath, item.Value.Protocol, "protocol.json");
            var version_path = Path.Combine(dataPath, item.Value.Version, "version.json");

            if (File.Exists(version_path))
                if (File.Exists(protocol_path))
                {
                    var version_json = await File.ReadAllTextAsync(version_path);


                    var version = JsonSerializer.Deserialize<VersionInfo>(version_json);

                    if (version.Version >= 754 && version.Version != 1073741839)
                    {
                        var protocol_json = await File.ReadAllTextAsync(protocol_path);

                        ProtodefParser parser = new(protocol_json);

                        var protocol = parser.Parse();


                        collection.Add(version.Version, new Protocol
                        {
                            JsonPackets = protocol,
                            Version = version
                        });
                    }
                }
        }


        var clone1 = (ProtodefProtocol)collection.Protocols[754].JsonPackets.Clone();

        Console.WriteLine(clone1);

        var protocolDir = Path.Combine(Root, "Protocols");


        Directory.CreateDirectory(protocolDir);
    }
}