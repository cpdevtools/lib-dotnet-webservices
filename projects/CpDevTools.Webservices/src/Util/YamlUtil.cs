

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace CpDevTools.Webservices.Util
{
    public static class YamlUtil
    {
        private static readonly ISerializer _serializer = new SerializerBuilder().Build();
        private static readonly IDeserializer _deserializer = new DeserializerBuilder()
            .WithNamingConvention(CamelCaseNamingConvention.Instance)
            .Build();
        public static string Serialize<T>(T value)
        {
            if (value == null)
            {
                return "";
            }
            return _serializer.Serialize(value);
        }
        
        public static T Deserialize<T>(string value)
        {
            return _deserializer.Deserialize<T>(value);
        }



    }

}
