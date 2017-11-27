using LemonCore.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace LemonCore.IO
{
    public class JsonConsoleWriter<T> : Core.Interfaces.IDataWriter<T>
    {
        private long _index;

        public JsonConsoleWriter()
        {
            _index = 0;
        }

        public void Dispose()
        {

        }

        public void Write(IEnumerable<T> records)
        {
            foreach (var record in records)
            {
                Write(record);
            }
        }

        public void Write(T record)
        {
            var json = Newtonsoft.Json.JsonConvert.SerializeObject(record);

            Console.WriteLine("[{0}]-{1}", _index, json);

            _index++;
        }
    }
}
