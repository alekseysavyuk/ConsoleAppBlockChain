using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace ConsoleAppBlockChain
{
    [DataContract]
    public class User : Helper
    {
        [DataMember]
        [Column("first_name")]
        public string Name { get; private set; }
        [DataMember]
        [Column("last_name")]
        public string LastName { get; private set; }

        [NotMapped]
        public string Hash { get; private set; }

        public User(string name, string lastName)
        {
            #region Requirest
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentNullException($"Пустой аргумент {nameof(name)}");

            if (string.IsNullOrWhiteSpace(lastName))
                throw new ArgumentNullException($"Пустой аргумент {nameof(lastName)}");
            #endregion

            Name = name;
            LastName = lastName;
            Hash = GetHash(GetStringForHash(Name, LastName));
        }
    }
}