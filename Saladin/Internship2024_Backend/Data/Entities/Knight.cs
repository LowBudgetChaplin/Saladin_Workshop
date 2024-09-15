using Newtonsoft.Json;
using System;
using System.Text.Json.Serialization;

namespace Data.Entities
{
    public class Knight
    {
        public Knight(string name, string dictionaryKnightTypeName, string legionName)
        {
            Name = name;
            DictionaryKnightTypeName = dictionaryKnightTypeName;
            LegionName = legionName;
        }

        [JsonConstructor]
        public Knight()
        {

        }

        public int KnightId { get; set; }
        public string Name { get; set; }
        public string DictionaryKnightTypeName { get; set; }
        public string LegionName { get; set; }
        public string BattleName { get; set; }
        public int CoinsAwardedPerBattle { get; set; }
        public int BattleId { get; set; }
    }
}
