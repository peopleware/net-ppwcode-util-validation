// Copyright 2019 by PeopleWare n.v..
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// http://www.apache.org/licenses/LICENSE-2.0
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

using NUnit.Framework;

namespace PPWCode.Util.Validation.III.UnitTests
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

        protected virtual T DeepCloneUsingBinaryFormatter<T>(T obj)
            where T : class
        {
            using (MemoryStream ms = new MemoryStream())
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(ms, obj);
                ms.Position = 0;

                return (T)formatter.Deserialize(ms);
            }
        }
    }
}
