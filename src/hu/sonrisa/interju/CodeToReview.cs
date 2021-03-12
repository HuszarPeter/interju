using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CsharpInterview.ConsoleApp
{
    public class MrzExtractor
    {
        private readonly Action<string, object> updateExtractedValue;
        private readonly Action<ExtractionAttempt> addExtractionAttemptToHistory;
        private readonly IObservable<ImmutableDictionary<string, object>> whenValues;

        private Dictionary<string, JObject> cache;

        // ...

        public async void ExtractMrz(
            ImageDescriptor imageDescriptor,
            IEnumerable<Field> fields,
            ImmutableDictionary<string, object> values) 
        {
            if (!cache.TryGetValue(imageDescriptor.ImageClassifier, out var result)) 
            {
                var extractionResult =
                    await LocalMrzReader
                        .Extract(
                            imageDescriptor.Bitmap)
                        .ConfigureAwait(true); 

                if (!IsNotNullNorEmpty(extractionResult)) 
                {
                    cache.Add(imageDescriptor.ImageClassifier, null); 
                    return;
                }

                result = JObject.Parse(extractionResult);
                cache.Add(imageDescriptor.ImageClassifier, result);
            } 

            if (result is null)
            {
                return;
            }

            var rawLines = GetRawLinesFromResult(result);

            if (rawLines.Count() < 2 || ConfidenceValue(result) < 0.9f)
            {
                return;
            }

            var fieldList = fields as Field[] ?? fields.ToArray();
            fieldList
                .Where(x => x.DataSources.Any() && values.Any(v => v.Key == x.Name && !HasValue(v.Value)))
                .Select(f => (rawLines, f.Name, f.DataSources.First(ds => ds.Type == DataSourceType.Mrz), f))
                .ForEach(FillField);

            fieldList
                .Where(x => x is MrzField && values.Any(v => v.Key == x.Name && !HasValue(v.Value)))
                .Select(f => (rawLines, (MrzField)f))
                .ForEach(FillMrzCheckDigits);
        }

        private static double ConfidenceValue(JObject result)
        {
            IDictionary<string, JToken> resultFieldsList = (JObject)((JArray)result["fields"]).First();
            return resultFieldsList
                .Where(x => x.Key == "DocType" || x.Key == "DocTypeCode" || x.Key == "Country")
                .Select(x => float.Parse(x.Value?["Confidence"]?.ToString() ?? "0.0"))
                .Average();
        }

        private static JToken GetRawLinesFromResult(JObject result)
        {
            IDictionary<string, JToken> resultFieldsList = (JObject)((JArray)result["fields"]).First();
            return resultFieldsList.First(x => x.Key == "RawMrzLines").Value;
        }

        private void FillField((JToken rawLines, string fieldName, DataSource dataSource, Field field) result)
        {
            var (rawLines, fieldName, dataSource, field) = result;
            var extractedStr = GetValueFromRawLines(rawLines, dataSource.Row, dataSource.StartIndex, dataSource.Length);

            if (IsNotNullNorEmpty(dataSource.SplitToCharacter))
            {
                extractedStr = extractedStr.Split(new[] { dataSource.SplitToCharacter }, StringSplitOptions.RemoveEmptyEntries).First();
            }
            else if (IsNotNullNorEmpty(dataSource.SplitFromCharacter))
            {
                extractedStr = extractedStr.Split(new[] { dataSource.SplitFromCharacter }, StringSplitOptions.RemoveEmptyEntries).Last();
            }

            //Log.Debug($"Parse from raw lines: {extractedStr} Field: {fieldName}");

            extractedStr = CleanUpResult(extractedStr);

            if (string.IsNullOrEmpty(extractedStr))
            {
                return;
            }

            switch (field)
            {
                //case DateField df:
                //    {
                //        if (DateTime.TryParseExact(extractedStr, "yyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.AllowWhiteSpaces, out var date))
                //        {
                //            var value = date.ToString(df.Formatter.Pattern, CultureInfo.InvariantCulture);
                //            updateExtractedValue(fieldName, DateModel.Create(df.Formatter.Pattern, value));
                //            AddToHistory(fieldName, date.ToInternalDateFormat());
                //        }
                //        break;
                //    }
                //case FormattedField ff:
                //    {
                //        updateExtractedValue(fieldName, extractedStr);
                //        AddToHistory(fieldName, extractedStr);
                //        break;
                //    }
                //case CountryField cf:
                //    {
                //        updateExtractedValue(fieldName, Countries.FromCode(extractedStr));
                //        AddToHistory(fieldName, extractedStr);
                //        break;
                //    }
                //case SelectionField sf:
                //    {
                //        updateExtractedValue(fieldName, extractedStr);
                //        AddToHistory(fieldName, extractedStr);
                //        break;
                //    }
            }
        }

        private bool HasValue(object value)
        {
            return default; // ...
        }

        private bool IsNotNullNorEmpty(string extractionResult)
        {
            return !string.IsNullOrEmpty(extractionResult);
        }

        private void FillMrzCheckDigits((JToken rawLines, MrzField field) result)
        {
            var (rawLines, field) = result;
            var cfg = field.Config;

            var mrzDigits = cfg.CheckDigits
                .Select(digitCfg => GetValueFromRawLines(rawLines, digitCfg.Row, digitCfg.Position, 1))
                .Select(val => int.Parse(val.Replace("<", "0")))
                .Select(val => /*(Optional<int>)*/val)
                .ToArray();

            var mrzValue = new MrzValue(mrzDigits);
            updateExtractedValue(field.Name, mrzValue);
            AddToHistory(field.Name, mrzValue.ToString());
        }

        private string GetValueFromRawLines(JToken jToken, int row, int start, int length)
        {
            return jToken[row - 1]?["Value"]?.ToString().Substring(start - 1, length);
        }

        private void AddToHistory(string fieldName, string extractionResult)
        {
            addExtractionAttemptToHistory(
                new ExtractionAttempt(
                    fieldName,
                    "MachineReadableZone",
                    "SingleField",
                    extractionResult));
        }

        private static string CleanUpResult(string extractionResult)
        {
            return new Regex(@"^[,.'´`‚’‘‘°\)\(\-\s<]+")
                .Replace(extractionResult, string.Empty)
                .Trim();
        }
    }

    internal class MrzValue
    {
        private object mrzDigits;

        public MrzValue(object mrzDigits)
        {
            this.mrzDigits = mrzDigits;
        }
    }

    internal class Optional<T>
    {
    }

    internal class MrzField : Field
    {
        public MRZCheckDigitsConfig Config { get; internal set; }
        public string Name { get; internal set; }
    }

    public class MRZCheckDigitsConfig
    {
        public IEnumerable<IMrzCheckDigit> CheckDigits { get; }
    }

    public interface IMrzCheckDigit
    {
        string FieldLabel { get; }

        public int Row { get; }

        public int Position { get; }
    }

    public static class LocalMrzReader
    {
        public static async Task<string> Extract(Bitmap bitmapData)
        {
            if (bitmapData is null)
            {
                return string.Empty;
            }

            const int timeout = 5000;
            var output = new StringBuilder();
            var error = new StringBuilder();
            using var process = new Process();
            using var outputWaitHandle = new AutoResetEvent(false);
            using var errorWaitHandle = new AutoResetEvent(false);

            // ...

            process.Start();
            if (process.WaitForExit(timeout) &&
                outputWaitHandle.WaitOne(timeout) &&
                errorWaitHandle.WaitOne(timeout))
            {
                if (process.ExitCode != 0)
                {
                    //Log.Information($"MRZ Extractor errors: {error}");
                    return string.Empty;
                }

                var result = output.ToString();
                //Log.Information($"MRZ Extractor result: {result}");
                return result;
            }
            else
            {
                //Log.Information($"MRZ Extractor timed out");
                return string.Empty;
            }
        }
    }

    public class ExtractionAttempt
    {
        public ExtractionAttempt(string fieldName, string extractionSource, string trigger, string value)
        {
            FieldName = fieldName;
            ExtractionSource = extractionSource;
            Trigger = trigger ?? string.Empty;
            Value = value ?? string.Empty;
        }

        [JsonIgnore]
        public string FieldName { get; }

        [JsonProperty("extractionSource")]
        public string ExtractionSource { get; }

        [JsonProperty("trigger")]
        public string Trigger { get; }

        [JsonIgnore]
        public string Value { get; }

        [JsonProperty("levenshteinDistance")]
        public int LevenshteinDistance { get; private set; }

        public void CalculateLevensteinDistance(object finalValue)
        {
            // LevenshteinDistance = ...
        }
    }

    public sealed class ImageDescriptor
    {
        public static readonly string RejectReasonAnnotationsGroupName = "RejectReasonAnnotations";
        public static readonly string ExtractionRectangleGroupName = "ExtractionRects";

        public string ImageClassifier { get; }

        public Bitmap Bitmap { get; }
    }

    public abstract class Field
    {
        public string Name { get; }

        public string Label { get; }

        public bool IsReadOnly { get; }

        public int Order { get; }

        public int Rows { get; }

        public int Columns { get; }

        public IEnumerable<Func<object, bool>> Validators { get; }

        public IEnumerable<DataSource> DataSources { get; }
    }

    public class DataSource
    {
        public DataSourceType Type { get; set; }

        public int Row { get; set; }

        public int StartIndex { get; set; }

        public int Length { get; set; }

        public string SplitFromCharacter { get; set; }

        public string SplitToCharacter { get; set; }
    }

    public enum DataSourceType
    {
        Mrz,
    }
    public static class EnumerableExtensions
    {
        public static void ForEach<T>(this IEnumerable<T> @this, Action<T> action)
        {
            foreach (var item in @this)
            {
                action(item);
            }
        }
    }
}
