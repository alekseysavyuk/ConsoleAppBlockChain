using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace ConsoleAppBlockChain
{
    [NotMapped]
    public class Chain
    {
        public List<Block> Blocks { get; set; }
        public Block PreviousBlock { get; set; }

        public Chain()
        {
            Blocks = LoadChainFromDatabase();

            if (Blocks.Count() == 0)
            {
                Block genesisBlock = new Block();

                Blocks.Add(genesisBlock);
                PreviousBlock = genesisBlock;
                SaveBlock(PreviousBlock); //don`t save to database
            }

            else
            {
                if (Check())
                    PreviousBlock = Blocks.Last();

                else throw new Exception("Error getting blocks from DB\nBlockchain is False!!!!!");
            }
        }

        public void AddBlock(Block block)
        {
            Blocks.Add(block);
            PreviousBlock = block;
            SaveBlock(block);
        }

        private void SaveBlock(Block block)
        {
            using (ConsoleAppBlockChainContext db = new ConsoleAppBlockChainContext())
            {
                db.Blocks.Add(block);
                db.SaveChanges();
            }
        }

        public bool Check()
        {
            Block genesisBlock = new Block();
            string previousHash = genesisBlock.Hash;

            foreach (var block in Blocks.Skip(1)) // don`t .skip(1)
            {
                string hash = block.PreviousHash;

                if (previousHash != hash)
                    return false;

                previousHash = block.Hash;
            }

            return true;
        }

        public string GetLastHashFromDB()
        {
            return PreviousBlock.Hash;
        }

        /// <summary>
        /// Отримання даних з БД в коллекцію
        /// </summary>
        /// <returns> Коллекція даних </returns>
        private List<Block> LoadChainFromDatabase()
        {
            List<Block> list;

            using (ConsoleAppBlockChainContext db = new ConsoleAppBlockChainContext())
            {
                var count = db.Blocks.Count();

                list = new List<Block>(count * 2);

                list.AddRange(db.Blocks);
            }

            return list;
        }
    }
}