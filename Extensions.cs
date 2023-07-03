using PlayerRoles;
using PluginAPI.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ListenIn
{
    public class VoiceOverrides
    {
        public bool HearAll = false;

        public List<RoleTypeId> RoleOverrides = new List<RoleTypeId>();

        public List<Team> TeamOverrides = new List<Team>();

        public List<ReferenceHub> PlayerOverrides = new List<ReferenceHub>();
    }

    public static class Extensions
    {
        public static Dictionary<ReferenceHub, VoiceOverrides> Overrides = new Dictionary<ReferenceHub, VoiceOverrides>();

        public static bool CanHearOverride(this ReferenceHub hub, ReferenceHub from)
        {
            if (hub == null || from == null)
                return false;

            if (hub != from)
            {
                if (Plugin.Singleton.Config.ParsedTeamOverrides != null && 
                    Plugin.Singleton.Config.ParsedTeamOverrides.TryGetValue(hub.GetTeam(), out VoiceOverrides teamOverrides) &&
                    (teamOverrides.HearAll || teamOverrides.RoleOverrides.Contains(from.GetRoleId())
                    || teamOverrides.TeamOverrides.Contains(from.GetTeam())))
                {
                    return true;
                }

                if (Plugin.Singleton.Config.ParsedRoleOverrides != null &&
                    Plugin.Singleton.Config.ParsedRoleOverrides.TryGetValue(hub.GetRoleId(), out VoiceOverrides roleOverrides) &&
                    (roleOverrides.HearAll || roleOverrides.RoleOverrides.Contains(from.GetRoleId())
                    || roleOverrides.TeamOverrides.Contains(from.GetTeam())))
                {
                    return true;
                }

                if (Overrides.TryGetValue(hub, out VoiceOverrides vo) && (vo.HearAll || vo.RoleOverrides.Contains(from.GetRoleId())
                || vo.TeamOverrides.Contains(from.GetTeam()) || vo.PlayerOverrides.Contains(from)))
                    return true;
            }
            return false;
        }
    }
}
