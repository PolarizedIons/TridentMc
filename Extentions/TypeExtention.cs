using System;
using System.Collections.Generic;
using System.Linq;
using TridentMc.Networking.Packets;

namespace TridentMc.Extentions
{
    public static class TypeExtention
    {
        public static IEnumerable<Type> FindAllInAssembly(this Type type)
        {
            return AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(x => x.GetTypes())
                .Where(x => type.IsAssignableFrom(x) &&
                            !x.IsInterface &&
                            !x.IsAbstract &&
                            x.GetConstructors().Any(c => c.IsPublic)
                            );
        }
        
        public static PacketAttribute GetPacketAttribute(this Type packetType)
        {
            return Attribute.GetCustomAttribute(packetType, typeof(PacketAttribute)) as PacketAttribute;
        }
    }
}
