using System;
using Catalyst;
using Catalyst.Models;
using Microsoft.Extensions.Logging;
using Mosaik.Core;
using Newtonsoft.Json;

namespace CatalystQuickstart
{
	public class Program
	{
		public Program()
		{
		}

		private static ILogger Logger = ApplicationLogging.CreateLogger<Program>();

		static async Task Main(string[] args)
		{
			Catalyst.Models.French.Register();

			string question = "Est-ce que'un chat peut voler?";

            //Storage.Current = new DiskStorage("catalyst-models");
            var nlp = await Pipeline.ForAsync(Language.French);
			var doc = new Document(question, Language.French);
			nlp.ProcessSingle(doc);
			Console.WriteLine(doc.ToJson());

			var text = doc.Value.ToString();
			Dictionary<string, string> TokenPOSDict = new Dictionary<string, string>();
			Dictionary<string, List<string>> POSTokenDict = new Dictionary<string, List<string>>();

			foreach (var data in doc.TokensData)
			{
				foreach (var PosData in data)
				{
					string tokenStr = text.Substring(PosData.LowerBound, PosData.UpperBound - PosData.LowerBound + 1);
					string tokenTag = PosData.Tag.ToString();

					TokenPOSDict[tokenStr] = tokenTag;

					if (!POSTokenDict.ContainsKey(tokenTag))
					{
						POSTokenDict[tokenTag] = new List<string>();

					}
					POSTokenDict[tokenTag].Add(tokenStr);

					Console.WriteLine(tokenStr + " , " + tokenTag);
				}
			}
			//noun, propn,
			//verb
			//Console.WriteLine("Noun: " + string.Join(',', POSTokenDict["NOUN"]));
            Console.WriteLine("Verb: " + string.Join(',', POSTokenDict["VERB"]));

			
        }

		private static void PrintDocumentEntities(IDocument doc)
		{
			Console.WriteLine($"Input text:\n\t'{doc.Value}'\n\nTokenized Value:\n\t'{doc.TokenizedValue(mergeEntities: true)}'\n\nEntities: \n{string.Join("\n", doc.SelectMany(span => span.GetEntities()).Select(e => $"\t{e.Value} [{e.EntityType.Type}]"))}");
		}
    }
}

