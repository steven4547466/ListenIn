using HarmonyLib;
using NorthwoodLib.Pools;
using PluginAPI.Core;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using VoiceChat.Codec;
using VoiceChat.Networking;
using static HarmonyLib.AccessTools;

namespace ListenIn
{
    [HarmonyPatch(typeof(VoiceTransceiver), nameof(VoiceTransceiver.ServerReceiveMessage))]
    public static class VoiceTransceiverServerReceiveMessagePatch
    {
        public static bool CheckManual(VoiceMessage message, ReferenceHub sendTo)
        {
            if (!message.SpeakerNull && sendTo.CanHearOverride(message.Speaker))
            {
                VoiceMessage newMessage = new VoiceMessage(message.Speaker, VoiceChat.VoiceChatChannel.RoundSummary, message.Data, message.DataLength, false);
                sendTo.connectionToClient.Send(newMessage, 0);

                return false;
            }

            return true;
        }

        public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions, ILGenerator generator)
        {
            List<CodeInstruction> newInstructions = ListPool<CodeInstruction>.Shared.Rent(instructions);
            int index = newInstructions.FindLastIndex(c => c.opcode == OpCodes.Call) - 1;

            Label label = generator.DefineLabel();
            newInstructions[index].labels.Add(label);

            index = newInstructions.FindIndex(c => c.opcode == OpCodes.Stloc_3) + 1;
            newInstructions.InsertRange(index, new[] 
            {
                new CodeInstruction(OpCodes.Ldarg_1),
                new CodeInstruction(OpCodes.Ldloc_3),
                new CodeInstruction(OpCodes.Call, Method(typeof(VoiceTransceiverServerReceiveMessagePatch), nameof(VoiceTransceiverServerReceiveMessagePatch.CheckManual))),
                new CodeInstruction(OpCodes.Brfalse_S, label),
            });

            for (int z = 0; z < newInstructions.Count; z++) yield return newInstructions[z];

            ListPool<CodeInstruction>.Shared.Return(newInstructions);
        }
    }
}
