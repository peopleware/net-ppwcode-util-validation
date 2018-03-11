// Copyright 2018 by PeopleWare n.v..
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

using System;
using System.Globalization;
using System.Runtime.Serialization;

namespace PPWCode.Util.Validation.I.European.Germany
{
    /// <summary>
    ///     see <see href="https://de.wikipedia.org/wiki/Steuerliche_Identifikationsnummer" />
    /// </summary>
    [Serializable]
    [DataContract]
    public class TIN
        : AbstractDeIdentification,
          INationalNumberIdentification
    {
        public TIN(string rawVersion)
            : base(rawVersion)
        {
        }

        public override char PaddingCharacter
            => '0';

        protected override string OnPaperVersion
            => $"{CleanedVersion.Substring(0, 2)} {CleanedVersion.Substring(2, 3)} {CleanedVersion.Substring(5, 3)} {CleanedVersion.Substring(8, 3)}";

        public override int StandardMinLength
            => 11;

        public DateTime? BirthDate
            => null;

        public Sexe? Sexe
            => null;

        protected override bool OnValidate(string identification)
        {
            if (identification.Length != StandardMaxLength)
            {
                return false;
            }

            if (identification == new string('0', StandardMaxLength))
            {
                return false;
            }

            int product = 10;
            for (int i = 0; i < StandardMaxLength - 1; i++)
            {
                int sum = (CharUnicodeInfo.GetDecimalDigitValue(identification[i]) + product) % 10;
                if (sum == 0)
                {
                    sum = 10;
                }

                product = sum * 2 % 11;
            }

            int checkDigit = 11 - product;
            if (checkDigit == 10)
            {
                checkDigit = 0;
            }

            return checkDigit == CharUnicodeInfo.GetDecimalDigitValue(identification[StandardMaxLength - 2]);
        }
    }
}
