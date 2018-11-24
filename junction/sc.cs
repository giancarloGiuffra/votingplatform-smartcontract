using Neo.SmartContract.Framework.Services.Neo;
using System.Collections.Generic;

namespace Neo.SmartContract
{
    public class DAOVoting : Framework.SmartContract
    {
        static List<string> validVoters = new List<string>{ "AK2nJJpJr6o664CWJKi1QRXjqeic2zRp8y" };

        public static object Main(string operation, params object[] args)
        {
            switch (operation)
            {
                case "vote":
                    return RegisterVote((byte[])args[0], (byte[])args[1], (byte[])args[2]);
                default:
                    return false;
            }
        }

        private static bool RegisterVote(byte[] publicKey, byte[] encryptedVote, byte[] signature)
        {
            if (!ValidVoter(publicKey)) return false;
            if (AlreadyVoted(publicKey)) return false;
            if (!ValidSignature(publicKey, encryptedVote, signature)) return false;

            Storage.Put(Storage.CurrentContext, publicKey.ToHexString(), encryptedVote);
            Storage.Put(Storage.CurrentContext, VotedKey(publicKey), "true");
            return true;
        }

        private static string VotedKey(byte[] publicKey)
        {
            return $"{publicKey.ToHexString()}_voted";
        }

        private static bool ValidVoter(byte[] publicKey)
        {
            var ecpoint = Neo.Cryptography.ECC.ECPoint.FromBytes(publicKey, Neo.Cryptography.ECC.ECCurve.Secp256r1);
            var sc = Neo.SmartContract.Contract.CreateSignatureContract(ecpoint);
            return validVoters.Contains(sc.Address);
        }

        private static bool AlreadyVoted(byte[] publicKey)
        {
            byte[] value = Storage.Get(Storage.CurrentContext, VotedKey(publicKey));
            return value != null;
        }

        private static bool ValidSignature(byte[] publicKey, byte[] encryptedVote, byte[] signature)
        {
            return Neo.Cryptography.Crypto.Default.VerifySignature(encryptedVote, signature, publicKey);
        }

    }

}