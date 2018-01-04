// Copyright 2017-2017 by PeopleWare n.v..
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
// 

using System;
using System.Runtime.Serialization;

namespace PPWCode.Util.Validation.I.European.Belgium
{
    [Serializable]
    [DataContract]
    public class KBO : AbstractIdentification
    {
        public KBO(string rawVersion) : base(rawVersion)
        {
        }

        protected override string OnPaperVersion
            => $"{CleanedVersion.Substring(0, 4)}.{CleanedVersion.Substring(4, 3)}.{CleanedVersion.Substring(7, 3)}";

        public override char PaddingCharacter
            => '0';

        public override int StandardMinLength
            => 10;

        protected override bool OnValidate(string identification)
        {
            long rest = 97 - (long.Parse(identification.Substring(0, StandardMaxLength - 2)) % 97);
            return rest == long.Parse(identification.Substring(StandardMaxLength - 2, 2));
        }
    }
}
