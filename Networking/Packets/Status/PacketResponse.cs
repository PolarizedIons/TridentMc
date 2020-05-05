using System.Linq;
using System.Text.Json.Serialization;
using Newtonsoft.Json;
using TridentMc.Networking.Packets.DataTypes;
using TridentMc.Networking.State;

namespace TridentMc.Networking.Packets.Status
{
    public class PacketResponse : IClientPacket
    {
        public ConnectionState ConnectionState => ConnectionState.Status;
        public int Id => 0x00;

        public Response Response;

        public PacketResponse()
        {
        }

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

        public byte[] Encode(IClientPacket packet)
        {
            return String.Encode(JsonConvert.SerializeObject(Response));
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
