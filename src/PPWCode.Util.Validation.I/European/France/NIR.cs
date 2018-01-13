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
using System.Runtime.Serialization;

namespace PPWCode.Util.Validation.I.European.France
{
    /// <summary>
    ///     see <see href="https://en.wikipedia.org/wiki/INSEE_code" />.
    ///     see
    ///     <see href="https://en.wikipedia.org/wiki/Institut_national_de_la_statistique_et_des_%C3%A9tudes_%C3%A9conomiques" />
    /// </summary>
    [Serializable]
    [DataContract]
    public class NIR
        : AbstractFrIdentification,
          INationalNumberIdentification
    {
        private ParseResult _parseResult;

        public NIR(string rawVersion)
            : base(rawVersion)
        {
        }

        protected override string OnPaperVersion
            => $"{CleanedVersion.Substring(0, 1)} {CleanedVersion.Substring(1, 2)} {CleanedVersion.Substring(3, 2)} {CleanedVersion.Substring(5, 5)} {CleanedVersion.Substring(10, 3)} {CleanedVersion.Substring(13, 2)}";

        public override char PaddingCharacter
            => '0';

        public Sexe? Sexe
        {
            get
            {
                if (_parseResult == null)
                {
                    _parseResult = ParseINSEE();
                }

                return _parseResult.Sexe;
            }
        }

        public override int StandardMinLength
            => 15;

        public DateTime? BirthDate
        {
            get
            {
                if (_parseResult == null)
                {
                    _parseResult = ParseINSEE();
                }

                return _parseResult.BirthDate;
            }
        }

        private ParseResult ParseINSEE()
        {
            DateTime? birthdate = null;
            Sexe? sexe = null;

            if (IsValid)
            {
                switch (int.Parse(CleanedVersion.Substring(0, 1)))
                {
                    case 1:
                    case 7:
                        sexe = I.Sexe.MALE;
                        break;

                    case 2:
                    case 8:
                        sexe = I.Sexe.FEMALE;
                        break;

                    default:
                        sexe = I.Sexe.NOT_KNOWN;
                        break;
                }

                int yy = 1900 + int.Parse(CleanedVersion.Substring(1, 2));
                int mm = int.Parse(CleanedVersion.Substring(3, 2));
                if (1 <= mm && mm <= 12)
                {
                    try
                    {
                        birthdate = new DateTime(yy, mm, 1);
                        if (birthdate > DateTime.Today)
                        {
                            birthdate = null;
                        }
                    }
                    // ReSharper disable once EmptyGeneralCatchClause
                    catch
                    {
                    }
                }
            }

            return new ParseResult(birthdate, sexe);
        }

        protected override bool OnValidate(string identification)
        {
            if (identification == new string('0', StandardMaxLength))
            {
                return false;
            }

            long number = long.Parse(identification.Substring(0, 13));
            long controlNumber = long.Parse(identification.Substring(13, 2));
            long modulo97 = number % 97;
            return modulo97 == controlNumber;
        }

        [Serializable]
        [DataContract]
        private class ParseResult
        {
            public ParseResult(DateTime? birthDate, Sexe? sexe)
            {
                BirthDate = birthDate;
                Sexe = sexe;
            }

            [DataMember]
            public DateTime? BirthDate { get; }

            [DataMember]
            public Sexe? Sexe { get; }
        }
    }
}
