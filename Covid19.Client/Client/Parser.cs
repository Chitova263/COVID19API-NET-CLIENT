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
        public IEnumerable<TResult> Parse<TResult, TClassMap>(Stream stream)
            where TClassMap : ClassMap<TResult>
        {
            if (stream is null)
            {
                throw new ArgumentNullException(nameof(stream));
            }

            var csvReader = new CsvReader(new StreamReader(stream, Encoding.UTF8), CultureInfo.InvariantCulture);
            csvReader.Context.Configuration.TrimOptions = TrimOptions.Trim;
            csvReader.Context.RegisterClassMap<TClassMap>();

            return csvReader.GetRecords<TResult>();
        }
    }
}
