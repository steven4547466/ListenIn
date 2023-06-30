using PluginAPI.Core.Attributes;
using PluginAPI.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VoiceChat.Networking;

namespace ListenIn
{
    internal class EventHandlers
    {
        [PluginEvent(ServerEventType.WaitingForPlayers)]
        void OnWaitingForPlayers()
        {
            Extensions.Overrides.Clear();
        }
    }
}
