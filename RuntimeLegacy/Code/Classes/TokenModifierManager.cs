﻿using RiskOfOptions.Components.Panel;
using RoR2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using SearchableAttribute = HG.Reflection.SearchableAttribute;

namespace Moonstorm
{
    public static class TokenModifierManager
    {
        //Since 1.5 introduces risk of options support, we now need to keep a cache of the attributes themselves so we can hot-reload languages.
        private static Dictionary<string, TokenModifierAttribute[]> cachedFormattingArray = null;
        [SystemInitializer(typeof(ConfigSystem))]
        private static void Init()
        {
            MSULog.Info($"Initializing TokenModifierManager");
            On.RoR2.Language.LoadStrings += (orig, self) =>
            {
                orig(self);
                ModifyTokensInLanguage(self);
            };
            RoR2Application.onLoad += () => ModifyTokensInLanguage(null);

            ModOptionPanelController.OnModOptionsExit += () =>
            {
                string langName = Language.currentLanguageName;
                Language.currentLanguage?.UnloadStrings();
                Language.SetCurrentLanguage(langName);
            };
        }

        private static void ModifyTokensInLanguage(Language lang)
        {
            lang = lang ?? Language.currentLanguage;

            if (cachedFormattingArray == null)
                CreateFormattingArray(lang);

            //Do formatting with cached array
            FormatTokens(lang);
        }

        private static void CreateFormattingArray(Language lang)
        {
            GetTokenModifierLists(out var propertyTokenModifiers, out var fieldTokenModifiers);
            var formattingDictionaryFromFields = CreateFormattingDictionary(fieldTokenModifiers);
            var formattingDictionaryFromProperties = CreateFormattingDictionary(propertyTokenModifiers);

            cachedFormattingArray = new Dictionary<string, TokenModifierAttribute[]>();

            foreach (var (token, formattingArray) in formattingDictionaryFromFields)
            {
                //Add token from dictionary, this replaces the array, but that's ok as this dictionary is currently empty
                cachedFormattingArray[token] = Array.Empty<TokenModifierAttribute>();
                var arrayFromCache = cachedFormattingArray[token];
                for (int i = 0; i < formattingArray.Length; i++)
                {
                    //Resize if needed
                    if (arrayFromCache.Length < i + 1)
                    {
                        Array.Resize(ref arrayFromCache, i + 1);
                    }

                    //only set value if the value in the cache is not null
                    if (arrayFromCache[i] == null)
                        arrayFromCache[i] = formattingArray[i];
                }
                cachedFormattingArray[token] = arrayFromCache;
            }
            foreach (var (token, formattingArray) in formattingDictionaryFromProperties)
            {
                //We do not overwrite the array if the token is already in the dictionary.
                //This is due to the fact that the kye may already be in the dictionary due to being created from fields with the token modifiers

                if (!cachedFormattingArray.ContainsKey(token))
                {
                    cachedFormattingArray[token] = Array.Empty<TokenModifierAttribute>();
                }
                var arrayFromCache = cachedFormattingArray[token];
                for (int i = 0; i < formattingArray.Length; i++)
                {
                    if (arrayFromCache.Length < i + 1)
                    {
                        Array.Resize(ref arrayFromCache, i + 1);
                    }
                    //only set value if the value in the cache is not null 
                    if (arrayFromCache[i] == null)
                        arrayFromCache[i] = formattingArray[i];
                }
                cachedFormattingArray[token] = arrayFromCache;
            }
        }

        private static void GetTokenModifierLists(out List<TokenModifierAttribute> propertyTokenModifiers, out List<TokenModifierAttribute> fieldTokenModifiers)
        {
            propertyTokenModifiers = new List<TokenModifierAttribute>();
            fieldTokenModifiers = new List<TokenModifierAttribute>();
            var allTokenModifiers = SearchableAttribute.GetInstances<TokenModifierAttribute>() ?? new List<SearchableAttribute>();
            foreach (TokenModifierAttribute tokenModifier in allTokenModifiers)
            {
                if (tokenModifier.target is FieldInfo)
                {
                    fieldTokenModifiers.Add(tokenModifier);
                }
                else
                {
                    propertyTokenModifiers.Add(tokenModifier);
                }
            }
        }

        private static Dictionary<string, TokenModifierAttribute[]> CreateFormattingDictionary(List<TokenModifierAttribute> tokenModifiers)
        {
            var dictionary = new Dictionary<string, TokenModifierAttribute[]>();
            if (tokenModifiers.Count == 0)
                return dictionary;

            foreach (TokenModifierAttribute tokenModifier in tokenModifiers)
            {
                try
                {
                    var token = tokenModifier.langToken;
                    var formattingIndex = tokenModifier.formatIndex;
                    //If the token is not in the dictionary, add it and initialize an empty array.
                    if (!dictionary.ContainsKey(token))
                    {
                        dictionary[token] = Array.Empty<TokenModifierAttribute>();
                    }

                    var dictArray = dictionary[token];
                    //Ensure array is big enough for the new modifier
                    if (dictArray.Length < formattingIndex + 1)
                    {
                        Array.Resize(ref dictArray, formattingIndex + 1);
                    }

                    //We should only set the modifier if there is no modifier already
                    if (dictArray[formattingIndex] == null)
                    {
                        dictArray[formattingIndex] = tokenModifier;
                    }
                    dictionary[token] = dictArray;
                }
                catch (Exception ex)
                {
                    MSULog.Error(ex);
                }
            }
            return dictionary;
        }

        private static void FormatTokens(Language lang)
        {
            if (cachedFormattingArray.Count == 0)
                return;

            MSULog.Info($"Modifying a total of {cachedFormattingArray.Count} tokens.");
            foreach (var (token, attributes) in cachedFormattingArray)
            {
                try
                {
                    if (!lang.stringsByToken.ContainsKey(token))
                    {
#if DEBUG
                        MSULog.Error($"Token {token} could not be found in the tokenToModifiers dictionary in {lang.name}! Either the mod that implements the token doesnt support the language {lang.name} or theyre adding their tokens via R2Api's LanguageAPI");
#endif
                        continue;
                    }
#if DEBUG
                    MSULog.Debug($"Modifying {token}");
#endif
                    FormatToken(lang, token, attributes);
                }
                catch (Exception e) { MSULog.Error($"{e}\n(Token={token})"); }
            }
        }

        private static void FormatToken(Language lang, string token, TokenModifierAttribute[] formattingArray)
        {
            var tokenValue = lang.stringsByToken[token];
            object[] format = formattingArray.Select(att => att.GetFormattingValue()).ToArray();
            var formatted = string.Format(tokenValue, format);
            lang.stringsByToken[token] = formatted;
        }
    }
}
