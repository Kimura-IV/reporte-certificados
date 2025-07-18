using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace certificados.models.Entitys
{
    public class ResponseApp
    {
        [JsonPropertyName("cod")]
        public string Cod { get; set; }
        [JsonPropertyName("message")]
        public string Message { get; set; }
        [JsonPropertyName("data")]
        public object Data { get; set; }
        public ResponseApp() { }

        public ResponseApp(string cod, string message, object data)
        {
            Cod = cod;
            Message = message;
            Data = data;
        }
    }
}
