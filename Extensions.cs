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
            if (Overrides.TryGetValue(hub, out VoiceOverrides vo) && (vo.HearAll || vo.RoleOverrides.Contains(from.GetRoleId()) 
                || vo.TeamOverrides.Contains(from.GetTeam()) || vo.PlayerOverrides.Contains(from)))
                return true;
            return false;
        }
    }
}
