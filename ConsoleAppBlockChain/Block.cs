using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace ConsoleAppBlockChain
{
    [DataContract]
    public class Block : Helper
    {
        public int Id { get; private set; }
        [DataMember]
        public User User { get; private set; }
        [DataMember]
        public UserTransaction Transaction { get; private set; }

        [DataMember]
        public int Version { get; private set; }
        [DataMember]
        public DateTime CreatedOn { get; private set; }

        [DataMember]
        public string Hash { get; private set; }
        [DataMember]
        public string PreviousHash { get; private set; } 

        public Block()
        {
            User = new User("GenesisFirstName", "GenesisLastName");
            Transaction = new UserTransaction("GenesisTransaction");

            Version = 1;
            CreatedOn = DateTime.Parse("2022-01-01 00:00:00.000");

            PreviousHash = GetHash("sdbvsubvsncosnvrepuiew48wwwnv"); //Create good genesis hash
            Hash = GetHash(GetStringForHash(Version.ToString(),
                                            CreatedOn.Ticks.ToString(),
                                            User.Hash, PreviousHash));
        }

        public Block(string previousHash, string userName, string userLastName, string transaction)
        {
            #region Requires
            if (previousHash == null)
                throw new ArgumentNullException($"Пустой аргумент {nameof(previousHash)}");

            if (userName == null)
                throw new ArgumentNullException($"Пустой аргумент {nameof(userName)}");

            if (userLastName == null)
                throw new ArgumentNullException($"Пустой аргумент {nameof(userLastName)}");
            #endregion

            Version = 1;
            CreatedOn = DateTime.Now.ToUniversalTime(); //???? todo format time

            User = new User(userName, userLastName);
            Transaction = new UserTransaction(transaction);

            Hash = GetHash(GetStringForHash(Version.ToString(),
                                            CreatedOn.Ticks.ToString(),
                                            User.Hash, PreviousHash));
            PreviousHash = previousHash;
        }

        /// <summary>
        /// Выполнить десериализацию в JSON строку
        /// </summary>
        /// <returns></returns>
        public string Serialize()
        {
            var jsonSerializer = new DataContractJsonSerializer(typeof(Block));

            using (MemoryStream memory = new MemoryStream())
            {
                jsonSerializer.WriteObject(memory, this);
                return Encoding.UTF8.GetString(memory.ToArray());
            }
        }
        
        /// <summary>
        /// Выполнить десериализацию
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public static Block Deserialize(string json)
        {
            var jsonDeserializer = new DataContractJsonSerializer(typeof(Block));

            using (MemoryStream memory = new MemoryStream(Encoding.UTF8.GetBytes(json)))
            {
                return (Block)jsonDeserializer.ReadObject(memory);
            }
        }
    }
}