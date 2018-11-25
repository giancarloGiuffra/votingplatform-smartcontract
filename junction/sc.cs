using Neo.SmartContract.Framework.Services.Neo;
using Neo.SmartContract.Framework;
using System;

namespace Neo.SmartContract
{
    public class DAOVoting : Framework.SmartContract
    {
        static readonly byte[] validVoters = "AK2nJJpJr6o664CWJKi1QRXjqeic2zRp8y".ToScriptHash();

        public static object Main(string operation, params object[] args)
        {
            switch (operation)
            {
                case "vote":
                    return RegisterVote((string)args[0], (string)args[1]);
                case "getciphers":
                    return Storage.Get(Storage.CurrentContext, "votes").AsString();
                default:
                    return false;
            }
        }

        private static bool RegisterVote(string address, string encryptedVote)
        {
            if (!ValidVoter(address)) return false;
            if (AlreadyVoted(address)) return false;
            if (!RightVoter(address)) return false;

            byte[] currentVotes = Storage.Get(Storage.CurrentContext, "votes");

            string votes = BuildJson(currentVotes, address, encryptedVote);

            //Console.Write(votes);

            Storage.Put(Storage.CurrentContext, "votes", votes);
            Storage.Put(Storage.CurrentContext, VotedKey(address), "true");
            return true;
        }

        public static string BuildJson(byte[] currentVotes, string address, string encryptedVote)
        {
            if(currentVotes == null)
            {
                return string.Concat(OneVoteJson(address, encryptedVote));
            }
            return string.Concat(currentVotes.AsString(), ",", OneVoteJson(address, encryptedVote));
        }

        private static string OneVoteJson(string address, string encryptedVote)
        {
            return string.Concat("{", OneFieldJson("address", address), ",", OneFieldJson("vote", encryptedVote), "}");
        }

        private static string OneFieldJson(string field, string value)
        {
            return string.Concat("'", field, "'", ":", "'", value, "'");
        }

        private static string VotedKey(string publicKey)
        {
            return string.Concat(publicKey, "_voted");
        }

        private static bool ValidVoter(string address)
        {
            return true;
        }

        private static bool AlreadyVoted(string publicKey)
        {
            byte[] value = Storage.Get(Storage.CurrentContext, VotedKey(publicKey));
            return value != null;
        }

        private static bool RightVoter(string address) => true;

    }

}