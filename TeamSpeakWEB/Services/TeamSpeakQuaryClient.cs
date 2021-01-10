using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using TeamSpeak3QueryApi.Net.Specialized;

namespace TeamSpeakWEB.Services
{
    public class TeamSpeakQuaryClient : TeamSpeakClient
    {

        public TeamSpeakQuaryClient(IConfiguration configuration) : 
            base(configuration.GetValue<string>("TeamSpeakQueryConf:Hostname"), configuration.GetValue<int>("TeamSpeakQueryConf:Port"))
        {
            Connect().Wait();
            Login(configuration.GetValue<string>("TeamSpeakQueryConf:Username"), configuration.GetValue<string>("TeamSpeakQueryConf:Password")).Wait();
        }
    }
}
