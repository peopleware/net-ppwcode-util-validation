﻿// Copyright 2017-2017 by PeopleWare n.v..
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

using System.Linq;
using System.Text;

namespace PPWCode.Util.Validation.I
{
    public abstract class AbstractIdentification : IIdentification
    {
        protected AbstractIdentification(string rawVersion)
        {
            RawVersion = rawVersion;
            CleanedVersion = Cleanup(rawVersion);
        }

        public abstract char PaddingCharacter { get; }

        public string CleanedVersion { get; }

        public bool IsValid => Validate(CleanedVersion);

        public bool IsStrictValid => Validate(RawVersion);

        public abstract int StandardLength { get; }

        public string RawVersion { get; }

        public virtual string ElectronicVersion => IsValid ? CleanedVersion : null;

        public abstract string PaperVersion { get; }

        protected string GetDigitStream(string stream)
        {
            if (stream == null)
                return string.Empty;

            var sb = new StringBuilder(stream.Length);
            foreach (var ch in stream.Where(char.IsDigit))
                sb.Append(ch);

            return sb.ToString();
        }

        protected string Pad(string identification)
        {
            return identification?.PadLeft(StandardLength, PaddingCharacter);
        }

        protected virtual string Cleanup(string identification)
        {
            return Pad(GetDigitStream(identification));
        }

        protected abstract bool Validate(string identification);
    }
}