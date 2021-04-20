using CsvHelper;
using CsvHelper.Configuration;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;

namespace Covid19
{
    internal sealed class Parser
    {
        public static IAsyncEnumerable<TResult> ParseAsync<TResult, TClassMap>(Stream stream)
            where TClassMap : ClassMap<TResult>
        {
            if (stream is null)
            {
                throw new ArgumentNullException(nameof(stream));
            }

            var csvReader = new CsvReader(new StreamReader(stream, Encoding.UTF8), CultureInfo.InvariantCulture);
            csvReader.Context.Configuration.TrimOptions = TrimOptions.Trim;
            csvReader.Context.RegisterClassMap<TClassMap>();

            IAsyncEnumerable<TResult>? records = csvReader.GetRecordsAsync<TResult>();
            return records;
        }

        public static IEnumerable<TResult> Parse<TResult, TClassMap>(Stream stream)
            where TClassMap : ClassMap<TResult>
        {
            if (stream is null)
            {
                throw new ArgumentNullException(nameof(stream));
            }

            var csvReader = new CsvReader(new StreamReader(stream, Encoding.UTF8), CultureInfo.InvariantCulture);
            csvReader.Context.Configuration.TrimOptions = TrimOptions.Trim;
            csvReader.Context.RegisterClassMap<TClassMap>();

            IEnumerable<TResult>? records = csvReader.GetRecords<TResult>();
            return records;
        }
    }
}
