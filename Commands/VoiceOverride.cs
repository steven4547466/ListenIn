using CommandSystem;
using PlayerRoles;
using PluginAPI.Core;
using RemoteAdmin;
using System;
using System.Collections.Generic;
using System.Linq;


namespace ListenIn.Commands
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    class VoiceOverride : ICommand
    {
        public string[] Aliases { get; set; } = { "vo" };

        public string Description { get; set; } = "Listen to voice chat regardless of your role";

        public string Usage { get; set; } = "voiceoverride [players/roles]";

        string ICommand.Command { get; } = "voiceoverride";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (sender is PlayerCommandSender playerSender)
            {
                if (arguments.ElementAt(0) == "clear")
                {
                    Extensions.Overrides.Remove(playerSender.ReferenceHub);

                    response = "Cleared";
                    return true;
                }

                List<ReferenceHub> players = new List<ReferenceHub>();
                List<RoleTypeId> roles = new List<RoleTypeId>();
                List<Team> teams = new List<Team>();

                foreach (string str in arguments)
                {
                    if (Enum.TryParse(str, out RoleTypeId role))
                    {
                        roles.Add(role);
                    }
                    else if (Enum.TryParse(str, out Team team))
                    {
                        teams.Add(team);
                    }
                    else
                    {
                        Player player = null;
                        if (int.TryParse(arguments.ElementAt(0), out int id))
                            player = Player.Get(id);
                        else
                            player = GetPlayer(arguments.ElementAt(0));

                        if (player != null)
                            players.Add(player.ReferenceHub);
                    }
                }

                VoiceOverrides overrides;
                if (!Extensions.Overrides.TryGetValue(playerSender.ReferenceHub, out overrides))
                {
                    overrides = new VoiceOverrides();
                    Extensions.Overrides.Add(playerSender.ReferenceHub, overrides);
                }

                foreach (RoleTypeId role in roles)
                {
                    if (overrides.RoleOverrides.Contains(role))
                        overrides.RoleOverrides.Remove(role);
                    else
                        overrides.RoleOverrides.Add(role);
                }

                foreach (Team team in teams)
                {
                    if (overrides.TeamOverrides.Contains(team))
                        overrides.TeamOverrides.Remove(team);
                    else
                        overrides.TeamOverrides.Add(team);
                }

                foreach (ReferenceHub hub in players)
                {
                    if (overrides.PlayerOverrides.Contains(hub))
                        overrides.PlayerOverrides.Remove(hub);
                    else
                        overrides.PlayerOverrides.Add(hub);
                }

                response = "Set overrides.";
                return true;
            }
            else
            {
                response = "Only players may use this command";
                return false;
            }
        }

        private static int Compute(string s, string t)
        {
            int n = s.Length;
            int m = t.Length;
            int[,] d = new int[n + 1, m + 1];

            // Step 1
            if (n == 0)
            {
                return m;
            }

            if (m == 0)
            {
                return n;
            }

            // Step 2
            for (int i = 0; i <= n; d[i, 0] = i++)
            {
            }

            for (int j = 0; j <= m; d[0, j] = j++)
            {
            }

            // Step 3
            for (int i = 1; i <= n; i++)
            {
                //Step 4
                for (int j = 1; j <= m; j++)
                {
                    // Step 5
                    int cost = (t[j - 1] == s[i - 1]) ? 0 : 1;

                    // Step 6
                    d[i, j] = Math.Min(
                        Math.Min(d[i - 1, j] + 1, d[i, j - 1] + 1),
                        d[i - 1, j - 1] + cost);
                }
            }
            // Step 7
            return d[n, m];
        }

        internal static Player GetPlayer(string args)
        {
            //Takes a string and finds the closest player from the playerlist
            int maxNameLength = 31, LastnameDifference = 31/*, lastNameLength = 31*/;
            Player plyer = null;
            string str1 = args.ToLower();
            foreach (Player pl in Player.GetPlayers())
            {
                if (!pl.Nickname.ToLower().Contains(args.ToLower())) { goto NoPlayer; }
                if (str1.Length < maxNameLength)
                {
                    int x = maxNameLength - str1.Length;
                    int y = maxNameLength - pl.Nickname.Length;
                    string str2 = pl.Nickname;
                    for (int i = 0; i < x; i++)
                    {
                        str1 += "z";
                    }
                    for (int i = 0; i < y; i++)
                    {
                        str2 += "z";
                    }
                    int nameDifference = Compute(str1, str2);
                    if (nameDifference < LastnameDifference)
                    {
                        LastnameDifference = nameDifference;
                        plyer = pl;
                    }
                }
            NoPlayer:;
            }
            return plyer;
        }
    }
}