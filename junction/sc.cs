using Neo.SmartContract.Framework.Services.Neo;
using Neo.SmartContract.Framework;

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
                    return RegisterVote((byte[])args[0], (byte[])args[1]);
                default:
                    return false;
            }
        }

        private static bool RegisterVote(byte[] address, byte[] encryptedVote)
        {
            if (!ValidVoter(address)) return false;
            if (AlreadyVoted(address)) return false;
            if (!RightVoter(address)) return false;

            Storage.Put(Storage.CurrentContext, address, encryptedVote);
            Storage.Put(Storage.CurrentContext, VotedKey(address), "true");
            return true;
        }

        private static string VotedKey(byte[] publicKey)
        {
            return string.Concat(publicKey, "_voted");
        }

        private static bool ValidVoter(byte[] address)
        {
            return address == validVoters;
        }

        private static bool AlreadyVoted(byte[] publicKey)
        {
            byte[] value = Storage.Get(Storage.CurrentContext, VotedKey(publicKey));
            return value != null;
        }

        private static bool RightVoter(byte[] address) => Runtime.CheckWitness(address);

    }

}