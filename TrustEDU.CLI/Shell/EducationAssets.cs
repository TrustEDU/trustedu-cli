using System;
using System.Linq;
using Akka.Actor;
using TrustEDU.Core;
using TrustEDU.Core.Base.Types;
using TrustEDU.Core.Models.Coins;
using TrustEDU.Core.Models.Ledger;
using TrustEDU.Core.Models.SmartContract;
using TrustEDU.Core.Models.Transactions;
using TrustEDU.Core.Models.Wallets;
using TrustEDU.Core.Network.Peer2Peer;
using Snapshot = TrustEDU.Core.Persistence.Snapshot;

namespace TrustEDU.CLI.Shell
{
    public class EducationAssets
    {
        private readonly Wallet currentWallet;
        private readonly TrustEDUNetwork system;

        public EducationAssets(Wallet wallet, TrustEDUNetwork system)
        {
            this.currentWallet = wallet;
            this.system = system;
        }

        public Fixed8 UnavailableBonus()
        {
            using (Snapshot snapshot = Blockchain.Singleton.GetSnapshot())
            {
                uint height = snapshot.Height + 1;
                Fixed8 unavailable;

                try
                {
                    unavailable = snapshot.CalculateBonus(currentWallet.FindUnspentCoins().Where(p => p.Output.AssetId.Equals(Blockchain.GoverningToken.Hash)).Select(p => p.Reference), height);
                }
                catch (Exception)
                {
                    unavailable = Fixed8.Zero;
                }

                return unavailable;
            }
        }


        public Fixed8 AvailableBonus()
        {
            using (Snapshot snapshot = Blockchain.Singleton.GetSnapshot())
            {
                return snapshot.CalculateBonus(currentWallet.GetUnclaimedCoins().Select(p => p.Reference));
            }
        }


        public ClaimTransaction Claim()
        {

            if (this.AvailableBonus() == Fixed8.Zero)
            {
                Console.WriteLine($"no gas to claim");
                return null;
            }

            CoinReference[] claims = currentWallet.GetUnclaimedCoins().Select(p => p.Reference).ToArray();
            if (claims.Length == 0) return null;

            using (Snapshot snapshot = Blockchain.Singleton.GetSnapshot())
            {
                ClaimTransaction tx = new ClaimTransaction
                {
                    Claims = claims,
                    Attributes = new TransactionAttribute[0],
                    Inputs = new CoinReference[0],
                    Outputs = new[]
                    {
                        new TransactionOutput
                        {
                            AssetId = Blockchain.UtilityToken.Hash,
                            Value = snapshot.CalculateBonus(claims),
                            ScriptHash = currentWallet.GetChangeAddress()
                        }
                    }

                };

                return (ClaimTransaction)SignTransaction(tx);
            }
        }


        private Transaction SignTransaction(Transaction tx)
        {
            if (tx == null)
            {
                Console.WriteLine($"no transaction specified");
                return null;
            }
            ContractParametersContext context;

            try
            {
                context = new ContractParametersContext(tx);
            }
            catch (InvalidOperationException)
            {
                Console.WriteLine($"unsynchronized block");

                return null;
            }

            currentWallet.Sign(context);

            if (context.Completed)
            {
                context.Verifiable.Witnesses = context.GetWitnesses();
                currentWallet.ApplyTransaction(tx);

                bool relay_result = system.Blockchain.Ask<RelayResultReason>(tx).Result == RelayResultReason.Succeed;

                if (relay_result)
                {
                    return tx;
                }
                else
                {
                    Console.WriteLine($"Local Node could not relay transaction: {tx.Hash.ToString()}");
                }
            }
            else
            {
                Console.WriteLine($"Incomplete Signature: {context.ToString()}");
            }

            return null;
        }
    }
}
