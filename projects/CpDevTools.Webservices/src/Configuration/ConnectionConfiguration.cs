using System.Collections;
using Microsoft.Extensions.Configuration;

namespace CpDevTools.Webservices.Configuration
{

    public class ConnectionConfiguration : IEnumerable<KeyValuePair<string, string>>
    {
        public static implicit operator string(ConnectionConfiguration c) => c.ToConnectionString();
       
        public static ConnectionConfiguration FromConnectionString(string connectionString, string propertyDelimiter = ";", string valueDelimiter = "=")
        {
            return new ConnectionConfiguration(propertyDelimiter, valueDelimiter)
                .SetProperties(connectionString, propertyDelimiter, valueDelimiter);
        }

        public static ConnectionConfiguration FromConfigurationSection(IConfigurationSection section, string propertyDelimiter = ";", string valueDelimiter = "=")
        {
            return new ConnectionConfiguration(propertyDelimiter, valueDelimiter)
                .SetProperties(section);
        }

        public static ConnectionConfiguration FromConfigurationByName(IConfiguration config, string connectionStringName, string propertyDelimiter = ";", string valueDelimiter = "=")
        {
            var cs = new ConnectionConfiguration(propertyDelimiter, valueDelimiter);
            var conStr = config.GetConnectionString(connectionStringName);
            if (!String.IsNullOrWhiteSpace(conStr))
            {
                cs.SetProperties(conStr, propertyDelimiter, valueDelimiter);
            }
            var conSection = config.GetSection(connectionStringName);
            if (conSection != null)
            {
                cs.SetProperties(conSection);
            }

            var a = new ConnectionConfiguration();
            string b = (string)a;

            return cs;
        }

        private readonly Dictionary<string, string> _properties = new();
        public string PropertyDelimiter { get; set; }
        public string ValueDelimiter { get; set; }

        public ConnectionConfiguration(string propertyDelimiter = ";", string valueDelimiter = "=")
        {
            PropertyDelimiter = propertyDelimiter;
            ValueDelimiter = valueDelimiter;
        }

        public string ToConnectionString(string? propertyDelimiter = null, string? valueDelimiter = null)
        {
            return String.Join(
                propertyDelimiter ?? PropertyDelimiter,
                _properties.Select(i => $"{i.Key}{valueDelimiter ?? ValueDelimiter}{i.Value}")
            );
        }

        public string? GetProperty(string key, string? defaultValue = null)
        {
            return _properties.ContainsKey(key) ? _properties[key] : defaultValue;
        }

        public ConnectionConfiguration RemoveProperty(string key)
        {
            _properties.Remove(key);
            return this;
        }

        public bool ContainsProperty(string key)
        {
            return _properties.ContainsKey(key);
        }

        public List<string> GetPropertyKeys()
        {
            return _properties.Keys.ToList();
        }

        public ConnectionConfiguration SetProperty(string key, string value)
        {
            _properties.Add(key, value);
            return this;
        }

        public ConnectionConfiguration SetProperties(Dictionary<string, string> props)
        {
            foreach (var item in props)
            {
                SetProperty(item.Key, item.Value);
            }
            return this;
        }

        public ConnectionConfiguration SetProperties(string connectionString, string? propertyDelimiter = null, string? valueDelimiter = null)
        {
            return SetProperties(parseConnectionString(connectionString, propertyDelimiter, valueDelimiter));
        }

        public ConnectionConfiguration SetProperties(IConfigurationSection section)
        {
            return SetProperties(parseConnectionSection(section));
        }

        public ConnectionConfiguration ClearProperties(IConfigurationSection config)
        {
            _properties.Clear();
            return this;
        }


        private Dictionary<string, string> parseConnectionString(string connectionString, string? propertyDelimiter = null, string? valueDelimiter = null)
        {
            return connectionString
                .Split(propertyDelimiter ?? PropertyDelimiter)
                .Select(p => p.Split(valueDelimiter ?? ValueDelimiter))
                .ToDictionary(
                    p => p[0],
                    p => p[1]
                );
        }
        private Dictionary<string, string> parseConnectionSection(IConfigurationSection section)
        {
            return section
                .GetChildren()
                .ToDictionary(
                    p => p.Key,
                    p => p.Value ?? ""
                );
        }

        public IEnumerator<KeyValuePair<string, string>> GetEnumerator()
        {
            return _properties.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public override string ToString()
        {
            return ToConnectionString();
        }
    }
}