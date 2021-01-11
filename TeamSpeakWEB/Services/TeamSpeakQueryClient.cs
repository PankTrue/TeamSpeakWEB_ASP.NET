using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using TeamSpeak3QueryApi.Net.Specialized;

namespace TeamSpeakWEB.Services
{
    public class TeamSpeakQueryClient : TeamSpeakClient
    {

        private readonly int _timeout;


        public TeamSpeakQueryClient(IConfiguration conf,ILogger<TeamSpeakQueryClient> logger) : 
            base(conf.GetValue<string>("TeamSpeakQueryConf:Hostname"), conf.GetValue<int>("TeamSpeakQueryConf:Port"))
        {
            _timeout = conf.GetValue<int>("TeamSpeakQueryConf:Timeout");

            this.Client.Client.ReceiveTimeout = _timeout;
            this.Client.Client.SendTimeout = _timeout;

            try
            {
                Connect().Wait(_timeout);
            }
            catch (Exception e)
            {
                logger.Log(LogLevel.Error, e.Message);
            }

            if (this.Client.IsConnected)
            {
                logger.Log(LogLevel.Information, "Connected to TeamSpeak server!");
            }
            else
            {
                logger.Log(LogLevel.Warning, "Failed to connect to TeamSpeak server!");
                return;
            }

            try
            {
                Login(conf.GetValue<string>("TeamSpeakQueryConf:Username"),
                    conf.GetValue<string>("TeamSpeakQueryConf:Password")).Wait(_timeout);
            }
            catch (Exception e)
            {
                logger.Log(LogLevel.Error, "Wrong login or password!");
            }
            //TODO: implement auto-reconnecter
        }
    }
}
