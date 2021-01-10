using R2API;

namespace DwarfMushrum.Modules
{
    public static class Tokens
    {
        public static void RegisterLanguageTokens()
        {
            LanguageAPI.Add("DWARFMUSHRUM_BODY_NAME", StaticValues.monsterName);
            LanguageAPI.Add("DWARFMUSHRUM_BODY_LORE", StaticValues.monsterLore);
        }
    }
}
