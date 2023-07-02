using PlayerRoles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YamlDotNet.Serialization;

namespace ListenIn
{
    public sealed class Config
    {
        public Dictionary<RoleTypeId, List<string>> DefaultRoleOverrides { get; set; } = new Dictionary<RoleTypeId, List<string>>();
        public Dictionary<Team, List<string>> DefaultTeamOverrides { get; set; } = new Dictionary<Team, List<string>>();


        private Dictionary<RoleTypeId, VoiceOverrides> _defaultRoleOverrides = null;
        [YamlIgnore]
        internal Dictionary<RoleTypeId, VoiceOverrides> ParsedRoleOverrides
        { 
            get
            {
                if (_defaultRoleOverrides == null)
                {
                    foreach (var roleOverrides in DefaultRoleOverrides)
                    {
                        VoiceOverrides voiceOverrides = new VoiceOverrides();

                        foreach (string str in roleOverrides.Value)
                        {
                            if (str == "all")
                            {
                                voiceOverrides.HearAll = true;
                            }
                            else if (Enum.TryParse(str, out RoleTypeId role))
                            {
                                voiceOverrides.RoleOverrides.Add(role);
                            }
                            else if (Enum.TryParse(str, out Team team))
                            {
                                voiceOverrides.TeamOverrides.Add(team);
                            }
                        }

                        _defaultRoleOverrides.Add(roleOverrides.Key, voiceOverrides);
                    }
                }

                return _defaultRoleOverrides;
            }
        }

        private Dictionary<Team, VoiceOverrides> _defaultTeamOverrides = null;
        [YamlIgnore]
        internal Dictionary<Team, VoiceOverrides> ParsedTeamOverrides
        {
            get
            {
                if (_defaultTeamOverrides == null)
                {
                    foreach (var teamOverrides in DefaultTeamOverrides)
                    {
                        VoiceOverrides voiceOverrides = new VoiceOverrides();

                        foreach (string str in teamOverrides.Value)
                        {
                            if (str == "all")
                            {
                                voiceOverrides.HearAll = true;
                            }
                            else if (Enum.TryParse(str, out RoleTypeId role))
                            {
                                voiceOverrides.RoleOverrides.Add(role);
                            }
                            else if (Enum.TryParse(str, out Team team))
                            {
                                voiceOverrides.TeamOverrides.Add(team);
                            }
                        }

                        _defaultTeamOverrides.Add(teamOverrides.Key, voiceOverrides);
                    }
                }

                return _defaultTeamOverrides;
            }
        }
    }
}
