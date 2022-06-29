using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace ConsoleAppBlockChain
{
    [DataContract]
    public class UserTransaction : Helper
    {
        [DataMember]
        [Column("transaction_details")]
        public string Details { get; private set; }

        [NotMapped]
        public string Hash { get; private set; }

        public UserTransaction(string details)
        {
            #region Requirest
            if (string.IsNullOrWhiteSpace(details))
                throw new ArgumentNullException($"Пустой аргумент {nameof(details)}");
            #endregion

            Details = details;
            Hash = GetHash(GetStringForHash(Details));
        }
    }
}