using ACTCore.CollectionService.Application.Interface;
using ACTCore.CollectionService.Application.Security;
using ACTCore.CollectionService.Domain.ValueObjects;
using SharedKernel.Templates;
using System.ComponentModel.DataAnnotations;
using System.Net.NetworkInformation;

namespace ACTCore.CollectionService.Application.Helpers
{
    public static class LangHelper
    {
        public static async Task<LanguageValue> GetResponseMsgAsync(
            ILanguageService languageService, string key, params object[] param)
        {
            try
            {
                return (await languageService.GetMessageAsync(x => x.Key == key))?.Value
                    ?.Format(param ?? Array.Empty<object>())
                    ?? new LanguageValue { En = key, Th = key };
            }
            catch(Exception) {
                return new LanguageValue { En = key, Th = key };
            }
        }
        
        public static async Task<EnumValue[]> GetEnumBoolAsync(
            ILanguageService languageService) => new EnumValue[] 
            { 
                new EnumValue { Label = await GetResponseMsgAsync(languageService,"Yes"), value = true },
                new EnumValue { Label = await GetResponseMsgAsync(languageService,"No"), value = false },
            };
        
    }
}
