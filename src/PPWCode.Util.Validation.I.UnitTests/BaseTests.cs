// Copyright 2017 by PeopleWare n.v..
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
// http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml;

using NUnit.Framework;

namespace PPWCode.Util.Validation.I.UnitTests
{
    [TestFixture]
    public abstract class BaseTests
    {
        [SetUp]
        public void Setup()
        {
            OnSetup();
        }

        [TearDown]
        public void TearDown()
        {
            OnTearDown();
        }

        protected virtual void OnSetup()
        {
        }

        protected virtual void OnTearDown()
        {
        }

        private static string SerializeToXmlString(object obj)
        {
            string str = string.Empty;
            using (MemoryStream memoryStream = new MemoryStream())
            {
                if (obj != null)
                {
                    new NetDataContractSerializer().WriteObject(memoryStream, obj);
                    memoryStream.Flush();
                    memoryStream.Position = 0L;
                    str = new StreamReader(memoryStream).ReadToEnd();
                }
            }

            return str;
        }

        private static T DeserializeFromXmlString<T>(string obj)
            where T : class
        {
            if (string.IsNullOrEmpty(obj))
            {
                return default(T);
            }

            using (StringReader stringReader = new StringReader(obj))
            {
                using (XmlReader reader = XmlReader.Create(stringReader))
                {
                    return (T)new NetDataContractSerializer().ReadObject(reader);
                }
            }
        }

        protected virtual T DeepCloneUsingXml<T>(T obj)
            where T : class
        {
            string s = SerializeToXmlString(obj);
            return DeserializeFromXmlString<T>(s);
        }

        protected virtual T DeepCloneUsingBinaryFormatter<T>(T obj)
            where T : class
        {
            using (var ms = new MemoryStream())
            {
                var formatter = new BinaryFormatter();
                formatter.Serialize(ms, obj);
                ms.Position = 0;

                return (T)formatter.Deserialize(ms);
            }
        }
    }
}
