using System.Linq;
using System.Text.Json.Serialization;
using Newtonsoft.Json;
using TridentMc.Networking.Packets.DataTypes;
using TridentMc.Networking.State;

namespace TridentMc.Networking.Packets.Status
{
    public class PacketResponse : ClientPacket
    {
        public override int Id => 0;

        public Response Response;

        public PacketResponse(Response response)
        {
            Response = response;
        }
        
        public static PacketResponse FromSystem()
        {
            return new PacketResponse(
                new Response
                {
                    description = TridentSettings.Description,
                    version = new ResponseVersion
                    {
                        name = TridentMc.ProtocolName,
                        protocol = TridentMc.ProtocolVersion
                    },
                    players = new ResponsePlayers
                    {
                        max = TridentSettings.MaxPlayers,
                        online = TridentMc.Instance.NetworkManager.Sessions.Count(session => session.ConnectionState == ConnectionState.Play)
                    }
                }
            );
        }

        public override PacketBuffer Encode()
        {
            var buffer = new PacketBuffer();
            buffer.WriteString(JsonConvert.SerializeObject(Response));

            return buffer;
        }
    }

    public struct Response
    {
        public ResponseVersion version;
        public ResponsePlayers players;
        public string description;
    }

    public struct ResponseVersion
    {
        public string name;
        public int protocol;
    }

    public struct ResponsePlayers
    {
        public int online;
        public int max;
    }
}
