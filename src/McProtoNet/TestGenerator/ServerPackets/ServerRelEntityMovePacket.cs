//AutoGenerated

namespace McProtoNet.Packets;

public sealed class ServerRelEntityMovePacket
{
	public int EntityId { get; set; }
	public short Dx { get; set; }
	public short Dy { get; set; }
	public short Dz { get; set; }
	public bool OnGround { get; set; }
}
