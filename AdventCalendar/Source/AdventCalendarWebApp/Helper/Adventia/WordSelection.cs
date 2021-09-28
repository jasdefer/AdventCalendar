using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventCalendarWebApp.Helper.Adventia
{
    public static class WordSelection
    {
        public static readonly string[] GermanBlacklist = new string[]
        {
            "die","der","und","in","zu","den","das","nicht","von","sie","ist","des","sich","mit","dem","dass","er","es","ein","ich","auf","so","eine","auch","als","an","nach","wie","im","für","man","aber","aus","durch","wenn","nur","war","noch","werden","bei","hat","wir","was","wird","sein","einen","welche","sind","oder","zur","um","haben","einer","mir","über","ihm","diese","einem","ihr","uns","da","zum","kann","doch","vor","dieser","mich","ihn","du","hatte","seine","mehr","am","denn","nun","sehr","selbst","schon","hier","bis","habe","ihre","dann","ihnen","seiner","alle","wieder","meine","gegen","vom","ganz","einzelnen","wo","muss","ohne","eines","können","sei","ja","wurde","jetzt","immer","seinen","wohl","dieses","ihren","würde","diesen","sondern","weil","welcher","nichts","diesem","alles","waren","will","mein","also","soll","worden","lassen","dies","machen","ihrer","weiter","recht","etwas","keine","seinem","ob","dir","allen","großen","Weise","müssen","welches","wäre","erst","einmal","hätte","zwei","dich","allein","während","anders","kein","damit","gar","euch","sollte","konnte","ersten","deren","zwischen","wollen","denen","dessen","sagen","bin","gut","darauf","wurden","weiß","gewesen","bald","große","solche","hatten","eben","andern","beiden","macht","sehen","ganze","anderen","wer","ihrem","zwar","gemacht","dort","kommen","heute","werde","derselben","ganzen","lässt","vielleicht","meiner","wichtigen","teilt","eins","drei","vier","fünf","sechs","sieben","acht","neun","zehn","one","two","three","four","five","six","seven","eight","nine","ten","erhalten","mittel","einzelnachweise","gelangt","danach","eingreift","eingreifen","gleich","gleiches","seitdem","seit","seid","ansonsten","weiteres"
        };

        public static readonly string[] EnglishBlacklist = new string[]
        {
            "the","be","to","of","and","been","a","in","that","have","I","it","for","not","on","with","he","as","you","do","at","this","but","his","by","from","they","we","say","her","she","or","an","will","my","one","all","would","there","their","what","so","up","out","if","about","who","get","which","go","me","when","make","can","like","time","no","just","him","know","take","into","your","some","could","them","see","other","than","then","now","only","come","its","over","also","use","two","how","our","well","way","even","new","want","because","any","these","give","day","most","us"
        };

        public static string[] GetWords(string text, int count, string[] blacklist, string keyword, Random random = null)
        {
            random ??= new Random(1);
            text = Cleanup(text);
            var words = text.Split(' ')
                .Where(x => x.Length > 2)
                .Select(x => x.ToLowerInvariant())
                .Where(x => !x.Contains(keyword.ToLowerInvariant()))
                .Distinct()
                .ToArray();
            words = Filter(words, blacklist);
            var maxCount = Math.Min(count, words.Length);
            var result = words
                .OrderBy(x => random.NextDouble())
                .Take(maxCount)
                .ToArray();

            return result;
        }

        private static string[] Filter(string[] words, string[] blacklist)
        {
            var result = new List<string>();
            for (int i = 0; i < words.Length; i++)
            {
                if (!blacklist.Contains(words[i]))
                {
                    result.Add(words[i]);
                }
            }
            return result.ToArray();
        }

        public static string Cleanup(string text)
        {
            var sb = new StringBuilder();
            foreach (var character in text)
            {
                if ((character >= 65 &&
                    character <= 90) ||
                    (character >= 97 &&
                    character <= 122) ||
                    (character >= 192 &&
                    character <= 255) ||
                    character == 32)
                {
                    sb.Append(character);
                }
                else if (character == 46)
                {
                    sb.Append(' ');
                }
            }
            return sb.ToString();
        }
    }
}